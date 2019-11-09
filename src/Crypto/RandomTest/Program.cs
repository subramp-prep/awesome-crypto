using System;

namespace RandomTest
{
    static class Helper
    {
        public static byte[] ToByteArray(this string str)
        {
            return System.Text.Encoding.ASCII.GetBytes(str);
        }

        public static string ToStringArray(this byte[] bytes)
        {
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var encoded = Base64Encode("Prasad".ToByteArray());
            var decoded = Base64Decode(encoded).ToStringArray();
            
            Console.WriteLine("Hello World!");
        }


        /// <summary>
        /// Convert byte array to Base64 encoded text
        /// </summary>
        /// <returns>The encode.</returns>
        /// <param name="plainTextBytes">Plain text bytes.</param>
        private static string Base64Encode(byte[] plainTextBytes)
        {
            return System.Convert.ToBase64String(plainTextBytes);
        }

        /// <summary>
        /// Convert Base64 ecoded text to byte array
        /// </summary>
        /// <returns>The decode.</returns>
        /// <param name="base64EncodedData">Base64 encoded data.</param>
        private static byte[] Base64Decode(string base64EncodedData)
        {
            return System.Convert.FromBase64String(base64EncodedData);
        }
    }
}
