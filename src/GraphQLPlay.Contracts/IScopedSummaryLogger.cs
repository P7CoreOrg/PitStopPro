using System;
using System.Collections;
using System.Collections.Generic;

namespace GraphQLPlay.Contracts
{
    public interface ISummaryLogger : IDictionary<string, string>
    {

    }
    public interface IScopedSummaryLogger : ISummaryLogger
    {

    }
}
