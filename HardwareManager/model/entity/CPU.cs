using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.entity
{
    public class CPU : Entity
    {
        public string Name { get; set; }
        public int CoreCount { get; set; }
        public string CashMemoryType { get; set; }
        public double CashMemoryValue { get; set; }
        public CPU() { }
    }
}
