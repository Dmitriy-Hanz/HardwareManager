using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace HardwareManager.model
{
    public class StaticCatalogsExemplar
    {
        public List<string> CashMemoryTypeCatalog { get; set; } = StaticCatalogs.CashMemoryTypeCatalog;
        public List<string> DiskTypeCatalog { get; set; } = StaticCatalogs.DiskTypeCatalog;
        public List<string> FormFactorSSDCatalog { get; set; } = new()
        {
            "M.2\"",
            "2.5\"",
            "mSATA"
        };
        public List<string> FormFactorHDDCatalog { get; set; } = new ()
        {
            "2.5\""
        };
}
}
