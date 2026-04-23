using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HardwareManager.model.entity
{
    public class AdapterCable : Wire
    {
        public string FirstConnectorType { get; set; } //Тип разъёма (первый конец кабеля)
        public string SecondConnectorType { get; set; } //Тип разъёма (второй конец кабеля)
        public bool IsInputFirst { get; set; } //Входной разъём/Выходной разъём (первый конец кабеля)
        public bool IsInputSecond { get; set; } //Входной разъём/Выходной разъём (второй конец кабеля)
        public bool HasWire { get; set; } // Проводной/корпусный


        public AdapterCable() { }
        public AdapterCable(AdapterCable source)
        {
            Id = source.Id;
            InventorialStatus = source.InventorialStatus;
            HardwareImage = source.HardwareImage.CloneByteArray();
            Model = source.Model;
            ItemCount = source.ItemCount;
            FirstConnectorType = source.FirstConnectorType;
            SecondConnectorType = source.SecondConnectorType;
            IsInputFirst = source.IsInputFirst;
            IsInputSecond = source.IsInputSecond;
            HasWire = source.HasWire;
        }
        public override AdapterCable Clone()
        {
            return new AdapterCable(this);
        }
    }
}
