using ProtoBuf;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;
using System.Collections.Generic;

namespace SharedContract
{
    [Service]
    public interface IgRPCService
    {
        IAsyncEnumerable<Callback> SubscribeToCallback(CallContext context = default);
    }

    [ProtoContract]
    public class Callback
    {
        [ProtoMember(1)]
        public string Text { get; set; }
    }
}
