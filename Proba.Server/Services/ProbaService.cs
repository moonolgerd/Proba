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

            using var db = new ProbaContext();
            foreach (var proba in db.Probas)
            {
                proba.Date = Timestamp.FromDateTimeOffset(proba.Added.ToUniversalTime());
                reply.Messages.Add(proba);
            }

            await responseStream.WriteAsync(reply);

            //var reply2 = new ProbaReply();
            //reply2.Messages.Add(new ProbaMessage
            //{
            //    Greeting = "Bye now",
            //    Count = 100,
            //    Value = 0.45,
            //    Date = Timestamp.FromDateTimeOffset(DateTime.Now.AddMinutes(120).ToUniversalTime())
            //});

            //await responseStream.WriteAsync(reply2);
        }

        public async override Task<ProbaReply> AddProba(ProbaMessage request, ServerCallContext context)
        {
            using var db = new ProbaContext();

            db.Probas.Add(request);
            await db.SaveChangesAsync();

            var reply = new ProbaReply();
            reply.Messages.Add(request);
            return reply;
        }
    }
}
