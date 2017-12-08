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
using Laba_7.WiFiRecources;

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


        private List<WiFiPoint> wifiNetworks = new List<WiFiPoint>();
        public List<WiFiPoint> WifiNetworks
        {
            get
            {
                if (wifiNetworks == null)
                    wifiNetworks = new List<WiFiPoint>();
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
            //var sss = new ObservableCollection<WiFiPoint>();
            bool isFindedSelected = false;
            foreach(var asd in wifiNetworks)
            {
                //sss.Add(asd);
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


        private List<WiFiPoint> GetWiFiList()
        {
            var wlanClient = new WlanClient();
            List<WiFiPoint> a = new List<WiFiPoint>();
            foreach (WlanClient.WlanInterface wlanInterface in wlanClient.Interfaces)
            {
                List<Wlan.WlanBssEntry> wlanBssEntries = wlanInterface.GetNetworkBssList().ToList();
                List<Wlan.WlanAvailableNetwork> wlanAvalibleEntries = wlanInterface.GetAvailableNetworkList(0).ToList();


                foreach (Wlan.WlanAvailableNetwork wlanEntry in wlanAvalibleEntries)
                {

                    Wlan.WlanBssEntry network = new Wlan.WlanBssEntry();
                    bool finded = false;
                    foreach (Wlan.WlanBssEntry n in wlanBssEntries)
                    {

                        if (System.Text.ASCIIEncoding.ASCII.GetString(n.dot11Ssid.SSID).Equals(System.Text.ASCIIEncoding.ASCII.GetString(wlanEntry.dot11Ssid.SSID)))
                        {
                            network = n;
                            finded = true;
                            wlanBssEntries.Remove(n);
                            break;
                        }
                    }


                    if (!finded)
                        continue;
                    string mac = (finded)?ConvertMac(network.dot11Bssid):"";
                    WiFiPoint point = new WiFiPoint
                    {
                        SSID = System.Text.ASCIIEncoding.ASCII.GetString(wlanEntry.dot11Ssid.SSID).Trim((char)0),
                        Quality = wlanEntry.wlanSignalQuality.ToString(),
                        AuthAlgorithm = wlanEntry.dot11DefaultAuthAlgorithm.ToString().Trim((char)0),
                        Mac = mac,
                        HasConnected = (wlanEntry.flags.HasFlag(Wlan.WlanAvailableNetworkFlags.Connected)) ? true : false,
                        HasProfile = (wlanEntry.flags.HasFlag(Wlan.WlanAvailableNetworkFlags.HasProfile)) ? true : false,
                    };
                    a.Add(point);
                }

            }

            
            return a;
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
            try
            {
                WlanClient client = new WlanClient();


                foreach (WlanClient.WlanInterface wlanIface in client.Interfaces)
                {
                    Wlan.WlanAvailableNetwork[] wlanBssEntries = wlanIface.GetAvailableNetworkList(0);
                    foreach (Wlan.WlanAvailableNetwork network in wlanBssEntries)
                    {
                        String profileName = System.Text.ASCIIEncoding.ASCII.GetString(network.dot11Ssid.SSID).Trim((char)0);

                        // подключаемся именно к выбранной сети
                        if (SelectedWiFiItem.SSID.Equals(profileName))
                        {
                            

                            WiFiConnection.Connect(network, wlanIface, Password);
                            return;

                            
                            String strTemplate = "";
                            String authentication = "";
                            String encryption = "";
                            String key = "";
                            
                        switch (network.dot11DefaultAuthAlgorithm)
                            {
                                case Wlan.Dot11AuthAlgorithm.IEEE80211_Open:
                                    
                                    strTemplate = Properties.Resources.OPEN_NETWORK;

                                    authentication = "open";

                                    encryption = network.dot11DefaultCipherAlgorithm.ToString().Trim((char)0);
                                    encryption = "AES";
                                    
                                    if (!network.flags.HasFlag(Wlan.WlanAvailableNetworkFlags.HasProfile))
                                    {
                                        String profileXml = String.Format(strTemplate, profileName, authentication, encryption, "");
                                        wlanIface.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
                                    }

                                    //SMTH
                                    wlanIface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, profileName);
                                    return;
                                    break;
                                case Wlan.Dot11AuthAlgorithm.WPA:
                                    //Not found.
                                    break;

                                case Wlan.Dot11AuthAlgorithm.WPA_PSK: // WPA_PSK

                                    strTemplate = Properties.Resources.WPAPSK;

                                    authentication = "WPAPSK";

                                    encryption = network.dot11DefaultCipherAlgorithm.ToString().Trim((char)0);
                                    encryption = "AES";
                                    // пароль к сети
                                    key = Password;
                                    //34096811

                                    if (!network.flags.HasFlag(Wlan.WlanAvailableNetworkFlags.HasProfile) || key.Length > 0)
                                    {
                                        String profileXml = String.Format(strTemplate, profileName, authentication, encryption, key);
                                        wlanIface.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
                                    }

                                    //SMTH
                                    wlanIface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, profileName);
                                    return;
                                    break;

                                case Wlan.Dot11AuthAlgorithm.RSNA_PSK: // WPA_PSK

                                    strTemplate = Properties.Resources.WPAPSK;

                                    authentication = "WPA2PSK";

                                    //encryption = network.dot11DefaultCipherAlgorithm.ToString().Trim((char)0);
                                    encryption = "AES";
                                    // пароль к сети
                                    key = Password;
                                    //34096811

                                    if (!network.flags.HasFlag(Wlan.WlanAvailableNetworkFlags.HasProfile) && key.Length > 0)
                                    {
                                        String profileXml = String.Format(strTemplate, profileName, authentication, encryption, key);
                                        wlanIface.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
                                    }

                                    //SMTH
                                    wlanIface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, profileName);
                                    return;
                                    break;

                                default:
                                    break;
                            }





                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
