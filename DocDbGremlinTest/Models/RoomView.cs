using System.Collections.Generic;

namespace DocDbGremlinTest.Models
{
    public class RoomView
    {
        public Room Room { get; set; }
        public IEnumerable<FurnitureItem> FurnitureItems { get; set; }
        public IEnumerable<ElectronicItem> ElectronicItems { get; set; }
    }
}