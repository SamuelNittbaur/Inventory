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
    /// Controller zur Verwaltung von Benutzeroperationen, einschließlich Registrierung, Login und Konfiguration.
    /// </summary>
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Konstruktor des Controllers, der die Konfiguration injiziert.
        /// </summary>
        /// <param name="configuration">Die Konfiguration für das API.</param>
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generiert ein JWT-Token für einen Benutzer basierend auf seinem Benutzernamen und seiner ID.
        /// </summary>
        /// <param name="userName">Der Benutzername des Benutzers.</param>
        /// <param name="id">Die ID des Benutzers.</param>
        /// <returns>Ein JWT-Token als String.</returns>
        [HttpGet("getToken")]
        public string GetToken(string userName, Guid id)
        {
            var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Name, userName),
                };
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])
                        ),
                    SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        /// <summary>
        /// Authentifiziert einen Benutzer und gibt ein JWT-Token zurück.
        /// </summary>
        /// <param name="request">Das verschlüsselte Login-Request-Objekt.</param>
        /// <returns>Ein verschlüsseltes Response-Objekt mit der Benutzer-ID und dem JWT-Token.</returns>
        [HttpPost("login")]
        public async Task<CryptionElement> Login(CryptionElement request)
        {
            var loginRequest = JsonConvert.DeserializeObject<BusinessLogic.Shared.LoginRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(request.data, BusinessLogic.Cryption.CryptionMethod.data));
            var loginSuccess = await BusinessLogic.UserLogic.UserLogic.Login(loginRequest);
            var token = GetToken(loginRequest.userName, loginSuccess);

            var response = new Loginresponse()
            {
                userId = loginSuccess,
                jwtToken = token
            };

            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }

        /// <summary>
        /// Gibt die Benutzerkonfiguration für einen authentifizierten Benutzer zurück.
        /// </summary>
        /// <param name="data">Das verschlüsselte Request-Objekt mit Benutzerinformationen.</param>
        /// <returns>Ein verschlüsseltes Response-Objekt mit der Benutzerkonfiguration.</returns>
        [Authorize]
        [HttpPost("getUserConfiguration")]
        public async Task<CryptionElement> GetUserConfiguration(CryptionElement data)
        {
            var request = JsonConvert.DeserializeObject<GeneralRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(data.data, BusinessLogic.Cryption.CryptionMethod.data));
            var configuration = await BusinessLogic.UserLogic.UserLogic.GetUserConfiguration(request);

            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(configuration), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }

        /// <summary>
        /// Ändert das Passwort eines Benutzers.
        /// </summary>
        /// <param name="data">Das verschlüsselte Request-Objekt mit den neuen Passwortinformationen.</param>
        /// <returns>Ein verschlüsseltes Response-Objekt mit dem Erfolg der Passwortänderung.</returns>
        [HttpPost("changePassword")]
        public async Task<CryptionElement> ChangePassword(CryptionElement data)
        {
            var request = JsonConvert.DeserializeObject<ChangePasswordRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(data.data, BusinessLogic.Cryption.CryptionMethod.data));
            var configuration = await BusinessLogic.UserLogic.UserLogic.ChangePassword(request);

            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(configuration), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }

        /// <summary>
        /// Erstellt die Benutzerkonfiguration für einen neuen Benutzer.
        /// </summary>
        /// <param name="data">Das verschlüsselte Request-Objekt mit den Benutzerkonfigurationsdaten.</param>
        /// <returns>Ein verschlüsseltes Response-Objekt mit dem Erfolg der Erstellung der Benutzerkonfiguration.</returns>
        [Authorize]
        [HttpPost("createUserConfiguration")]
        public async Task<CryptionElement> CreateUserConfiguration(CryptionElement data)
        {
            var configuration = JsonConvert.DeserializeObject<UserConfiguration>(BusinessLogic.Cryption.CryptionLogic.Decrypt(data.data, BusinessLogic.Cryption.CryptionMethod.data));
            var response = await BusinessLogic.UserLogic.UserLogic.CreateUserConfiguration(configuration);

            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }

        /// <summary>
        /// Setzt das Passwort eines Benutzers zurück.
        /// </summary>
        /// <param name="data">Das verschlüsselte Request-Objekt mit den Rücksetzungsdaten.</param>
        /// <returns>Ein verschlüsseltes Response-Objekt mit dem Erfolg der Passwortzurücksetzung.</returns>
        [HttpPost("resetPassword")]
        public async Task<CryptionElement> ResetPassword(CryptionElement data)
        {
            var request = JsonConvert.DeserializeObject<BusinessLogic.Shared.ResetPasswordRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(data.data, BusinessLogic.Cryption.CryptionMethod.data));
            var response = await BusinessLogic.UserLogic.UserLogic.ResetPassword(request);

            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }

        /// <summary>
        /// Sendet eine E-Mail zum Zurücksetzen des Passworts.
        /// </summary>
        /// <param name="data">Das verschlüsselte Request-Objekt mit den Benutzerinformationen.</param>
        /// <returns>Ein verschlüsseltes Response-Objekt mit dem Erfolg des E-Mail-Versands.</returns>
        [HttpPost("sendPasswordEmail")]
        public async Task<CryptionElement> SendPasswordEmail(CryptionElement data)
        {
            var request = JsonConvert.DeserializeObject<GeneralRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(data.data, BusinessLogic.Cryption.CryptionMethod.data));
            var response = await BusinessLogic.UserLogic.UserLogic.SendChangeEmail(request);

            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }

        /// <summary>
        /// Holt das Token für einen Benutzer basierend auf den übermittelten Daten.
        /// </summary>
        /// <param name="data">Das verschlüsselte Request-Objekt mit den Benutzerinformationen.</param>
        /// <returns>Ein verschlüsseltes Response-Objekt mit dem JWT-Token.</returns>
        [HttpPost("ksdjhfpiu")]
        public async Task<CryptionElement> GetToken(CryptionElement data)
        {
            var request = JsonConvert.DeserializeObject<BusinessLogic.Shared.GeneralRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(data.data, BusinessLogic.Cryption.CryptionMethod.data));

            var response = await BusinessLogic.UserLogic.UserLogic.GetUserConfiguration(request);
            if (response.id != Guid.Empty)
            {
                var token = GetToken(request.userName, request.userId);
                return new CryptionElement()
                {
                    data = BusinessLogic.Cryption.CryptionLogic.Encrypt(token, BusinessLogic.Cryption.CryptionMethod.data)
                };
            }
            else
            {
                return new CryptionElement();
            }
        }

        /// <summary>
        /// Registriert einen neuen Benutzer und gibt ein JWT-Token zurück.
        /// </summary>
        /// <param name="data">Das verschlüsselte Request-Objekt mit den Registrierungsdaten.</param>
        /// <returns>Ein verschlüsseltes Response-Objekt mit der Benutzer-ID und dem JWT-Token.</returns>
        [HttpPost("register")]
        public async Task<CryptionElement> Register(CryptionElement data)
        {
            var register = JsonConvert.DeserializeObject<BusinessLogic.Shared.RegisterRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(data.data, BusinessLogic.Cryption.CryptionMethod.data));

            var response = await BusinessLogic.UserLogic.UserLogic.Register(register);
            if (response.userId != Guid.Empty)
            {
                var token = GetToken(register.userName, register.userId);
                response.jwtToken = token;
            }

            string responseData = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data);

            return new CryptionElement()
            {
                data = responseData
            };
        }
    }
}
