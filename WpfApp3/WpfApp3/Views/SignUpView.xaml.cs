using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using WpfApp3.DB;
using WpfApp3.ENCRYPTION;
using WpfApp3.Handler;

namespace WpfApp3.Views
{
    public class SignUpViewModel : Notifier
    {
        private int IDLen { get; set; }
        private int NameLen { get; set; }
        private int PasswordLen { get; set; }


        private SignUpView _view;
        public SignUpViewModel(SignUpView view) 
        { 
            _view = view;
            UserID = string.Empty;
            UserPassword = string.Empty;
            UserPasswordChk = string.Empty;
            UserName = string.Empty;
        }

        private string _UserName;
        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
                NameLen = value.Length;
                //UserIdLen = _UserName.Length;
                //OnPropertyChange("UserName");
            }
        }

        private string _UserID;
        public string UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
                IDLen = value.Length;
            }
        }

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
                PasswordLen = value.Length;
            }
        }

        private string _UserPasswordChk;
        public string UserPasswordChk
        {
            get
            {
                return _UserPasswordChk;
            }
            set
            {
                _UserPasswordChk = value;
            }
        }

        public void InsertUser()
        {
            string errMsg = SignUpChk();

            if (errMsg.Length == 0)
            {
                // DB 연결
                DB_CONNECTOR db = new DB_CONNECTOR();

                if(db.userIdDuplication(UserID))
                {
                    // 비밀번호는 암호화 후 전달
                    SHA256_ENCRYP sha256 = new SHA256_ENCRYP();
                    string pwdStr = sha256.Connect(UserPassword);

                    db.userInsertData(UserID, pwdStr, UserName);

                    MessageBox.Show("등록되었습니다.", "성공");

                    //if (GlobalVariable.Instance().MainWindow != null)
                    //{
                        //GlobalVariable.Instance().MainWindow.xSignUpView.Visibility = Visibility.Hidden;
                        //GlobalVariable.Instance().MainWindow.xLoginView.Visibility = Visibility.Visible;
                    //}
                } else
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

            if (NameLen < 2 || NameLen > 20) // 이름 검증
            {
                errMsg = "이름은 2자 이상 20자 이하로 입력해주세요.";
            }
            else if (IDLen < 8 || IDLen > 10) // 아이디 검증
            {
                errMsg = "아이디는 8자 이상 10자 이하로 입력해주세요.";
            }
            else if (PasswordLen < 8 || PasswordLen > 10) // 비밀번호 검증
            {
                errMsg = "비밀번호는 8자 이상 10자 이하로 입력해주세요.";
            }
            else if (UserPassword != UserPasswordChk) // 비밀번호, 비밀번호 확인 값 검증
            {
                errMsg = "비밀번호가 일치하지 않습니다.";
            }

            return errMsg;
        }
    }

    /// <summary>
    /// SignUpView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SignUpView : UserControl
    {
        public SignUpViewModel ViewModel;
        public SignUpView()
        {
            InitializeComponent();
            ViewModel = new SignUpViewModel(this);
            this.DataContext = ViewModel;
        }

        // 비밀번호 입력 이벤트
        private void xUserPwdChk_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;

            if (sender != null)
            {
                PasswordBox passwordBox = sender as PasswordBox;
                ViewModel.UserPassword = passwordBox.Password;
                //ViewModel.UserPasswordLen = passwordBox.Password.Length;
            }
        }

        // 비밀번호 확인 입력 이벤트
        private void xUserPwdChk_PasswordChanged_1(object sender, RoutedEventArgs e)
        {
            if (ViewModel == null) return;

            if (sender != null)
            {
                PasswordBox passwordBox = sender as PasswordBox;
                ViewModel.UserPasswordChk = passwordBox.Password;
                //ViewModel.UserPasswordLen = passwordBox.Password.Length;
            }
        }

        // 이름 입력 이벤트
        private void xUserName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ViewModel == null) return;

            if (sender != null)
            {
                TextBox textBox = sender as TextBox;
                ViewModel.UserName = textBox.Text;
                //ViewModel.UserPasswordLen = passwordBox.Password.Length;
            }

        }
        
        // 아이디 입력 이벤트
        private void xUserId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ViewModel == null) return;

            if (sender != null)
            {
                TextBox textBox = sender as TextBox;
                ViewModel.UserID = textBox.Text;
                //ViewModel.UserPasswordLen = passwordBox.Password.Length;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.InsertUser();
        }

        
    }
}
