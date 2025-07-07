namespace ClientApplicationTestProject.Utilities
{
    public class TestDataGenerator
    {
        private static readonly Random _random = new Random();

        public static string GenerateRandomEmail()
        {
            long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            return $"testuser{timestamp}@example.com";
        }

        public static string GenerateRandomPassword()
        {
            const string lowercase = "abcdefghijklmnopqrstuvwxyz";
            const string uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            const string specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";
            const string allChars = lowercase + uppercase + numbers + specialChars;

            var password = new char[12];

            // Step 1: Fill guaranteed positions
            password[0] = lowercase[_random.Next(lowercase.Length)];  // e.g., 'a'
            password[1] = uppercase[_random.Next(uppercase.Length)];  // e.g., 'B'  
            password[2] = numbers[_random.Next(numbers.Length)];      // e.g., '3'
            password[3] = specialChars[_random.Next(specialChars.Length)]; // e.g., '@'

            // Now array looks like: ['a', 'B', '3', '@', '\0', '\0', '\0', '\0', '\0', '\0', '\0', '\0']

            // Step 2: Fill remaining 8 positions (index 4-11)
            for (int i = 4; i < 12; i++)
            {
                password[i] = allChars[_random.Next(allChars.Length)]; // Random chars from any category
            }
            // Now array might look like: ['a', 'B', '3', '@', 'x', 'Y', '7', '&', 'c', 'D', '9', '!']
            //                        Index:  0    1    2    3    4    5    6    7    8    9    10   11
            // Step 3: Shuffle the array to randomize positions
            // (This prevents the password from always starting with lowercase, uppercase, number, special)

            for (int i = password.Length - 1; i > 0; i--)  // Start from last index (11), go down to 1
            {
                int j = _random.Next(i + 1);                 // Pick random index from 0 to i (inclusive)
                (password[i], password[j]) = (password[j], password[i]);  // Swap characters
            }
            return new string(password);

        }
    }
}
