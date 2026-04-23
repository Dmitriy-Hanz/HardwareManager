using HardwareManager.infrastructure.utils.Base;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    public class WiredTelephoneView
    {
        public string ConnectionType { get; set; } //Тип подключения
        public bool HasScreen { get; set; } //ЖК-экран

        public WiredTelephoneView() { }
        public WiredTelephoneView(WiredTelephone obj)
        {
            if (obj != null)
            {
                ConnectionType = obj.ConnectionType;
                HasScreen = obj.HasScreen;
            }
        }

        public WiredTelephone Convert()
        {
            return new WiredTelephone
            {
                ConnectionType = ConnectionType,
                HasScreen = HasScreen
            };
        }
        public void Merge(WiredTelephone target)
        {
            target.ConnectionType = ConnectionType;
            target.HasScreen = HasScreen;
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(ConnectionType, "Тип подключения")) return false;
            return true;
        }
    }
}
