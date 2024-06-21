using Google.Protobuf.Compiler;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
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
using WpfApp3.Handler;
using WpfApp3.Model;
using WpfApp3.Views;

namespace WpfApp3.Window
{
    /// <summary>
    /// MemberViewWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public class MemberViewModel : Notifier
    {
        private DB_CONNECTOR dbConnect;

        private MemberView _View;

        private ObservableCollection<MemberInfo> _MemberList;
        public ObservableCollection<MemberInfo> MemberList
        {
            get { return _MemberList; }
            set { _MemberList = value; OnPropertyChange("MemberList"); }
        }

        private RelayCommand _AuthorityCommand;
        public RelayCommand AuthorityCommand
        {
            get { return _AuthorityCommand ?? (_AuthorityCommand = new RelayCommand(OnBackBtn)); }
        }

        private RelayCommand _MemeberDataSave;
        public RelayCommand MemeberDataSave
        {
            get { return _MemeberDataSave ?? (_MemeberDataSave = new RelayCommand(OnMemberDataSave)); }
        }


        public MemberViewModel(MemberView view)
        {
            _View = view;
            MemberList = new ObservableCollection<MemberInfo>();
            dbConnect = new DB_CONNECTOR();

            LoadMembers();
        }

        private string _user_info;

        public string user_info
        {
            get { return _user_info; }
            set
            {
                _user_info = value;
                OnPropertyChange(nameof(user_info));
            }
        }
        public void OnBackBtn(object obj)
        {
            new KakaoAPI().Show();
            GlobalVariable._instance.myLocale.Name = "";
            GlobalVariable._instance.myLocale.Lng = 0;
            GlobalVariable._instance.myLocale.Lat = 0;
            _View.Close();
        }
        public void LoadMembers()
        {
            this.MemberList.Clear();
            try
            {
                using (MySqlConnection conn = dbConnect.MemberViewConn())
                {
                    conn.Open();
                    string query = "SELECT sequence_id, user_name, user_id, user_email, user_tel, user_auth, cre_date, user_info FROM members;"; 
                    
                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MemberInfo member = new MemberInfo();
                                member.sequence_id = reader["sequence_id"].ToString();
                                member.user_name = reader["user_name"].ToString();
                                member.user_id = reader["user_id"].ToString();
                                member.user_email = reader["user_email"].ToString();
                                member.user_tel = reader["user_tel"].ToString();
                                member.user_auth = reader["user_auth"].ToString();
                                member.cre_date = reader["cre_date"].ToString();
                                member.user_info = reader["user_info"].ToString();
                                member.selected_user_info = member.user_info;

                                MemberList.Add(member);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

        }

        public ObservableCollection<MemberInfo> members { get; set; }

        public void OnMemberDataSave(object obj)
        {
            DB_CONNECTOR db = new DB_CONNECTOR();
            foreach (var list in MemberList)
            {
                db.UserNameChangeUpdate(list.sequence_id, list.selected_user_info);
            }
            System.Windows.MessageBox.Show("수정되었습니다.");
            new KakaoAPI().Show();

            _View.Close();
        }
    }


public class MemberDataView : ObservableObject
    {
        private MemberView _View;

        public MemberViewModel ViewModel = null;
        private IList<MemberInfo> _memberInfo = new List<MemberInfo>
        {

        };
        public IList<MemberInfo> Persons { get { return _memberInfo; } }
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

    }
}
