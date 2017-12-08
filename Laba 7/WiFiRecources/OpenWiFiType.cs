using NativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_7.WiFiRecources
{
    class OpenWiFiType:AWifiType
    {
        


        public override void Connect()
        {
            template = Properties.Resources.OPEN_NETWORK;
            Encryption = "AES";
            Authentication = "open";

            AddProfileIsIsntHas(String.Format(template, ProfileName, Authentication, Encryption, ""));

            base._connect();

        }
    }
}
