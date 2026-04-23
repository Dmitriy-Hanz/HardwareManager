using HardwareManager.infrastructure.utils.Base;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    public class CameraView
    {
        public Camera Original {  get; set; }
        public string MaxResolution { get; set; }
        public bool IsRotatable { get; set; } //Поворотный механизм
        public bool HasMicro { get; set; } //Встроенный микрофон

        public CameraView() { }
        public CameraView(Camera obj)
        {
            if (obj != null)
            {
                Original = obj;
                MaxResolution = obj.MaxResolution;
                IsRotatable = obj.IsRotatable;
                HasMicro = obj.HasMicro;
            }
        }

        public Camera Convert()
        {
            return new Camera
            {
                MaxResolution = MaxResolution,
                IsRotatable = IsRotatable,
                HasMicro = HasMicro
            };
        }
        public void Merge(Camera target)
        {
            target.MaxResolution = MaxResolution;
            target.IsRotatable = IsRotatable;
            target.HasMicro = HasMicro;
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(MaxResolution, "Максимальное разрешение")) return false;
            return true;
        }
    }
}
