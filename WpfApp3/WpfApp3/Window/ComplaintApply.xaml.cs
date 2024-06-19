using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp3.DB;
using WpfApp3.ENCRYPTION;
using WpfApp3.Handler;
using WpfApp3.Model;

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
        private string authData = GlobalVariable._instance.userInfo.authData;

        public ComplaintApplyViewModel(ComplaintApply view)
        {
            _View = view;
            dbConnect = new DB_CONNECTOR();
            ComplaintsList = new ObservableCollection<MyLocaleList>();

            if (authData == "admin")
            {
                LoadComplaint();
            }
            else
            {
                if(!dbConnect.complaintChk(userid, lng, lat))
                {
                    LoadComplaint();
                }
            }
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

            //string xStatusComboBox = "신청";

            if (errMsg.Length == 0)
            {
                db.complaintDatainsert(userid, xContentTextBox, regionName, xDatePicker, xStatusComboBox, lng, lat);
                System.Windows.MessageBox.Show("등록되었습니다.", "성공");
                _View.Close();
            }
            else
            {
                System.Windows.MessageBox.Show(errMsg, "실패");
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

        private DateTime _DatePicker = DateTime.Now;
        public string xDatePicker
        {
            get { return _DatePicker.ToString("yyyy-MM-dd"); }
            set
            {
                DateTime result;
                if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
                {
                    _DatePicker = result;
                    OnPropertyChange("xDatePicker");
                }
            }
        }
        private string _StatusComboBox;
        public string xStatusComboBox
        {
            get { return _StatusComboBox; }
            set
            {
                _StatusComboBox = value;
                OnPropertyChange("xStatusComboBox");
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

        private ObservableCollection<MyLocaleList> _ComplaintsList;
        public ObservableCollection<MyLocaleList> ComplaintsList
        {
            get { return _ComplaintsList; }
            set { _ComplaintsList = value; OnPropertyChange("ComplaintsList"); }
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
        public void LoadComplaint()
        {
            if(authData == "admin")
            {
                if (lng != 0)
                {
                    this.ComplaintsList.Clear();
                    try
                    {
                        using (MySqlConnection conn = dbConnect.MemberViewConn())
                        {
                            conn.Open();

                            // 중복 ID 조회
                            string sql1 = "select complaints_key, cp_contents, cp_region, cp_date, cp_state, COALESCE(cpl_date, CURRENT_DATE) as cpl_date from complaints " +
                                "where CP_LNG = '" + lng + "' and CP_LAT = '" + lat + "';";

                            using (var cmd = new MySqlCommand(sql1, conn))
                            {
                                using (var reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        MyLocaleList locale = new MyLocaleList();
                                        locale.cp_contents = reader["cp_contents"].ToString();
                                        locale.cp_region = reader["cp_region"].ToString();
                                        locale.cp_date = reader["cp_date"].ToString();
                                        locale.cp_state = reader["cp_state"].ToString();
                                        locale.cpl_date = reader["cpl_date"].ToString();

                                        ComplaintsList.Add(locale);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); }
                }
                else
                {
                    this.ComplaintsList.Clear();
                    try
                    {
                        using (MySqlConnection conn = dbConnect.MemberViewConn())
                        {
                            conn.Open();

                            // 중복 ID 조회
                            string sql1 = "select complaints_key, cp_contents, cp_region, cp_date, cp_state, COALESCE(cpl_date, CURRENT_DATE) as cpl_date from complaints ';";

                            using (var cmd = new MySqlCommand(sql1, conn))
                            {
                                using (var reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        MyLocaleList locale = new MyLocaleList();
                                        locale.cp_contents = reader["cp_contents"].ToString();
                                        locale.cp_region = reader["cp_region"].ToString();
                                        locale.cp_date = reader["cp_date"].ToString();
                                        locale.cp_state = reader["cp_state"].ToString();
                                        locale.cpl_date = reader["cpl_date"].ToString();

                                        ComplaintsList.Add(locale);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex) { System.Windows.MessageBox.Show(ex.Message); }

                }
            }
            else if(authData == "user" || authData == "customer")
            {
                string[] complaintListView = dbConnect.complaintList(userid, lng, lat);

                xContentTextBox = complaintListView[1];
                xRegionTextBox = complaintListView[2];
                xDatePicker = complaintListView[3];
                xStatusComboBox = complaintListView[4];
            }
        }

    }

    public partial class ComplaintApply
    {
        private ComplaintApply _View;
        private DB_CONNECTOR dbConnect;

        public ComplaintApplyViewModel ViewModel = null;

        public ComplaintApply()
        {
            InitializeComponent();

            ViewModel = new ComplaintApplyViewModel(this);
            this.DataContext = ViewModel;
            dbConnect = new DB_CONNECTOR();
            authDataChk();
        }

        public void authDataChk()
        {
            string authData = GlobalVariable._instance.userInfo.authData;

            //_LoginChk.PubAuthChk.ToString();
            if (authData == "admin")
            {
                xAdminComplaintView.Visibility = Visibility.Visible;
                xUserComplaintView.Visibility = Visibility.Hidden;
            }
            else
            {
                xAdminComplaintView.Visibility = Visibility.Hidden;
                xUserComplaintView.Visibility = Visibility.Visible;
            }
        }

    }

}
