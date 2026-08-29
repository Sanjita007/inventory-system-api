using Newtonsoft.Json;
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
            DRAFT = 1,
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

            var doc = JsonConvert.DeserializeXNode(obj.ToJson().ToUpper(), rootName)!;
            var declaration = doc.Declaration ?? _defaultDeclaration;
            return $"{declaration}{Environment.NewLine}{doc}";
        }

   

        public static string HashPassword(string password)
        {
            // "WorkFactor" 12 is a good balance between speed and security
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 12);
        }

        // 2. Verify the password (for Login)
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
