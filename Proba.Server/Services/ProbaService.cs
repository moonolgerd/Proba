using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Greet;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Proba.Server
{
    public class ProbaService : ProbaServer.ProbaServerBase
    {
        private readonly ILogger<ProbaService> logger;
        private readonly SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the server key used to sign the JWT token is here, use more than 16 chars"));

        public ProbaService(ILogger<ProbaService> logger)
        {
            this.logger = logger;
        }

        [AllowAnonymous]
        public override Task<AuthorizeReply> Autorize(AuthorizeRequest request, ServerCallContext context)
        {
            var token = new JwtSecurityToken(
                expires: DateTime.Now.AddMinutes(10),
                claims: new Claim[] {
                    new Claim(ClaimTypes.Name, request.Name),
                    new Claim(ClaimTypes.Role, "User")
                },
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return Task.FromResult(new AuthorizeReply { Token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="responseStream"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [Authorize("User")]
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
        }

        /// <summary>
        /// Add proba
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns>ProbaActionReply</returns>
        [Authorize(Policy = "User")]
        public async override Task<ProbaActionReply> AddProba(ProbaMessage request, ServerCallContext context)
        {
            var reply = new ProbaActionReply();
            using var db = new ProbaContext();

            try
            {
                db.Probas.Add(request);
                await db.SaveChangesAsync();
                reply.Message = "Successfully added";

            }
            catch (Exception ex)
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
        [Authorize(Policy = "User")]
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
        [Authorize(Policy = "User")]
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

        private void WriteException(ProbaActionReply reply, Exception ex)
        {
            logger.LogError(ex, ex.Message);
            reply.Message = $"Error occured {ex}";
            reply.Success = false;
        }
    }
}
