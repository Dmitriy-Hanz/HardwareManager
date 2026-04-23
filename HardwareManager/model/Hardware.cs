using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model
{
    public abstract class Hardware : Entity
    {
        public int InventorialNumber { get; set; }
        public string InventorialStatus { get; set; }
        public byte[] HardwareImage { get; set; }
        public string Model { get; set; }
    }
}
