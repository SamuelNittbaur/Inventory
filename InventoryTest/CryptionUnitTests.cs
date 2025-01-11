using Logic.Cryption;

namespace InventoryTest
{
    public class UnitTest1
    {
        // Test für die Verschlüsselung und Entschlüsselung mit der Default-Methode
        [Fact]
        public void EncryptDecrypt_ShouldReturnSameString_WithDefaultKey()
        {
            // Arrange
            string input = "TestString";

            // Act
            string encrypted = CryptionLogic.Encrypt(input, CryptionMethod.data);
            string decrypted = CryptionLogic.Decrypt(encrypted, CryptionMethod.data);

            // Assert
            Assert.Equal(input, decrypted); // Die entschlüsselte Zeichenkette sollte der ursprünglichen entsprechen
        }

        // Test für die Verschlüsselung und Entschlüsselung mit einem benutzerdefinierten Schlüssel (secOne)
        [Fact]
        public void EncryptDecrypt_ShouldReturnSameString_WithSecOneKey()
        {
            // Arrange
            string input = "TestStringSecOne";

            // Act
            string encrypted = CryptionLogic.Encrypt(input, CryptionMethod.secOne);
            string decrypted = CryptionLogic.Decrypt(encrypted, CryptionMethod.secOne);

            // Assert
            Assert.Equal(input, decrypted); // Die entschlüsselte Zeichenkette sollte der ursprünglichen entsprechen
        }

        // Test für die Verschlüsselung und Entschlüsselung mit einem benutzerdefinierten Schlüssel (secTwo)
        [Fact]
        public void EncryptDecrypt_ShouldReturnSameString_WithSecTwoKey()
        {
            // Arrange
            string input = "TestStringSecTwo";

            // Act
            string encrypted = CryptionLogic.Encrypt(input, CryptionMethod.secTwo);
            string decrypted = CryptionLogic.Decrypt(encrypted, CryptionMethod.secTwo);

            // Assert
            Assert.Equal(input, decrypted); // Die entschlüsselte Zeichenkette sollte der ursprünglichen entsprechen
        }

        // Test, dass die Verschlüsselung bei Null-Eingabe eine leere Zeichenkette zurückgibt
        [Fact]
        public void Encrypt_ShouldReturnEmpty_WhenInputIsNull()
        {
            // Act
            string encrypted = CryptionLogic.Encrypt(null, CryptionMethod.data);

            // Assert
            Assert.Equal(string.Empty, encrypted); // Verschlüsselte Eingabe sollte eine leere Zeichenkette sein
        }

        // Test, dass die Entschlüsselung bei Null-Eingabe eine leere Zeichenkette zurückgibt
        [Fact]
        public void Decrypt_ShouldReturnEmpty_WhenInputIsNull()
        {
            // Act
            string decrypted = CryptionLogic.Decrypt(null, CryptionMethod.data);

            // Assert
            Assert.Equal(string.Empty, decrypted); // Entschlüsselte Eingabe sollte eine leere Zeichenkette sein
        }

        // Test für die Methode CreateSecurityHeader
        [Fact]
        public void CreateSecurityHeader_ShouldReturnEncryptedHeader()
        {
            // Act
            var (headerKey, headerValue) = CryptionLogic.CreateSecurityHeader();

            // Assert
            Assert.NotNull(headerKey); // Der Header-Schlüssel sollte nicht null sein
            Assert.NotNull(headerValue); // Der Header-Wert sollte nicht null sein
            Assert.NotEmpty(headerKey); // Der Header-Schlüssel sollte nicht leer sein
            Assert.NotEmpty(headerValue); // Der Header-Wert sollte nicht leer sein
        }
    }
}