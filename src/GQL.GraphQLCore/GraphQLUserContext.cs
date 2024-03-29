﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GQL.GraphQLCore
{
    public class GraphQLUserContext
    {
        public IHttpContextAccessor HttpContextAccessor { get; private set; }

        public GraphQLUserContext(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }
    }
}
