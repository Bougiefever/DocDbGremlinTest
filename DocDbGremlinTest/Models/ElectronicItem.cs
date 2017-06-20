using System;
using Newtonsoft.Json;

namespace DocDbGremlinTest.Models
{
    public class ElectronicItem : Asset
    {
        [JsonProperty(PropertyName = "electronictype")]
        public ElectronicType ElectronicType { get; set; }
        [JsonProperty(PropertyName = "serialnumber")]
        public string SerialNumber { get; set; }
        [JsonProperty(PropertyName = "purchasedate")]
        public DateTime PurchaseDate { get; set; }

    }
}