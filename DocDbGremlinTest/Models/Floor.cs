using Newtonsoft.Json;

namespace DocDbGremlinTest.Models
{
    public class Floor : Item
    {
        [JsonProperty(PropertyName = "floornumber")]
        public int FloorNumber { get; set; }
        [JsonProperty(PropertyName = "numberofrooms")]
        public int NumberOfRooms { get; set; }
    }
}