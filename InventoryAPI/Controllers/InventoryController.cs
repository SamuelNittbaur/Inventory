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
    /// Controller für die Verwaltung von Inventar-Daten.
    /// </summary>
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("inventory")]
    public class InventoryController : ControllerBase
    {
        /// <summary>
        /// Fügt ein neues Inventar-Item hinzu.
        /// </summary>
        /// <param name="request">Das Inventar-Item als verschlüsselte Anfrage.</param>
        /// <returns>Ein verschlüsseltes Antwortobjekt mit dem Ergebnis der Inventar-Hinzufügung.</returns>
        [Authorize]
        [HttpPost("insertNewItem")]
        public async Task<CryptionElement> InsertNewItem(CryptionElement request)
        {
            // Entschlüsselt die Anfrage
            InventoryRequest inventoryRequest = JsonConvert.DeserializeObject<InventoryRequest>(
                BusinessLogic.Cryption.CryptionLogic.Decrypt(request.data, BusinessLogic.Cryption.CryptionMethod.data)
            );
            // Füge das Inventar-Item hinzu und hole die Antwort
            var response = await BusinessLogic.InventoryLogic.InventoryLogic.InsertNewItem(inventoryRequest);
            // Rückgabe der verschlüsselten Antwort
            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }

        /// <summary>
        /// Holt die Inventar-Daten.
        /// </summary>
        /// <param name="request">Die Anfrage mit den Filterkriterien als verschlüsselte Daten.</param>
        /// <returns>Ein verschlüsseltes Antwortobjekt mit den abgerufenen Inventar-Daten.</returns>
        [Authorize]
        [HttpPost("getInventory")]
        public async Task<CryptionElement> GetItems(CryptionElement request)
        {
            // Entschlüsselt die Anfrage
            GeneralRequest generalRequest = JsonConvert.DeserializeObject<GeneralRequest>(
                BusinessLogic.Cryption.CryptionLogic.Decrypt(request.data, BusinessLogic.Cryption.CryptionMethod.data)
            );
            // Hole die Inventar-Daten basierend auf der Anfrage
            var response = await BusinessLogic.InventoryLogic.InventoryLogic.GetItems(generalRequest);
            // Rückgabe der verschlüsselten Antwort
            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }
    }
}
