using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.entity
{
    public class Printer : Hardware
    {
        public string MaxFormat { get; set; }
        public string PrintTechnology { get; set; } //Технология печати
        public string Color { get; set; }
        public bool DoubleSidedPrinting { get; set; } //Автоматическая двусторонняя печать
        public bool HasScanner { get; set; } //Сканнер

        public Printer() { }
    }
}
