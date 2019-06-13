using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Greet;
using Grpc.Core;

namespace Proba.Server
{
    public class ProbaService : ProbaServer.ProbaServerBase
    {
        public async override Task SayHello(ProbaRequest request, IServerStreamWriter<ProbaReply> responseStream, ServerCallContext context)
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

            await responseStream.WriteAsync(reply);

            var reply2 = new ProbaReply();
            reply2.Messages.Add(new ProbaMessage
            {
                Greeting = "Bye now",
                Count = 100,
                Value = 0.45,
                Date = Timestamp.FromDateTimeOffset(DateTime.Now.AddMinutes(120).ToUniversalTime())
            });

            await responseStream.WriteAsync(reply2);
        }
    }
}
