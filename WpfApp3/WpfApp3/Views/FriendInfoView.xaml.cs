using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WpfApp3.Handler;
using WpfApp3.Model;
using static System.Net.Mime.MediaTypeNames;

namespace WpfApp3.Views
{
    public class FriendInfoViewModel : Notifier
    {
        // FriendInfo
        private FriendInfoView _View;
        public FriendInfoViewModel(FriendInfoView view) 
        {
            _View = view;
            GetData();
        }

        private ObservableCollection<FriendInfo> _FriendInfoList;
        public ObservableCollection<FriendInfo> FriendInfoList
        {  
            get { return _FriendInfoList ?? (_FriendInfoList = new ObservableCollection<FriendInfo>()); } 
        }

        private void GetData()
        {
            FriendInfoList.Clear();

            BitmapImage imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Image/img01.png"));
            FriendInfoList.Add(new FriendInfo()
            {
                ImageSrc = imageSource,
                FriendName = "친구1",
                FriendStatusMsg = "친구1상태",
            });

            imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Image/img02.png"));
            FriendInfoList.Add(new FriendInfo()
            {
                ImageSrc = imageSource,
                FriendName = "친구2",
                FriendStatusMsg = "친구2상태",
            });

            imageSource = new BitmapImage(new Uri(@"pack://application:,,,/Image/img03.png"));
            FriendInfoList.Add(new FriendInfo()
            {
                ImageSrc = imageSource,
                FriendName = "친구3",
                FriendStatusMsg = "친구3상태",
            });
        }
    }

    /// <summary>
    /// FriendInfoView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FriendInfoView : UserControl
    {
        public FriendInfoViewModel ViewModel { get; set; }
        public FriendInfoView()
        {
            InitializeComponent();

            FriendInfoViewModel ViewModel = new FriendInfoViewModel(this);
            this.DataContext = ViewModel;
        }
    }
}
