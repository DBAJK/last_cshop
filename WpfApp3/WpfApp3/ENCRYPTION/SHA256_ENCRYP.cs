using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3.ENCRYPTION
{
    public class SHA256_ENCRYP
    {
        private string SHA256Hash(string pwd)
        {
            System.Security.Cryptography.SHA256 sha = new System.Security.Cryptography.SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(pwd));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hash) 
            {
                sb.AppendFormat("{0:x2}", b);
            }

            return sb.ToString();
        }

        public string Connect(string pwd)
        {
            string sha256 = SHA256Hash(pwd);

            return sha256;
        }
    }
}
