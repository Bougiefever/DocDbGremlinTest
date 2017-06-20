using Newtonsoft.Json;
using WebGrease;

namespace DocDbGremlinTest.Models
{
    public interface IItem
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }
        [JsonProperty(PropertyName = "accountid")]
        int AccountId { get; set; }

        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
        [JsonProperty(PropertyName = "itemtype")]
        ItemType ItemType { get; set; }
    }
}