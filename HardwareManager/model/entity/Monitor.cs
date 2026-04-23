using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.entity
{
    public class Monitor : Hardware
    {
        public double Diagonal { get; set; }
        public string AspectRatio { get; set; }
        public string Matrix { get; set; }
        public int Frequency { get; set; }
        public string Resolution { get; set; }

        public Monitor()
        {
            
        }
    }
}
