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


        public void Connect()
        {

        }


    }

    public class WiFiPoints : ObservableCollection<WiFiPoint>
    {
        public WiFiPoints()
        {
            Add(new WiFiPoint());
            /*Add(new Customer("Chris", "Ashton",
                    "34 West Fifth Street, Apartment 67"));
            Add(new Customer("Cassie", "Hicks",
                    "56 East Seventh Street, Apartment 89"));
            Add(new Customer("Guido", "Pica",
                    "78 South Ninth Street, Apartment 10"));*/
        }





        
    }
}
