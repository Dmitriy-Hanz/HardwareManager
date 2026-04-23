using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.viewModel.Base;
using HardwareManager.infrastructure.commands.Base;
using HardwareManager.model.entity;
using HardwareManager.infrastructure.utils.database;
using HardwareManager.view;
using System.Windows;
using System.Windows.Input;
using HardwareManager.model.view;
using HardwareManager.model;
using System.Windows.Media;
using Monitor = HardwareManager.model.entity.Monitor;
using Keyboard = HardwareManager.model.entity.Keyboard;
using Mouse = HardwareManager.model.entity.Mouse;
using System.IO;
using System.Threading;
using Microsoft.VisualBasic.Devices;
using System.Windows.Media.Media3D;
using Camera = HardwareManager.model.entity.Camera;
using Computer = HardwareManager.model.entity.Computer;



namespace HardwareManager.viewModel
{
    class ViewHardwareVM : ViewModelBase
    {
        ///TypicalActions
        ///TypicalValidations

        #region СВОЙСТВА

        private HardwareView hardwareView;
        public HardwareView HardwareView
        {
            get => hardwareView;
            set => Set(ref hardwareView, value);
        }

        private ComputerView computerView;
        public ComputerView ComputerView
        {
            get => computerView;
            set => Set(ref computerView, value);
        }
        private MonitorView monitorView;
        public MonitorView MonitorView
        {
            get => monitorView;
            set => Set(ref monitorView, value);
        }
        private KeyboardView keyboardView;
        public KeyboardView KeyboardView
        {
            get => keyboardView;
            set => Set(ref keyboardView, value);
        }
        private MouseView mouseView;
        public MouseView MouseView
        {
            get => mouseView;
            set => Set(ref mouseView, value);
        }

        private CameraView cameraView;
        public CameraView CameraView
        {
            get => cameraView;
            set => Set(ref cameraView, value);
        }
        private HeadphonesView headphonesView;
        public HeadphonesView HeadphonesView
        {
            get => headphonesView;
            set => Set(ref headphonesView, value);
        }

        private PrinterView printerView;
        public PrinterView PrinterView
        {
            get => printerView;
            set => Set(ref printerView, value);
        }
        private WiredTelephoneView wiredTelephoneView;
        public WiredTelephoneView WiredTelephoneView
        {
            get => wiredTelephoneView;
            set => Set(ref wiredTelephoneView, value);
        }

        private BackupBatteryView backupBatteryView;
        public BackupBatteryView BackupBatteryView
        {
            get => backupBatteryView;
            set => Set(ref backupBatteryView, value);
        }
        private SurgeProtectorView surgeProtectorView;
        public SurgeProtectorView SurgeProtectorView
        {
            get => surgeProtectorView;
            set => Set(ref surgeProtectorView, value);
        }


        private string selectedTab;
        public string SelectedTab
        {
            get => selectedTab;
            set => Set(ref selectedTab, value);
        }


        private Visibility cpuPanelVisibility;
        public Visibility CpuPanelVisibility
        {
            get => cpuPanelVisibility;
            set => Set(ref cpuPanelVisibility, value);
        }


        #endregion
        #region КОМАНДЫ

        public ICommand ExitCommand { get; }
        private void ExitCommandAction(object p)
        {
            TypicalActions.CloseWindow<ViewHardwareWin>();
        }

        #endregion
        #region МОДЕЛЬ
        #endregion


        public ViewHardwareVM()
        {
            ExitCommand = new UnconditionalLambdaCommand(ExitCommandAction);

            try
            {
                HardwareView = new HardwareView(SessionStorage.GetInstance().GetValue("Hardware") as Hardware);
                SelectedTab = HardwareView.HardwareType;

                switch (SelectedTab)
                {
                    case "Computer":
                        ComputerView = new ComputerView(HardwareView.Original as Computer);
                        CpuPanelVisibility = ComputerView.Cpu == null ? Visibility.Collapsed : Visibility.Visible;
                        break;
                    case "Monitor":
                        MonitorView = new MonitorView(HardwareView.Original as Monitor);
                        break;
                    case "Keyboard":
                        keyboardView = new KeyboardView(HardwareView.Original as Keyboard);
                        break;
                    case "Mouse":
                        MouseView = new MouseView(HardwareView.Original as Mouse);
                        break;

                    case "Camera":
                        CameraView = new(HardwareView.Original as Camera);
                        break;
                    case "Headphones":
                        HeadphonesView = new(HardwareView.Original as Headphones);
                        break;

                    case "Printer":
                        PrinterView = new(HardwareView.Original as Printer);
                        break;
                    case "WiredTelephone":
                        WiredTelephoneView = new(HardwareView.Original as WiredTelephone);
                        break;

                    case "BackupBattery":
                        BackupBatteryView = new(HardwareView.Original as BackupBattery);
                        break;
                    case "SurgeProtector":
                        SurgeProtectorView = new(HardwareView.Original as SurgeProtector);
                        break;
                }
            }
            catch (Exception)
            {
                HardwareView = new HardwareView("Computer");
                HardwareView.Model = "Рр";
                HardwareView.InventorialNumber = 1000000030;
                SelectedTab = HardwareView.HardwareType;
            }
        }
    }
}
