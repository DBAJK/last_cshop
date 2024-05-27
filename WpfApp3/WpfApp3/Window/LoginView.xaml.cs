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
using System.Diagnostics;
using WpfApp3.Handler;
using WpfApp3.DB;
using WpfApp3.ENCRYPTION;
using WpfApp3.Model;

namespace WpfApp3.Views
{
    public class LoginViewModel : Notifier
    {
        private string ID { get; set; }
        private string Password { get; set; }

        private LoginView _View;
        private MainView _Main;

        // 로그인화면 -> 로그인 버튼 바인딩
        private RelayCommand _LoginCommand;
        public RelayCommand LoginCommand 
        {
            get { return _LoginCommand ?? (_LoginCommand = new RelayCommand(OnLogIn)); }
        }
        // 로그인화면 -> 로그인 버튼 바인딩

        // 회원가입화면 -> 회원가입 버튼 바인딩
        private RelayCommand _SignUpCommand;
        public RelayCommand SignUpCommand
        {
            get { return _SignUpCommand ?? (_SignUpCommand = new RelayCommand(OnSignUp));  }
        }
        // 회원가입화면 -> 회원가입 버튼 바인딩

        // 회원가입화면 -> 이전화면 버튼 바인딩
        private RelayCommand _MoveLoginCommand;
        public RelayCommand MoveLoginCommand
        {
            get { return _MoveLoginCommand ?? (_MoveLoginCommand = new RelayCommand(OnMoveLoginView)); }
        }
        // 회원가입화면 -> 이전화면 버튼 바인딩

        // 로그인 검증 모델
        public LoginViewModel(LoginView view)
        {
            _View = view;

            // Login은 ID, PWD 이고, CanLogin은 조건 검증
            //LogInCommand = new RelayCommand(LogIn, CanLogIn);
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
                OnPropertyChange("UserID");
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
            }
        }
        public int UserPasswordLen { get; set; }

        // 회원가입 변수
        public int RegNameLen { get; set; }
        public int RegIDLen { get; set; }
        public int RegPasswordLen { get; set; }

        private string _RegUserName;
        public string RegUserName
        {
            get
            {
                return _RegUserName;
            }
            set
            {
                _RegUserName = value;
                RegNameLen = value.Length;
                OnPropertyChange("RegUserName");
                //UserIdLen = _UserName.Length;
                //OnPropertyChange("UserName");
            }
        }

        private string _RegUserID;
        public string RegUserID
        {
            get
            {
                return _RegUserID;
            }
            set
            {
                _RegUserID = value;
                RegIDLen = value.Length;
                OnPropertyChange("RegUserID");
            }
        }

        private string _RegUserPassword;
        public string RegUserPassword
        {
            get
            {
                return _RegUserPassword;
            }
            set
            {
                _RegUserPassword = value;
                RegPasswordLen = value.Length;
            }
        }

        private string _RegUserPasswordChk;
        public string RegUserPasswordChk
        {
            get
            {
                return _RegUserPasswordChk;
            }
            set
            {
                _RegUserPasswordChk = value;
            }
        }
        // 회원가입 변수

        //private RelayCommand _LogInCommand;
        /*public RelayCommand LogInCommand
        {
            set;
            get;
        }*/

        // 로그인화면 -> 로그인 버튼 클릭시 호출 이벤트
        public void OnLogIn(object obj)
        {
            // 로그인 처리
            if (UserIdLen > 0 && UserPasswordLen > 0 && ChkLoginData())
            {
                UserInfo UserInfo = new UserInfo();
                UserInfo.UserName = _userInfo[0];
                UserInfo.UserSeq = _userInfo[1];

                GlobalVariable.Instance().userInfo = UserInfo;

                new KakaoAPI().Show();

                _View.Close();
            }
            else
            {
                MessageBox.Show("아이디 혹은 비밀번호가 일치하지 않습니다.", "실패");
            }
        }
        // 로그인화면 -> 로그인 버튼 클릭시 호출 이벤트

