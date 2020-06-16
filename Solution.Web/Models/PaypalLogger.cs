using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Solution.Web.Models
{
    public class PaypalLogger
    {
        public static string LogDirectoryPath = HttpContext.Current.Server.MapPath("/Context/ghada");
        public static void Log(String messages)
        {
            try
            {
                 StreamWriter strw = new StreamWriter("C://Users//ahmed//Documents//Visual Studio 2015//Projects//Consomi//Consomi.Web//Content//ghada//PaypalError.log", true);
                strw.WriteLine("{0} --->{1}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), messages);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}