namespace BusinessLogic.Shared
{
    /// <summary>
    /// Repräsentiert die Konfiguration eines Benutzers mit persönlichen und geschäftlichen Informationen.
    /// </summary>
    public record class UserConfiguration
    {
        /// <summary>
        /// Ruft die eindeutige ID des Benutzers ab oder legt sie fest.
        /// </summary>
        public Guid id { get; set; } = Guid.Empty;

        /// <summary>
        /// Ruft den Vornamen des Benutzers ab oder legt ihn fest.
        /// </summary>
        public string firstName { get; set; } = String.Empty;

        /// <summary>
        /// Ruft den Nachnamen des Benutzers ab oder legt ihn fest.
        /// </summary>
        public string lastName { get; set; } = String.Empty;

        /// <summary>
        /// Ruft die E-Mail-Adresse des Benutzers ab oder legt sie fest.
        /// </summary>
        public string email { get; set; } = string.Empty;

        /// <summary>
        /// Ruft den Benutzernamen ab oder legt ihn fest.
        /// </summary>
        public string userName { get; set; } = string.Empty;

        /// <summary>
        /// Ruft die Zahlungs-ID des Benutzers ab oder legt sie fest.
        /// </summary>
        public string paymentId { get; set; } = string.Empty;

        /// <summary>
        /// Ruft die Kategorien ab, denen der Benutzer zugeordnet ist, oder legt sie fest.
        /// </summary>
        public List<string> categories { get; set; } = new List<string>();

        /// <summary>
        /// Ruft den Namen des Unternehmens des Benutzers ab oder legt ihn fest.
        /// </summary>
        public string company { get; set; } = string.Empty;

        /// <summary>
        /// Gibt an, ob der Benutzer bezahlt hat, oder legt diesen Wert fest.
        /// </summary>
        public bool paid { get; set; } = false;

        /// <summary>
        /// Ruft das Beitrittsdatum des Benutzers ab oder legt es fest.
        /// </summary>
        public DateTime joinDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Ruft das Datum der letzten Passwortänderung des Benutzers ab oder legt es fest.
        /// </summary>
        public DateTime lastPasswordChange { get; set; } = DateTime.MinValue;
    }
}