        // 회원가입화면 -> 화원가입 버튼 클릭시 호출 이벤트
        public void OnSignUp(object obj)
        {
            string errMsg = SignUpChk();

            if (errMsg.Length == 0)
            {
                // DB 연결
                DB_CONNECTOR db = new DB_CONNECTOR();   

                if (db.userIdDuplication(RegUserID))
                {
                    // 비밀번호는 암호화 후 전달
                    SHA256_ENCRYP sha256 = new SHA256_ENCRYP();
                    string pwdStr = sha256.Connect(RegUserPassword);

                    db.userInsertData(UserID, pwdStr, RegUserName);

                    MessageBox.Show("등록되었습니다.", "성공");

                    _View.xLoginView.Visibility = Visibility.Visible;
                    _View.xSignUpView.Visibility = Visibility.Hidden;
                }
                else
                {
                    MessageBox.Show("이미 존재하는 아이디입니다.", "실패");
                }
            }
            else
            {
                MessageBox.Show(errMsg, "실패");
            }
        }
        private string SignUpChk()
        {
            string errMsg = "";

            if (RegNameLen < 2 || RegNameLen > 20) // 이름 검증
            {
                errMsg = "이름은 2자 이상 20자 이하로 입력해주세요.";
            }
            else if (RegIDLen < 8 || RegIDLen > 10) // 아이디 검증
            {
                errMsg = "아이디는 8자 이상 10자 이하로 입력해주세요.";
            }
            else if (RegPasswordLen < 8 || RegPasswordLen > 10) // 비밀번호 검증
            {
                errMsg = "비밀번호는 8자 이상 10자 이하로 입력해주세요.";
            }
            else if (RegUserPassword != RegUserPasswordChk) // 비밀번호, 비밀번호 확인 값 검증
            {
                errMsg = "비밀번호가 일치하지 않습니다.";
            }

            return errMsg;
        }
        // 회원가입화면 -> 화원가입 버튼 클릭시 호출 이벤트

        // 회원가입화면 -> 이전화면 버튼 바인딩
        public void OnMoveLoginView(object obj)
        {
            RegUserName = "";
            RegUserID = "";
            _View.xRegUserPwd.Password = "";
            _View.xRegUserPwdChk.Password = "";

            _View.xLoginView.Visibility = Visibility.Visible;
            _View.xSignUpView.Visibility = Visibility.Hidden;
        }
        // 회원가입화면 -> 이전화면 버튼 바인딩

        public bool CanLogIn()
        {
            if (UserIdLen < 2 || UserPasswordLen < 4)
            {
                return false;
            }

            return true;
        }

        private string[] _userInfo;
        public bool ChkLoginData()
        {
            // DB 연결
            DB_CONNECTOR db = new DB_CONNECTOR();

            // 비밀번호는 암호화 후 전달
            SHA256_ENCRYP sha256 = new SHA256_ENCRYP();
            string pwdStr = sha256.Connect(UserPassword);

            _userInfo = db.userLoginChk(UserID, pwdStr);

            return _userInfo[0].Length > 0;
        }


    }

    /// <summary>
    /// LoginView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginViewModel ViewModel = null;

        public LoginView()
        {
            InitializeComponent();
            ViewModel = new LoginViewModel(this);
            this.DataContext = ViewModel;
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            xLoginView.Visibility = Visibility.Hidden;
            xSignUpView.Visibility = Visibility.Visible;
        }
        private void xUserId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ViewModel == null) return;

            if (sender != null)
            {
                TextBox textBox = sender as TextBox;
                ViewModel.UserID = textBox.Text;
                //ViewModel.UserIdLen = textBox.Text.Length;
            }
        }

        private void xUserPwd_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;

            if (sender != null)
            {
                PasswordBox passwordBox = sender as PasswordBox;
                ViewModel.UserPassword = passwordBox.Password;
                //ViewModel.UserPasswordLen = passwordBox.Password.Length;
            }
        }
















        // 비밀번호 입력 이벤트
        private void xRegUserPwdChk_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;

            if (sender != null)
            {
                PasswordBox passwordBox = sender as PasswordBox;
                ViewModel.RegUserPassword = passwordBox.Password;
                //ViewModel.UserPasswordLen = passwordBox.Password.Length;
            }
        }

        // 비밀번호 확인 입력 이벤트
        private void xRegUserPwdChk_PasswordChanged_1(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;

            if (sender != null)
            {
                PasswordBox passwordBox = sender as PasswordBox;
                ViewModel.RegUserPasswordChk = passwordBox.Password;
                //ViewModel.UserPasswordLen = passwordBox.Password.Length;
            }
        }

    }


}
