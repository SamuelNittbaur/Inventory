using BusinessLogic.Shared;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Data;
using System.Collections.Concurrent;
using System;
using System.Text;
using System.Net;
using System.Net.Mail;
using DataBase;
using Firebase.Auth;
namespace BusinessLogic.UserLogic
{
    /// <summary>
    /// Eine Klasse, die die Benutzerverwaltung mit Funktionen zum Abrufen von Konfigurationen, Zurücksetzen von Passwörtern und E-Mail-Benachrichtigungen unterstützt.
    /// </summary>
    public static class UserLogic
    {
        /// <summary>
        /// Ruft die Benutzerkonfiguration anhand des Benutzernamens und der Benutzer-ID ab.
        /// </summary>
        /// <param name="request">Ein Objekt, das die Benutzerinformationen enthält.</param>
        /// <returns>Die Benutzerkonfiguration für den angegebenen Benutzer.</returns>
        public static async Task<UserConfiguration> GetUserConfiguration(GeneralRequest request)
        {
            var configuration = await DataBase.UserDataBase.GetUserConfiguration(request.userName, request.userId);
            return configuration;
        }

        /// <summary>
        /// Setzt das Passwort eines Benutzers zurück, wenn die Benutzerinformationen gültig sind.
        /// </summary>
        /// <param name="request">Die Anforderungsinformationen zum Zurücksetzen des Passworts.</param>
        /// <returns>True, wenn das Passwort erfolgreich zurückgesetzt wurde, andernfalls false.</returns>
        public static async Task<bool> ResetPassword(ResetPasswordRequest request)
        {
            var password = await DataBase.UserDataBase.GetExistsFile(request.userName);
            if (password != String.Empty)
            {
                var changeResponse = await DataBase.UserDataBase.DeletePasswordFile(request.userName, password);
                if (changeResponse)
                {
                    var createNewPasswordResponse = await DataBase.UserDataBase.CreateNewUserPassword(request.userName, request.password, request.userId);
                    var createNewExists = await DataBase.UserDataBase.CreateExistFile(request.userName, request.password);
                    if (createNewPasswordResponse != Guid.Empty)
                    {
                        return true;
                    }
                    else return false;
                }
                else
                {
                    return false;
                }
            }
            else return false;
        }

        /// <summary>
        /// Sendet eine E-Mail zum Ändern des Passworts an den Benutzer.
        /// </summary>
        /// <param name="request">Die Anfrage mit den Benutzerinformationen.</param>
        /// <returns>True, wenn die E-Mail erfolgreich gesendet wurde, andernfalls false.</returns>
        public static async Task<bool> SendChangeEmail(GeneralRequest request)
        {
            var password = await DataBase.UserDataBase.GetExistsFile(request.userName);
            var userId = await DataBase.UserDataBase.LoadUserId(request.userName, password);
            var configuration = await UserDataBase.GetUserConfiguration(request.userName, userId);

            return await SendEmail(Environment.GetEnvironmentVariable("emailServer"), 587, Environment.GetEnvironmentVariable("email"), configuration.email, "Change Password", GetPasswordChangeEmail(request.userName, $"https://shelfwise-inventory.web.app/change-password/{userId}/{request.userName}", configuration.company, "info@sn-dev.de"), "info@sn-dev.de", Environment.GetEnvironmentVariable("emailPassword"));
        }

