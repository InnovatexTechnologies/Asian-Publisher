using System.Security.Cryptography;
using System.Text;

namespace AsianPublisher
{
    public static class Utility
    {
        public static string ConnString = "Data Source=AppData/database.db; foreign keys= true;";
        //public static string salt = "A4476C2062FFA58980DC8F79EB6A799E";
        //public static string passphrase = "A4476C2062FFA58980DC8F79EB6A799E";
        //public static string salt1 = "75AEF0FA1B94B3C10D4F5B268F757F11";
        //public static string passphrase1 = "75AEF0FA1B94B3C10D4F5B268F757F11";
        //public static string merchId = "8952";
        //public static string password = "Test@123";
        public static string salt = "B3DF7A7EE8F2CC44F427E2C401E77800";//"A4476C2062FFA58980DC8F79EB6A799E";
        public static string passphrase = "B3DF7A7EE8F2CC44F427E2C401E77800";//"A4476C2062FFA58980DC8F79EB6A799E";
        public static string salt1 = "5681B1EAAC5ED0FEF24D64EFF522FCE4";//"75AEF0FA1B94B3C10D4F5B268F757F11";
        public static string passphrase1 = "5681B1EAAC5ED0FEF24D64EFF522FCE4";//"75AEF0FA1B94B3C10D4F5B268F757F11";
        public static string merchId = "599215";//"8952";
        public static string password = "5b4ff2b6";//"Test@123";
        public static string userId = "";
        public static string response = "";
        //public static string ReactSuccessurl = "http://localhost:3000/response";
        public static string ReactSuccessurl = "https://asianpublisher.in/response";


        public enum FillStyle
        {
            Basic,
            AllProperties,
            WithBasicNav,
            WithFullNav,
            Custom
        }
        public static String Encrypt(String plainText, String passphrase, String salt, Byte[] iv, int iterations)
        {
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            string data = ByteArrayToHexString(Encrypt(plainBytes, GetSymmetricAlgorithm(passphrase, salt, iv, iterations))).ToUpper();


            return data;
        }
        public static String decrypt(String plainText, String passphrase, String salt, Byte[] iv, int iterations)
        {
            byte[] str = HexStringToByte(plainText);

            string data1 = Encoding.UTF8.GetString(decrypt(str, GetSymmetricAlgorithm(passphrase, salt, iv, iterations)));
            return data1;
        }

        public static byte[] Encrypt(byte[] plainBytes, SymmetricAlgorithm sa)
        {
            return sa.CreateEncryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        }
        public static byte[] decrypt(byte[] plainBytes, SymmetricAlgorithm sa)
        {
            return sa.CreateDecryptor().TransformFinalBlock(plainBytes, 0, plainBytes.Length);
        }
        public static string GenerateRandomPassword(int length) 
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789@#$%=";
            var random = new Random();
            var passwordBuilder = new StringBuilder(length);

            for (int i = 0; i < length; i++)
            {  
                passwordBuilder.Append(chars[random.Next(chars.Length)]);
            }
            return passwordBuilder.ToString();
        }
        public static byte[] HexStringToByte(string hexString)
        {
            try
            {
                int bytesCount = (hexString.Length) / 2;
                byte[] bytes = new byte[bytesCount];
                for (int x = 0; x < bytesCount; ++x)
                {
                    bytes[x] = Convert.ToByte(hexString.Substring(x * 2, 2), 16);
                }
                return bytes;
            }
            catch
            {
                throw;
            }
        }
        public static SymmetricAlgorithm GetSymmetricAlgorithm(String passphrase, String salt, Byte[] iv, int iterations)
        {
            var saltBytes = new byte[16];
            var ivBytes = new byte[16];
            Rfc2898DeriveBytes rfcdb = new System.Security.Cryptography.Rfc2898DeriveBytes(passphrase, Encoding.UTF8.GetBytes(salt), iterations, HashAlgorithmName.SHA512);
            saltBytes = rfcdb.GetBytes(32);
            var tempBytes = iv;
            Array.Copy(tempBytes, ivBytes, Math.Min(ivBytes.Length, tempBytes.Length));
            var rij = new RijndaelManaged(); //SymmetricAlgorithm.Create();
            rij.Mode = CipherMode.CBC;
            rij.Padding = PaddingMode.PKCS7;
            rij.FeedbackSize = 128;
            rij.KeySize = 128;

            rij.BlockSize = 128;
            rij.Key = saltBytes;
            rij.IV = ivBytes;
            return rij;
        }
        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static object CreateDynamicObject(Dictionary<string, string> dictionary)
        {
            IDictionary<string, object> expando = new System.Dynamic.ExpandoObject();
            foreach (var entry in dictionary)
            {
                expando[entry.Key] = entry.Value;
            }
            return expando;
        }

        public static DateTime ToDate(this int date)
        {
            int d = date % 100;
            int m = (date / 100) % 100;
            int y = date / 10000;

            return new DateTime(y, m, d);
        }

        public static DateTime ToTime(this int date)
        {
            int SS = date % 100;
            int MM = (date / 100) % 100;
            int HH = date / 10000;

            return new DateTime(DateTime.Now.Year, 1, 1, HH, MM, SS);
        }
    }
}
