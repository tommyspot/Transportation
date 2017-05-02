using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Collections.ObjectModel;

namespace Transportation
{
    public class WagonSettlementReportData
    {
        public string Code { get; set; }
        public long TotalAmount { get; set; }
        public long TotalPayment { get; set; }
        public long Profit { get; set; }
        
        public JObject ToJson()
        {
            JObject json = new JObject();
            json["code"] = Code;
            json["totalAmount"] = TotalAmount;
            json["totalPayment"] = TotalPayment;
            json["profit"] = Profit;
            return json;
        }
    }
}
