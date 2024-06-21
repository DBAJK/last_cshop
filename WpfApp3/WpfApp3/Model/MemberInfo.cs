using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Handler;

namespace WpfApp3.Model
{
    public class MemberInfo : Notifier
    {
        public string sequence_id { get; set; }
        public string user_name { get; set; }
        public string user_id { get; set; }
        public string user_email { get; set; }
        public string user_tel { get; set; }
        public string user_auth { get; set; }
        public string cre_date { get; set; }
        public string user_info { get; set; }

        private string _selected_user_info;
        public string selected_user_info
        {
            get { return _selected_user_info; }
            set
            {
                if (_selected_user_info != value)
                {
                    _selected_user_info = value;
                    OnPropertyChange(nameof(selected_user_info));
                }
            }
        }
    }
}
