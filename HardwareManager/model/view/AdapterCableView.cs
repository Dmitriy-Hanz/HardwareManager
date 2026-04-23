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
    public class AdapterCableView
    {
        public AdapterCable Original { get; set; }
        public string FirstConnectorType { get; set; } //Тип разъёма (первый конец кабеля)
        public string SecondConnectorType { get; set; } //Тип разъёма (второй конец кабеля)
        public bool IsInputFirst { get; set; } //Входной разъём/Выходной разъём (первый конец кабеля)
        public bool IsInputSecond { get; set; } //Входной разъём/Выходной разъём (второй конец кабеля)
        public bool HasWire { get; set; } // Проводной/корпусный

        public AdapterCableView() { }

        public AdapterCableView(AdapterCable obj)
        {
            if (obj != null)
            {
                Original = obj;
                FirstConnectorType = obj.FirstConnectorType;
                SecondConnectorType = obj.SecondConnectorType;
                IsInputFirst = obj.IsInputFirst;
                IsInputSecond = obj.IsInputSecond;
                HasWire = obj.HasWire;
            }
        }

        public void Merge(AdapterCable target)
        {
            target.FirstConnectorType = FirstConnectorType;
            target.SecondConnectorType = SecondConnectorType;
            target.IsInputFirst = IsInputFirst;
            target.IsInputSecond = IsInputSecond;
            target.HasWire = HasWire;
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(FirstConnectorType, "Тип разъёма")) return false;
            if (!TypicalValidations.ValidateTextField(SecondConnectorType, "Тип разъёма")) return false;
            if (DataBaseService.CheckAdapterCableDuplicates(this) != 0)
            {
                TypicalValidations.ShowValidationMessageBox("Адаптер с такими характеристиками уже существует");
                return false;
            }
            return true;
        }
        public bool Validate(WorkPlace wp)
        {
            if (!TypicalValidations.ValidateTextField(FirstConnectorType, "Тип разъёма")) return false;
            if (!TypicalValidations.ValidateTextField(SecondConnectorType, "Тип разъёма")) return false;
            if (DataBaseService.CheckAdapterCableDuplicatesOnWorkplace(this,wp) != 0)
            {
                TypicalValidations.ShowValidationMessageBox("Адаптер с такими характеристиками уже существует");
                return false;
            }
            return true;
        }
    }
}
