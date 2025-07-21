using Grpc.Core;
using Grpc.Net.Client;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Client;
using SharedContract;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        private static CancellationTokenSource _callbackCancellationToken = new CancellationTokenSource();

        static void Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var service = channel.CreateGrpcService<IgRPCService>();

            var callbackHandler = Task.Run(async () => await CallbackHandler(service.SubscribeToCallback(new CallContext(new CallOptions(cancellationToken: _callbackCancellationToken.Token)))), _callbackCancellationToken.Token);

            Console.WriteLine("Press enter to disconnect");
            Console.ReadLine();

            //!! THIS IS THE LINE THAT CRASH WITH A UNBALANCED ENTER/EXIT EXCEPTION ON ANDROID !!
            _callbackCancellationToken.Cancel(); // Cancel the callback subscription. 

            Console.WriteLine("Disconnected");
        }

        private static async Task CallbackHandler(IAsyncEnumerable<Callback> callbacks)
        {
            Console.WriteLine("Connected");
            try
            {
                await foreach (var callback in callbacks)
                {
                    Console.WriteLine(callback.Text);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
