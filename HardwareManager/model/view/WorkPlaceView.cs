using HardwareManager.infrastructure.utils.Base;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    public class WorkPlaceView
    {
        public WorkPlace Original { get; set; }
        public string Name { get; set; }
        public List<HardwareView> HardwareList { get; set; }
        public List<WireView> WireList { get; set; }
        public List<RequestView> Requests { get; set; }


        public WorkPlaceView() { }
        public WorkPlaceView(WorkPlace obj)
        {
            if (obj != null)
            {
                Original = obj;
                Name = obj.Name;
                HardwareList = [];
                WireList = [];
                Requests = [];
                foreach (Hardware item in obj.HardwareList)
                {
                    HardwareList.Add(new HardwareView(item));
                }
                foreach (Wire item in obj.WireList)
                {
                    WireList.Add(new WireView(item));
                }
                foreach (Request item in obj.Requests)
                {
                    Requests.Add(new RequestView(item));
                }
            }
        }
        public WorkPlace Convert()
        {
            return new WorkPlace
            {
                Name = Name,
                HardwareList = [],
                WireList = [],
                Requests = []
            };
        }
        public void Merge(WorkPlace target)
        {
            target.Name = Name;
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(Name, "Название")) return false;
            return true;
        }

        public void OriginalizeWires()
        {
            foreach (WireView item in WireList)
            {
                item.Originalize();
            }
        }
    }
}
