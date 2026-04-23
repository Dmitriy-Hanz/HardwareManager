using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HardwareManager.model.entity
{
    public class MouseView
    {
        public bool IsWired { get; set; }

        public MouseView() { }
        public MouseView(Mouse obj)
        {
            if (obj != null)
            {
                IsWired = obj.IsWired;
            }
        }

        public Mouse Convert()
        {
            return new Mouse
            {
                IsWired = IsWired
            };
        }
        public void Merge(Mouse target)
        {
            target.IsWired = IsWired;
        }

        public bool Validate()
        {
            return true;
        }
    }
}
