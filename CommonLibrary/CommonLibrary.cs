using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public static class Base64
    {
        public static string DecodeBase64(string val)
        {
            return Uri.UnescapeDataString(Encoding.UTF8.GetString(DecodeBase64ToBytes(val)));
        }
        private static byte[] DecodeBase64ToBytes(string val)
        {
            var data = val.PadRight(val.Length + (4 - val.Length % 4) % 4, '=');
            return Convert.FromBase64String(data);
        }
    }
    public class UriQueryDict : Dictionary<string, string>
    {
        public UriQueryDict(string queryString)
        {
            if (queryString.IndexOf('?') == 0) 
            {
                queryString = queryString.Substring(1);
            }
            if(queryString == "")
            {
                return;
            }
            var parameters = queryString.Split('&',';');
            foreach(var param in parameters)
            {
                var dict = param.Split('=');
                this.Add(dict[0], dict[1]);
            }
        }
    }
}
