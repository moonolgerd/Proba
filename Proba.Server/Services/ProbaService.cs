using System;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Greet;
using Grpc.Core;

namespace Proba.Server
{
    public class ProbaService : ProbaServer.ProbaServerBase
    {
        public async override Task GetProbas(ProbaRequest request, IServerStreamWriter<ProbaReply> responseStream, ServerCallContext context)
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

        /// <summary>
        /// Add proba
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>ProbaActionReply</returns>
        public async override Task<ProbaActionReply> AddProba(ProbaMessage request, ServerCallContext context)
        {
            var reply = new ProbaActionReply();
            using var db = new ProbaContext();

            try
            {
                db.Probas.Add(request);
                await db.SaveChangesAsync();
                reply.Message = "Successfully added";

            } catch (Exception ex)
            {
                WriteException(reply, ex);
            }        
            
            return reply;
        }

        /// <summary>
        /// Delete proba
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>ProbaActionReply</returns>
        public async override Task<ProbaActionReply> DeleteProba(ProbaMessage request, ServerCallContext context)
        {
            var reply = new ProbaActionReply();
            using var db = new ProbaContext();

            try
            {
                var r = db.Probas.Remove(request);
                
                await db.SaveChangesAsync();
                reply.Message = "Successfully deleted";
                reply.Success = true;
            }
            catch (Exception ex)
            {
                WriteException(reply, ex);
            }

            return reply;
        }

        /// <summary>
        /// Edit proba
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>ProbaActionReply</returns>
        public async override Task<ProbaActionReply> EditProba(ProbaMessage request, ServerCallContext context)
        {
            var reply = new ProbaActionReply();
            using var db = new ProbaContext();

            try
            {
                var existing = await db.Probas.FindAsync(request.Greeting);
                
                if (existing != null)
                {
                    existing.Date = request.Date;
                    existing.Count = request.Count;
                    existing.Added = request.Added;
                    db.Probas.Update(existing);
                    await db.SaveChangesAsync();

                    reply.Message = "Successfully edited";
                    reply.Success = true;
                }
                else
                {
                    reply.Message = "Unable to find the item for editing";
                    reply.Success = false;
                }
            }
            catch (Exception ex)
            {
                WriteException(reply, ex);
            }

            return reply;
        }

        private static void WriteException(ProbaActionReply reply, Exception ex)
        {
            reply.Message = $"Error occured {ex}";
            reply.Success = false;
        }
    }
}
