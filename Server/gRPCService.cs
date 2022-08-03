using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc;
using SharedContract;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    public class gRPCService : IgRPCService
    {
        private readonly ILogger _logger;
        public gRPCService(ILogger<gRPCService> logger)
        {
            _logger = logger;
        }

        public Task SendFoo(Model foo, CallContext context)
        {
            return Task.CompletedTask;
        }
    }
}
