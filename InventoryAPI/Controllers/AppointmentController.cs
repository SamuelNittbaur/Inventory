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
    /// Controller f�r die Verwaltung von Terminen.
    /// Alle Endpunkte sind durch Authentifizierung gesch�tzt.
    /// </summary>
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("appointment")]
    public class AppointmentController : ControllerBase
    {
        /// <summary>
        /// F�gt einen neuen Termin ein.
        /// </summary>
        /// <param name="request">Das verschl�sselte Request-Objekt mit den Termin-Daten.</param>
        /// <returns>Das verschl�sselte Antwort-Objekt mit dem Erfolg der Operation.</returns>
        [Authorize]
        [HttpPost("insertNewItem")]
        public async Task<CryptionElement> InsertNewItem(CryptionElement request)
        {
            // Dekodiert und deserialisiert die Anfrage.
            AppointmentRequest inventoryRequest = JsonConvert.DeserializeObject<AppointmentRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(request.data, BusinessLogic.Cryption.CryptionMethod.data));

            // Ruft die Logik auf, um den neuen Termin zu erstellen.
            var response = await BusinessLogic.AppointmentLogic.AppointmentLogic.InsertNewItem(inventoryRequest);

            // Verschl�sselt und gibt die Antwort zur�ck.
            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }

        /// <summary>
        /// Ruft die bestehenden Termine ab.
        /// </summary>
        /// <param name="request">Das verschl�sselte Request-Objekt mit den allgemeinen Anfragedaten.</param>
        /// <returns>Das verschl�sselte Antwort-Objekt mit der Liste der Termine.</returns>
        [Authorize]
        [HttpPost("getAppointment")]
        public async Task<CryptionElement> GetItems(CryptionElement request)
        {
            // Dekodiert und deserialisiert die Anfrage.
            GeneralRequest generalRequest = JsonConvert.DeserializeObject<GeneralRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(request.data, BusinessLogic.Cryption.CryptionMethod.data));

            // Ruft die Logik auf, um die Termine abzurufen.
            var response = await BusinessLogic.AppointmentLogic.AppointmentLogic.GetItems(generalRequest);

            // Verschl�sselt und gibt die Antwort zur�ck.
            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }

        /// <summary>
        /// L�scht einen bestehenden Termin.
        /// </summary>
        /// <param name="request">Das verschl�sselte Request-Objekt mit den zu l�schenden Termin-Daten.</param>
        /// <returns>Das verschl�sselte Antwort-Objekt mit dem Erfolg der L�schung.</returns>
        [Authorize]
        [HttpPost("deleteAppointment")]
        public async Task<CryptionElement> DeleteAppointment(CryptionElement request)
        {
            // Dekodiert und deserialisiert die Anfrage.
            AppointmentRequest inventoryRequest = JsonConvert.DeserializeObject<AppointmentRequest>(BusinessLogic.Cryption.CryptionLogic.Decrypt(request.data, BusinessLogic.Cryption.CryptionMethod.data));

            // Ruft die Logik auf, um den Termin zu l�schen.
            var response = await BusinessLogic.AppointmentLogic.AppointmentLogic.DeleteItem(inventoryRequest);

            // Verschl�sselt und gibt die Antwort zur�ck.
            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }
    }
}
