using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.utils.database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    public class WireView
    {
        public Wire Original { get; set; }
        public string WireType { get; set; }
        public string WireName { get; set; }
        public string InventorialStatus { get; set; }
        public byte[] HardwareImage { get; set; }
        public string Model { get; set; }
        public int? ItemCount { get; set; }

        public WireView() { }
        public WireView(string wireType) 
        {
            WireType = wireType;
            WireName = DefineWireName();
        }

        public WireView(Wire wire)
        {
            Original = wire;
            WireType = wire.GetType().Name;
            WireName = DefineWireName();
            Model = wire.Model;
            ItemCount = wire.ItemCount == 0? null : wire.ItemCount;
            InventorialStatus = wire.InventorialStatus;
            HardwareImage = wire.HardwareImage;
        }

        public void Merge(Wire target)
        {
            target.Model = Model;
            target.ItemCount = (int)ItemCount;
            target.InventorialStatus = InventorialStatus;
            target.HardwareImage = HardwareImage;
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(Model, WireName)) return false;
            if (!TypicalValidations.ValidateIntegerField(ItemCount, "Количество", false, false)) return false;
            return true;
        }

        public void Originalize()
        {
            WireType = Original.GetType().Name;
            WireName = DefineWireName();
            Model = Original.Model;
            ItemCount = Original.ItemCount == 0 ? null : Original.ItemCount;
            InventorialStatus = Original.InventorialStatus;
            HardwareImage = Original.HardwareImage;
        }

        private string DefineWireName()
        {
            return WireType switch
            {
                "Cable" => "Кабель",
                "AdapterCable" => "Адаптер",
                _ => "Провод",
            };
        }
    }
}
