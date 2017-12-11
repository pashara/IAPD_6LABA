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
    }

    
}
