using DocDbGremlinTest.Models;

namespace DocDbGremlinTest.Data
{
    public class ItemRepository : DocumentDbRepository<Item>
    {
        public ItemRepository()
        {
        }
    }
}