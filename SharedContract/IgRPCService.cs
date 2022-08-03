using ProtoBuf;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;
using System.Threading.Tasks;

namespace SharedContract
{
    [Service]
    public interface IgRPCService
    {
        Task SendFoo(Model foo, CallContext context = default);
    }

    [ProtoContract]
    public sealed class Model
    {
        public Model()
        {
            Foo = new FooA();
        }

        [ProtoMember(1)]
        public FooBase Foo { get; set; }

    }

    [ProtoContract]
    [ProtoInclude(100, typeof(FooA))]
    [ProtoInclude(200, typeof(FooB))]
    public abstract class FooBase
    {
    }

    [ProtoContract]
    public sealed class FooA : FooBase
    {
        [ProtoMember(1)]
        public string Property1 { get; set; }
    }

    [ProtoContract]
    public sealed class FooB : FooBase
    {
        [ProtoMember(1)]
        public string Property2 { get; set; }
    }
}
