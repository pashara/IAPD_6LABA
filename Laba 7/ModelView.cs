using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NativeWifi;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Windows;
using System.Threading;
using SimpleWifi;

namespace Laba_7
{
    class ModelView : INotifyPropertyChanged
    {
        public string H1Title => Properties.Resources.H1Title;
        public string WindowTitle => Properties.Resources.WindowTitle;
        public string SendButtonText => Properties.Resources.ConnectButton;
        public string PasswordPlaceholder => Properties.Resources.PasswordPlaceholder;

        WiFiPoint _selectedWiFiItem = new WiFiPoint();
        public WiFiPoint SelectedWiFiItem
        {
            get
            {
                return _selectedWiFiItem;
            }
            set
            {
                _selectedWiFiItem = value;
                OnPropertyChanged(nameof(SelectedWiFiItem));
            }
        }

        string password;
        public string Password
        {
            get
            {
                if (password == null)
                    return "";
                return password;
            }
            set
            {
                password = value;
            }
        }
        
        private Timer timer;

        private ObservableCollection<WiFiPoint> wifiNetworks;
        public ObservableCollection<WiFiPoint> WifiNetworks
        {
            get
            {
                if (wifiNetworks == null)
                    wifiNetworks = new ObservableCollection<WiFiPoint>();
                return wifiNetworks;
            }
        }
  
        public event PropertyChangedEventHandler PropertyChanged;


        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        
        private void UpdateWiFiList()
        {
            WiFiPoint a = SelectedWiFiItem;

            var wifiNetworks1 = GetWiFiList();
            wifiNetworks = wifiNetworks1;
            bool isFindedSelected = false;
            foreach(var asd in wifiNetworks)
            {
                if(!isFindedSelected)
                {
                    if (asd.SSID.Equals(a.SSID))
                    {
                        SelectedWiFiItem = asd;
                    }
                }
            }

            OnPropertyChanged(nameof(WifiNetworks));
        }

        private void OnLanChangeList(Object stateInfo)
        {
            UpdateWiFiList();
        }

        private ObservableCollection<WiFiPoint> GetWiFiList()
        {
            ObservableCollection<WiFiPoint> a = new ObservableCollection<WiFiPoint>();
            
            var _wifi = new Wifi();
            var _wlanClient = new WlanClient();

            ObservableCollection<WiFiPoint> networks = new ObservableCollection<WiFiPoint>();
            List<AccessPoint> accessPoints = _wifi.GetAccessPoints();

            foreach (AccessPoint accessPoint in accessPoints)
            {
                networks.Add(new WiFiPoint(accessPoint.Name, accessPoint.SignalStrength.ToString() + "%", accessPoint.ToString(), GetBssId(accessPoint), accessPoint.IsSecure, accessPoint.IsConnected));
            }
            return networks;
        }
        private List<string> GetBssId(AccessPoint accessPoint)
        {
            var wlanInterface = _wlanClient.Interfaces.FirstOrDefault();
            return wlanInterface?.GetNetworkBssList()
                .Where(x => Encoding.ASCII.GetString(x.dot11Ssid.SSID, 0, (int)x.dot11Ssid.SSIDLength).Equals(accessPoint.Name))
                .Select(y => Dot11BSSTostring(y)).ToList();
        }

        private string Dot11BSSTostring(Wlan.WlanBssEntry entry)
        {
            StringBuilder bssIdBuilder = new StringBuilder();
            foreach (byte bssByte in entry.dot11Bssid)
            {
                bssIdBuilder.Append(bssByte.ToString("X"));
                bssIdBuilder.Append("-");
            }
            bssIdBuilder.Remove(bssIdBuilder.Length - 1, 1);
            return bssIdBuilder.ToString();
        }



        public ModelView()
        {
            TimeSpan dueTime = new TimeSpan(0, 0, 0);
            TimeSpan period = new TimeSpan(0, 0, 5);

            TimerCallback timeCB = new TimerCallback(OnLanChangeList);
            timer = new Timer(timeCB, null, dueTime, period);

            
            MainWindow.OnConnectClick += MainWindow_OnConnectClick;



        }


        private string ConvertMac(byte[] macAddr)
        {
            var macAddrLen = (uint)macAddr.Length;
            var str = new string[(int)macAddrLen];

            string mac = "";
            for (int i = 0; i < macAddrLen; i++)
            {
                mac += ((i == 0) ? "" : ":") + macAddr[i].ToString("x2").ToUpper();
            }
            return mac;
        }


        private void ConnectToNetwork(WiFiPoint SelectedWiFiItem)
        {
            SelectedWiFiItem.Connect(Password);
        }



        private void MainWindow_OnConnectClick()
        {

            WiFiPoint item = SelectedWiFiItem;
            if (item != null)
            {
                MessageBoxResult result = MessageBox.Show(String.Format(Properties.Resources.ConnectConfirmText, item.SSID), "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    ConnectToNetwork(item);
                }
                UpdateWiFiList();
            }
        }

    }
}
