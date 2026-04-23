using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.entity
{
    public class GraphicalAdapter : Entity
    {
        public string Model { get; set; }
        public string GraphicalProcessor { get; set; }
        public double RamMemory { get; set; }

        public GraphicalAdapter() { }
    }
}
