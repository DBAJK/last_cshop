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
using System.Xml.Linq;
using WpfApp3.DB;
using WpfApp3.Handler;

namespace WpfApp3.Views
{

    public class MainViewModel : Notifier
    {

        private MainView _View;
        public MainViewModel(MainView view)
        {
            _View = view;
            string userName = GlobalVariable.Instance().userInfo.UserName;
            SaveNameText = userName;
            UserNameText = userName;
            UserNameBlock = userName;
        }

    // 메인화면 -> 이름 바인딩
    private string _SaveNameText;
        public string SaveNameText
        {
            get { return _SaveNameText; }
            set
            {
                _SaveNameText = value;
                OnPropertyChange("SaveNameText");
            }
        }
        // 메인화면 -> 이름 바인딩

        // 설정화면 -> 이름변경 TextBox 바인딩
        private string _UserNameText;
        public string UserNameText
        {
            get { return _UserNameText; }
            set
            {
                _UserNameText = value;
                OnPropertyChange("UserNameText");
            }
        }
        // 설정화면 -> 이름변경 TextBox 바인딩

        // 설정화면 -> 이름변경 TextBlock 바인딩
        private string _UserNameBlock;
        public string UserNameBlock
        {
            get { return _UserNameBlock; }
            set
            {
                _UserNameBlock = value;
                OnPropertyChange("UserNameBlock");
            }
        }
        // 설정화면 -> 이름변경 TextBlock 바인딩

        // 메인화면 -> 타자연습 버튼 바인딩
        private RelayCommand _MoveTypingPrac;
        public RelayCommand MoveTypingPrac
        {
            get { return _MoveTypingPrac ?? (_MoveTypingPrac = new RelayCommand(onMoveTypingPrac)); }
        }
        // 메인화면 -> 타자연습 버튼 바인딩

        // 메인화면 -> 설정화면 버튼 바인딩
        private RelayCommand _MoveSetting;
        public RelayCommand MoveSetting
        {
            get { return _MoveSetting ?? (_MoveSetting = new RelayCommand(onMoveSetting)); }
        }
        // 메인화면 -> 설정화면 버튼 바인딩

        // 설정화면 -> 이전화면 버튼 바인딩
        private RelayCommand _MoveMainView;
        public RelayCommand MoveMainView
        {
            get { return _MoveMainView ?? (_MoveMainView = new RelayCommand(onMoveMainView)); }
        }
        // 설정화면 -> 이전화면 버튼 바인딩

        // 설정화면 -> 이름 변경 이미지 바인딩
        private RelayCommand _UserNameFixCommand;
        public RelayCommand UserNameFixCommand
        {
            get { return _UserNameFixCommand ?? (_UserNameFixCommand = new RelayCommand(onChangeUserName)); }
        }
        // 설정화면 -> 이름 변경 이미지 바인딩

        // 설정화면 -> 비밀번호 변경 버튼 바인딩
        private RelayCommand _ChangePassword;
        public RelayCommand ChangePassword
        {
            get { return _ChangePassword ?? (_ChangePassword = new RelayCommand(onChangePassword)); }
        }
        // 설정화면 -> 비밀번호 변경 버튼 바인딩

        // 설정화면 -> 기록 삭제 버튼 바인딩
        private RelayCommand _UserRecordDelete;
        public RelayCommand UserRecordDelete
        {
            get { return _UserRecordDelete ?? (_UserRecordDelete = new RelayCommand(onUserRecordDelete)); }
        }
        // 설정화면 -> 기록 삭제 버튼 바인딩

        // 설정화면 -> 기록 삭제 버튼 바인딩
        private RelayCommand _UserDelete;
        public RelayCommand UserDelete
        {
            get { return _UserDelete ?? (_UserDelete = new RelayCommand(onUserDelete)); }
        }
        // 설정화면 -> 기록 삭제 버튼 바인딩

        // 설정화면 -> 기록 삭제 버튼 바인딩
        private RelayCommand _ChangeNameSaveCommand;
        public RelayCommand ChangeNameSaveCommand
        {
            get { return _ChangeNameSaveCommand ?? (_ChangeNameSaveCommand = new RelayCommand(onChangeNameSave)); }
        }
        // 설정화면 -> 기록 삭제 버튼 바인딩

        // 설정화면 -> 기록 삭제 버튼 바인딩
        private RelayCommand _ChangeNameCancelCommand;
        public RelayCommand ChangeNameCancelCommand
        {
            get { return _ChangeNameCancelCommand ?? (_ChangeNameCancelCommand = new RelayCommand(onChangeNameCancel)); }
        }
        // 설정화면 -> 기록 삭제 버튼 바인딩

        private void onMoveTypingPrac(object obj)
        {
            

        }

        private void onMoveSetting(object obj)
        {
            _View.xMainView.Visibility = Visibility.Hidden;
            _View.xSettingView.Visibility = Visibility.Visible;
        }

        private void onMoveMainView(object obj)
        {
            _View.xSettingView.Visibility = Visibility.Hidden;
            _View.xMainView.Visibility = Visibility.Visible;
        }

        private void onChangeUserName(object obj)
        {
            _View.xTextBlockName.Visibility = Visibility.Hidden;
            _View.xTextBoxName.Visibility = Visibility.Visible;

            _View.xButtonNameChange.Visibility = Visibility.Hidden;
            _View.xUserNameChangeBtn.Visibility = Visibility.Visible;
        }

        private void onChangePassword(object obj)
        {
            MessageBox.Show("Click");
        }

        private void onUserRecordDelete(object obj)
        {
            MessageBox.Show("Click");
        }

        private void onUserDelete(object obj)
        {
            MessageBox.Show("Click");
        }

        // 설정화면 -> 저장 버튼 클릭 이벤트
        private void onChangeNameSave(object obj)
        {
        
            // DB 연결
            DB_CONNECTOR db = new DB_CONNECTOR();

            string userSeq = GlobalVariable.Instance().userInfo.UserSeq;

            db.UserNameChangeUpdate(userSeq, UserNameText);

            GlobalVariable.Instance().userInfo.UserName = UserNameText;
            _View.xTextBlockName.Text = UserNameText;

            MessageBox.Show("저장되었습니다.", "성공");

            ChangeViewMode();
        }
        // 설정화면 -> 저장 버튼 클릭 이벤트

        // 설정화면 -> 취소 버튼 클릭 이벤트
        private void onChangeNameCancel(object obj)
        {
            ChangeViewMode();

            string userName = GlobalVariable.Instance().userInfo.UserName;
            UserNameText = userName;
        }
        // 설정화면 -> 취소 버튼 클릭 이벤트

        private void ChangeViewMode()
        {
            _View.xTextBoxName.Visibility = Visibility.Hidden;
            _View.xTextBlockName.Visibility = Visibility.Visible;

            _View.xUserNameChangeBtn.Visibility = Visibility.Hidden;
            _View.xButtonNameChange.Visibility = Visibility.Visible;
        }
    }

    /// <summary>
    /// MainView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainView : Window
    {
        private MainViewModel _ViewModel = null;

        public MainView() {
            InitializeComponent();
            _ViewModel = new MainViewModel(this);
            this.DataContext = _ViewModel;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
