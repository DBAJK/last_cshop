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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using WpfApp3.Handler;
using WpfApp3.DB;
using WpfApp3.ENCRYPTION;
using WpfApp3.Model;
using System.IO;
using System.Net;
//using System.Windows.Forms;


namespace WpfApp3.Views
{
    /// <summary>
    /// KakaoAPI.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class KakaoAPI
    {
        public KakaoAPI()
        {
            InitializeComponent();
        }

        /*private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }



        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            *//*  if (listBox.SelectedIndex != -1)
              {
                  return;
              }
              Locale ml = listBox.SelectedItem as Locale;
              object[] pos = new object[] { ml.Lat, ml.Lng };
              HtmlDocument hdoc = webBrowser.Document;
              hdoc.InvokeScript("setCenter", pos);*//*
        }*/
    }
}
