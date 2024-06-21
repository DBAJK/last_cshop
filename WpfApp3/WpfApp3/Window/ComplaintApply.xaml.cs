using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public string complaintsKey ;
        public string userid = GlobalVariable._instance.userInfo.UserSeq;
        public string userName = GlobalVariable._instance.userInfo.UserName;
        public double lng = GlobalVariable._instance.myLocale.Lng;
        public double lat = GlobalVariable._instance.myLocale.Lat;
        public string regionName = GlobalVariable._instance.myLocale.Name;
        private string authData = GlobalVariable._instance.userInfo.authData;

        MyLocaleList locale = new MyLocaleList();

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

        private RelayCommand _backPageView;
        public RelayCommand backPageView
        {
            get { return _backPageView ?? (_backPageView = new RelayCommand(OnBackBtn)); }
        }

        
        private RelayCommand _complaintUpdate;
        public RelayCommand complaintUpdate
        {
            get { return _complaintUpdate ?? (_complaintUpdate = new RelayCommand(OnSaveComplaintsData)); }
        }

        private void OnBackBtn(object obj)
        {
            GlobalVariable._instance.myLocale.Name = "";
            GlobalVariable._instance.myLocale.Lng = 0;
            GlobalVariable._instance.myLocale.Lat = 0;
            _View.Close();

        }

        public void OnComplaintsSave(object obj) {
            // 민원인 저장하기
            string errMsg = SaveListChk();
            // DB 연결
            DB_CONNECTOR db = new DB_CONNECTOR();
            

            if (errMsg.Length == 0)
            {
                if (!db.complaintChk(userid,lng,lat))
                {
                    db.complaintDataUpdate(complaintsKey, userid, xContentTextBox);
                    System.Windows.MessageBox.Show("수정되었습니다.");

                }
                else
                {
                    db.complaintDatainsert(userid, userName, xContentTextBox, regionName, xDatePicker, cp_stateItem, lng, lat);
                    System.Windows.MessageBox.Show("등록되었습니다.");
                } 
                _View.Close();
            }
            else
            {
                System.Windows.MessageBox.Show(errMsg, "실패");
            }

        }
        
        private void OnSaveComplaintsData(object obj)
        {
            // 관리자 저장하기
            // DB 연결
            DB_CONNECTOR db = new DB_CONNECTOR();

            foreach(var list in ComplaintsList)
            {
                db.complaintAdminUpdate(list.complaints_key, list.cp_stateItem);
            }
            
            System.Windows.MessageBox.Show("수정되었습니다.");
            _View.Close();
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
                return GlobalVariable._instance.myLocale.Name;
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

        private string _cp_stateItem;

        public string cp_stateItem
        {
            get { return _cp_stateItem; }
            set
            {
                _cp_stateItem = value;
                OnPropertyChange(nameof(cp_stateItem));
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
                /// 검색 하여 민원 페이지 왔을때
                if (lng != 0)
                {
                    this.ComplaintsList.Clear();
                    try
                    {
                        using (MySqlConnection conn = dbConnect.MemberViewConn())
                        {
                            conn.Open();

                            // 중복 ID 조회
                            string sql1 = "select complaints_key, user_name, cp_contents, cp_region, DATE_FORMAT(cp_date, '%Y-%m-%d') as cp_date, cp_state, DATE_FORMAT(cpl_date, '%Y-%m-%d') as cpl_date from complaints " +
                                "where CP_LNG = '" + lng + "' and CP_LAT = '" + lat + "';";

                            using (var cmd = new MySqlCommand(sql1, conn))
                            {
                                using (var reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        MyLocaleList locale = new MyLocaleList();

                                        locale.complaints_key = reader["complaints_key"].ToString();
                                        locale.user_name = reader["user_name"].ToString();
                                        locale.cp_contents = reader["cp_contents"].ToString();
                                        locale.cp_region = reader["cp_region"].ToString();
                                        locale.cp_date = reader["cp_date"].ToString();
                                        locale.cp_stateItem = reader["cp_state"].ToString();
                                        locale.cpl_date = reader["cpl_date"].ToString();

                                        locale.selected_cp_stateItem = locale.cp_stateItem;

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
                            string sql1 = "select complaints_key, user_name, cp_contents, cp_region, DATE_FORMAT(cp_date, '%Y-%m-%d') as cp_date, cp_state, DATE_FORMAT(cpl_date, '%Y-%m-%d') as cpl_date from complaints ;";

                            using (var cmd = new MySqlCommand(sql1, conn))
                            {
                                using (var reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        MyLocaleList locale = new MyLocaleList();

                                        locale.complaints_key = reader["complaints_key"].ToString();
                                        locale.user_name = reader["user_name"].ToString();
                                        locale.cp_contents = reader["cp_contents"].ToString();
                                        locale.cp_region = reader["cp_region"].ToString();
                                        locale.cp_date = reader["cp_date"].ToString();
                                        locale.cp_stateItem = reader["cp_state"].ToString();
                                        locale.cpl_date = reader["cpl_date"].ToString();

                                        locale.selected_cp_stateItem = locale.cp_stateItem;
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

                complaintsKey = complaintListView[0];
                xContentTextBox = complaintListView[1];
                xRegionTextBox = complaintListView[2];
                xDatePicker = complaintListView[3];
                cp_stateItem = complaintListView[4];
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
            //_View.xRegionTextBox = string();
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
/*        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is System.Windows.Controls.ComboBox comboBox)
            {
                var selectedItem = comboBox.SelectedItem as ComboBoxItem;
                if (selectedItem != null)
                {
                    _View.selected_cp_stateItem = selectedItem.Content.ToString();

                }
            }
        }

*/
    }

}
