using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Handler;

namespace WpfApp3
{
    public class MyLocale : Notifier
    {
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public MyLocale(string name, double lat, double lng)
        {
            Name = name;
            Lat = lat;
            Lng = lng;
        }

        public override string ToString()
        {
            return Name;
        }

    }
}
