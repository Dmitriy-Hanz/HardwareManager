using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.utils.database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.entity
{
    public class MonitorView
    {
        public double? Diagonal { get; set; }
        public string AspectRatio { get; set; }
        public string Matrix { get; set; }
        public int? Frequency { get; set; }
        public string Resolution { get; set; }

        public MonitorView() { }
        public MonitorView(Monitor obj)
        {
            if (obj != null)
            {
                Diagonal = obj.Diagonal == 0 ? null : obj.Diagonal;
                AspectRatio = obj.AspectRatio;
                Matrix = obj.Matrix;
                Frequency = obj.Frequency == 0 ? null : obj.Frequency;
                Resolution = obj.Resolution;
            }
        }

        public Monitor Convert()
        {
            return new Monitor
            {
                Diagonal = Diagonal.GetValueOrDefault(0),
                AspectRatio = AspectRatio,
                Matrix = Matrix,
                Frequency = Frequency.GetValueOrDefault(0),
                Resolution = Resolution
            };
        }
        public void Merge(Monitor target)
        {
            target.Diagonal = Diagonal.GetValueOrDefault(0);
            target.AspectRatio = AspectRatio;
            target.Matrix = Matrix;
            target.Frequency = Frequency.GetValueOrDefault(0);
            target.Resolution = Resolution;
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateFloatField(Diagonal, "Диагональ")) return false;
            if (!TypicalValidations.ValidateTextField(AspectRatio, "Соотношение сторон")) return false;
            if (!TypicalValidations.ValidateTextField(Matrix, "Тип матрицы")) return false;
            if (!TypicalValidations.ValidateIntegerField(Frequency, "Частота матрицы")) return false;
            if (!TypicalValidations.ValidateTextField(Resolution, "Разрешение")) return false;
            return true;
        }
    }
}
