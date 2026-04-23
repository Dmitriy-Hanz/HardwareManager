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
using System.IO;
using Monitor = HardwareManager.model.entity.Monitor;
using Keyboard = HardwareManager.model.entity.Keyboard;
using Mouse = HardwareManager.model.entity.Mouse;
using System.Windows.Forms;


namespace HardwareManager.viewModel
{
    class EditWireVM : ViewModelBase
    {
        ///TypicalActions
        ///TypicalValidations

        #region СВОЙСТВА

        private WireView wireView;
        public WireView WireView
        {
            get => wireView;
            set
            {
                if (value.HardwareImage != null)
                {
                    DeleteHardwareImageButtonVisibility = Visibility.Visible;
                }
                Set(ref wireView, value);
            }
        }


        private CableView cableView;
        public CableView CableView
        {
            get => cableView;
            set => Set(ref cableView, value);
        }
        private AdapterCableView adapterCableView;
        public AdapterCableView AdapterCableView
        {
            get => adapterCableView;
            set => Set(ref adapterCableView, value);
        }



        //private string selectedTab;
        //public string SelectedTab
        //{
        //    get => selectedTab;
        //    set => Set(ref selectedTab, value);
        //}

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

        private Visibility deleteHardwareImageButtonVisibility = Visibility.Collapsed;
        public Visibility DeleteHardwareImageButtonVisibility
        {
            get => deleteHardwareImageButtonVisibility;
            set => Set(ref deleteHardwareImageButtonVisibility, value);
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
                WireView.HardwareImage = buffer;
                Update(ref wireView, "hardwareView");
                DeleteHardwareImageButtonVisibility = Visibility.Visible;
            }
        }
        public ICommand DeleteHardwareImageCommand { get; }
        private void DeleteHardwareImageCommandAction(object p)
        {
            WireView.HardwareImage = null;
            DeleteHardwareImageButtonVisibility = Visibility.Collapsed;
            Update(ref wireView, "hardwareView");
        }

        public ICommand ExitCommand { get; }
        private void ExitCommandAction(object p)
        {
            Exit();
        }

        public ICommand SaveCommand { get; }
        private void SaveCommandAction(object p)
        {
            if (!WireView.Validate()) return;

            switch (WireView.WireType)
            {
                case "Cable":
                    if (WorkPlace == null && !CableView.Validate()) return;
                    if (WorkPlace != null && !CableView.Validate(WorkPlace)) return;
                    CableView.Original ??= new();
                    WireView.Merge(CableView.Original);
                    CableView.Merge(CableView.Original);
                    Wire = CableView.Original;
                    break;
                case "AdapterCable":
                    if (WorkPlace == null && !AdapterCableView.Validate()) return;
                    if (WorkPlace != null && !AdapterCableView.Validate(WorkPlace)) return;
                    AdapterCableView.Original ??= new();
                    WireView.Merge(AdapterCableView.Original);
                    AdapterCableView.Merge(AdapterCableView.Original);
                    Wire = AdapterCableView.Original;
                    break;
            }

            if (!EditMode)
            {
                Wire.InventorialStatus = WorkPlace == null ? "На складе" : "В работе";
                DataBaseService.SaveWire(Wire, WorkPlace);
                Exit();
            }
            else
            {
                DataBaseService.UpdateWire(Wire);
            }
        }


        #endregion
        #region МОДЕЛЬ

        private bool EditMode { get; set; }
        public Wire Wire { get; set; }
        private WorkPlace WorkPlace { get; set; }
       
        #endregion


        public EditWireVM()
        {
            AddImageCommand = new UnconditionalLambdaCommand(AddImageCommandAction);
            DeleteHardwareImageCommand = new UnconditionalLambdaCommand(DeleteHardwareImageCommandAction);
            ExitCommand = new UnconditionalLambdaCommand(ExitCommandAction);
            SaveCommand = new UnconditionalLambdaCommand(SaveCommandAction);

            try
            {
                WorkPlace = SessionStorage.GetInstance().GetValue<WorkPlace>("targetWorkplace");
                if (SessionStorage.GetInstance().Contains("WireType"))
                {
                    EditMode = false;
                    WireView = new(SessionStorage.GetInstance().GetValue("WireType") as string);
                    HardwareStatusPanelVisibility = Visibility.Collapsed;
                    switch (WireView.WireType)
                    {
                        case "Cable":
                            HardwareHeaderText = "Новый кабель";
                            CableView = new();
                            break;
                        case "AdapterCable":
                            HardwareHeaderText = "Новый адаптер";
                            AdapterCableView = new();
                            break;
                    };
                }
                else
                {
                    EditMode = true;
                    Wire = SessionStorage.GetInstance().GetValue("EditedWire") as Wire;
                    WireView = new WireView(Wire);
                    HardwareHeaderText = WireView.WireName;
                    HardwareStatusPanelVisibility = Visibility.Visible;
                    switch (WireView.WireType)
                    {
                        case "Cable":
                            CableView = new CableView(Wire as Cable);
                            break;
                        case "AdapterCable":
                            AdapterCableView = new AdapterCableView(Wire as AdapterCable);
                            break;
                    };
                }

            }
            catch (Exception)
            {
                HardwareStatusPanelVisibility = Visibility.Visible;

                HardwareHeaderText = "Новый кабель";
                WireView = new("AdapterCable");
            }
        }

        private void Exit()
        {
            if (EditMode)
            {
                SessionStorage.GetInstance().SetFlag("EditedWire");
            }
            else
            {
                SessionStorage.GetInstance().PutValue("NewWire", Wire);
            }
            TypicalActions.CloseWindow<EditWireWin>();
        }
    }
}
