using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HardwareManager.model.entity
{
    public class WiredTelephone : Hardware
    {
        public string ConnectionType { get; set; } //Тип подключения
        public bool HasScreen { get; set; } //ЖК-экран

        public WiredTelephone() { }
    }
}
