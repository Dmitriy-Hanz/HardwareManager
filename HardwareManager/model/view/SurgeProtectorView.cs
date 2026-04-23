using HardwareManager.infrastructure.utils.Base;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    public class SurgeProtectorView
    {
        public int? SocketsCount { get; set; } //Количество розеток
        public bool HasSwitcher { get; set; } // Наличие выключателя
        public bool HasWire { get; set; } // Проводной

        public SurgeProtectorView() { }
        public SurgeProtectorView(SurgeProtector obj)
        {
            if (obj != null)
            {
                SocketsCount = obj.SocketsCount == 0 ? null : obj.SocketsCount;
                HasSwitcher = obj.HasSwitcher;
                HasWire = obj.HasWire;
            }
        }

        public SurgeProtector Convert()
        {
            return new SurgeProtector
            {
                SocketsCount = SocketsCount.GetValueOrDefault(0),
                HasSwitcher = HasSwitcher,
                HasWire = HasWire
            };
        }
        public void Merge(SurgeProtector target)
        {
            target.SocketsCount = SocketsCount.GetValueOrDefault(0);
            target.HasSwitcher = HasSwitcher;
            target.HasWire = HasWire;
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateIntegerField(SocketsCount, "Количество розеток")) return false;
            return true;
        }
    }
}
