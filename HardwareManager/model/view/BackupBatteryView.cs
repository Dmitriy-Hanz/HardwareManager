using HardwareManager.infrastructure.utils.Base;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    public class BackupBatteryView
    {
        public int? SocketsCount { get; set; } //Количество розеток с резервным питанием
        public int? BatteryLife { get; set; } //Время работы без подзарядки (в минутах)
        public string BatteryType { get; set; } //Тип батареи

        public BackupBatteryView() { }
        public BackupBatteryView(BackupBattery obj)
        {
            if (obj != null)
            {
                SocketsCount = obj.SocketsCount == 0 ? null : obj.SocketsCount;
                BatteryLife = obj.BatteryLife == 0 ? null : obj.BatteryLife;
                BatteryType = obj.BatteryType;
            }
        }

        public BackupBattery Convert()
        {
            return new BackupBattery
            {
                SocketsCount = SocketsCount.GetValueOrDefault(0),
                BatteryLife = BatteryLife.GetValueOrDefault(0),
                BatteryType = BatteryType
            };
        }
        public void Merge(BackupBattery target)
        {
            target.SocketsCount = SocketsCount.GetValueOrDefault(0);
            target.BatteryLife = BatteryLife.GetValueOrDefault(0);
            target.BatteryType = BatteryType;
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateIntegerField(SocketsCount, "Количество розеток с резервным питанием")) return false;
            if (!TypicalValidations.ValidateIntegerField(BatteryLife, "Время работы без подзарядки")) return false;
            if (!TypicalValidations.ValidateTextField(BatteryType, "Тип батареи")) return false;
            return true;
        }
    }
}
