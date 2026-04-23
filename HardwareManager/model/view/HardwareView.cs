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
    public class HardwareView
    {
        public Hardware Original { get; set; }
        public string HardwareType { get; set; }
        public string HardwareName { get; set; }
        public string Model { get; set; }
        public int? InventorialNumber { get; set; }
        public string InventorialStatus { get; set; }
        public byte[] HardwareImage { get; set; }

        public HardwareView() { }
        public HardwareView(string hardwareType) 
        {
            HardwareType = hardwareType;
            HardwareName = DefineHardwareName();
        }

        public HardwareView(Hardware hardware)
        {
            Original = hardware;
            HardwareType = hardware.GetType().Name;
            HardwareName = DefineHardwareName();
            Model = hardware.Model;
            InventorialNumber = hardware.InventorialNumber == 0? null : hardware.InventorialNumber;
            InventorialStatus = hardware.InventorialStatus;
            HardwareImage = hardware.HardwareImage;
        }

        public void Merge(Hardware target)
        {
            target.Model = Model;
            target.InventorialNumber = (int)InventorialNumber;
            target.InventorialStatus = InventorialStatus;
            target.HardwareImage = HardwareImage;
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(Model, HardwareName)) return false;
            if (!TypicalValidations.ValidateTextField(InventorialNumber.ToString(), "Инвентарный номер")) return false;
            if (InventorialNumber < 100000000 || InventorialNumber > 1000000000-1 || InventorialNumber<0)
            {
                TypicalValidations.ShowValidationMessageBox("Введенный инвентарный номер не соответствует формату");
                return false;
            }
            if (!DataBaseService.CheckInventorialNumber((int)InventorialNumber))
            {
                TypicalValidations.ShowValidationMessageBox("Оборудование с введенным инвентарным номером уже существует в базе данных");
                return false;
            }

            return true;
        }
        public bool ValidateWithoutIN()
        {
            if (!TypicalValidations.ValidateTextField(Model, HardwareName)) return false;
            return true;
        }


        public void Originalize()
        {
            HardwareType = Original.GetType().Name;
            HardwareName = DefineHardwareName();
            Model = Original.Model;
            InventorialNumber = Original.InventorialNumber == 0 ? null : Original.InventorialNumber;
            InventorialStatus = Original.InventorialStatus;
            HardwareImage = Original.HardwareImage;
        }


        private string DefineHardwareName()
        {
            return HardwareType switch
            {
                "Computer" => "Компьютер",
                "Monitor" => "Монитор",
                "Keyboard" => "Клавиатура",
                "Mouse" => "Мышь",

                "Camera" => "Камера",
                "Headphones" => "Наушники",

                "Printer" => "Принтер",
                "WiredTelephone" => "Телефон",

                "BackupBattery" => "Источник бесперебойного питания",
                "SurgeProtector" => "Разветвитель",
                _ => "Оборудование",
            };
        }
    }
}
