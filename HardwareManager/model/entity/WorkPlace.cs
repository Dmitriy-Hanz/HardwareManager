using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace HardwareManager.model.entity
{
    public class WorkPlace : Entity//Класс для рабочих мест
    {
        public string Name { get; set; }
        public List<Hardware> HardwareList { get; set; }
        public List<Wire> WireList { get; set; }
        public List<Request> Requests { get; set; }


        public WorkPlace()
        {
            HardwareList = [];
            WireList = [];
            Requests = [];
        }

        public WorkPlace(long id, string name)
        {
            Id = id;
            Name = name;
            HardwareList = [];
            WireList = [];
            Requests = [];
        }
    }
}
