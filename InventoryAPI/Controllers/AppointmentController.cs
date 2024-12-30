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
using System.Net;

namespace ShiftMuveApi.Controllers
{
    /// <summary>
    /// Controller für die Verwaltung von Terminen.
    /// Alle Endpunkte sind durch Authentifizierung geschützt.
    /// </summary>
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("appointment")]
    public class AppointmentController : ControllerBase
    {
        /// <summary>
        /// Fügt einen neuen Termin ein.
        /// </summary>
        /// <param name="request">Das verschlüsselte Request-Objekt mit den Termin-Daten.</param>
        /// <returns>Das verschlüsselte Antwort-Objekt mit dem Erfolg der Operation.</returns>
        [Authorize]
        [HttpPost("insertNewItem")]
        public async Task<CryptionElement> InsertNewItem(CryptionElement request)
        {
            // Dekodiert und deserialisiert die Anfrage.
            AppointmentRequest inventoryRequest = JsonConvert.DeserializeObject<AppointmentRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(request.data, BusinessLogic.Cryption.CryptionMethod.data));

            // Ruft die Logik auf, um den neuen Termin zu erstellen.
            var response = await BusinessLogic.AppointmentLogic.AppointmentLogic.InsertNewItem(inventoryRequest);

            // Verschlüsselt und gibt die Antwort zurück.
            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }

        /// <summary>
        /// Ruft die bestehenden Termine ab.
        /// </summary>
        /// <param name="request">Das verschlüsselte Request-Objekt mit den allgemeinen Anfragedaten.</param>
        /// <returns>Das verschlüsselte Antwort-Objekt mit der Liste der Termine.</returns>
        [Authorize]
        [HttpPost("getAppointment")]
        public async Task<CryptionElement> GetItems(CryptionElement request)
        {
            // Dekodiert und deserialisiert die Anfrage.
            GeneralRequest generalRequest = JsonConvert.DeserializeObject<GeneralRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(request.data, BusinessLogic.Cryption.CryptionMethod.data));

            // Ruft die Logik auf, um die Termine abzurufen.
            var response = await BusinessLogic.AppointmentLogic.AppointmentLogic.GetItems(generalRequest);

            // Verschlüsselt und gibt die Antwort zurück.
            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }

        /// <summary>
        /// Löscht einen bestehenden Termin.
        /// </summary>
        /// <param name="request">Das verschlüsselte Request-Objekt mit den zu löschenden Termin-Daten.</param>
        /// <returns>Das verschlüsselte Antwort-Objekt mit dem Erfolg der Löschung.</returns>
        [Authorize]
        [HttpPost("deleteAppointment")]
        public async Task<CryptionElement> DeleteAppointment(CryptionElement request)
        {
            // Dekodiert und deserialisiert die Anfrage.
            AppointmentRequest inventoryRequest = JsonConvert.DeserializeObject<AppointmentRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(request.data, BusinessLogic.Cryption.CryptionMethod.data));

            // Ruft die Logik auf, um den Termin zu löschen.
            var response = await BusinessLogic.AppointmentLogic.AppointmentLogic.DeleteItem(inventoryRequest);

            // Verschlüsselt und gibt die Antwort zurück.
            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }
    }
}
