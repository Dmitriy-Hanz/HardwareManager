using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace HardwareManager.model
{
    public class StaticCatalogs
    {
        public static StaticCatalogsExemplar staticCatalogsExemplar = new();
        public static List<string> CashMemoryTypeCatalog { get; set; } =
        [
            "L1",
            "L2",
            "L3"
        ];
        public static List<string> DiskTypeCatalog { get; set; } =
        [
            "HDD",
            "SSD"
        ];
        public static List<string> FormFactorSSDCatalog { get; set; } = new()
        {
            "M.2\"",
            "2.5\"",
            "mSATA"
        };
        public static List<string> FormFactorHDDCatalog { get; set; } = new()
        {
            "2.5\""
        };
        public static List<string> FormFactorCatalog { get; set; } = new()
        {
            "M.2\"",
            "2.5\"",
            "mSATA"
        };
        public static List<string> InterfaceSSDCatalog { get; set; } = new()
        {
            "PCI Express 3.0 x2",
            "PCI Express 3.0 x4",
            "PCI Express 3.0 x8",
            "PCI Express 4.0 x4",
            "PCI Express 4.0 x8",
            "PCI Express 4.0 x16",
            "PCI Express 5.0 x2",
            "PCI Express 5.0 x4",
            "SAS 3",
            "SAS 4 (24G)",
            "SATA 1.0",
            "SATA 3.0",
            "U.2",
            "U.3",
            "mSATA"
        };
        public static List<string> InterfaceHDDCatalog { get; set; } = new()
        {
            "SATA 3.0 (6Gbps)",
            "SATA 2.0 (3Gbps)",
            "SATA",
            "SAS 3.0 (12Gbps)",
            "SAS 2.0 (6Gbps)",
            "SAS 1.0 (3Gbps)",
            "SAS",
            "SCSI",
            "IDE",
            "Fibre Channel (4Gbps)"
        };
        public static List<string> DiskInterfaceCatalog { get; set; } = new()
        {
            "SATA 3.0 (6Gbps)",
            "SATA 2.0 (3Gbps)",
            "SATA",
            "SAS 3.0 (12Gbps)",
            "SAS 2.0 (6Gbps)",
            "SAS 1.0 (3Gbps)",
            "SAS",
            "SCSI",
            "IDE",
            "Fibre Channel (4Gbps)",
            "PCI Express 3.0 x2",
            "PCI Express 3.0 x4",
            "PCI Express 3.0 x8",
            "PCI Express 4.0 x4",
            "PCI Express 4.0 x8",
            "PCI Express 4.0 x16",
            "PCI Express 5.0 x2",
            "PCI Express 5.0 x4",
            "SAS 3",
            "SAS 4 (24G)",
            "SATA 1.0",
            "SATA 3.0",
            "U.2",
            "U.3",
            "mSATA"
        };


        public static List<string> RamTypeCatalog { get; set; } = new()
        {
            "DDR DIMM",
            //"DDR DIMM Registered",
            //"DDR SO-DIMM",
            "DDR2 DIMM",
            //"DDR2 DIMM Registered",
            //"DDR2 SO-DIMM",
            "DDR3 DIMM",
            //"DDR3 DIMM Registered",
            //"DDR3 SO-DIMM",
            "DDR4 DIMM",
            //"DDR4 DIMM Registered",
            //"DDR4 SO-DIMM",
            //"DDR4 SO-DIMM Registered",
            "DDR5 DIMM"
            //"DDR5 DIMM Registered"
            //"DDR5 SO-DIMM"
        };

        public static List<string> AspectRatioCatalog { get; set; } =
        [
            "3:2",
            "3:4",
            "32:9",
            "4:3",
            "5:4",
            "16:9",
            "16:10",
            "16:18",
            "21:9",
            "24:10"
        ];
        public static List<string> MatrixCatalog { get; set; } =
        [
            "IPS",
            "OLED",
            "TN+Film",
            "VA",
            "PLS"
        ];
        public static List<string> ResolutionCatalog { get; set; } =
        [
            "1920x550",
            "1024x768",
            "1366x768",
            "1440x900",
            "1600x900",
            "1280x1024",
            "1680x1050",
            "1920x1080",
            "2560x1080",
            "3840x1080",
            "1900x1200",
            "2240x1400",
            "2560x1440",
            "3440x1440",
            "3840x1440",
            "5120x1440",
            "2560x1600",
            "3840x1600",
            "1536x2048",
            "3840x2160",
            "5120x2160",
            "7680x2160",
            "3840x2560",
            "2560x2880",
            "5120x2880",
            "6016x3384",
            "6144x3456",
            "7680x4320"
        ];

        public static List<string> CameraMaxResolutionCatalog { get; set; } =
        [
            "4096x2160",
            "3840x2160",
            "2560x1440",
            "3840x1080",
            "2592x1944",
            "2592x1520",
            "2160x1440",
            "1920x1080",
            "1600x1200",
            "1280x720",
            "640x480"
        ];

        public static List<string> PrinterMaxFormatCatalog { get; set; } =
        [
            "A6 (105x148 мм)",
            "A4 (210x297 мм)",
            "A3 (297x420 мм)",
            "A2 (420x594 мм)",
            "A1 (594x841 мм)",
        ];
        public static List<string> PrintTechnologyCatalog { get; set; } =
        [
            "Лазерный",
            "Струйный",
            "Светодиодный"
        ];
        
        public static List<string> PrinterColorCatalog { get; set; } =
        [
            "Цветной",
            "Черно-белый"
        ];     
        
        public static List<string> WiredTelephoneConnectionTypeCatalog { get; set; } =
        [
            "Телефонная линия/модем",
            "Ethernet (IP-телефон)"
        ];      
        
        public static List<string> BatteryTypeCatalog { get; set; } =
        [
            "Свинцово-кислотная",
            "VRLA",
            "Li-ion"
        ];

        public static List<string> WireConnectorTypeCatalog { get; set; } =
        [
            "Розеточный штекер",
            "USB",
            "HDMI",
            "VGA"
        ];

        public static List<string> RequestReasonTypeCatalog { get; set; } =
        [
            "DefectiveHardware",
            "RequiredHardware"
        ];        
        public static List<string> RequestReasonNameCatalog { get; set; } =
        [
            "Неисправное оборудование",
            "Необходимо оборудование"
        ];
        public static List<string> AllHardwareNamesCatalog { get; set; } =
        [
            "Компьютер",
            "Монитор",
            "Клавиатура",
            "Мышь",
            "Камера",
            "Наушники",
            "Принтер",
            "Телефон",
            "Источник бесперебойного питания",
            "Разветвитель",
            "Кабель",
            "Адаптер",
        ];
        public static List<string> AllHardwareTypesCatalog { get; set; } =
        [
            "Computer",
            "Monitor",
            "Keyboard",
            "Mouse",
            "Camera",
            "Headphones",
            "Printer",
            "WiredTelephone",
            "BackupBattery",
            "SurgeProtector",
            "Cable",
            "AdapterCable"
        ];
        public static List<string> RequestStatusCatalog { get; set; } =
        [
            "В обработке",
            "Отменено",
            "Одобрено"
        ];        
        public static List<string> InventorialStatusCatalog { get; set; } =
        [
            "В работе",
            "На складе",
            "Списан",
            "Неисправен"
        ];
        public static List<string> HistoryOperationCatalog { get; set; } =
        [
            "",
            "Оборудование создано",
            "Оборудование изменено",
            "Оборудование закреплено",
            "Оборудование откреплено",
            "Оборудование списано",
        ];



        public static void RewriteCollectionElements(ObservableCollection<string> target, List<string> catalog)
        {
            if (target != null)
            {
                target.Clear();
                foreach (string catalogItem in catalog)
                {
                    target.Add(catalogItem);
                } 
            }
        }

        public static string EInventorialStatusToString(EInventorialStatus inventorialStatus)
        {
            return inventorialStatus switch
            {
                EInventorialStatus.InWork => "В работе",
                EInventorialStatus.InStock => "На складе",
                EInventorialStatus.Removed => "Списан",
                EInventorialStatus.Defective => "Неисправен"
            };
        }

        public static string RequestReasonNameToRequestReasonType(string requestReasonName)
        {
            return requestReasonName switch
            {
                "Неисправное оборудование" => "DefectiveHardware",
                "Необходимо оборудование" => "RequiredHardware"
            };
        }
        public static string HardwareNameToHardwareType(string hardwareName)
        {
            return hardwareName switch
            {
                "Компьютер" => "Computer",
                "Монитор" => "Monitor",
                "Клавиатура" => "Keyboard",
                "Мышь" => "Mouse",

                "Камера" => "Camera",
                "Наушники" => "Headphones",

                "Принтер" => "Printer",
                "Телефон" => "WiredTelephone",

                "Источник бесперебойного питания" => "BackupBattery",
                "Разветвитель" => "SurgeProtector",

                "Кабель" => "Cable",
                "Адаптер" => "AdapterCable"
            };
        }
        public static string HardwareTypeToHardwareName(string hardwareName)
        {
            return hardwareName switch
            {
                "Computer" => "Компьютер",
                "Monitor" => "Монитор",
                "Keyboard" => "Клавиатура",
                "Mouse" => "Мышь",

                "Camera" => "Камера",
                "Headphones" => "Наушники",

                "Printer" => "Принтер",
                "WiredTelephone" => "Телефон",

                "BackupBattery" => "Источник бесперебойного питания",
                "SurgeProtector" => "Разветвитель",

                "Cable" => "Кабель",
                "AdapterCable" => "Адаптер",
            };
        }
    }
}
