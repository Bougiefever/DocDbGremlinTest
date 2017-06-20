using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DocDbGremlinTest.Models
{
    public abstract class Asset : Item, IAsset
    {
        [JsonProperty(PropertyName = "value")]
        public decimal Value { get; set; }
    }
}