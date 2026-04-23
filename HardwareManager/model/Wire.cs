using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model
{
    public abstract class Wire : Entity
    {
        public string InventorialStatus { get; set; }
        public byte[] HardwareImage { get; set; }
        public string Model { get; set; }
        public int ItemCount { get; set; }

        public abstract Wire Clone();
    }
}
