namespace BusinessLogic.Shared
{
    /// <summary>
    /// Gibt den Status eines Vorgangs zurück.
    /// </summary>
    public enum ReturnValue
    {
        /// <summary>
        /// Vorgang war erfolgreich.
        /// </summary>
        Success,

        /// <summary>
        /// Vorgang führte zu einer Warnung.
        /// </summary>
        Warning,

        /// <summary>
        /// Vorgang führte zu einem Fehler.
        /// </summary>
        Error
    }

    /// <summary>
    /// Repräsentiert die Antwort auf eine erfolgreiche Anmeldung.
    /// </summary>
    public class Loginresponse
    {
        /// <summary>
        /// Ruft die eindeutige ID des Benutzers ab oder legt sie fest.
        /// </summary>
        public Guid userId { get; set; } = Guid.Empty;

        /// <summary>
        /// Ruft das generierte JWT-Token für den Benutzer ab oder legt es fest.
        /// </summary>
        public string jwtToken { get; set; } = String.Empty;
    }

    /// <summary>
    /// Repräsentiert die Antwort auf eine erfolgreiche Registrierung.
    /// </summary>
    public class RegisterResponse
    {
        /// <summary>
        /// Ruft die eindeutige ID des registrierten Benutzers ab oder legt sie fest.
        /// </summary>
        public Guid userId { get; set; } = Guid.Empty;

        /// <summary>
        /// Ruft das generierte JWT-Token für den Benutzer ab oder legt es fest.
        /// </summary>
        public string jwtToken { get; set; } = String.Empty;
    }
}
