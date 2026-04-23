using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.utils.database;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    class CPUView
    {
        public string Name { get; set; }
        public int? CoreCount { get; set; }
        public string CashMemoryType { get; set; }
        public double? CashMemoryValue { get; set; }


        public CPUView() { }
        public CPUView(CPU obj)
        {
            if (obj != null)
            { 
                Name = obj.Name;
                CoreCount = obj.CoreCount == 0? null : obj.CoreCount;
                CashMemoryType = obj.CashMemoryType;
                CashMemoryValue = obj.CashMemoryValue == 0 ? null : obj.CashMemoryValue;
            }
        }

        public CPU Convert()
        {
            return new CPU
            {
                Name = Name,
                CoreCount = CoreCount.GetValueOrDefault(0),
                CashMemoryType = CashMemoryType,
                CashMemoryValue = CashMemoryValue.GetValueOrDefault(0)
            };
        }
        public void Merge(CPU target)
        {
            target.Name = Name;
            target.CoreCount = CoreCount.GetValueOrDefault(0);
            target.CashMemoryType = CashMemoryType;
            target.CashMemoryValue = CashMemoryValue.GetValueOrDefault(0);
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(Name, "Модель процессора")) return false;
            if (!TypicalValidations.ValidateIntegerField(CoreCount, "Число ядер", false, false)) return false;
            if (!TypicalValidations.ValidateTextField(CashMemoryType, "Тип кэш-памяти")) return false;
            if (!TypicalValidations.ValidateFloatField(CashMemoryValue, "Объем кэш-памяти", false, false)) return false;
            return true;
        }
    }
}
