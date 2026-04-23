using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.utils.database;
using HardwareManager.model.entity;
using Microsoft.Identity.Client.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    public class RequestView
    {
        private string reasonName;
        private string requestedTypeName;

        public Request Original { get; set; }
        public string ReasonType { get; set; }

        public string ReasonName 
        { 
            get => reasonName;
            set
            {
                reasonName = value;
                ReasonType = StaticCatalogs.RequestReasonNameToRequestReasonType(reasonName);
            }
        }
        public string RequestedType { get; set; }
        public string RequestedTypeName 
        { 
            get => requestedTypeName;
            set
            {
                requestedTypeName = value;
                RequestedType = StaticCatalogs.HardwareNameToHardwareType(requestedTypeName);
            }
        }
        public int? HardwareIN { get; set; }
        public string Status { get; set; }
        public string OwnerWorkPlaceName { get; set; }


        public RequestView() { }
        public RequestView(Request obj) 
        {
            if (obj != null)
            {
                Original = obj;
                ReasonType = obj.ReasonType;
                ReasonName = DefineReasonName();
                RequestedType = obj.RequestedType;
                RequestedTypeName = DefineRequestedTypeName();
                HardwareIN = obj.HardwareIN == 0? null : obj.HardwareIN;
                Status = obj.Status;
            }
        }

        public void Merge(Request target)
        {
            target.ReasonType = ReasonType;
            target.HardwareIN = HardwareIN.GetValueOrDefault(0);
            target.RequestedType = RequestedType;
            target.Status = Status;
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(ReasonName, "Причина запроса")) { return false; }
            if (!TypicalValidations.ValidateTextField(RequestedTypeName, "Тип оборудования")) { return false; }
            if (ReasonName.Equals("Неисправное оборудование") && !(RequestedTypeName.Equals("Кабель") || RequestedTypeName.Equals("Адаптер")))
            {
                if (!TypicalValidations.ValidateIntegerField(HardwareIN, "ИН оборудования")) return false;
                return true;
            }
            return true;
        }

        private string DefineReasonName()
        {
            return ReasonType switch
            {
                "DefectiveHardware" => "Неисправное оборудование",
                "RequiredHardware" => "Необходимо оборудование"
            };
        }

        private string DefineRequestedTypeName()
        {
            return RequestedType switch
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

                "Cable" => "Кабель",
                "AdapterCable" => "Адаптер"
            };
        }
    }
}
