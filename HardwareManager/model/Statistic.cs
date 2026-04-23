using HardwareManager.infrastructure.utils.database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model
{
    public class Statistic
    {
        public int All { get; set; }
        public int HardwareAll { get; set; }
        public int WireAll { get; set; }
        public int AllInInventory { get; set; }
        public int AllOnWorkplaces { get; set; }
        public int AllDefective { get; set; }
        public int HardwareInInventory { get; set; }
        public int HardwareOnWorkplaces { get; set; }
        public int WireInInventory { get; set; }
        public int WireOnWorkplaces { get; set; }

        public Statistic() { }
        public void Load()
        {
            DataBaseService.GetStatistic(this);
        }
    }
}
