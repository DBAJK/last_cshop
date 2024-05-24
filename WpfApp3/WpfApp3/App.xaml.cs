using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using WpfApp3.Views;

namespace WpfApp3
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Mutex mutex = null;

            string processName = Process.GetCurrentProcess().ProcessName;

            try
            {
                bool isCreateNew;

                mutex = new Mutex(false, processName, out isCreateNew);

                if (isCreateNew)
                {
                    new LoginView().Show();

                    base.OnStartup(e);
                }
                else
                {
                    Console.WriteLine("이미 프로그램이 실행중 입니다.");
                    Application.Current.Shutdown();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Application.Current.Shutdown();
            }
        }
    }
}
