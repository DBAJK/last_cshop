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
using System.Windows.Shapes;
using WpfApp3.Handler;
using WpfApp3.Views;

namespace WpfApp3.Window
{
    /// <summary>
    /// MemberViewWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public class MemberViewModel : Notifier
    {
        private MemberView _View;

        private RelayCommand _AuthorityCommand;
        public RelayCommand AuthorityCommand
        {
            get { return _AuthorityCommand ?? (_AuthorityCommand = new RelayCommand(OnBackBtn)); }
        }

        public MemberViewModel(MemberView view)
        {
            _View = view;

            // Login은 ID, PWD 이고, CanLogin은 조건 검증
            //LogInCommand = new RelayCommand(LogIn, CanLogIn);
        }

        public void OnBackBtn(object obj)
        {
            new KakaoAPI().Show();

            _View.Close();

        }
    }
    public class Data
    {
        public string name { get; set; }
        public string id { get; set; }
        public string major { get; set; }
        public int grade { get; set; }
        public string etc { get; set; }
    }
    public partial class MemberView
    {
        private MemberView _View;

        public MemberViewModel ViewModel = null;

        public MemberView()
        {
            InitializeComponent();

            ViewModel = new MemberViewModel(this);
            this.DataContext = ViewModel;

        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Data> list = new List<Data>();
            list.Add(new Data { name = "이지원", id = "210651", major = "컴퓨터공학", grade = 1, etc = "" });
            list.Add(new Data { name = "김현호", id = "210184", major = "컴퓨터공학", grade = 1, etc = "" });
            list.Add(new Data { name = "강희진", id = "210017", major = "컴퓨터공학", grade = 1, etc = "" });
            list.Add(new Data { name = "박서준", id = "210439", major = "컴퓨터공학", grade = 1, etc = "" });
            list.Add(new Data { name = "강나연", id = "210005", major = "컴퓨터공학", grade = 1, etc = "" });
            xMemberList.ItemsSource = list;
        }

    }
}
