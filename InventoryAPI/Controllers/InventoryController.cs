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
    /// Controller f�r die Verwaltung von Inventar-Daten.
    /// </summary>
    [EnableCors("AllowAll")]
    [ApiController]
    [Route("inventory")]
    public class InventoryController : ControllerBase
    {
        /// <summary>
        /// F�gt ein neues Inventar-Item hinzu.
        /// </summary>
        /// <param name="request">Das Inventar-Item als verschl�sselte Anfrage.</param>
        /// <returns>Ein verschl�sseltes Antwortobjekt mit dem Ergebnis der Inventar-Hinzuf�gung.</returns>
        [Authorize]
        [HttpPost("insertNewItem")]
        public async Task<CryptionElement> InsertNewItem(CryptionElement request)
        {
            // Entschl�sselt die Anfrage
            InventoryRequest inventoryRequest = JsonConvert.DeserializeObject<InventoryRequest>(
                BusinessLogic.Cryption.CryptionLogic.Decrypt(request.data, BusinessLogic.Cryption.CryptionMethod.data)
            );
            // F�ge das Inventar-Item hinzu und hole die Antwort
            var response = await BusinessLogic.InventoryLogic.InventoryLogic.InsertNewItem(inventoryRequest);
            // R�ckgabe der verschl�sselten Antwort
            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }

        /// <summary>
        /// Holt die Inventar-Daten.
        /// </summary>
        /// <param name="request">Die Anfrage mit den Filterkriterien als verschl�sselte Daten.</param>
        /// <returns>Ein verschl�sseltes Antwortobjekt mit den abgerufenen Inventar-Daten.</returns>
        [Authorize]
        [HttpPost("getInventory")]
        public async Task<CryptionElement> GetItems(CryptionElement request)
        {
            // Entschl�sselt die Anfrage
            GeneralRequest generalRequest = JsonConvert.DeserializeObject<GeneralRequest>(
                BusinessLogic.Cryption.CryptionLogic.Decrypt(request.data, BusinessLogic.Cryption.CryptionMethod.data)
            );
            // Hole die Inventar-Daten basierend auf der Anfrage
            var response = await BusinessLogic.InventoryLogic.InventoryLogic.GetItems(generalRequest);
            // R�ckgabe der verschl�sselten Antwort
            return new CryptionElement()
            {
                data = BusinessLogic.Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(response), BusinessLogic.Cryption.CryptionMethod.data)
            };
        }
    }
}
