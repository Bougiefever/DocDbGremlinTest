using System.Security.Permissions;
using Newtonsoft.Json;

namespace DocDbGremlinTest.Models
{
    public class FurnitureItem : Asset
    {
        [JsonProperty(PropertyName = "furnituretype")]
        public FurnitureType FurnitureType { get; set; }
    }
}