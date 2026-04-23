using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.viewModel.Base;
using HardwareManager.infrastructure.commands.Base;
using HardwareManager.model.entity;
using HardwareManager.infrastructure.utils.database;
using HardwareManager.view;
using System.Windows;
using System.Windows.Input;
using HardwareManager.model;
using System.Windows.Forms;
using System.IO;
using Monitor = HardwareManager.model.entity.Monitor;
using Keyboard = HardwareManager.model.entity.Keyboard;
using Mouse = HardwareManager.model.entity.Mouse;
using HardwareManager.model.view;
using System.Windows.Media;



namespace HardwareManager.viewModel
{
    class EditHardwareVM : ViewModelBase
    {
        ///TypicalActions
        ///TypicalValidations

        #region СВОЙСТВА
        
        private HardwareView hardwareView;
        public HardwareView HardwareView
        {
            get => hardwareView;
            set 
            {
                if (value.HardwareImage != null)
                {
                    DeleteHardwareImageButtonVisibility = Visibility.Visible;
                }
                Set(ref hardwareView, value); 
            }
        }

        private ComputerView computerView;
        public ComputerView ComputerView
        {
            get => computerView;
            set
            {
                if (value != null)
                {
                    AddCpuButtonVisibility = value.Cpu == null? Visibility.Visible : Visibility.Collapsed;
                }
                Set(ref computerView, value);
            }
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

        private string hardwareHeaderText;
        public string HardwareHeaderText
        {
            get => hardwareHeaderText;
            set => Set(ref hardwareHeaderText, value);
        }

        private Visibility hardwareStatusPanelVisibility;
        public Visibility HardwareStatusPanelVisibility
        {
            get => hardwareStatusPanelVisibility;
            set => Set(ref hardwareStatusPanelVisibility, value);
        }        
        
        private Visibility generateInventorialNumberButtonVisibility;
        public Visibility GenerateInventorialNumberButtonVisibility
        {
            get => generateInventorialNumberButtonVisibility;
            set => Set(ref generateInventorialNumberButtonVisibility, value);
        }
        private bool isInventorialNumberTBReadOnly;
        public bool IsInventorialNumberTBReadOnly
        {
            get => isInventorialNumberTBReadOnly;
            set => Set(ref isInventorialNumberTBReadOnly, value);
        }

        private Visibility deleteHardwareImageButtonVisibility = Visibility.Collapsed;
        public Visibility DeleteHardwareImageButtonVisibility
        {
            get => deleteHardwareImageButtonVisibility;
            set => Set(ref deleteHardwareImageButtonVisibility, value);
        }

        private Visibility addCpuButtonVisibility;
        public Visibility AddCpuButtonVisibility
        {
            get => addCpuButtonVisibility;
            set => Set(ref addCpuButtonVisibility, value);
        }


        private Visibility markAsDefectiveButtonVisibility;
        public Visibility MarkAsDefectiveButtonVisibility
        {
            get => markAsDefectiveButtonVisibility;
            set => Set(ref markAsDefectiveButtonVisibility, value);
        }        
        private Visibility markAsButtonPanelVisibility;
        public Visibility MarkAsButtonPanelVisibility
        {
            get => markAsButtonPanelVisibility;
            set => Set(ref markAsButtonPanelVisibility, value);
        }

        #endregion
        #region КОМАНДЫ


        public ICommand AddImageCommand { get; }
        private void AddImageCommandAction(object p)
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != null && !openFileDialog.FileName.Equals(""))
            {
                FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open);
                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                HardwareView.HardwareImage = buffer;
                Update(ref hardwareView, "hardwareView");
                DeleteHardwareImageButtonVisibility = Visibility.Visible;
            }
        }
        public ICommand DeleteHardwareImageCommand { get; }
        private void DeleteHardwareImageCommandAction(object p)
        {
            HardwareView.HardwareImage = null;
            DeleteHardwareImageButtonVisibility = Visibility.Collapsed;
            Update(ref hardwareView, "hardwareView");
        }


        public ICommand GenerateInventorialNumberCommand { get; }
        private void GenerateInventorialNumberCommandAction(object p)
        {
            HardwareView.InventorialNumber = DataBaseService.GenerateInventorialNumber();
            Update(ref hardwareView, "hardwareView");
        }


        public ICommand MarkAsDefectiveCommand { get; }
        private void MarkAsDefectiveCommandAction(object p)
        {
            DataBaseService.SetInventorialStatusToHardware((HardwareView).Original, EInventorialStatus.Defective);
            HardwareView.Originalize();
            MarkAsDefectiveButtonVisibility = Visibility.Collapsed;
            Update(ref hardwareView, "hardwareView");
        }
        public ICommand MarkAsServiceableCommand { get; }
        private void MarkAsServiceableCommandAction(object p)
        {
            DataBaseService.SetInventorialStatusToHardware((HardwareView).Original, EInventorialStatus.InStock);
            HardwareView.Originalize();
            MarkAsDefectiveButtonVisibility = Visibility.Visible;
            Update(ref hardwareView, "hardwareView");
        }


        public ICommand AddCpuCommand { get; }
        private void AddCpuCommandAction(object p)
        {
            AddCpuButtonVisibility = Visibility.Collapsed;
            ComputerView.Cpu = new();
            Update(ref computerView, "computerView");
        }
        public ICommand DeleteCpuCommand { get; }
        private void DeleteCpuCommandAction(object p)
        {
            AddCpuButtonVisibility = Visibility.Visible;
            ComputerView.Cpu = null;
            Update(ref computerView, "computerView");
        }
        
        public ICommand AddGraphicalAdapterCommand { get; }
        private void AddGraphicalAdapterCommandAction(object p)
        {
            if (ComputerView.GraphicalAdapters.Count == 4)
            {
                TypicalValidations.ShowValidationMessageBox("Число графических адаптеров не может превышать 4");
                return;
            }
            ComputerView.GraphicalAdapters.Add(new());
            Update(ref computerView, "computerView");

        }
        public ICommand DeleteGraphicalAdapterCommand { get; }
        private void DeleteGraphicalAdapterCommandAction(object p)
        {
            ComputerView.GraphicalAdapters.Remove(p as GraphicalAdapterView);
            Update(ref computerView, "computerView");
        }

        public ICommand AddPhisicalDiskCommand { get; }
        private void AddPhisicalDiskCommandAction(object p)
        {
            if (ComputerView.PhisicalDisks.Count == 4)
            {
                TypicalValidations.ShowValidationMessageBox("Число физических дисков не может превышать 4");
                return;
            }
            ComputerView.PhisicalDisks.Add(new());
            Update(ref computerView, "computerView");
        }
        public ICommand DeletePhisicalDiskCommand { get; }
        private void DeletePhisicalDiskCommandAction(object p)
        {
            ComputerView.PhisicalDisks.Remove(p as PhisicalDiskView);
            Update(ref computerView, "computerView");
        }

        public ICommand AddRamModuleCommand { get; }
        private void AddRamModuleCommandAction(object p)
        {
            if (ComputerView.RamModules.Count == 6)
            {
                TypicalValidations.ShowValidationMessageBox("Число модулей ОЗУ не может превышать 6");
                return;
            }
            ComputerView.RamModules.Add(new());
            Update(ref computerView, "computerView");
        }
        public ICommand DeleteRamModuleCommand { get; }
        private void DeleteRamModuleCommandAction(object p)
        {
            ComputerView.RamModules.Remove(p as RamModuleView);
            Update(ref computerView, "computerView");
        }



        public ICommand ExitCommand { get; }
        private void ExitCommandAction(object p)
        {
            Exit();
        }

        public ICommand SaveCommand { get; }
        private void SaveCommandAction(object p)
        {
            if (!EditMode && !HardwareView.Validate()) return;
            if (Hardware != null && !HardwareView.ValidateWithoutIN()) return;

            switch (SelectedTab)
            {
                case "Computer":
                    if (!ComputerView.Validate()) return;
                    Computer ??= new();
                    HardwareView.Merge(Computer);
                    ComputerView.Merge(Computer);
                    Hardware = Computer;
                    break;
                case "Monitor":
                    if (!MonitorView.Validate()) return;
                    Monitor ??= new();
                    HardwareView.Merge(Monitor);
                    MonitorView.Merge(Monitor);
                    Hardware = Monitor;
                    break;
                case "Keyboard":
                    if (!KeyboardView.Validate()) return;
                    Keyboard ??= new();
                    HardwareView.Merge(Keyboard);
                    KeyboardView.Merge(Keyboard);
                    Hardware = Keyboard;
                    break;
                case "Mouse":
                    if (!MouseView.Validate()) return;
                    Mouse ??= new();
                    HardwareView.Merge(Mouse);
                    MouseView.Merge(Mouse);
                    Hardware = Mouse;
                    break;

                case "Camera":
                    if (!CameraView.Validate()) return;
                    Camera ??= new();
                    HardwareView.Merge(Camera);
                    CameraView.Merge(Camera);
                    Hardware = Camera;
                    break;
                case "Headphones":
                    if (!HeadphonesView.Validate()) return;
                    Headphones ??= new();
                    HardwareView.Merge(Headphones);
                    HeadphonesView.Merge(Headphones);
                    Hardware = Headphones;
                    break;

                case "Printer":
                    if (!PrinterView.Validate()) return;
                    Printer ??= new();
                    HardwareView.Merge(Printer);
                    PrinterView.Merge(Printer);
                    Hardware = Printer;
                    break;
                case "WiredTelephone":
                    if (!WiredTelephoneView.Validate()) return;
                    WiredTelephone ??= new();
                    HardwareView.Merge(WiredTelephone);
                    WiredTelephoneView.Merge(WiredTelephone);
                    Hardware = WiredTelephone;
                    break;

                case "BackupBattery":
                    if (!BackupBatteryView.Validate()) return;
                    BackupBattery ??= new();
                    HardwareView.Merge(BackupBattery);
                    BackupBatteryView.Merge(BackupBattery);
                    Hardware = BackupBattery;
                    break;
                case "SurgeProtector":
                    if (!SurgeProtectorView.Validate()) return;
                    SurgeProtector ??= new();
                    HardwareView.Merge(SurgeProtector);
                    SurgeProtectorView.Merge(SurgeProtector);
                    Hardware = SurgeProtector;
                    break;
            }

            if (!EditMode)
            {
                Hardware.InventorialStatus = "На складе";
                DataBaseService.SaveHardware(Hardware);
                Exit();
            }
            else
            {
                DataBaseService.UpdateHardware(Hardware);
            }
        }


        #endregion
        #region МОДЕЛЬ

        private bool EditMode { get; set; }
        public Hardware Hardware { get; set; }
        public Monitor Monitor { get; set; }
        public Computer Computer { get; set; }
        public Keyboard Keyboard { get; set; }
        public Mouse Mouse { get; set; }

        public Camera Camera { get; set; }
        public Headphones Headphones { get; set; }

        public Printer Printer { get; set; }
        public WiredTelephone WiredTelephone { get; set; }

        public BackupBattery BackupBattery { get; set; }
        public SurgeProtector SurgeProtector { get; set; }

        #endregion


        public EditHardwareVM()
        {
            AddImageCommand = new UnconditionalLambdaCommand(AddImageCommandAction);
            DeleteHardwareImageCommand = new UnconditionalLambdaCommand(DeleteHardwareImageCommandAction);
            GenerateInventorialNumberCommand = new UnconditionalLambdaCommand(GenerateInventorialNumberCommandAction);
            MarkAsDefectiveCommand = new UnconditionalLambdaCommand(MarkAsDefectiveCommandAction);
            MarkAsServiceableCommand = new UnconditionalLambdaCommand(MarkAsServiceableCommandAction);
            AddCpuCommand = new UnconditionalLambdaCommand(AddCpuCommandAction);
            DeleteCpuCommand = new UnconditionalLambdaCommand(DeleteCpuCommandAction);
            AddGraphicalAdapterCommand = new UnconditionalLambdaCommand(AddGraphicalAdapterCommandAction);
            DeleteGraphicalAdapterCommand = new UnconditionalLambdaCommand(DeleteGraphicalAdapterCommandAction);
            AddPhisicalDiskCommand = new UnconditionalLambdaCommand(AddPhisicalDiskCommandAction);
            DeletePhisicalDiskCommand = new UnconditionalLambdaCommand(DeletePhisicalDiskCommandAction);
            AddRamModuleCommand = new UnconditionalLambdaCommand(AddRamModuleCommandAction);
            DeleteRamModuleCommand = new UnconditionalLambdaCommand(DeleteRamModuleCommandAction);
            ExitCommand = new UnconditionalLambdaCommand(ExitCommandAction);
            SaveCommand = new UnconditionalLambdaCommand(SaveCommandAction);


            try
            {
                if (SessionStorage.GetInstance().Contains("HardwareType"))
                {
                    EditMode = false;
                    SelectedTab = SessionStorage.GetInstance().GetValue("HardwareType") as string;
                    HardwareView = new(SelectedTab);

                    switch (SelectedTab)
                    {
                        case "Computer":
                            ComputerView = new();
                            HardwareHeaderText = "Новый компьютер";
                            break;
                        case "Monitor":
                            MonitorView = new();
                            HardwareHeaderText = "Новый монитор";
                            break;
                        case "Keyboard":
                            KeyboardView = new();
                            HardwareHeaderText = "Новая клавиатура";
                            break;
                        case "Mouse":
                            MouseView = new();
                            HardwareHeaderText = "Новая мышь";
                            break;

                        case "Camera":
                            CameraView = new();
                            HardwareHeaderText = "Новая камера";
                            break;
                        case "Headphones":
                            HeadphonesView = new();
                            HardwareHeaderText = "Новые наушники";
                            break;

                        case "Printer":
                            PrinterView = new();
                            HardwareHeaderText = "Новый принтер";
                            break;
                        case "WiredTelephone":
                            WiredTelephoneView = new();
                            HardwareHeaderText = "Новый телефон";
                            break;

                        case "BackupBattery":
                            BackupBatteryView = new();
                            HardwareHeaderText = "Новый источник бесперебойного питания";
                            break;
                        case "SurgeProtector":
                            SurgeProtectorView = new();
                            HardwareHeaderText = "Новый разветвитель";
                            break;
                    }
                    HardwareStatusPanelVisibility = Visibility.Collapsed;
                    IsInventorialNumberTBReadOnly = false;
                    GenerateInventorialNumberButtonVisibility = Visibility.Visible;
                }
                else
                {
                    EditMode = true;
                    Hardware = SessionStorage.GetInstance().GetValue("EditedHardware") as Hardware;
                    HardwareView = new HardwareView(Hardware);
                    SelectedTab = HardwareView.HardwareType;
                    HardwareHeaderText = HardwareView.HardwareName;

                    switch (SelectedTab)
                    {
                        case "Computer":
                            Computer = Hardware as Computer;
                            ComputerView = new ComputerView(Computer);
                            break;
                        case "Monitor":
                            Monitor = Hardware as Monitor;
                            MonitorView = new MonitorView(Monitor);
                            break;
                        case "Keyboard":
                            Keyboard = Hardware as Keyboard;
                            keyboardView = new KeyboardView(Keyboard);
                            break;
                        case "Mouse":
                            Mouse = Hardware as Mouse;
                            MouseView = new MouseView(Mouse);
                            break;

                        case "Camera":
                            Camera = Hardware as Camera;
                            CameraView = new(Camera);
                            break;
                        case "Headphones":
                            Headphones = Hardware as Headphones;
                            HeadphonesView = new(Headphones);
                            break;

                        case "Printer":
                            Printer = Hardware as Printer;
                            PrinterView = new(Printer);
                            break;
                        case "WiredTelephone":
                            WiredTelephone = Hardware as WiredTelephone;
                            WiredTelephoneView = new(WiredTelephone);
                            break;

                        case "BackupBattery":
                            BackupBattery = Hardware as BackupBattery;
                            BackupBatteryView = new(BackupBattery);
                            break;
                        case "SurgeProtector":
                            SurgeProtector = Hardware as SurgeProtector;
                            SurgeProtectorView = new(SurgeProtector);
                            break;
                    }
                    HardwareStatusPanelVisibility = Visibility.Visible;
                    IsInventorialNumberTBReadOnly = true;
                    GenerateInventorialNumberButtonVisibility = Visibility.Collapsed;
                }

                if (HardwareView.InventorialStatus != null && (HardwareView.InventorialStatus.Equals("На складе") || HardwareView.InventorialStatus.Equals("Неисправен")))
                {
                    MarkAsButtonPanelVisibility = Visibility.Visible;
                    MarkAsDefectiveButtonVisibility = HardwareView.InventorialStatus.Equals("Неисправен") ? Visibility.Collapsed : Visibility.Visible;
                }
                else
                {
                    MarkAsButtonPanelVisibility = Visibility.Collapsed;
                }

            }
            catch (Exception)
            {
                HardwareStatusPanelVisibility = Visibility.Visible;

                HardwareHeaderText = "Новая камера";
                SelectedTab = "Printer";
            }
        }

        private void Exit()
        {
            if (EditMode)
            {
                SessionStorage.GetInstance().PutValue("EditedHardware", Hardware);
            }
            else
            {
                SessionStorage.GetInstance().PutValue("NewHardware", Hardware);
            }
            TypicalActions.CloseWindow<EditHardwareWin>();
        }
    }
}
