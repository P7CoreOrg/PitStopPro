using System;
using System.Collections.Generic;
using System.Text;
using GraphQLPlay.Contracts;

namespace P7Core.GraphQLCore.Stores
{
    public class DefaultScopedSummaryLogger : Dictionary<string,string>,IScopedSummaryLogger
    {
    }
}
