namespace BusinessLogic.Shared
{
    /// <summary>
    /// Stellt ein Element für kryptografische Operationen dar.
    /// </summary>
    public record class CryptionElement
    {
        /// <summary>
        /// Ruft die Daten ab oder legt sie fest, die verschlüsselt oder entschlüsselt werden sollen.
        /// </summary>
        /// <remarks>
        /// Der Standardwert ist ein leerer String.
        /// </remarks>
        public string data { get; set; } = String.Empty;
    }
}
