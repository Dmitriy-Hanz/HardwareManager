using HardwareManager.infrastructure.utils.Base;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    public class HeadphonesView
    {
        public bool IsWired { get; set; }
        public bool HasMicro { get; set; }

        public HeadphonesView() { }
        public HeadphonesView(Headphones obj)
        {
            if (obj != null)
            {
                IsWired = obj.IsWired;
                HasMicro = obj.HasMicro;
            }
        }

        public Headphones Convert()
        {
            return new Headphones
            {
                IsWired = IsWired,
                HasMicro = HasMicro
            };
        }
        public void Merge(Headphones target)
        {
            target.IsWired = IsWired;
            target.HasMicro = HasMicro;
        }

        public bool Validate()
        {
            return true;
        }
    }
}
