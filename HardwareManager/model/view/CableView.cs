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
    public class CableView
    {
        public Cable Original { get; set; }
        public string ConnectorType { get; set; } //Тип разъёма
        public bool IsInputFirst { get; set; } //Входной разъём/Выходной разъём
        public bool IsInputSecond { get; set; } //Входной разъём/Выходной разъём

        public CableView() { }

        public CableView(Cable obj)
        {
            if (obj != null)
            {
                Original = obj;
                ConnectorType = obj.ConnectorType;
                IsInputFirst = obj.IsInputFirst;
                IsInputSecond = obj.IsInputSecond;
            }
        }

        public Cable Convert()
        {
            Cable result = new();
            result.ConnectorType = ConnectorType;
            result.IsInputFirst = IsInputFirst;
            result.IsInputSecond = IsInputSecond;
            return result;
        }

        public void Merge(Cable target)
        {
            target.ConnectorType = ConnectorType;
            target.IsInputFirst = IsInputFirst;
            target.IsInputSecond = IsInputSecond;
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(ConnectorType, "Тип разъёма")) return false;
            if (DataBaseService.CheckCableDuplicates(this) != 0)
            {
                TypicalValidations.ShowValidationMessageBox("Кабель с такими характеристиками уже существует");
                return false;
            }
            return true;
        }
        public bool Validate(WorkPlace wp)
        {
            if (!TypicalValidations.ValidateTextField(ConnectorType, "Тип разъёма")) return false;
            if (DataBaseService.CheckCableDuplicatesOnWorkplace(this, wp) != 0)
            {
                TypicalValidations.ShowValidationMessageBox("Кабель с такими характеристиками уже существует");
                return false;
            }
            return true;
        }
    }
}
