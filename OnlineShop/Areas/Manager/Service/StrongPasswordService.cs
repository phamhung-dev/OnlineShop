using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace OnlineShop.Areas.Manager.Service
{
    public class StrongPasswordService
    {
        static RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        public static string GeneratePassword(int passwordLength)
        {
            string CapitalLetters = "QWERTYUIOPASDFGHJKLZXCVBNM";
            string SmallLetters = "qwertyuiopasdfghjklzxcvbnm";
            string Digits = "0123456789";
            string SpecialCharacters = "!@#$%^&*()-_=+<,>.";
            string AllChar = CapitalLetters + SmallLetters + Digits + SpecialCharacters;
            StringBuilder sb = new StringBuilder();
            for (int n = 0; n < passwordLength; n++)
            {
                sb = sb.Append(GenerateChar(AllChar));
            }
            return sb.ToString();
        }
        private static char GenerateChar(string availableChars)
        {
            var byteArray = new byte[1];
            char c;
            do
            {
                provider.GetBytes(byteArray);
                c = (char)byteArray[0];

            } while (!availableChars.Any(x => x == c));

            return c;
        }
    }
}