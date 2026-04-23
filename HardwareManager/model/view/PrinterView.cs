using HardwareManager.infrastructure.utils.Base;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    public class PrinterView
    {
        public string MaxFormat { get; set; } //Максимальный формат печати
        public string PrintTechnology { get; set; } //Технология печати
        public string Color { get; set; } //Цвет печати
        public bool DoubleSidedPrinting { get; set; } //Автоматическая двусторонняя печать
        public bool HasScanner { get; set; } //Сканнер

        public PrinterView() { }
        public PrinterView(Printer obj)
        {
            if (obj != null)
            {
                MaxFormat = obj.MaxFormat;
                PrintTechnology = obj.PrintTechnology;
                Color = obj.Color;
                DoubleSidedPrinting = obj.DoubleSidedPrinting;
                HasScanner = obj.HasScanner;
            }
        }

        public Printer Convert()
        {
            return new Printer
            {
                MaxFormat = MaxFormat,
                PrintTechnology = PrintTechnology,
                Color = Color,
                DoubleSidedPrinting = DoubleSidedPrinting,
                HasScanner = HasScanner
            };
        }
        public void Merge(Printer target)
        {
            target.MaxFormat = MaxFormat;
            target.PrintTechnology = PrintTechnology;
            target.Color = Color;
            target.DoubleSidedPrinting = DoubleSidedPrinting;
            target.HasScanner = HasScanner;
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(MaxFormat, "Максимальный формат печати")) return false;
            if (!TypicalValidations.ValidateTextField(MaxFormat, "Технология печати")) return false;
            if (!TypicalValidations.ValidateTextField(MaxFormat, "Цвет печати")) return false;
            return true;
        }
    }
}
