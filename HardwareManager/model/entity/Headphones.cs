using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HardwareManager.model.entity
{
    public class Headphones : Hardware
    {
        public bool IsWired { get; set; }
        public bool HasMicro { get; set; }

        public Headphones() { }
    }
}
