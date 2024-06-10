using MySql.Data.MySqlClient;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Spatial;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

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

        public void userInsertData(string user_id, string user_password, string user_name, string user_email, string user_tel)//, string user_info, string user_auth)
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
                    string user_info = "관리자";
                    string user_auth = "adsf";
                    string cre_date = DateTime.Now.ToString("yyyy-MM-dd");
                    rdr1.Close(); // DataReader Read 시작 했으면 끝나고 close 무조건 해줘야됨

                    // 데이터 임시 등록
                    //string sql2 = "insert into members values('" + userId+"','"+userPwd+"', '"+userName+"', "+ user_seq + ");";
                    string sql2 = "insert into members values('" + sequence_id + "','" + user_id+"','"+ user_password +"', '"+ user_name +"', '"+ user_email+ "', '" +
                            user_tel  + "', '" + user_info + "', '" + user_auth + "', '" + cre_date + "');";
                    MySqlCommand cmd2 = new MySqlCommand(sql2, conn);
                    cmd2.ExecuteNonQuery();
                    MessageBox.Show("입력 성공");

                    // Mysql DB Table 값 가져오기
/*                    string sql3 = "select * from members";
                    MySqlCommand cmd3 = new MySqlCommand(sql3, conn);
                    MySqlDataReader rdr = cmd3.ExecuteReader();
*/
                    /*
                    while (rdr.Read())
                    {
                        string id = rdr[0].ToString();
                        string pwd = rdr[1].ToString();
                        string name = rdr[2].ToString();
                        string seq = rdr[3].ToString();
                        aa = "id : " + name + ", pwd : " + pwd + ", name : " + name + ", seq : " + seq;
                        
                    }
                    rdr.Close();
                    */

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

        // 로그인 검증
        public string[] userLoginChk(string userId, string userPwd)
        {
            string[] login = {"", ""};

            try
            {
                using (conn = new MySqlConnection(dbInfo()))
                {
                    conn.Open();

                    // 중복 ID 조회
                    string sql1 = "select user_name, sequence_id from members where user_id = '" + userId + "' and user_password = '" + userPwd + "';";

                    MySqlCommand cmd1 = new MySqlCommand(sql1, conn);
                    MySqlDataReader rdr1 = cmd1.ExecuteReader();
                    rdr1.Read(); // 첫번째 값을 가져와야 되기 때문에 한번만 실행

                    string user_name = rdr1[0].ToString();
                    string user_seq = rdr1[1].ToString();

                    login[0] = user_name;
                    login[1] = user_seq;

                    //COALESCE(sequence_id, '');

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
        /*public DataTable LoadData()
        {
            DataTable dataTable = new DataTable();

            
            using (conn = new MySqlConnection(dbInfo()))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT sequence_id,user_name,user_id,user_email,user_tel,user_auth,cre_date,user_info FROM members";
                    MySqlCommand cmd2 = new MySqlCommand(query, conn);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd2);
                    adapter.Fill(dataTable);
                    Console.WriteLine("Rows retrieved: " + dataTable.Rows.Count);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            return dataTable;

        }*/

    }
}
