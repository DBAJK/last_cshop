using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Handler;

namespace WpfApp3.Model
{
    public class MyLocaleList : Notifier
    {

        public int complaints_key { get; set; } 
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public string cp_contents { get; set; }
        public string cp_region { get; set; }
        public string cp_date { get; set; }
        public string cp_state { get; set;}

        public string cpl_date { get;set; }
    }
}
