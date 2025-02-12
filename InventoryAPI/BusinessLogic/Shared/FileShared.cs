namespace BusinessLogic.Shared
{
    /// <summary>
    /// Repräsentiert eine Datei, die hochgeladen werden soll.
    /// </summary>
    public record class UploadFile
    {
        /// <summary>
        /// Ruft den Benutzernamen ab oder legt ihn fest, der die Datei hochlädt.
        /// </summary>
        /// <remarks>
        /// Der Standardwert ist ein leerer String.
        /// </remarks>
        public string userName { get; set; } = String.Empty;

        /// <summary>
        /// Ruft den Datenstrom der Datei ab oder legt ihn fest.
        /// </summary>
        /// <remarks>
        /// Der Standardwert ist ein leeres Array.
        /// </remarks>
        public byte[] fileStream { get; set; } = new byte[0];

        /// <summary>
        /// Ruft den Namen der Datei ab oder legt ihn fest.
        /// </summary>
        /// <remarks>
        /// Der Standardwert ist ein leerer String.
        /// </remarks>
        public string fileName { get; set; } = String.Empty;
    }

    /// <summary>
    /// Repräsentiert eine Anforderung zum Löschen einer Datei.
    /// </summary>
    public record class DeleteFileRequest
    {
        /// <summary>
        /// Ruft den Benutzernamen ab oder legt ihn fest, der die Datei löschen möchte.
        /// </summary>
        /// <remarks>
        /// Der Standardwert ist ein leerer String.
        /// </remarks>
        public string userName { get; set; } = String.Empty;

        /// <summary>
        /// Ruft den Namen der Datei ab oder legt ihn fest, die gelöscht werden soll.
        /// </summary>
        /// <remarks>
        /// Der Standardwert ist ein leerer String.
        /// </remarks>
        public string fileName { get; set; } = String.Empty;
    }
}
