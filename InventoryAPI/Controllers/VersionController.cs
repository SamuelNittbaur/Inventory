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
    /// Controller zur Verwaltung der Versionsinformationen der API.
    /// </summary>
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("version")]
    public class VersionController : ControllerBase
    {
        /// <summary>
        /// Gibt die aktuelle Version der API zurück.
        /// </summary>
        /// <returns>Die Version als String.</returns>
        [HttpGet("getVersion")]
        public string GetVersion()
        {
            return "v1.5";
        }
    }
}
