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

        public IAsyncEnumerable<Callback> SubscribeToCallback(CallContext context = default) => SubscribeToCallback(context.CancellationToken, context);

        private async IAsyncEnumerable<Callback> SubscribeToCallback([EnumeratorCancellation] CancellationToken cancel, CallContext context = default)
        {
            int cnt = 0;
            var timerTask = Task.Delay(1000, cancel); // send a string every second
            while (!cancel.IsCancellationRequested)
            {
                try
                {
                    await timerTask;
                }
                catch (OperationCanceledException)
                {
                    // Normal, client disconnected
                    yield break;
                }

                if (!cancel.IsCancellationRequested)
                {
                    yield return new Callback { Text = $"New counter: {cnt++}" };
                    timerTask = Task.Delay(1000, cancel);
                }
            }
        }
    }
}
