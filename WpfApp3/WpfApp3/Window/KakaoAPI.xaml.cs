using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using WpfApp3.Handler;
using WpfApp3.DB;
using WpfApp3.ENCRYPTION;
using WpfApp3.Model;
using System.IO;
using System.Net;
using WpfApp3.Window;
//using System.Windows.Forms;


namespace WpfApp3.Views
{
    /// <summary>
    /// KakaoAPI.xaml에 대한 상호 작용 논리
    /// </summary>
    /// 
    public class KakaoAPIViewModel : Notifier
    {
        private KakaoAPI _View;
        private LoginView _LoginView;

        private RelayCommand _LogoutCommand;
        public RelayCommand LogoutCommand
        {
            get { return _LogoutCommand ?? (_LogoutCommand = new RelayCommand(OnLogout)); }
        }
        private RelayCommand _MemberTableCommand;
        public RelayCommand MemberTableCommand
        {
            get { return _MemberTableCommand ?? (_MemberTableCommand = new RelayCommand(OnGoMemberTable)); }
        }


        public KakaoAPIViewModel(KakaoAPI view)
        {
            _View = view;

            // Login은 ID, PWD 이고, CanLogin은 조건 검증
            //LogInCommand = new RelayCommand(LogIn, CanLogIn);
        }



        public void OnLogout(object obj)
        {
            new LoginView().Show();

            _View.Close();
        }

        public void OnGoMemberTable(object obj)
        {
            new MemberView().Show();

            _View.Close();
        }

    }
    public partial class KakaoAPI
    {
        private KakaoAPI _View;
        private LoginView _LoginView;

        public KakaoAPIViewModel ViewModel = null;

        public KakaoAPI()
        {
            InitializeComponent();

            ViewModel = new KakaoAPIViewModel(this);
            this.DataContext = ViewModel;

        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            _LoginView.xLoginView.Visibility = Visibility.Visible;
            _View.xMapView.Visibility = Visibility.Hidden;
        }*/
        /*private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }



        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex != -1)
            {
                return;
            }
            Locale ml = listBox.SelectedItem as Locale;
            object[] pos = new object[] { ml.Lat, ml.Lng };
            HtmlDocument hdoc = webBrowser.Document;
            hdoc.InvokeScript("setCenter", pos);
        }*/
    }
}
