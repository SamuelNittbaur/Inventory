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
    /// Controller zur Überprüfung des Systemstatus.
    /// </summary>
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Gibt den Gesundheitsstatus des Systems zurück.
        /// </summary>
        /// <returns>Der String "healthy", der den gesunden Status des Systems anzeigt.</returns>
        [HttpGet("health")]
        public string GetHealth()
        {
            return "healthy";
        }
    }
}