        /// <summary>
        /// Generiert die HTML-E-Mail-Vorlage für das Zurücksetzen des Passworts.
        /// </summary>
        /// <param name="userName">Der Benutzername des Empfängers.</param>
        /// <param name="passwordResetLink">Der Link zum Zurücksetzen des Passworts.</param>
        /// <param name="companyName">Der Name des Unternehmens.</param>
        /// <param name="supportLink">Der Link zur Support-Seite.</param>
        /// <returns>Die HTML-E-Mail als String.</returns>
        public static string GetPasswordChangeEmail(string userName, string passwordResetLink, string companyName, string supportLink)
        {
            return $@"
                <!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Password Change Request</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            margin: 0;
                            padding: 0;
                            background-color: #f4f4f4;
                        }}
                        .email-container {{
                            max-width: 600px;
                            margin: 0 auto;
                            background-color: #ffffff;
                            border-radius: 8px;
                            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                        }}
                        .email-header {{
                            background-color: #4CAF50;
                            color: white;
                            padding: 20px;
                            text-align: center;
                            border-top-left-radius: 8px;
                            border-top-right-radius: 8px;
                        }}
                        .email-body {{
                            padding: 20px;
                            color: #555555;
                            line-height: 1.6;
                        }}
                        .cta-button {{
                            display: inline-block;
                            background-color: #4CAF50;
                            color: white;
                            text-decoration: none;
                            padding: 15px 25px;
                            border-radius: 4px;
                            font-weight: bold;
                            text-align: center;
                            margin-top: 20px;
                        }}
                        .footer {{
                            text-align: center;
                            color: #999999;
                            font-size: 12px;
                            padding: 15px;
                        }}
                        .footer a {{
                            color: #4CAF50;
                            text-decoration: none;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""email-container"">
                        <div class=""email-header"">
                            <h2>Password Change Request</h2>
                        </div>
                        <div class=""email-body"">
                            <p>Hello {userName},</p>
                            <p>We received a request to change the password for your account. If you made this request, please click the button below to reset your password.</p>
                            <a href=""{passwordResetLink}"" class=""cta-button"">Reset My Password</a>
                            <p>If you did not request a password change, you can ignore this email. Your password will remain unchanged.</p>
                            <p>For any assistance, feel free to reach out to our support team.</p>
                        </div>
                        <div class=""footer"">
                            <p>Best regards, <br> The {companyName} Team</p>
                            <p>If you need help, visit our <a href=""{supportLink}"">support page</a>.</p>
                        </div>
                    </div>
                </body>
                </html>
                ";
        }

        /// <summary>
        /// Generiert ein zufälliges Passwort mit der angegebenen Länge.
        /// </summary>
        /// <param name="length">Die gewünschte Länge des generierten Passworts.</param>
        /// <returns>Das generierte Passwort als String.</returns>
        public static string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=<>?/";
            StringBuilder password = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < length; i++)
            {
                char randomChar = validChars[random.Next(validChars.Length)];
                password.Append(randomChar);
            }

