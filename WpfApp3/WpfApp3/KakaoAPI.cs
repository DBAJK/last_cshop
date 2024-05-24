using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp3
{
    public class KakaoAPI
    {
        public static List<Locale> Search(string text)
        {
            List<Locale> list = new List<Locale>();

            string url = "https://dapi.kakao.com/v2/local/search/keyword.json";
            string query = $"{url}?query={text}";
            string restAPIKey = "bd419e294343736f22452d5ee0d2309a";
            string Header = $"KakaoAK {restAPIKey}";
            WebRequest request = WebRequest.Create(query);
            request.Headers.Add("Authorization", Header);

            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string json = reader.ReadToEnd();

            JavaScriptSerializer js = new JavaScriptSerializer();

            dynamic dob = js.Deserialize<dynamic>(json);
            dynamic docs = dob["documents"];
            object[] buf = docs;
            int length = buf.Length;

            for (int i = 0; i < length; i++)
            {
                string lname = docs[i]["place_name"];
                double x = double.Parse(docs[i]["x"]);
                double y = double.Parse(docs[i]["y"]);
                list.Add(new Locale(lname, x, y));
            }
            return list;
        }
    }
}
