using System.Collections.Generic;
using Newtonsoft.Json;

namespace DocDbGremlinTest.Models
{
    public class BuildingView
    {
        public Building Building { get; set; }

        public IEnumerable<Floor> Floors { get; set; }

        public decimal TotalValue { get; set; }
    }
}