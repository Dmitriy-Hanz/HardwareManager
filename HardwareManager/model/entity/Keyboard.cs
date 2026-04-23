using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HardwareManager.model.entity
{
    public class Keyboard : Hardware
    {
        //public string Model { get; set; }
        public bool IsWired { get; set; }

        public Keyboard() { }
    }
}
