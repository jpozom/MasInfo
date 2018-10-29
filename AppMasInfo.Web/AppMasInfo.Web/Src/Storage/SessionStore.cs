using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AppMasInfo.Web.Src.Storage
{
    public class SessionStore
    {
        public static void Delete(string key)
        {
            HttpContext.Current.Session.Remove(key);
        }

        public static T Get<T>(string key)
        {
            return (T)HttpContext.Current.Session[key];
        }

        public static void Store<T>(string key, T value)
        {
            HttpContext.Current.Session[key] = value;
        }

        public static IList<T> GetList<T>(string key)
        {
            throw new NotImplementedException();
        }

    }
}