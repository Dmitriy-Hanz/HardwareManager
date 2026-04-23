using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HardwareManager.model.entity
{
    public class BackupBattery : Hardware
    {
        public int SocketsCount { get; set; } //Количество розеток с резервным питанием
        public int BatteryLife { get; set; } //Время работы без подзарядки
        public string BatteryType { get; set; } //Тип батареи

        public BackupBattery() { }
    }
}
