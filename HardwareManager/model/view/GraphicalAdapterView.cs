using HardwareManager.infrastructure.utils.Base;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    class GraphicalAdapterView
    {
        public string Model { get; set; }
        public string GraphicalProcessor { get; set; }
        public double? RamMemory { get; set; }


        public GraphicalAdapterView() { }
        public GraphicalAdapterView(GraphicalAdapter obj)
        {
            if (obj != null)
            {
                Model = obj.Model;
                GraphicalProcessor = obj.GraphicalProcessor;
                RamMemory = obj.RamMemory == 0 ? null : obj.RamMemory;
            }
        }

        public GraphicalAdapter Convert()
        {
            return new GraphicalAdapter
            {
                Model = Model,
                GraphicalProcessor = GraphicalProcessor,
                RamMemory = RamMemory.GetValueOrDefault(0)
            };
        }
        public void Merge(GraphicalAdapter target)
        {
            target.Model = Model;
            target.GraphicalProcessor = GraphicalProcessor;
            target.RamMemory = RamMemory.GetValueOrDefault(0);
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(Model, "Модель графического адаптера")) return false;
            if (!TypicalValidations.ValidateTextField(GraphicalProcessor, "Графический процессор")) return false;
            if (!TypicalValidations.ValidateFloatField(RamMemory, "Видеопамять", false, false)) return false;
            return true;
        }
    }
}
