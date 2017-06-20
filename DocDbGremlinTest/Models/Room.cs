using Newtonsoft.Json;

namespace DocDbGremlinTest.Models
{
    public class Room : Item
    {
        [JsonProperty(PropertyName = "roomnumber")]
        public int RoomNumber { get; set; }
    }
}