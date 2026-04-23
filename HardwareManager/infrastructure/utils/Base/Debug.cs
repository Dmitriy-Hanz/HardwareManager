using HardwareManager.viewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HardwareManager.infrastructure.utils.Base
{
    public static class Debug
    {
        private const bool DEBUG_MODE = true;

        private static bool DebugMode() {  return DEBUG_MODE; }
        public static void DoAction(bool activate = true)
        {
            if (!DebugMode() || !activate) { return; }
        }

    }
}
