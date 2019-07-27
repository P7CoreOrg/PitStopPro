using System;
using System.Collections.Generic;
using System.Text;
using GQL.Contracts;

namespace GQL.GraphQLCore.Stores
{
    public class DefaultScopedSummaryLogger : Dictionary<string, string>, IScopedSummaryLogger
    {
    }
}
