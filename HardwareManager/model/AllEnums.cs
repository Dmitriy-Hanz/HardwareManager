using HardwareManager.infrastructure.utils.converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace HardwareManager.model
{
    public enum EAllHardware
    {
        Computer = 100,
        Monitor = 101
    }
    public enum EInventorialStatus
    {
        InWork = 100,
        InStock = 101,
        Removed = 102,
        Defective = 103
    }
    public enum ECashMemoryType
    {
        L1 = 100,
        L2 = 101,
        L3 = 102
    }
}
