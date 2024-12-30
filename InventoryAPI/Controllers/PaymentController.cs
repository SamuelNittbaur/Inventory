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
using Microsoft.Extensions.Logging;
using Stripe;
using BusinessLogic.Cryption;
using CryptoNet;
using DataBase;
using Firebase.Storage;
using Microsoft.Extensions.Logging;

namespace ShiftMuveApi.Controllers
{
    /// <summary>
    /// Controller für die Zahlungsabwicklung mit Stripe.
    /// </summary>
    [ApiController]
    [Route("payment")]
    public class PaymentController : ControllerBase
    {
        // Endpoint Secret für Stripe Webhooks (sollte nicht hartkodiert sein)
        private readonly string endpointSecret = Environment.GetEnvironmentVariable("stripeKey");

        /// <summary>
        /// Verarbeitet Stripe-Zahlungsereignisse und aktualisiert die Benutzerkonfiguration nach Zahlungseingang.
        /// </summary>
        /// <returns>Ein IActionResult, das den Status der Anfrage angibt (Ok oder BadRequest).</returns>
        [HttpPost("payment")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                // Verarbeitet das Stripe-Ereignis basierend auf der Webhook-Nachricht und der Signatur
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], endpointSecret);

                // Überprüft, ob das Ereignis eine abgeschlossene Checkout-Session ist
                if (stripeEvent.Type == Events.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    string decryptedValue = session.ClientReferenceId.Split("---").First();
                    string decryptedUserName = session.ClientReferenceId.Split("---")[1];

                    // Lädt die Benutzerkonfiguration basierend auf der Benutzer-ID und dem Benutzernamen
                    var configuraton = await BusinessLogic.UserLogic.UserLogic.GetUserConfiguration(new GeneralRequest()
                    {
                        userId = Guid.Parse(decryptedValue),
                        userName = decryptedUserName
                    });

                    // Aktualisiert die Konfiguration mit der Zahlungs-ID und dem Zahlungsstatus
                    configuraton.paymentId = session.PaymentIntentId;
                    configuraton.paid = true;

                    // Speichert die aktualisierte Benutzerkonfiguration
                    var response = await BusinessLogic.UserLogic.UserLogic.CreateUserConfiguration(configuraton);
                    if (response)
                        return Ok();
                    else
                        return BadRequest(response);
                }
                else
                {
                    // Falls ein unbekannter Ereignistyp empfangen wurde
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                // Fehlerbehandlung, wenn ein Stripe-Fehler auftritt
                return BadRequest();
            }
        }
    }
}
