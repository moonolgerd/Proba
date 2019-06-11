using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Greet;
using Grpc.Core;

namespace Proba.Server
{
    public class ProbaService : ProbaServer.ProbaServerBase
    {
        public override Task<ProbaReply> SayHello(ProbaRequest request, ServerCallContext context)
        {
            var reply = new ProbaReply();

            reply.Messages.Add(new ProbaMessage
            {
                Greeting = "Hello " + request.Name,
                Count = 1,
                Value = 123.45,
                Date = Timestamp.FromDateTimeOffset(DateTime.Now.AddMinutes(120).ToUniversalTime())
            });
            reply.Messages.Add(new ProbaMessage
            {
                Greeting = "Hello " + request.Name,
                Count = 2,
                Value = 13.145,
                Date = Timestamp.FromDateTimeOffset(DateTime.Now.AddMinutes(120).ToUniversalTime())
            });
            reply.Messages.Add(new ProbaMessage
            {
                Greeting = "Hello " + request.Name,
                Count = 3,
                Value = 3.145,
                Date = Timestamp.FromDateTimeOffset(DateTime.Now.AddMinutes(120).ToUniversalTime())
            });
            reply.Messages.Add(new ProbaMessage
            {
                Greeting = "Hello " + request.Name,
                Count = 4,
                Value = 123.5,
                Date = Timestamp.FromDateTimeOffset(DateTime.Now.AddMinutes(120).ToUniversalTime())
            });

            return Task.FromResult(reply);
        }
    }
}
