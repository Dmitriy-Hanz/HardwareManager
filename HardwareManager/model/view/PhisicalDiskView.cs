using HardwareManager.infrastructure.utils.Base;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    class PhisicalDiskView
    {
        public ObservableCollection<string> FormFactorCatalog { get; set; }
        public ObservableCollection<string> InterfaceCatalog { get; set; }



        public string Model { get; set; }
        private string diskType;
        public string DiskType 
        { 
            get => diskType;
            set
            {
                if (value != null && !value.Equals(diskType))
                {
                    diskType = value;
                    if (diskType.Equals("HDD"))
                    {
                        StaticCatalogs.RewriteCollectionElements(FormFactorCatalog, StaticCatalogs.FormFactorHDDCatalog);
                        StaticCatalogs.RewriteCollectionElements(InterfaceCatalog, StaticCatalogs.InterfaceHDDCatalog);
                    }
                    if (diskType.Equals("SSD"))
                    {
                        StaticCatalogs.RewriteCollectionElements(FormFactorCatalog, StaticCatalogs.FormFactorSSDCatalog);
                        StaticCatalogs.RewriteCollectionElements(InterfaceCatalog, StaticCatalogs.InterfaceSSDCatalog);
                    }
                }
            }
        }
        public string Interface { get; set; }
        public string FormFactor { get; set; }
        public int? Memory { get; set; }

        public PhisicalDiskView() 
        {
            FormFactorCatalog = [];
            InterfaceCatalog = [];
        }
        public PhisicalDiskView(PhisicalDisk obj)
        {
            if (obj != null)
            {
                FormFactorCatalog = [];
                InterfaceCatalog = [];
                Model = obj.Model;
                DiskType = obj.DiskType;
                Interface = obj.Interface;
                FormFactor = obj.FormFactor;
                Memory = obj.Memory == 0? null : obj.Memory;
            }
        }

        public PhisicalDisk Convert()
        {
            return new PhisicalDisk
            {
                Model = Model,
                DiskType = DiskType,
                Interface = Interface,
                FormFactor = FormFactor,
                Memory = Memory.GetValueOrDefault(0)
            };
        }
        public void Merge(PhisicalDisk target)
        {
            target.Model = Model;
            target.DiskType = DiskType;
            target.Interface = Interface;
            target.FormFactor = FormFactor;
            target.Memory = Memory.GetValueOrDefault(0);
        }

        public bool Validate()
        {
            if (!TypicalValidations.ValidateTextField(Model, "Модель физического диска")) return false;
            if (!TypicalValidations.ValidateTextField(DiskType, "Тип физического диска")) return false;
            if (!TypicalValidations.ValidateTextField(Interface, "Интерфейс физического диска")) return false;
            if (!TypicalValidations.ValidateTextField(FormFactor, "Форм-фактор физического диска")) return false;
            if (!TypicalValidations.ValidateFloatField(Memory, "Объем памяти физического диска", false, false)) return false;
            return true;
        }
    }
}
