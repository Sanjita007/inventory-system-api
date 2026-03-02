using Newtonsoft.Json;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace inventory_system_api.Shared
{
    public static class Utility
    {
        public static string ToJson(this object obj) 
        {

            //// Convert object → JSON
            string json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            // this is just a workaround for now.. find a better way to convert object into json file
            //string json = obj.ToString();
            // If JSON does NOT start with "{", wrap it in one
            if (!json.TrimStart().StartsWith("{"))
            {
                json = "{ \"DETAILS\": " + json + " }";
            }

            return json;

        }

        // return image file as base64 to send to the frontend through api
        public static string ToBase64(this byte[] imageStr)
        {
            if (imageStr == null) return "";

            string imgStr = Convert.ToBase64String(imageStr);
           return string.Join(',', "data:image/jpeg;base64", imgStr);

        }

        // return image file from base64 to send to the database to save

        public static byte[] FromBase64(this string imageStr)
        {
            return Convert.FromBase64String(imageStr.Replace("data:image/jpeg;base64,", ""));

        }

        public enum Status
        {
            DRAFT=1,
            UNPAID,
            PAID
        }

        private static readonly XDeclaration _defaultDeclaration = new("1.0", null, null);

        //public static string ToXml(this object obj)
        //{
        //    var doc = JsonConvert.DeserializeXNode(obj.ToJson(rootName))!;

        //    var declaration = doc.Declaration ?? _defaultDeclaration;

        //    return $"{declaration}{Environment.NewLine}{doc}";
        //}

        public static string ToXml(this object obj, string rootName)
        {
            if (obj == null) return "";

            var doc = JsonConvert.DeserializeXNode(obj.ToJson(), rootName)!;
            var declaration = doc.Declaration ?? _defaultDeclaration;
            return $"{declaration}{Environment.NewLine}{doc}";
        }

        public class EncryptionHelper
        {
            private static readonly string Key = "mN4kQ8rTzV2xY6pLwS9bE1dUoF3bH7xX"; // 32 chars for AES-256
            private static readonly string IV = "s1b2c3d9e5f6g7h8"; // 16 chars for AES block size

            public static string Encrypt(string plainText)
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
                byte[] ivBytes = Encoding.UTF8.GetBytes(IV);
                byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = keyBytes;
                    aesAlg.IV = ivBytes;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                                csEncrypt.FlushFinalBlock();
                                return Convert.ToBase64String(msEncrypt.ToArray());
                            }
                        }
                    }
                }
            }

            public static string Decrypt(string cipherText)
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(Key);
                byte[] ivBytes = Encoding.UTF8.GetBytes(IV);
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = keyBytes;
                    aesAlg.IV = ivBytes;
                    aesAlg.Mode = CipherMode.CBC;
                    aesAlg.Padding = PaddingMode.PKCS7;

                    using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                    {
                        using (var msDecrypt = new MemoryStream(cipherBytes))
                        {
                            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (var srDecrypt = new StreamReader(csDecrypt))
                                {
                                    return srDecrypt.ReadToEnd();
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
