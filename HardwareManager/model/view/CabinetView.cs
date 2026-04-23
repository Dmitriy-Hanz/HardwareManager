using HardwareManager.infrastructure.utils.Base;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    public class CabinetView
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public CabinetView() { }
        public CabinetView(Cabinet cab)
        {
            Id = cab.Id;
            Name = cab.Name;
        }

        public Cabinet Convert()
        {
            return new Cabinet(Id, Name);
        }
        public void Merge(Cabinet target)
        {
            target.Id = Id;
            target.Name = Name;
        }
        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(Name, "Название")) return false;
            return true;
        }
    }
}
