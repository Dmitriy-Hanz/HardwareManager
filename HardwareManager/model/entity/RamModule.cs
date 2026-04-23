using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.entity
{
    public class RamModule : Entity
    {
        public string Model { get; set; }
        public string RamType { get; set; }
        public int Memory { get; set; }

        public RamModule() { }
    }
}
