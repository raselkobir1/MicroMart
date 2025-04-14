using FluentValidation.Results;
using System.Data;

namespace Cart.API.Helper
{
    public static class CommonMethods
    {
        private static readonly Random _random = new Random();
        private const string SpecialCharacters = "@#*";

        public static DateTime GetCurrentTime()
        {
            return DateTime.UtcNow.Add(TimeSpan.FromHours(6));
        }
        public static List<string> ConvertFluentErrorMessages(List<ValidationFailure> errors)
        {
            List<string> errorsMessages = new List<string>();
            foreach (var failure in errors)
            {
                errorsMessages.Add(failure.ErrorMessage);
            }
            return errorsMessages;
        }
        public static string GenerateRandomPassword()
        {
            string capitalLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string smallLetters = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "0123456789";

            // Select one character from each category
            char capitalLetter = capitalLetters[_random.Next(capitalLetters.Length)];
            char smallLetter = smallLetters[_random.Next(smallLetters.Length)];
            char number = numbers[_random.Next(numbers.Length)];
            char specialChar = SpecialCharacters[_random.Next(SpecialCharacters.Length)];

            // Combine the characters
            string passwordChars = new string(new[] { capitalLetter, smallLetter, number, specialChar });

            // Generate the remaining characters
            string remainingChars = GenerateRandomChars(5);

            // Shuffle all characters and return the final password
            string finalPassword = ShuffleString(passwordChars + remainingChars);
            return finalPassword;
        }
        private static string GenerateRandomChars(int length)
        {
            const string allCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#*";
            return new string(Enumerable.Repeat(allCharacters, length)
                .Select(s => s[_random.Next(s.Length)]).ToArray());
        }
        private static string ShuffleString(string str)
        {
            char[] array = str.ToCharArray();
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = _random.Next(n + 1);
                char value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
            return new string(array);
        }
    }
}
