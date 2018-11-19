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

        /// <summary>
        /// Metodo para validar un determinado rut
        /// </summary>
        /// <param name="rut">Rut a validar</param>
        /// <returns>True/False dependiendo si esta correcto o no</returns>
        public static bool ValidarRut(string rut)
        {
            bool isValidRut = false;
            try
            {
                rut = rut.ToUpper();
                rut = rut.Replace(".", "");
                rut = rut.Replace("-", "");

                int rutAux = int.Parse(rut.Substring(0, rut.Length - 1));
                char dv = char.Parse(rut.Substring(rut.Length - 1, 1));
                int m = 0, s = 1;

                for (; rutAux != 0; rutAux /= 10)
                {
                    s = (s + rutAux % 10 * (9 - m++ % 6)) % 11;
                }
                if (dv == (char)(s != 0 ? s + 47 : 75))
                {
                    isValidRut = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al validar Rut", ex);
            }

            return isValidRut;
        }
    }
}
