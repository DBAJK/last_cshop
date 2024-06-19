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
using System.Windows.Forms;
using System.Web.Script.Serialization;
using System.Runtime.InteropServices;
using System.Web.UI;

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

        private RelayCommand _UserComplaint;
        public RelayCommand UserComplaint
        {
            get { return _UserComplaint ?? (_UserComplaint = new RelayCommand(OnGoComplaint)); }
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
        public void OnGoComplaint(object obj)
        {
            double lng = GlobalVariable._instance.myLocale.Lng;
            double lat = GlobalVariable._instance.myLocale.Lat;
            string authData = GlobalVariable._instance.userInfo.authData;
            if(authData == "admin" || (lng != 0 && lat != 0)) { 
                System.Windows.MessageBox.Show("민원 화면으로 이동합니다.");
                new ComplaintApply().Show();
            }
            else
            {
                System.Windows.MessageBox.Show("민원 위치를 선정해주세요.");
            }
        }
    }
    public partial class KakaoAPI
    {
        public KakaoAPI _View;
        private KakaoAPIViewModel _viewModel;
        private LoginView _LoginView;
        private LoginViewModel _LoginChk;
        private UserInfo _UserInfoChk;

        public KakaoAPIViewModel ViewModel = null;

        public KakaoAPI()
        {
            InitializeComponent();

            ViewModel = new KakaoAPIViewModel(this);

            this.DataContext = ViewModel;

            authDataChk();
        }

        public void callCSharpMethod(double lat, double lng)
        {
            System.Windows.MessageBox.Show("Lat: " + lat + ", Lng: " + lng);
        }


        // --!******** 회원관리 버튼 관리자에게 보이게 하기
        public void authDataChk()
        {
            string authData = GlobalVariable._instance.userInfo.authData;
            
                //_LoginChk.PubAuthChk.ToString();
            if (authData == "admin")
            {
                xAdminView.Visibility = Visibility.Visible;
            }
            else
            {
                xAdminView.Visibility = Visibility.Hidden;
            }
        }
        // --!********

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<MyLocale> mls = KakaoAPI.Search(tbox_query.Text);
            lbox_locale.ItemsSource = mls;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbox_locale.SelectedIndex == -1)
            {
                return;
            }
            MyLocale ml = lbox_locale.SelectedItem as MyLocale;
            object[] ps = new object[] { ml.Lat, ml.Lng };
            GlobalVariable._instance.myLocale.Lng = ml.Lng ;
            GlobalVariable._instance.myLocale.Lat = ml.Lat ;
            GlobalVariable._instance.myLocale.Name = ml.Name;

            kakaoMapB.InvokeScript("setCenter", ps);
        }


        internal static List<MyLocale> Search(string query)
        {
            List<MyLocale> mls = new List<MyLocale>();
            string site = "https://dapi.kakao.com/v2/local/search/keyword.json";
            string rquery = string.Format("{0}?query={1}", site, query);
            WebRequest request = WebRequest.Create(rquery);
            string rkey = "bd419e294343736f22452d5ee0d2309a";
            string header = "KakaoAK " + rkey;
            request.Headers.Add("Authorization", header);

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            String json = reader.ReadToEnd();

            JavaScriptSerializer js = new JavaScriptSerializer();
            dynamic dob = js.Deserialize<dynamic>(json);
            dynamic docs = dob["documents"];
            object[] buf = docs;
            int length = buf.Length;
            for (int i = 0; i < length; i++)
            {
                string lname = docs[i]["place_name"];
                double x = double.Parse(docs[i]["x"]);
                double y = double.Parse(docs[i]["y"]);
                mls.Add(new MyLocale(lname, y, x));
            }
            return mls;
        }
    }
}
