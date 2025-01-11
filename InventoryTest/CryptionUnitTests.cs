using Logic.Cryption;

namespace InventoryTest
{
    public class UnitTest1
    {
        // Test f�r die Verschl�sselung und Entschl�sselung mit der Default-Methode
        [Fact]
        public void EncryptDecrypt_ShouldReturnSameString_WithDefaultKey()
        {
            // Arrange
            string input = "TestString";

            // Act
            string encrypted = CryptionLogic.Encrypt(input, CryptionMethod.data);
            string decrypted = CryptionLogic.Decrypt(encrypted, CryptionMethod.data);

            // Assert
            Assert.Equal(input, decrypted); // Die entschl�sselte Zeichenkette sollte der urspr�nglichen entsprechen
        }

        // Test f�r die Verschl�sselung und Entschl�sselung mit einem benutzerdefinierten Schl�ssel (secOne)
        [Fact]
        public void EncryptDecrypt_ShouldReturnSameString_WithSecOneKey()
        {
            // Arrange
            string input = "TestStringSecOne";

            // Act
            string encrypted = CryptionLogic.Encrypt(input, CryptionMethod.secOne);
            string decrypted = CryptionLogic.Decrypt(encrypted, CryptionMethod.secOne);

            // Assert
            Assert.Equal(input, decrypted); // Die entschl�sselte Zeichenkette sollte der urspr�nglichen entsprechen
        }

        // Test f�r die Verschl�sselung und Entschl�sselung mit einem benutzerdefinierten Schl�ssel (secTwo)
        [Fact]
        public void EncryptDecrypt_ShouldReturnSameString_WithSecTwoKey()
        {
            // Arrange
            string input = "TestStringSecTwo";

            // Act
            string encrypted = CryptionLogic.Encrypt(input, CryptionMethod.secTwo);
            string decrypted = CryptionLogic.Decrypt(encrypted, CryptionMethod.secTwo);

            // Assert
            Assert.Equal(input, decrypted); // Die entschl�sselte Zeichenkette sollte der urspr�nglichen entsprechen
        }

        // Test, dass die Verschl�sselung bei Null-Eingabe eine leere Zeichenkette zur�ckgibt
        [Fact]
        public void Encrypt_ShouldReturnEmpty_WhenInputIsNull()
        {
            // Act
            string encrypted = CryptionLogic.Encrypt(null, CryptionMethod.data);

            // Assert
            Assert.Equal(string.Empty, encrypted); // Verschl�sselte Eingabe sollte eine leere Zeichenkette sein
        }

        // Test, dass die Entschl�sselung bei Null-Eingabe eine leere Zeichenkette zur�ckgibt
        [Fact]
        public void Decrypt_ShouldReturnEmpty_WhenInputIsNull()
        {
            // Act
            string decrypted = CryptionLogic.Decrypt(null, CryptionMethod.data);

            // Assert
            Assert.Equal(string.Empty, decrypted); // Entschl�sselte Eingabe sollte eine leere Zeichenkette sein
        }

        // Test f�r die Methode CreateSecurityHeader
        [Fact]
        public void CreateSecurityHeader_ShouldReturnEncryptedHeader()
        {
            // Act
            var (headerKey, headerValue) = CryptionLogic.CreateSecurityHeader();

            // Assert
            Assert.NotNull(headerKey); // Der Header-Schl�ssel sollte nicht null sein
            Assert.NotNull(headerValue); // Der Header-Wert sollte nicht null sein
            Assert.NotEmpty(headerKey); // Der Header-Schl�ssel sollte nicht leer sein
            Assert.NotEmpty(headerValue); // Der Header-Wert sollte nicht leer sein
        }
    }
}