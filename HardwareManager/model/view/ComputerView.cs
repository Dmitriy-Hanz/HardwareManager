using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.utils.database;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    class ComputerView
    {
        public CPUView Cpu { get; set; }
        public ObservableCollection<GraphicalAdapterView> GraphicalAdapters { get; set; }
        public List<PhisicalDiskView> PhisicalDisks { get; set; }
        public List<RamModuleView> RamModules { get; set; }

        public ComputerView() 
        {
            GraphicalAdapters = [];
            PhisicalDisks = [];
            RamModules = [];
        }
        public ComputerView(Computer obj)
        {
            if (obj != null)
            {
                GraphicalAdapters = [];
                PhisicalDisks = [];
                RamModules = [];

                if (obj.Cpu != null)
                {
                    Cpu = new CPUView(obj.Cpu);
                }
                for (int i = 0; i < obj.GraphicalAdapters.Count; i++)
                {
                    GraphicalAdapters.Add(new GraphicalAdapterView(obj.GraphicalAdapters[i]));
                }
                for (int i = 0; i < obj.PhisicalDisks.Count; i++)
                {
                    PhisicalDisks.Add(new PhisicalDiskView(obj.PhisicalDisks[i]));
                }
                for (int i = 0; i < obj.RamModules.Count; i++)
                {
                    RamModules.Add(new RamModuleView(obj.RamModules[i]));
                }
            }
        }

        public Computer Convert()
        {
            Computer resulut = new();
            if (Cpu != null)
            {
                resulut.Cpu = Cpu.Convert();
            }
            for (int i = 0; i < GraphicalAdapters.Count; i++)
            {
                resulut.GraphicalAdapters.Add(GraphicalAdapters[i].Convert());
            }
            for (int i = 0; i < PhisicalDisks.Count; i++)
            {
                resulut.PhisicalDisks.Add(PhisicalDisks[i].Convert());
            }
            for (int i = 0; i < RamModules.Count; i++)
            {
                resulut.RamModules.Add(RamModules[i].Convert());
            }
            return resulut;
        }
        public void Merge(Computer target)
        {
            if (Cpu == null)
            {
                target.Cpu = null;
            }
            else
            {
                if (target.Cpu == null)
                {
                    target.Cpu = Cpu.Convert();
                }
                else
                {
                    Cpu.Merge(target.Cpu);
                }
            }

            while (target.GraphicalAdapters.Count > GraphicalAdapters.Count)
            {
                target.GraphicalAdapters.RemoveAt(0);
            }
            while (target.GraphicalAdapters.Count < GraphicalAdapters.Count)
            {
                target.GraphicalAdapters.Add(new());
            }
            for (int i = 0; i < target.GraphicalAdapters.Count; i++)
            {
                GraphicalAdapters[i].Merge(target.GraphicalAdapters[i]);
            }

            while (target.PhisicalDisks.Count > PhisicalDisks.Count)
            {
                target.PhisicalDisks.RemoveAt(0);
            }
            while (target.PhisicalDisks.Count < PhisicalDisks.Count)
            {
                target.PhisicalDisks.Add(new());
            }
            for (int i = 0; i < PhisicalDisks.Count; i++)
            {
                PhisicalDisks[i].Merge(target.PhisicalDisks[i]);
            }

            while (target.RamModules.Count > RamModules.Count)
            {
                target.RamModules.RemoveAt(0);
            }
            while (target.RamModules.Count < RamModules.Count)
            {
                target.RamModules.Add(new());
            }
            for (int i = 0; i < RamModules.Count; i++)
            {
                RamModules[i].Merge(target.RamModules[i]);
            }
        }

        public bool Validate()
        {
            if (Cpu != null && !Cpu.Validate()) return false;
            for (int i = 0; i < GraphicalAdapters.Count; i++)
            {
                if (!GraphicalAdapters[i].Validate()) return false;
            }
            for (int i = 0; i < PhisicalDisks.Count; i++)
            {
                if (!PhisicalDisks[i].Validate()) return false;
            }
            for (int i = 0; i < RamModules.Count; i++)
            {
                if (!RamModules[i].Validate()) return false;
            }
            return true;
        }

        //for (int i = 0, f = 0; i < target.GraphicalAdapters.Count; i++)
        //{
        //    if (target.GraphicalAdapters[i].Id == 0)
        //    {
        //        break;
        //    }
        //    for (int j = 0; j < GraphicalAdapters.Count; j++)
        //    {
        //        if (target.GraphicalAdapters[i].Id == GraphicalAdapters[j].Id)
        //        {
        //            f++;
        //            GraphicalAdapters[j].Merge(target.GraphicalAdapters[i]);
        //            continue;
        //        }
        //        if (GraphicalAdapters[j].Id == 0)
        //        {
        //            target.GraphicalAdapters.Add(GraphicalAdapters[j].Convert());
        //        }
        //    }
        //    if (f != 0)
        //    {
        //        f = 0;
        //        continue;
        //    }
        //    target.GraphicalAdapters.Remove(target.GraphicalAdapters[i]);
        //    i = i - 1;
        //}



    }
}
