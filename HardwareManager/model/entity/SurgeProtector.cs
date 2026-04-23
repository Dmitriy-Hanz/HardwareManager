using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HardwareManager.model.entity
{
    public class SurgeProtector : Hardware
    {
        public int SocketsCount { get; set; } //Количество розеток
        public bool HasSwitcher { get; set; } // Наличие выключателя
        public bool HasWire { get; set; } // Проводной

        public SurgeProtector() { }
    }
}
