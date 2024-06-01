using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MioSocks_GUI
{
    public static class Base64
    {
        public static string DecodeBase64(string val)
        {
            return Uri.UnescapeDataString(Encoding.UTF8.GetString(DecodeBase64ToBytes(val)));
        }
        internal static byte[] DecodeBase64ToBytes(string val)
        {
            var data = val.PadRight(val.Length + (4 - val.Length % 4) % 4, '=');
            return Convert.FromBase64String(data);
        }
    }
}
