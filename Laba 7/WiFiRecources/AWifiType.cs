using NativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_7.WiFiRecources
{
    public abstract class AWifiType
    {

        protected string template;

        public Wlan.WlanAvailableNetwork Network
        {
            get;
            set;
        }

        public string ProfileName
        {
            get;
            set;
        }

        public WlanClient.WlanInterface WlanIface
        {
            get;
            set;
        }

        public string Authentication
        {
            get;
            set;
        }

        public string Encryption
        {
            get;
            set;
        }
        public string Key
        {
            get;
            set;
        }

        public abstract void Connect();

        protected void AddProfileIsIsntHas(String profileXml)
        {
            if (!Network.flags.HasFlag(Wlan.WlanAvailableNetworkFlags.HasProfile))
            {
                WlanIface.SetProfile(Wlan.WlanProfileFlags.AllUser, profileXml, true);
            }

        }

        protected void _connect()
        {
            WlanIface.Connect(Wlan.WlanConnectionMode.Profile, Wlan.Dot11BssType.Any, ProfileName);
        }
    }
}
