using HardwareManager.infrastructure.utils.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HardwareManager.model.entity
{
    public class KeyboardView
    {
        public bool IsWired { get; set; }

        public KeyboardView() { }
        public KeyboardView(Keyboard obj)
        {
            if (obj != null)
            {
                IsWired = obj.IsWired;
            }
        }

        public Keyboard Convert()
        {
            return new Keyboard
            {
                IsWired = IsWired
            };
        }
        public void Merge(Keyboard target)
        {
            target.IsWired = IsWired;
        }

        public bool Validate()
        {
            return true;
        }
    }
}
