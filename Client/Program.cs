using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using SharedContract;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var service = channel.CreateGrpcService<IgRPCService>();

            var foo = new Model
            {
                Foo = new FooB()
            };

            await service.SendFoo(foo);

            Console.WriteLine("Press enter to close");
            Console.ReadLine();
        }
    }
}
