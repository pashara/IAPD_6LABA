using Laba_7.WiFiRecources;
using NativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_7
{
    static class WiFiConnection
    {

        

        public static void Connect(Wlan.WlanAvailableNetwork network, WlanClient.WlanInterface wlanIface,String key = "")
        {
            
            AWifiType a = null;
            String profileName = System.Text.ASCIIEncoding.ASCII.GetString(network.dot11Ssid.SSID).Trim((char)0);
            


            switch (network.dot11DefaultAuthAlgorithm)
            {
                case Wlan.Dot11AuthAlgorithm.IEEE80211_Open:
                    a = new OpenWiFiType
                    {
                        Network = network,
                        ProfileName = profileName,
                        WlanIface = wlanIface,
                        Authentication = "",
                        Encryption = ""
                    };
                    break;

                case Wlan.Dot11AuthAlgorithm.WPA_PSK:
                    a = new WPA_PSKWifiType
                    {
                        Network = network,
                        ProfileName = profileName,
                        WlanIface = wlanIface,
                        Authentication = "",
                        Encryption = "",
                        Key = key,
                    };
                    break;

                case Wlan.Dot11AuthAlgorithm.RSNA_PSK:
                    a = new RSNA_PSKWiFiType
                    {
                        Network = network,
                        ProfileName = profileName,
                        WlanIface = wlanIface,
                        Authentication = "",
                        Encryption = "",
                        Key = key,
                    };
                    break;

                default:
                    a = null;
                    break;
            }

            if (a != null)
                a.Connect();
            else
                return;

        }
        
    }
}
