using Newtonsoft.Json;

namespace DocDbGremlinTest.Models
{
    public class Address
    {
        [JsonProperty(PropertyName = "streetaddress")]
        public string StreetAddress { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "zip")]
        public string Zip { get; set; }
    }
}