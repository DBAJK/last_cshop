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

        public string complaints_key { get; set; } 
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public string cp_contents { get; set; }
        public string cp_region { get; set; }
        public string cp_date { get; set; }
        public string cp_state { get; set;}

        public string cpl_date { get;set; }

        public string user_name { get; set; }

        private string _selected_cp_stateItem;
        public string selected_cp_stateItem
        {
            get { return _selected_cp_stateItem; }
            set
            {
                if (_selected_cp_stateItem != value)
                {
                    _selected_cp_stateItem = value;
                    OnPropertyChange(nameof(selected_cp_stateItem));
                    // user_info 속성도 같이 업데이트
                    cp_stateItem = value;
                }
            }
        }
        public string cp_stateItem { get;set; }
    }
}
