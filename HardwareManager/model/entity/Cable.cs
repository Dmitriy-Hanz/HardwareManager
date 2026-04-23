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
    public class Cable : Wire
    {
        public string ConnectorType { get; set; } //Тип разъёма
        public bool IsInputFirst { get; set; } //Входной разъём/Выходной разъём
        public bool IsInputSecond { get; set; } //Входной разъём/Выходной разъём

        public Cable() { }
        public Cable(Cable source)
        {
            Id = source.Id;
            InventorialStatus = source.InventorialStatus;
            HardwareImage = source.HardwareImage == null ? null : source.HardwareImage.CloneByteArray();
            Model = source.Model;
            ItemCount = source.ItemCount;
            ConnectorType = source.ConnectorType;
            IsInputFirst = source.IsInputFirst;
            IsInputSecond = source.IsInputSecond;
        }
        public override Cable Clone()
        {
            return new Cable(this);
        }
    }
}
