using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.entity
{
    public class Cabinet : Entity //Класс для кабинетов
    {
        public string Name { get; set; }
        public List<WorkPlace> WorkPlaceList { get; set; }

        public Cabinet()
        {
            WorkPlaceList = [];
        }
        public Cabinet(long id, string name)
        {
            Id = id;
            Name = name;
            WorkPlaceList = [];
        }
    }
}
