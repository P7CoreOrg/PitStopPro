﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using GraphQL;
using GraphQL.Builders;
using GraphQL.Types;

namespace GQL.GraphQLCore
{
    public static class GraphQLExtensions
    {
        public static readonly string PermissionsKey = "Permissions";

        public static bool RequiresPermissions(this IProvideMetadata type)
        {
            var permissions = type.GetMetadata<IEnumerable<string>>(PermissionsKey, new List<string>());
            return permissions.Any();
        }

        public static bool CanAccess(this IProvideMetadata type, IEnumerable<string> claims)
        {
            var permissions = type.GetMetadata<IEnumerable<string>>(PermissionsKey, new List<string>());
            return permissions.All(x => claims?.Contains(x) ?? false);
        }

        public static bool HasPermission(this IProvideMetadata type, string permission)
        {
            var permissions = type.GetMetadata<IEnumerable<string>>(PermissionsKey, new List<string>());
            return permissions.Any(x => string.Equals(x, permission));
        }

        public static void AddPermission(this IProvideMetadata type, string permission)
        {
            var permissions = type.GetMetadata<List<string>>(PermissionsKey);

            if (permissions == null)
            {
                permissions = new List<string>();
                type.Metadata[PermissionsKey] = permissions;
            }

            permissions.Fill(permission);
        }

        public static FieldBuilder<TSourceType, TReturnType> AddPermission<TSourceType, TReturnType>(
            this FieldBuilder<TSourceType, TReturnType> builder, string permission)
        {
            builder.FieldType.AddPermission(permission);
            return builder;
        }
    }
}
