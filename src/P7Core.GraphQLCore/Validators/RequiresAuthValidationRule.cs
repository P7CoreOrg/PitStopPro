﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Language.AST;
using GraphQL.Types;
using GraphQL.Validation;

using P7Core.GraphQLCore.Stores;

namespace P7Core.GraphQLCore.Validators
{
    interface ICurrentEnterLeaveListenerState
    {
        EnterLeaveListenerState EnterLeaveListenerState { get; }
    }

    public interface IPluginValidationRule : IValidationRule { }
    public class RequiresAuthValidationRule : IPluginValidationRule
    {
        class MyEnterLeaveListenerSink : IEnterLeaveListenerEventSink, ICurrentEnterLeaveListenerState
        {
            private EnterLeaveListenerState CurrentFragmentDefinitionRoot { get; set; }
            private Dictionary<string, Stack<EnterLeaveListenerState>> _fragmentMap;
            private Dictionary<string, Stack<EnterLeaveListenerState>> FragmentMap
            {
                get { return _fragmentMap ?? (_fragmentMap = new Dictionary<string, Stack<EnterLeaveListenerState>>()); }
            }

            private void SafeFragmentMapAdd(string name, EnterLeaveListenerState enterLeaveListenerState)
            {
                Stack<EnterLeaveListenerState> listenerStates;
                if (FragmentMap.ContainsKey(name))
                {
                    listenerStates = FragmentMap[name];
                }
                else
                {
                    listenerStates = new Stack<EnterLeaveListenerState>();
                    FragmentMap[name] = listenerStates;
                }
                listenerStates.Push(enterLeaveListenerState);
            }

            private EnterLeaveListenerState SafeFragmentMapPop(string name)
            {
                if (!FragmentMap.ContainsKey(name))
                {
                    throw new Exception("Frament Map Enter Leave corrupt");
                }

                var listenerStates = FragmentMap[name];
                var item = listenerStates.Pop();
                return item;
            }

            public EnterLeaveListenerState EnterLeaveListenerState { get; private set; }

            public void OnEvent(EnterLeaveListenerState enterLeaveListenerState)
            {

                if (enterLeaveListenerState.FragmentSpread != null)
                {
                    SafeFragmentMapAdd(enterLeaveListenerState.FragmentSpread.Name, enterLeaveListenerState);
                }
                else if (enterLeaveListenerState.FragmentDefinition != null)
                {
                    CurrentFragmentDefinitionRoot = SafeFragmentMapPop(enterLeaveListenerState.FragmentDefinition.Name);
                    FragmentMap.Remove(enterLeaveListenerState.FragmentDefinition.Name);

                }
                else
                {
                    EnterLeaveListenerState = enterLeaveListenerState;
                    if (CurrentFragmentDefinitionRoot != null)
                    {
                        EnterLeaveListenerState.CurrentFieldPath =
                            $"{CurrentFragmentDefinitionRoot.CurrentFieldPath}{EnterLeaveListenerState.CurrentFieldPath}";
                    }
                }
            }
        }

        private readonly IGraphQLFieldAuthority _graphQLFieldAuthority;

        public RequiresAuthValidationRule(IGraphQLFieldAuthority graphQLFieldAuthority)
        {

            _graphQLFieldAuthority = graphQLFieldAuthority;
        }

        public INodeVisitor Validate(ValidationContext context)
        {
            var userContext = context.UserContext.As<GraphQLUserContext>();
            var user = userContext.HttpContextAccessor.HttpContext.User;

            IEnumerable<string> claimsEnumerable = (from item in user.Claims
                                                    let c = item.Type
                                                    select c).ToList();
            //            IEnumerable<string> claimsEnumerable = query.ToList();
            var authenticated = user?.Identity.IsAuthenticated ?? false;

            var myEnterLeaveListenerSink = new MyEnterLeaveListenerSink();
            var currentEnterLeaveListenerState = (ICurrentEnterLeaveListenerState)myEnterLeaveListenerSink;
            var myEnterLeaveListener = new MyEnterLeaveListener(_ =>
            {

                _.Match<Operation>(op =>
                {
                    if (!authenticated)
                    {
                        //                        var usages = context.GetRecursiveVariables(op).Select(usage => usage.Node.Name);
                        //                        var selectionSet = op.SelectionSet;
                        if (op.OperationType == OperationType.Mutation)
                        {
                            context.ReportError(new ValidationError(
                                context.OriginalQuery,
                                "auth-required",
                                $"Authorization is required to access {op.Name}.",
                                op));
                        }
                    }

                });

                _.Match<Field>(fieldAst =>
                {
                    var currentFieldPath = currentEnterLeaveListenerState.EnterLeaveListenerState.CurrentFieldPath;
                    var currentOperationType = currentEnterLeaveListenerState.EnterLeaveListenerState.OperationType;
                    var requiredClaims = _graphQLFieldAuthority
                        .FetchRequiredClaimsAsync(currentOperationType, currentFieldPath).Result;
                    var canAccess = true;
                    if (requiredClaims.StatusCode == GraphQLFieldAuthority_CODE.FOUND)
                    {
                        if (!authenticated)
                        {
                            canAccess = false;
                        }
                    }

                    if (canAccess &&
                        requiredClaims != null &&
                        requiredClaims.Value.Any())
                    {
                        canAccess = CanAccess(requiredClaims, user);
                    }

                    //  var canAccess = rcQuery.All(x => claimsEnumerable?.Contains(x) ?? false);
                    if (!canAccess)
                    {
                        context.ReportError(new ValidationError(
                            context.OriginalQuery,
                            "auth-required",
                            $"You are not authorized to run this query.",
                            fieldAst));

                    }
                });
                /*
                // this could leak info about hidden fields in error messages
                // it would be better to implement a filter on the schema so it
                // acts as if they just don't exist vs. an auth denied error
                // - filtering the schema is not currently supported
                _.Match<Field>(fieldAst =>
                {
                    var fieldDef = context.TypeInfo.GetFieldDef();
                    if (fieldDef.RequiresPermissions() &&
                        (!authenticated || !fieldDef.CanAccess(userContext.User.Claims)))
                    {
                        context.ReportError(new ValidationError(
                            context.OriginalQuery,
                            "auth-required",
                            $"You are not authorized to run this query.",
                            fieldAst));
                    }
                });
                */
            });
            myEnterLeaveListener.RegisterEventSink(myEnterLeaveListenerSink);
            return myEnterLeaveListener;
        }

        private static bool CanAccess(FetchRequireClaimsResult<IEnumerable<Claim>> requiredClaims, ClaimsPrincipal user)
        {
            return requiredClaims.Value.All(x =>
            {
                foreach (var ce in user.Claims)
                {
                    if (ce.Type == x.Type)
                    {
                        if (string.IsNullOrEmpty(x.Value))
                        {
                            if (string.IsNullOrEmpty(ce.Value))
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (x.Value == ce.Value)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            });
        }
    }


}
