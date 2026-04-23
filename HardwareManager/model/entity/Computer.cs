using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.entity
{
    public class Computer : Hardware
    {
        public List<GraphicalAdapter> GraphicalAdapters { get; set; }
        public List<PhisicalDisk> PhisicalDisks { get; set; }
        public List<RamModule> RamModules { get; set; }
        public CPU Cpu { get; set; }

        public Computer()
        {
            GraphicalAdapters = [];
            PhisicalDisks = [];
            RamModules = [];
        }

    }
}
