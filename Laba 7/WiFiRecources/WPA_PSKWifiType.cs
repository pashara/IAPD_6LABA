using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_7.WiFiRecources
{
    class WPA_PSKWifiType:AWifiType
    {
        public override void Connect()
        {
            template = Properties.Resources.WPAPSK;
            Encryption = "WPAPSK";
            Authentication = "WPAPSK";

            AddProfileIsIsntHas(String.Format(template, ProfileName, Authentication, Encryption, Key));

            base._connect();


        }
    }
}
