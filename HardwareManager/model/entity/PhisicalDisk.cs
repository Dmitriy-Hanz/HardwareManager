using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.entity
{
    public class PhisicalDisk : Entity
    {
        public string Model { get; set; }
        public string DiskType { get; set; }
        public string Interface { get; set; }
        public string FormFactor { get; set; }
        public int Memory { get; set; }

        public PhisicalDisk() { }
    }
}
