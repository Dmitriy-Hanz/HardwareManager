using HardwareManager.infrastructure.utils.Base;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    class RamModuleView
    {
        public string Model { get; set; }
        public string RamType { get; set; }
        public int? Memory { get; set; }


        public RamModuleView() { }
        public RamModuleView(RamModule obj)
        {
            if (obj != null)
            {
                Model = obj.Model;
                RamType = obj.RamType;
                Memory = obj.Memory == 0? null: obj.Memory;
            }
        }

        public RamModule Convert()
        {
            return new RamModule
            {
                Model = Model,
                RamType = RamType,
                Memory = Memory.GetValueOrDefault(0)
            };
        }
        public void Merge(RamModule target)
        {
            target.Model = Model;
            target.RamType = RamType;
            target.Memory = Memory.GetValueOrDefault(0);
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(Model, "Модель модуля оперативной памяти")) return false;
            if (!TypicalValidations.ValidateTextField(RamType, "Тип оперативной памяти")) return false;
            if (!TypicalValidations.ValidateIntegerField(Memory, "Объем оперативной памяти", false, false)) return false;
            return true;
        }
    }
}
