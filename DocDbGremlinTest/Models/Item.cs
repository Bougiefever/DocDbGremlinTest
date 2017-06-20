namespace DocDbGremlinTest.Models
{
    public class Item : IItem
    {
        public string Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public ItemType ItemType { get; set; }
    }
}