﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Http;
using P7Core.Writers;

namespace GQL.GraphQLCore
{
    public class GraphQLDocumentWriter : JsonDocumentWriter, IDocumentWriter
    {
        public GraphQLDocumentWriter(IGraphQLJsonDocumentWriterOptions options) : base(options)
        {
        }


        public Task WriteAsync<T>(Stream stream, T value)
        {
            throw new NotImplementedException();
        }

        public Task<IByteResult> WriteAsync<T>(T value)
        {
            throw new NotImplementedException();
        }

        public string Write(object value)
        {
            return SerializeObject(value);
        }
    }
}
