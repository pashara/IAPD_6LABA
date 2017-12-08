using NativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_7.WiFiRecources
{
    class RSNA_PSKWiFiType : AWifiType
    {
        public override void Connect()
        {
            
            template = Properties.Resources.WPAPSK;
            Encryption = "AES";
            Authentication = "WPA2PSK";

            AddProfileIsIsntHas(String.Format(template, ProfileName, Authentication, Encryption, Key));

            base._connect();

        }
    }
}
