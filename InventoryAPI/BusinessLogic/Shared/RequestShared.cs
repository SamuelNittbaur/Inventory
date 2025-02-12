namespace BusinessLogic.Shared
{
    /// <summary>
    /// Repräsentiert eine Anfrage zur Registrierung eines Benutzers.
    /// </summary>
    public record class RegisterRequest
    {
        /// <summary>
        /// Ruft die Konfiguration des Benutzers ab oder legt sie fest.
        /// </summary>
        public UserConfiguration configuration { get; set; } = new UserConfiguration();

        /// <summary>
        /// Ruft die eindeutige ID des Benutzers ab oder legt sie fest.
        /// </summary>
        public Guid userId { get; set; } = Guid.Empty;

        /// <summary>
        /// Ruft den Benutzernamen ab oder legt ihn fest.
        /// </summary>
        public String userName { get; set; } = String.Empty;

        /// <summary>
        /// Ruft das Passwort ab oder legt es fest.
        /// </summary>
        public String password { get; set; } = String.Empty;
    }

    /// <summary>
    /// Repräsentiert eine Login-Anfrage.
    /// </summary>
    public record class LoginRequest
    {
        /// <summary>
        /// Ruft den Benutzernamen ab oder legt ihn fest.
        /// </summary>
        public String userName { get; set; } = String.Empty;

        /// <summary>
        /// Ruft das Passwort ab oder legt es fest.
        /// </summary>
        public String password { get; set; } = String.Empty;
    }

    /// <summary>
    /// Repräsentiert eine allgemeine Anfrage mit Benutzer- und Unternehmensdaten.
    /// </summary>
    public record class GeneralRequest
    {
        /// <summary>
        /// Ruft die Benutzer-ID ab oder legt sie fest.
        /// </summary>
        public Guid userId { get; set; } = Guid.Empty;

        /// <summary>
        /// Ruft den Benutzernamen ab oder legt ihn fest.
        /// </summary>
        public String userName { get; set; } = String.Empty;

        /// <summary>
        /// Ruft den Namen des Unternehmens ab oder legt ihn fest.
        /// </summary>
        public String company { get; set; } = String.Empty;
    }

    /// <summary>
    /// Repräsentiert eine Anfrage zum Zurücksetzen des Passworts eines Benutzers.
    /// </summary>
    public record class ResetPasswordRequest
    {
        /// <summary>
        /// Ruft die Benutzer-ID ab oder legt sie fest.
        /// </summary>
        public Guid userId { get; set; } = Guid.Empty;

        /// <summary>
        /// Ruft den Benutzernamen ab oder legt ihn fest.
        /// </summary>
        public String userName { get; set; } = String.Empty;

        /// <summary>
        /// Ruft das neue Passwort ab oder legt es fest.
        /// </summary>
        public String password { get; set; } = String.Empty;
    }

    /// <summary>
    /// Repräsentiert eine Anfrage für einen Inventarartikel.
    /// </summary>
    public record class InventoryRequest
    {
        /// <summary>
        /// Ruft den Inventarartikel ab oder legt ihn fest.
        /// </summary>
        public InventoryItem item { get; set; } = new InventoryItem();

        /// <summary>
        /// Ruft den Namen des Unternehmens ab oder legt ihn fest.
        /// </summary>
        public String company { get; set; } = String.Empty;
    }

    /// <summary>
    /// Repräsentiert eine Anfrage für einen Termin.
    /// </summary>
    public record class AppointmentRequest
    {
        /// <summary>
        /// Ruft den Termin ab oder legt ihn fest.
        /// </summary>
        public Appointment item { get; set; } = new Appointment();

        /// <summary>
        /// Ruft den Namen des Unternehmens ab oder legt ihn fest.
        /// </summary>
        public String company { get; set; } = String.Empty;
    }

    /// <summary>
    /// Repräsentiert eine Datei-Anfrage.
    /// </summary>
    public record class FileRequest
    {
        /// <summary>
        /// Ruft den Dateidatenstrom ab oder legt ihn fest.
        /// </summary>
        public byte[] fileStream { get; set; } = new byte[0];

        /// <summary>
        /// Ruft den Namen der Datei ab oder legt ihn fest.
        /// </summary>
        public string fileName { get; set; } = String.Empty;

        /// <summary>
        /// Ruft den Namen des Unternehmens ab oder legt ihn fest.
        /// </summary>
        public string company { get; set; } = String.Empty;
    }

    /// <summary>
    /// Repräsentiert eine Anfrage zum Ändern des Passworts eines Benutzers.
    /// </summary>
    public record class ChangePasswordRequest
    {
        /// <summary>
        /// Ruft den Benutzernamen ab oder legt ihn fest.
        /// </summary>
        public string userName { get; set; } = String.Empty;

        /// <summary>
        /// Ruft das alte Passwort ab oder legt es fest.
        /// </summary>
        public string oldPassword { get; set; } = String.Empty;

        /// <summary>
        /// Ruft das neue Passwort ab oder legt es fest.
        /// </summary>
        public string newPassword { get; set; } = String.Empty;
    }
}
