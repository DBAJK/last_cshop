using MySql.Data.MySqlClient;
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
using WpfApp3.DB;
using WpfApp3.ENCRYPTION;
using WpfApp3.Handler;

namespace WpfApp3.Window
{
    /// <summary>
    /// ComplaintApply.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ComplaintApplyViewModel : Notifier
    {
        private DB_CONNECTOR dbConnect;

        private ComplaintApply _View;
        public string userid = GlobalVariable._instance.userInfo.UserSeq;
        public double lng = GlobalVariable._instance.myLocale.Lng;
        public double lat = GlobalVariable._instance.myLocale.Lat;
        public string regionName = GlobalVariable._instance.myLocale.Name;
        public ComplaintApplyViewModel(ComplaintApply view)
        {
            _View = view;
            dbConnect = new DB_CONNECTOR();

        }

        public void LoadComplaintView()
        {
            DB_CONNECTOR db = new DB_CONNECTOR();
            db.complaintListChk(userid, lng, lat);
        }


        private RelayCommand _ComplaintsSaveBtn;
        public RelayCommand ComplaintsSaveBtn
        {
            get { return _ComplaintsSaveBtn ?? (_ComplaintsSaveBtn = new RelayCommand(OnComplaintsSave)); }
        }

        public void OnComplaintsSave(object obj) {
            string errMsg = SaveListChk();
            // DB 연결
            DB_CONNECTOR db = new DB_CONNECTOR();

            string StatusData = "신청";

            if (errMsg.Length == 0)
            {
                db.complaintDatainsert(userid, xContentTextBox, xRegionTextBox, xDatePicker, StatusData, xFinalDateBox, lng, lat);
                MessageBox.Show("등록되었습니다.", "성공");            }
            else
            {
                MessageBox.Show(errMsg, "실패");
            }

        }

        public int ContentTextBoxLen { get; set; }
        public int RegionTextBoxLen { get; set; }
        public int DatePickerLen { get; set; }

        private string _ContentTextBox;
        public string xContentTextBox
        {
            get
            {
                return _ContentTextBox;
            }
            set
            {
                _ContentTextBox = value;
                ContentTextBoxLen = value.Length;
                OnPropertyChange("xContentTextBox");

            }
        }

        private string _RegionTextBox;
        public string xRegionTextBox
        {
            get
            {
                return _RegionTextBox;
            }
            set
            {
                _RegionTextBox = value;
                RegionTextBoxLen=value.Length;
                OnPropertyChange("xRegionTextBox");
            }
        }

        private string _DatePicker;
        public string xDatePicker
        {
            get { return _DatePicker; }
            set
            {
                _DatePicker = value;
                DatePickerLen =value.Length;
                OnPropertyChange("xDatePicker");
            }
        }

        private string _FinalDateBox;
        public string xFinalDateBox
        {
            get { return _FinalDateBox; }
            set
            {
                _FinalDateBox = value;
                OnPropertyChange("xFinalDateBox");
            }
        }
        private string SaveListChk()
        {
            string errMsg = "";
            if (ContentTextBoxLen <= 0) // 아이디 검증
            {
                errMsg = "민원 내용을 작성해주세요.";
            }
            else if (RegionTextBoxLen <= 0) // 비밀번호 검증
            {
                errMsg = "민원지역을 입력해주세요.";
            }
            return errMsg;
        }

    }

    public partial class ComplaintApply
    {
        private ComplaintApply _View;

        public ComplaintApplyViewModel ViewModel = null;

        public ComplaintApply()
        {
            InitializeComponent();

            ViewModel = new ComplaintApplyViewModel(this);
            this.DataContext = ViewModel;

        }

    }

}
