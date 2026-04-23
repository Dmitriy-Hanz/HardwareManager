using HardwareManager.infrastructure.viewModel.Base;
using HardwareManager.model.entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Monitor = HardwareManager.model.entity.Monitor;

namespace HardwareManager.model.view
{
    public class SearchCriteria : ViewModelBase
    {
        private string hardwareName;
        private string hardwareClass;
        private string hardwareType;

        public string HardwareName 
        { 
            get => hardwareName;
            set
            {
                hardwareName = value;
                HardwareClass = hardwareName.Equals("Кабель") || hardwareName.Equals("Адаптер") ? "Wire" : "Hardware";
                HardwareType = StaticCatalogs.HardwareNameToHardwareType(hardwareName);
            } 
        }

        public string HardwareClass
        {
            get => hardwareClass;
            set => Set(ref hardwareClass, value);
        }
        public string HardwareType
        {
            get => hardwareClass;
            set => Set(ref hardwareClass, value);
        }
        public string InventorialStatus { get; set; }
        public string Model { get; set; }
        public int? InventorialNumber { get; set; }
        public int? ItemCountMin { get; set; }
        public int? ItemCountMax { get; set; }


        //Компьютер
        //      ЦПУ
        public string CpuModel { get; set; }
        public int? CoreCount { get; set; }
        public string CashMemoryType { get; set; }
        public double? CashMemoryValueMin { get; set; }
        public double? CashMemoryValueMax { get; set; }

        //      Графический адаптер
        public string GraphicalAdapterModel { get; set; }
        public string GraphicalProcessor { get; set; }
        public double? RamMemoryMin { get; set; }
        public double? RamMemoryMax { get; set; }

        //      Физический диск
        public string PhisicalDiskModel { get; set; }
        public string DiskType { get; set; }
        public string DiskInterface { get; set; }
        public string FormFactor { get; set; }
        public int? PhisicalDiskMemoryMin { get; set; }
        public int? PhisicalDiskMemoryMax { get; set; }

        //      Модуль ОЗУ
        public string RamModuleModel { get; set; }
        public string RamType { get; set; }
        public int? RamModuleMemoryMin { get; set; }
        public int? RamModuleMemoryMax { get; set; }


        //Монитор
        public double? Diagonal { get; set; }
        public string AspectRatio { get; set; }
        public string Matrix { get; set; }
        public int? FrequencyMin { get; set; }
        public int? FrequencyMax { get; set; }
        public string Resolution { get; set; }

        //Клавиатура
        public bool KeyboardIsWired { get; set; }
        //Мышь
        public bool MouseIsWired { get; set; }

        //Камера
        public string CameraMaxResolution { get; set; }
        public bool CameraIsRotatable { get; set; }
        public bool CameraHasMicro { get; set; }

        //Наушники
        public bool HeadphonesIsWired { get; set; }
        public bool HeadphonesHasMicro { get; set; }

        //Принтер
        public string PrinterMaxFormat { get; set; } //Максимальный формат печати
        public string PrinterPrintTechnology { get; set; } //Технология печати
        public string PrinterColor { get; set; } //Цвет печати
        public bool PrinterDoubleSidedPrinting { get; set; } //Автоматическая двусторонняя печать
        public bool PrinterHasScanner { get; set; } //Сканнер

        //Телефон
        public string TelephoneConnectionType { get; set; } //Тип подключения
        public bool TelephoneHasScreen { get; set; } //ЖК-экран
        
        //Бесперебойник
        public int? BackupBatterySocketsCount { get; set; } //Количество розеток с резервным питанием
        public int? BatteryLifeMin { get; set; } //Время работы без подзарядки (в минутах)
        public int? BatteryLifeMax { get; set; } //Время работы без подзарядки (в минутах)
        public string BatteryType { get; set; } //Тип батареи

        //Разветвитель
        public int? SurgeProtectorSocketsCount { get; set; } //Количество розеток
        public bool SurgeProtectorHasSwitcher { get; set; } // Наличие выключателя
        public bool SurgeProtectorHasWire { get; set; } // Проводной

