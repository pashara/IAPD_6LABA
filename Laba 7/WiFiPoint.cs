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

        private AccessPoint accessPoint;
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

        public WiFiPoint(AccessPoint accessPoint)
        {
            this.accessPoint = accessPoint;
        }

        public void Connect()
        {
            if (!accessPoint.IsConnected)
            {
                AuthRequest authRequest = new AuthRequest(accessPoint);
                accessPoint.Connect(authRequest);
            }
    }

    
}
