using BackEndInfrsastructure.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiExample.DB.Data.Domain
{
    public class WebServiceUser : Model<int>
    {
        public string USERNAME { get; set; }

        public string PASSWORD { get; set; }

        public int? SERVERSEED { get; set; } // Assuming it can be nullable

        public int? CLIENTSEED { get; set; } // Assuming it can be nullable

        public int? SESSIONID { get; set; } // Assuming it can be nullable

        public DateTime? LASTACTIVITYTIME { get; set; } // Assuming it can be nullable

        public string MACKEY { get; set; }

        public string PINKEY { get; set; }

        public string MASTERKEY { get; set; }

        public long OUTGOINGCHANNELID { get; set; }

        public string PROTOCOLNAME { get; set; }

        public bool CARDPRESENT { get; set; }

        public bool CVV2CHECK { get; set; }

        public string WEBSERVICECODE { get; set; }

        public string WEBSERVICEACCEPTORCODE { get; set; }

        public string SERVERIP { get; set; }
    }
}
