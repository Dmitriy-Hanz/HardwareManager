using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model
{
    public class Request : Entity
    {
        public string ReasonType { get; set; }
        public string RequestedType { get; set; }
        public int HardwareIN { get; set; }
        public string Status { get; set; }
        
        public Request() { }
    }
}
