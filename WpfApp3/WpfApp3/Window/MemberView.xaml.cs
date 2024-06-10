using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlTypes;
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

        public MemberViewModel(MemberView view)
        {
            _View = view;
            MemberList = new ObservableCollection<MemberInfo>();

            dbConnect = new DB_CONNECTOR();

            LoadMembers();
        }

        public void OnBackBtn(object obj)
        {
            new KakaoAPI().Show();

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
                    string query = "SELECT sequence_id, user_name, user_id, user_email, user_tel, user_auth, cre_date, user_info FROM members;"; // your_table_name을 실제 테이블 이름으로 변경하세요
                    //MySqlCommand cmd = new MySqlCommand(query, conn);
                    //MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    //DataTable dt = new DataTable();
                    //adapter.Fill(dt);

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
