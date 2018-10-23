using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AppMasInfo.Utils.Utils
{
    public class GlobalMethods
    {
        /// <summary>
        /// Metodo para encriptar un password utilizando el algoritmo SHA-256
        /// </summary>
        /// <param name="p_Pass"></param>
        /// <returns></returns>
        public static string EncryptPass(string p_Pass)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(p_Pass);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;

            foreach (byte x in hash)
                hashString += String.Format("{0:x2}", x);

            return hashString.ToUpper();
        }
    }
}
