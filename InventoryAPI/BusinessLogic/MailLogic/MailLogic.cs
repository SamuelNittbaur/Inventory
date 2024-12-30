using System.Net.Mail;
using System.Net;

namespace BusinessLogic.MailLogic
{
    /// <summary>
    /// Stellt die Geschäftslogik für das Versenden von E-Mails bereit.
    /// </summary>
    public static class MailLogic
    {
        /// <summary>
        /// Sendet eine Test-E-Mail an eine vordefinierte Adresse.
        /// </summary>
        /// <returns>Gibt true zurück, wenn die E-Mail erfolgreich gesendet wurde, andernfalls false.</returns>
        public static async Task<bool> SendEmail()
        {
            try
            {
                // Erstellt eine neue E-Mail-Nachricht
                MailMessage mail = new MailMessage
                {
                    From = new MailAddress(Environment.GetEnvironmentVariable("email")),
                    Subject = "Test Mail",
                    Body = "Das ist eine Test Mail",
                    IsBodyHtml = true // Auf true setzen, wenn der Nachrichtentext HTML enthält
                };

                // Empfänger hinzufügen
                mail.To.Add("samuel.nittbaur@gmail.com");

                // Konfiguration des SMTP-Clients
                SmtpClient smtpClient = new SmtpClient(Environment.GetEnvironmentVariable("emailServer"), 465)
                {
                    Credentials = new NetworkCredential(Environment.GetEnvironmentVariable("emailUserName"), Environment.GetEnvironmentVariable("emailPassword")),
                    EnableSsl = true
                };

                // E-Mail senden
                smtpClient.Send(mail);

                Console.WriteLine("Email sent successfully!");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");

                return false;
            }
        }
    }
}
