using SimpleWifi;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_7
{
    public class WiFiPoint
    {
        public string SSID
        {
            get;
            set;
        }

        public string Quality
        {
            get;
            set;
        }

        public string AuthAlgorithm
        {
            get;
            set;
        }


        public string Mac
        {
            get;
            set;
        }


        public bool HasProfile
        {
            get;
            set;
        }

        public bool HasConnected
        {
            get;
            set;
        }


        public void Connect(string password)
        {
            Wifi wifi = new Wifi();
            IEnumerable<AccessPoint> accessPoints = wifi.GetAccessPoints();

            foreach (AccessPoint ap in accessPoints)
            {
                String profileName = ap.Name.Trim((char)0);
                if (ap.Name.Equals(SSID))
                {
                    if (!ap.IsConnected)
                    {
                        AuthRequest authRequest = new AuthRequest(ap);
                        if (ap.IsSecure && password == "")
                        {
                            authRequest.Password = password;
                        }

                        ap.Connect(authRequest);

                    }
                }
            }
        }
    }

    
}
