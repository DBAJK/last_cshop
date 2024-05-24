using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp3.Handler;

namespace WpfApp3.Views
{
    public class UserInfoViewModel : Notifier
    {
        private string ID { get; set; }
        private string Password { get; set; }

        /*
        private Brush _SetErrLabelColor;
        public Brush SetErrLabelColor
        {
            get { return _SetErrLabelColor; }
            set
            {
                _SetErrLabelColor = value;
                OnPropertyChange("SetErrLabelColor");
            }
        }
        */

        private UserInfoView _View;
        public UserInfoViewModel(UserInfoView view) 
        {
            _View = view;
            UserID = string.Empty;

            ID = "구대광";
            Password = "1234";
        }

        private string _UserName;
        public string UserID 
        {
            get
            {
                return _UserName;
            } 
            set
            {
                _UserName = value;
                UserIdLen = _UserName.Length;
                OnPropertyChange("UserName");
            } 
        }

        public int UserIdLen { get; set; }

        private string _UserPassword;
        public string UserPassword
        {
            get
            {
                return _UserPassword;
            }
            set
            {
                _UserPassword = value;
                UserPasswordLen = _UserPassword.Length;
                OnPropertyChange("UserPassword");
            }
        }
        public int UserPasswordLen { get; set; }

        private RelayCommand _LogInCommand;
        public RelayCommand LogInCommand
        {
            set;
            get;
        }

        public void LogIn()
        {
            // 로그인 처리

            if(UserID == ID && UserPassword == Password)
            {
                _View.xErrMsg.Visibility = Visibility.Hidden;
                _View.Visibility = Visibility.Hidden;       // 로그인창 가리기
                
                //if(GlobalVariable.Instance().MainWindow != null)
                //{
                    //GlobalVariable.Instance.MainWindow.xFriendInfoView.Visibility = Visibility.Visible;
                //}
            }
            else
            {
                //SetErrLabelColor = Brushes.Red;
                _View.xErrMsg.Visibility = Visibility.Visible;
            }
        }

        public bool CanLogIn()
        {
            if(UserIdLen < 2 || UserPasswordLen < 4)
            {
                return false;
            }
            
            return true;
        }
    }

    /// <summary>
    /// UserInfoView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserInfoView : UserControl
    {
        public UserInfoViewModel ViewModel;

        public UserInfoView()
        {
            InitializeComponent();

            ViewModel = new UserInfoViewModel(this);
            this.DataContext = ViewModel;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ViewModel == null) return;

            if(sender != null)
            {
                TextBox textBox = sender as TextBox;
                ViewModel.UserID = textBox.Text;
                //ViewModel.UserIdLen = textBox.Text.Length;
            }
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;

            if (sender != null)
            {
                PasswordBox passwordBox = sender as PasswordBox;
                ViewModel.UserPassword = passwordBox.Password;
                //ViewModel.UserPasswordLen = passwordBox.Password.Length;
            }
        }
    }
}
