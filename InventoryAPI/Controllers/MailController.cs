using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessLogic.Shared;
using Newtonsoft.Json;
using static Google.Rpc.Context.AttributeContext.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity.Data;

namespace ShiftMuveApi.Controllers
{
    /// <summary>
    /// Controller für das Senden von E-Mails.
    /// </summary>
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("mail")]
    public class MailController : ControllerBase
    {
        /// <summary>
        /// Sendet eine Test-E-Mail.
        /// </summary>
        /// <returns>True, wenn die E-Mail erfolgreich gesendet wurde, ansonsten false.</returns>
        [HttpGet("sendTestMail")]
        public async Task<bool> SendTestMail()
        {
            // Sendet eine Test-E-Mail und gibt das Ergebnis zurück
            return await BusinessLogic.MailLogic.MailLogic.SendEmail();
        }
    }
}
