using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DocDbGremlinTest.Models
{
    public class Building : Asset
    {
        [JsonProperty(PropertyName = "address")]
        public Address Address { get; set; }
        [JsonProperty(PropertyName = "numberoffloors")]
        public int NumberOfFloors { get; set; }
    }
}