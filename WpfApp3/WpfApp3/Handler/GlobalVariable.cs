using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp3.Model;
using WpfApp3.Views;

namespace WpfApp3
{
    public class GlobalVariable
    {

        #region [ Singleton ]
        public static GlobalVariable _instance = null;
        private static readonly object locker = new object();

        public static GlobalVariable Instance()
        {
            lock (locker)
            {
                if (_instance == null)
                {
                    _instance = new GlobalVariable();
                }
                return _instance;
            }
        }
        #endregion [ Singleton ]

        #region [ Field ]
        public LoginView LoginWin = null;
        public LoginInfo loginInfo = new LoginInfo();
        public UserInfo userInfo = new UserInfo();
        public MyLocaleList myLocale = new MyLocaleList();
        #endregion [ Field ]

        public GlobalVariable()
        {

        }
    }
}
