using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Util
    {
        private static Util _instance;
        public static Util Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Util();
                }
                return _instance;
            }
        }

        private Util() { }

        public string FormatMoney(decimal money)
        {
            return money.ToString("N0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + ".000VND";
        }
    }
}
