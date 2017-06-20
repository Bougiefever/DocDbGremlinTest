using System.Collections.Generic;

namespace DocDbGremlinTest.Models
{
    public class FloorView
    {
        public Floor Floor { get; set; }
        public IEnumerable<Room> Rooms { get; set; }
    }
}