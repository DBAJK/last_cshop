using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp3.Model;
using WpfApp3.Views;

namespace WpfApp3.DB
{

    public class DB_CONNECTOR
    {
        MySqlConnection conn;
        string dbInfo()
        {
            string dbServer = "127.0.0.1";
            string port = "3306";
            string dbDatabase = "cproject";
            string dbUid = "root";
            string dbPwd = "1234";
            string dbSslMode = "none";
            string Conn = "Server=" + dbServer + ";" + "Port=" + port + ";" + "Database=" + dbDatabase + ";" + "Uid=" + dbUid + ";" + "Pwd=" + dbPwd + ";" + "SslMode=" + dbSslMode;
            return Conn;
        }

        public void userInsertData(string user_id, string user_password, string user_name, string user_email, string user_tel, string user_auth, string user_info)
        {
            try
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    conn.Open();

                    // user_seq 조회
                    string sql1 = "select COALESCE(max(sequence_id), 0) + 1 from members;";
                    MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                    MySqlDataReader rdr1 = cmd1.ExecuteReader();
                    rdr1.Read(); // 첫번째 값을 가져와야 되기 때문에 한번만 실행

                    string sequence_id = rdr1[0].ToString();
                    string cre_date = DateTime.Now.ToString("yyyy-MM-dd");
                    rdr1.Close(); // DataReader Read 시작 했으면 끝나고 close 무조건 해줘야됨

                    // 데이터 임시 등록
                    //string sql2 = "insert into members values('" + userId+"','"+userPwd+"', '"+userName+"', "+ user_seq + ");";
                    string sql2 = "insert into members values('" + sequence_id + "','" + user_id+"','"+ user_password +"', '"+ user_name +"', '"+ user_email+ "', '" +
                            user_tel  + "', '" + user_info + "', '" + user_auth + "', '" + cre_date + "');";
                    MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show("입력 성공");

                    conn.Close();   // 연결 종료
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        public bool userIdDuplication(string userId)
        {
            bool err = false;

            try
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    conn.Open();

                    // 중복 ID 조회
                    string sql1 = "select count(*) from members where user_id = '" + userId+"';";
                    MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                    MySqlDataReader rdr1 = cmd1.ExecuteReader();
                    rdr1.Read(); // 첫번째 값을 가져와야 되기 때문에 한번만 실행

                    int user_id_cnt = int.Parse(rdr1[0].ToString());

                    err = user_id_cnt == 0;

                    conn.Close();   // 연결 종료
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            return err;
        }

        public UserInfo _UserInfoView;
        // 로그인 검증
        public string[] userLoginChk(string userId, string userPwd)
        {
            string[] login = {"", "", "", ""};
            
            try
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    conn.Open();

                    // 중복 ID 조회
                    string sql1 = "select user_name, sequence_id, user_auth, user_info from members where user_id = '" + userId + "' and user_password = '" + userPwd + "';";

                    MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                    MySqlDataReader rdr1 = cmd1.ExecuteReader();
                    rdr1.Read(); // 첫번째 값을 가져와야 되기 때문에 한번만 실행

                    string user_name = rdr1[0].ToString();
                    string user_seq = rdr1[1].ToString();
                    string user_auth = rdr1[2].ToString();
                    string user_info = rdr1[3].ToString();

                    login[0] = user_name;
                    login[1] = user_seq;
                    login[2] = user_auth;
                    login[3] = user_info;
                    rdr1.Close();   // 연결 종료
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            return login;
        }

        public void UserNameChangeUpdate(string userSeq, string userName)
        {
            try
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    conn.Open();

                    // 중복 ID 조회
                    string sql1 = "update members set user_name = '" + userName+"' where sequence_id = "+userSeq;
                    MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                    MySqlDataReader rdr1 = cmd1.ExecuteReader();
                    rdr1.Read(); // 첫번째 값을 가져와야 되기 때문에 한번만 실행

                    conn.Close();   // 연결 종료
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }


        public MySqlConnection MemberViewConn()
        {
            if(conn==null)
            {
                conn= new MySqlConnection(dbInfo());
            }
            return conn;
        }

        public KakaoAPI _kakaoAPIView;
        /// <summary>
        /// 민원 신청 뷰
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="lng"></param>
        /// <param name="lat"></param>
        /// <returns></returns>
        public string[] complaintList(string userId, double lng, double lat)
        {
            string[] cpList = { "","","","","","" } ;

            try
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    conn.Open();

                    // 중복 ID 조회
                    string sql1 = "select complaints_key, cp_contents, cp_region, DATE_FORMAT(cp_date, '%Y-%m-%d') as cp_date, cp_state from complaints " +
                        "where sequence_id = '" + userId + "' and CP_LNG = '" + lng + "' and CP_LAT = '" + lat + "';";

                    MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                    MySqlDataReader rdr1 = cmd1.ExecuteReader();
                    rdr1.Read(); // 첫번째 값을 가져와야 되기 때문에 한번만 실행

                    string complaints_key = rdr1[0].ToString();
                    string cp_contents = rdr1[1].ToString();
                    string cp_region = rdr1[2].ToString();
                    string cp_date = rdr1[3].ToString();
                    string cp_state = rdr1[4].ToString();

                    cpList[0] = complaints_key;
                    cpList[1] = cp_contents;
                    cpList[2] = cp_region;
                    cpList[3] = cp_date;
                    cpList[4] = cp_state;
                    rdr1.Close();   // 연결 종료
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return cpList;
        }

        public bool complaintChk(string userId, double lng, double lat)
        {
            bool err = false;

            try
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    conn.Open();

                    // 중복 ID 조회
                    string sql1 = "select count(*) from complaints " +
                        "where sequence_id = '" + userId + "' and CP_LNG = '" + lng + "' and CP_LAT = '" + lat + "';";
                    MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                    MySqlDataReader rdr1 = cmd1.ExecuteReader();
                    rdr1.Read(); // 첫번째 값을 가져와야 되기 때문에 한번만 실행

                    int user_id_cnt = int.Parse(rdr1[0].ToString());

                    err = user_id_cnt == 0;

                    conn.Close();   // 연결 종료

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            return err;
        }


        public void complaintDatainsert(string userid, string ContentTextBox, string RegionTextBox, string DatePicker, string StatusData, double lng, double lat)
        {          
            try
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    conn.Open();

                    // user_seq 조회
                    string sql1 = "select COALESCE(max(complaints_key), 0) + 1 from complaints;";
                    MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                    MySqlDataReader rdr1 = cmd1.ExecuteReader();
                    rdr1.Read(); // 첫번째 값을 가져와야 되기 때문에 한번만 실행

                    string complaints_key = rdr1[0].ToString();
                    string FinalDateBox = "2000-00-00";
                    StatusData = "신청";
                    rdr1.Close(); // DataReader Read 시작 했으면 끝나고 close 무조건 해줘야됨

                    string sql2 = "insert into complaints values('" + userid + "','" + complaints_key + "','" + ContentTextBox + "','" + RegionTextBox + "', '" + DatePicker + "', '" + StatusData + "', '" +
                            FinalDateBox + "', '" + lng + "', '" + lat  + "');";
                    MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show("입력 성공");

                    conn.Close();   // 연결 종료
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