        //Кабель
        public string CableConnectorType { get; set; } //Тип разъёма
        public bool CableIsInputFirst { get; set; } //Входной разъём/Выходной разъём
        public bool CableIsInputSecond { get; set; } //Входной разъём/Выходной разъём

        //Адаптер
        public string AdapterFirstConnectorType { get; set; } //Тип разъёма (первый конец кабеля)
        public string AdapterSecondConnectorType { get; set; } //Тип разъёма (второй конец кабеля)
        public bool AdapterIsInputFirst { get; set; } //Входной разъём/Выходной разъём (первый конец кабеля)
        public bool AdapterIsInputSecond { get; set; } //Входной разъём/Выходной разъём (второй конец кабеля)
        public bool AdapterHasWire { get; set; } = true; // Проводной/корпусный



        public static void FilterHardware(SearchCriteria sc, HardwareView obj, ObservableCollection<HardwareView> list)
        {
            if (sc.HardwareName != null && !sc.HardwareName.Equals("") && !sc.HardwareName.Equals(obj.HardwareName)) return;
            if (sc.InventorialStatus != null && !sc.InventorialStatus.Equals("") && !sc.InventorialStatus.Equals(obj.InventorialStatus)) return;
            if (sc.Model != null && !sc.Model.Equals("") && NotContains(obj.Model, sc.Model)) return;
            if (sc.InventorialNumber != null && sc.InventorialNumber != obj.InventorialNumber) return;

            switch (sc.HardwareType)
            {
                case "Computer":
                    if ((obj.Original as Computer).Cpu != null)
                    {
                        if (NotEmpty(sc.CpuModel) && NotContains((obj.Original as Computer).Cpu.Name, sc.CpuModel)) return;
                        if (sc.CoreCount != null && sc.CoreCount != (obj.Original as Computer).Cpu.CoreCount) return;
                        if (NotEmpty(sc.CashMemoryType) && NotEqual((obj.Original as Computer).Cpu.CashMemoryType, sc.CashMemoryType)) return;
                        if (!MatchDiapason((obj.Original as Computer).Cpu.CashMemoryValue, sc.CashMemoryValueMin, sc.CashMemoryValueMax)) return;
                    }
                    else
                    {
                        if (NotEmpty(sc.CpuModel) || NotNull(sc.CoreCount) || NotEmpty(sc.CashMemoryType) || NotNull(sc.CashMemoryValueMin) || NotNull(sc.CashMemoryValueMax)) return;
                    }

                    if ((obj.Original as Computer).GraphicalAdapters.Count != 0)
                    {
                        bool ok = false;
                        foreach (GraphicalAdapter g in (obj.Original as Computer).GraphicalAdapters)
                        {
                            if (NotEmpty(sc.GraphicalAdapterModel) && NotContains(g.Model, sc.GraphicalAdapterModel)) continue;
                            if (NotEmpty(sc.GraphicalProcessor) && NotContains(g.GraphicalProcessor, sc.GraphicalProcessor)) continue;
                            if (!MatchDiapason(g.RamMemory, sc.RamMemoryMin, sc.RamMemoryMax)) continue;
                            ok = true;
                            break;
                        }
                        if (ok == false) return;
                    }
                    else
                    {
                        if (NotEmpty(sc.GraphicalAdapterModel) || NotEmpty(sc.GraphicalProcessor) || NotNull(sc.RamMemoryMin) || NotNull(sc.RamMemoryMax)) return;
                    }

                    if ((obj.Original as Computer).PhisicalDisks.Count != 0)
                    {
                        bool ok = false;
                        foreach (PhisicalDisk d in (obj.Original as Computer).PhisicalDisks)
                        {
                            if (NotEmpty(sc.PhisicalDiskModel) && NotContains(d.Model, sc.PhisicalDiskModel)) continue;
                            if (NotEmpty(sc.DiskType) && NotContains(d.DiskType, sc.DiskType)) continue;
                            if (NotEmpty(sc.DiskInterface) && NotContains(d.Interface, sc.DiskInterface)) continue;
                            if (NotEmpty(sc.FormFactor) && NotContains(d.FormFactor, sc.FormFactor)) continue;
                            if (!MatchDiapason(d.Memory, sc.PhisicalDiskMemoryMin, sc.PhisicalDiskMemoryMax)) continue;
                            ok = true;
                            break;
                        }
                        if (ok == false) return;
                    }
                    else
                    {
                        if (NotEmpty(sc.PhisicalDiskModel) || NotEmpty(sc.DiskType) || NotEmpty(sc.DiskInterface) || NotEmpty(sc.FormFactor) || NotNull(sc.PhisicalDiskMemoryMin) || NotNull(sc.PhisicalDiskMemoryMax)) return;
                    }

                    if ((obj.Original as Computer).RamModules.Count != 0)
                    {
                        bool ok = false;
                        foreach (RamModule r in (obj.Original as Computer).RamModules)
                        {
                            if (NotEmpty(sc.RamModuleModel) && NotContains(r.Model, sc.RamModuleModel)) continue;
                            if (NotEmpty(sc.RamType) && NotContains(r.RamType, sc.RamType)) continue;
                            if (!MatchDiapason(r.Memory, sc.RamModuleMemoryMin, sc.RamModuleMemoryMax)) continue;
                            ok = true;
                            break;
                        }
                        if (ok == false) return;
                    }
                    else
                    {
                        if (NotEmpty(sc.RamModuleModel) || NotEmpty(sc.RamType) || NotNull(sc.RamModuleMemoryMin) || NotNull(sc.RamModuleMemoryMax)) return;
                    }
                    break;
                case "Monitor":
                    Monitor m = obj.Original as Monitor;
                    if (NotNull(sc.Diagonal) && NotEqual(sc.Diagonal, m.Diagonal)) return;
                    if (NotEmpty(sc.AspectRatio) && NotContains(m.AspectRatio, sc.AspectRatio)) return;
                    if (NotEmpty(sc.Matrix) && NotContains(m.Matrix, sc.Matrix)) return;
                    if (NotEmpty(sc.Resolution) && NotContains(m.Resolution, sc.Resolution)) return;
                    if (!MatchDiapason(m.Frequency,sc.FrequencyMin, sc.FrequencyMax)) return;
                    break;
                case "Keyboard":
                    Keyboard k = obj.Original as Keyboard;
                    if (sc.KeyboardIsWired != k.IsWired) return;
                    break;
                case "Mouse":
                    Mouse mouse = obj.Original as Mouse;
                    if (sc.MouseIsWired != mouse.IsWired) return;
                    break;
                case "Camera":
                    Camera camera = obj.Original as Camera;
                    if (NotEmpty(sc.CameraMaxResolution) && NotContains(camera.MaxResolution, sc.CameraMaxResolution)) return;
                    if (sc.CameraIsRotatable != camera.IsRotatable) return;
                    if (sc.CameraHasMicro != camera.HasMicro) return;
                    break;
                case "Headphones":
                    Headphones headphones = obj.Original as Headphones;
                    if (sc.HeadphonesHasMicro != headphones.HasMicro) return;
                    if (sc.HeadphonesIsWired != headphones.IsWired) return;
                    break;
                case "Printer":
                    Printer printer = obj.Original as Printer;
                    if (NotEmpty(sc.PrinterMaxFormat) && NotContains(printer.MaxFormat, sc.PrinterMaxFormat)) return;
                    if (NotEmpty(sc.PrinterPrintTechnology) && NotContains(printer.PrintTechnology, sc.PrinterPrintTechnology)) return;
                    if (NotEmpty(sc.PrinterColor) && NotContains(printer.Color, sc.PrinterColor)) return;
                    if (sc.PrinterDoubleSidedPrinting != printer.DoubleSidedPrinting) return;
                    if (sc.PrinterHasScanner != printer.HasScanner) return;
                    break;
                case "WiredTelephone":
                    WiredTelephone wiredTelephone = obj.Original as WiredTelephone;
                    if (NotEmpty(sc.TelephoneConnectionType) && NotContains(wiredTelephone.ConnectionType, sc.TelephoneConnectionType)) return;
                    if (sc.HeadphonesHasMicro != wiredTelephone.HasScreen) return;
                    if (sc.TelephoneHasScreen != wiredTelephone.HasScreen) return;
                    break;
                case "BackupBattery":
                    BackupBattery backupBattery = obj.Original as BackupBattery;
                    if (NotNull(sc.BackupBatterySocketsCount) && NotEqual(sc.BackupBatterySocketsCount, backupBattery.SocketsCount)) return;
                    if (!MatchDiapason(backupBattery.BatteryLife, sc.BatteryLifeMin, sc.BatteryLifeMax)) return;
                    if (NotEmpty(sc.BatteryType) && NotContains(backupBattery.BatteryType, sc.BatteryType)) return;
                    break;
                case "SurgeProtector":
                    SurgeProtector surgeProtector = obj.Original as SurgeProtector;
                    if (NotNull(sc.SurgeProtectorSocketsCount) && NotEqual(sc.SurgeProtectorSocketsCount, surgeProtector.SocketsCount)) return;
                    if (sc.SurgeProtectorHasSwitcher != surgeProtector.HasSwitcher) return;
                    if (sc.SurgeProtectorHasWire != surgeProtector.HasWire) return;
                    break;
            }
            

            list.Add(obj);
            return;
        }
        public static void FilterWire(SearchCriteria sc, WireView obj, ObservableCollection<WireView> list)
        {
            if (sc.HardwareName != null && !sc.HardwareName.Equals("") && !sc.HardwareName.Equals(obj.WireName)) return;
            if (sc.InventorialStatus != null && !sc.InventorialStatus.Equals("") && !sc.InventorialStatus.Equals(obj.InventorialStatus)) return;
            if (sc.Model != null && !sc.Model.Equals("") && !sc.Model.Contains(obj.Model)) return;
            if (!MatchDiapason(obj.ItemCount, sc.ItemCountMin, sc.ItemCountMax)) return;

            switch (sc.HardwareType)
            {
                case "Cable":
                    Cable cable = obj.Original as Cable;
                    if (NotEmpty(sc.CableConnectorType) && NotContains(cable.ConnectorType, sc.CableConnectorType)) return;
                    if (sc.CableIsInputFirst != cable.IsInputFirst) return;
                    if (sc.CableIsInputSecond != cable.IsInputSecond) return;
                    break;
                case "AdapterCable":
                    AdapterCable adapterCable = obj.Original as AdapterCable;
                    if (NotEmpty(sc.AdapterFirstConnectorType) && NotContains(adapterCable.FirstConnectorType, sc.AdapterFirstConnectorType)) return;
                    if (NotEmpty(sc.AdapterSecondConnectorType) && NotContains(adapterCable.SecondConnectorType, sc.AdapterSecondConnectorType)) return;
                    if (sc.AdapterIsInputFirst != adapterCable.IsInputFirst) return;
                    if (sc.AdapterIsInputSecond != adapterCable.IsInputSecond) return;
                    if (sc.AdapterHasWire != adapterCable.HasWire) return;
                    break;
            }

            list.Add(obj);
            return;
        }

        private static bool NotEmpty(string p)
        {
            return !string.IsNullOrEmpty(p);
        }
        private static bool NotNull(int? p)
        {
            return p != null;
        }
        private static bool NotNull(double? p)
        {
            return p != null;
        }
        private static bool NotEqual(string p1, string p2)
        {
            return !p1.Equals(p2);
        }
        private static bool NotEqual(double? p1, double? p2)
        {
            return !p1.Equals(p2);
        }
        private static bool NotContains(string p1, string p2)
        {
            return !p1.Contains(p2);
        }
        private static bool MatchDiapason(int? p, int? pMin, int?pMax)
        {
            if (pMin != null && p < pMin) return false;
            if (pMax != null && p > pMax) return false;
            return true;
        }
        private static bool MatchDiapason(double? p, double? pMin, double? pMax)
        {
            if (pMin != null && p < pMin) return false;
            if (pMax != null && p > pMax) return false;
            return true;
        }
    }
}
