using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace HardwareManager.model.entity
{
    public class Camera : Hardware
    {
        public string MaxResolution { get; set; }
        public bool IsRotatable { get; set; } //Поворотный механизм
        public bool HasMicro { get; set; } //Встроенный микрофон

        public Camera() { }
    }
}