            return password.ToString();
        }

        /// <summary>
        /// Sendet eine E-Mail an einen Empfänger.
        /// </summary>
        /// <param name="smtpServer">Der SMTP-Server, der verwendet wird.</param>
        /// <param name="smtpPort">Der Port des SMTP-Servers.</param>
        /// <param name="fromEmail">Die Absender-E-Mail-Adresse.</param>
        /// <param name="toEmail">Die Empfänger-E-Mail-Adresse.</param>
        /// <param name="subject">Der Betreff der E-Mail.</param>
        /// <param name="body">Der Body der E-Mail.</param>
        /// <param name="smtpUsername">Der Benutzername für den SMTP-Server.</param>
        /// <param name="smtpPassword">Das Passwort für den SMTP-Server.</param>
        /// <returns>True, wenn die E-Mail erfolgreich gesendet wurde, andernfalls false.</returns>
        public static async Task<bool> SendEmail(string smtpServer, int smtpPort, string fromEmail, string toEmail, string subject, string body, string smtpUsername, string smtpPassword)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                    smtpClient.EnableSsl = true;

                    MailMessage email = new MailMessage(fromEmail, toEmail, subject, body)
                    {
                        IsBodyHtml = true
                    };

                    smtpClient.Send(email);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Erstellt eine neue Benutzerkonfiguration in der Datenbank.
        /// </summary>
        /// <param name="configuration">Die Konfigurationsdaten des Benutzers.</param>
        /// <returns>True, wenn die Konfiguration erfolgreich erstellt wurde, andernfalls false.</returns>
        public static async Task<bool> CreateUserConfiguration(UserConfiguration configuration)
        {
            var response = await DataBase.UserDataBase.CreateUserConfiguration(configuration);
            return response;
        }

        /// <summary>
        /// Authentifiziert einen Benutzer anhand von Benutzernamen und Passwort.
        /// </summary>
        /// <param name="request">Die Login-Anfrage mit Benutzername und Passwort.</param>
        /// <returns>Die Benutzer-ID, wenn der Login erfolgreich war, andernfalls eine leere GUID.</returns>
        public static async Task<Guid> Login(LoginRequest request)
        {
            var userExists = await DataBase.UserDataBase.CheckIfUserExists(request.userName, request.password);
            if (userExists)
            {
                var userId = await DataBase.UserDataBase.LoadUserId(request.userName, request.password);
                if (userId != Guid.Empty)
                {
                    var userConfiguration = await DataBase.UserDataBase.GetUserConfiguration(request.userName, userId);
                    return userConfiguration.id;
                }
                else
                {
                    return Guid.Parse("00000000-0000-0000-0000-000000000002");
                }
            }
            else
            {
                return Guid.Parse("00000000-0000-0000-0000-000000000001");
            }
        }

        /// <summary>
        /// Ändert das Passwort eines Benutzers.
        /// </summary>
        /// <param name="request">Die Anforderung zum Ändern des Passworts.</param>
        /// <returns>True, wenn das Passwort erfolgreich geändert wurde, andernfalls false.</returns>
        public static async Task<bool> ChangePassword(ChangePasswordRequest request)
        {
            try
            {
                var loadUserId = await DataBase.UserDataBase.LoadUserId(request.userName, request.oldPassword);
                if (loadUserId != Guid.Empty)
                {
                    var createNewPassword = await DataBase.UserDataBase.CreateNewUserPassword(request.userName, request.newPassword, loadUserId);

                    if (createNewPassword != Guid.Empty)
                    {
                        var deleteOldPassword = await DataBase.UserDataBase.DeletePasswordFile(request.userName, request.oldPassword);
                        return deleteOldPassword;
                    }
                    else return false;
                }
                else return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Registriert einen neuen Benutzer.
        /// </summary>
        /// <param name="request">Die Anforderung zum Registrieren eines neuen Benutzers.</param>
        /// <returns>Eine Antwort, die den Status der Registrierung enthält.</returns>
        public static async Task<RegisterResponse> Register(RegisterRequest request)
        {
            try
            {
                var userAlreadyExists = await DataBase.UserDataBase.CheckIfUserExists(request.userName, request.password);
                if (userAlreadyExists)
                {
                    return new RegisterResponse();
                }
                else
                {
                    var createExistsFile = await DataBase.UserDataBase.CreateExistFile(request.userName, request.password);
                    var createPasswordFile = await DataBase.UserDataBase.CreateNewUserPassword(request.userName, request.password, request.userId);
                    if (createPasswordFile != Guid.Empty)
                    {
                        var createUserConfiguration = await DataBase.UserDataBase.CreateUserConfiguration(request.configuration);
                        if (createUserConfiguration)
                            return new RegisterResponse()
                            {
                                userId = createPasswordFile
                            };
                        else return new RegisterResponse();
                    }
                    else
                    {
                        return new RegisterResponse();
                    }
                }
            }
            catch (Exception)
            {
                return new RegisterResponse();
            }
        }
    }


    /// <summary>
    /// Eine Klasse, die Login-Versuche verfolgt und Benutzer bei zu vielen fehlgeschlagenen Versuchen blockiert.
    /// </summary>
    public static class LoginAttemptTracker
    {
        private static readonly ConcurrentDictionary<string, LoginAttempt> loginAttempts = new();
        private static readonly int maxAttempts = 5;
        private static readonly TimeSpan blockDuration = TimeSpan.FromMinutes(15);

        /// <summary>
        /// Überprüft, ob der Login für einen Benutzer blockiert ist.
        /// </summary>
        /// <param name="username">Der Benutzername des zu prüfenden Benutzers.</param>
        /// <returns>Gibt <c>true</c> zurück, wenn der Login blockiert ist, andernfalls <c>false</c>.</returns>
        public static bool IsLoginBlocked(string username)
        {
            if (loginAttempts.TryGetValue(username, out var attempt))
            {
                // Überprüfen, ob die Blockierungszeit noch aktiv ist
                if (attempt.blockedUntil > DateTime.UtcNow)
                    return true;

                // Falls die maximale Anzahl erreicht ist, Benutzer blockieren
                if (attempt.failedAttempts >= maxAttempts)
                {
                    attempt.blockedUntil = DateTime.UtcNow.Add(blockDuration);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Registriert einen fehlgeschlagenen Login-Versuch eines Benutzers.
        /// </summary>
        /// <param name="username">Der Benutzername des Benutzers, für den der fehlgeschlagene Versuch registriert wird.</param>
        public static void RegisterFailedAttempt(string username)
        {
            var attempt = loginAttempts.GetOrAdd(username, new LoginAttempt());
            lock (attempt) // Synchronisation sicherstellen
            {
                if (attempt.blockedUntil <= DateTime.UtcNow)
                {
                    attempt.failedAttempts++;
                    if (attempt.failedAttempts >= maxAttempts)
                    {
                        attempt.blockedUntil = DateTime.UtcNow.Add(blockDuration);
                    }
                }
            }
        }

        /// <summary>
        /// Registriert einen erfolgreichen Login und entfernt blockierte oder fehlgeschlagene Versuche.
        /// </summary>
        /// <param name="username">Der Benutzername des Benutzers, für den der erfolgreiche Login registriert wird.</param>
        public static void RegisterSuccessfulLogin(string username)
        {
            loginAttempts.TryRemove(username, out _);
        }

        /// <summary>
        /// Interne Klasse zur Speicherung von Informationen über Login-Versuche eines Benutzers.
        /// </summary>
        private class LoginAttempt
        {
            /// <summary>
            /// Die Anzahl fehlgeschlagener Login-Versuche.
            /// </summary>
            public int failedAttempts { get; set; }

            /// <summary>
            /// Das Datum und die Uhrzeit, bis zu der der Benutzer blockiert ist.
            /// </summary>
            public DateTime blockedUntil { get; set; } = DateTime.MinValue;
        }
    }

}