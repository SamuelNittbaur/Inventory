using System.Numerics;

namespace Logic.UserLogic
{
    public static class UserTools
    {
        public static List<string> passwordStrengthColor = new List<string>()
        {
            "#eb4034",
            "#eb8634",
            "#e6b539",
            "#c3e639",
            "#76e639",
            "#00ff26"
        };
        public static int CalculatePasswordStrength(string password, double guessesPerSecond)
        {
            int charsetSize = GetCharacterSetSize(password);

            // Use BigInteger for total combinations
            BigInteger totalCombinations = BigInteger.Pow(charsetSize, password.Length);
            BigInteger guessesPerSecondBigInt = new BigInteger(guessesPerSecond);

            // Avoid division by zero
            BigInteger timeToCrack = totalCombinations / guessesPerSecondBigInt;

            // Convert crack time to double for scoring
            double timeToCrackInSeconds = (double)timeToCrack;

            return NormalizeScore(timeToCrackInSeconds);
        }

        public static int GetCharacterSetSize(string password)
        {
            bool hasLower = false, hasUpper = false, hasDigits = false, hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsLower(c)) hasLower = true;
                else if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsDigit(c)) hasDigits = true;
                else hasSpecial = true;
            }

            int size = 0;
            if (hasLower) size += 26; // Lowercase letters
            if (hasUpper) size += 26; // Uppercase letters
            if (hasDigits) size += 10; // Digits
            if (hasSpecial) size += 32; // Special characters

            return size;
        }

        public static int NormalizeScore(double timeToCrack)
        {
            // Define thresholds for scoring
            if (timeToCrack < 1) return 0;           // Less than 1 second
            if (timeToCrack < 60) return 1;          // Less than 1 minute
            if (timeToCrack < 3600) return 2;        // Less than 1 hour
            if (timeToCrack < 86400) return 3;       // Less than 1 day
            if (timeToCrack < 31556952) return 4;    // Less than 1 year
            return 5;                                // More than 1 year
        }

        public static List<string> commonPasswords = new List<string>
        {
            "123456", "password", "123456789", "12345", "12345678", "qwerty", "abc123", "111111", "123123", "admin",
            "letmein", "welcome", "monkey", "1234", "password1", "qwerty123", "123qwe", "1q2w3e4r", "123", "1111",
            "sunshine", "iloveyou", "qwertyuiop", "password123", "admin123", "123321", "password1", "iloveu", "abc12345",
            "qwert", "123qwe123", "qwertyuiop123", "123qwert", "1234qwerty", "princess", "qwerty12345", "hello", "123abc",
            "welcome1", "admin1", "trustno1", "letmein123", "superman", "qwerty1", "asdfgh", "123asd", "qwerty456",
            "iloveyou1", "master", "shadow", "qwerty1234", "batman", "dragon", "123123123", "iloveyou123", "1qaz2wsx",
            "qazwsx", "sunshine1", "baseball", "football", "monkey1", "letmein1", "passw0rd", "trustno123", "qwerty777",
            "password12", "1q2w3e4r5t", "password1234", "1234abcd", "11111111", "qwerty11", "letmein1234", "dragon123",
            "1234567", "superstar", "123qwertyuiop", "master123", "qwertyqwerty", "password12345", "123qwerty1", "password321",
            "welcome123", "1password", "lovers", "monkey123", "abc123456", "qwertyu", "love123", "sunshine123", "dolphin",
            "princess123", "qwertyui", "123p@ssw0rd", "football1", "flower", "qwerty333", "chocolate", "qwert12345", "12345abc",
            "password99", "iloveyou1234", "qwerty444", "dragon1234", "abcdefg", "hannah", "jessica", "password1q",
            "iloveit", "loveme", "dragonfly", "qwerty111", "123321123", "1q2w3e4r5", "babe", "123abc456", "passwordq",
            "1q2w3e4r5t6y", "password12345", "iloveyou2", "qwertytop", "qwertypp", "robin", "qwertyyy", "abc123qwerty", 
            "123456", "password", "123456789", "12345", "1234", "qwertz", "abc123", "iloveyou", "adidas", "zxcvbnm",
            "123123", "qwerty", "123", "123321", "1q2w3e4r", "111111", "sunshine", "qwerty123", "welcome", "1qaz2wsx",
            "football", "monkey", "letmein", "123qwe", "123abc", "superman", "1qazxsw2", "princess", "qazwsx", "trustno1",
            "password1", "1234qwer", "dragon", "1231", "1111", "iloveyou1", "qwertyuiop", "abcdef", "master", "qwerty1",
            "123123123", "1qaz", "sunshine1", "shadow", "qwertyuiop123", "welcome123", "letmein123", "11111111", "123qwerty",
            "monkey1", "1q2w3e4r5t", "hello123", "abc12345", "princess1", "qwertz123", "1234abcd", "iloveu", "asdfghjkl",
            "qwerty1234", "admin", "letmein1", "1q2w3e", "loveyou", "jordan", "12345qwerty", "qwerty1q", "password123",
            "jessica", "123abc123", "qwertyuiop1", "iloveyou123", "qwertz1234", "samsung", "123qwe123", "michael", "passw0rd",
            "dragon123", "1q2w3e4r5t6y", "test", "12345abcde", "welcome1", "password01", "1qaz2wsx3e", "12345qwertyuiop", "secret",
            "12345678", "sunshine123", "qwerty789", "qwertyui", "123abc1234", "letmein1234", "password2", "computer", "welcome1q",
            "football1", "superman1", "password12", "112233", "abc1234", "12345abc", "12345hello", "hello12345", "qwerty456",
            "iloveyou1234", "trustno1q", "abc12345q", "letmein12345"
        };
    }
}
