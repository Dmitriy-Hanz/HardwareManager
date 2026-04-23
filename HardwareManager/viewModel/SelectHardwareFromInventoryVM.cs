using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.viewModel.Base;
using HardwareManager.infrastructure.commands.Base;
using HardwareManager.model.entity;
using HardwareManager.infrastructure.utils.database;
using HardwareManager.view;
using System.Windows;
using System.Windows.Input;
using HardwareManager.model.view;
using System.Collections.ObjectModel;
using HardwareManager.model;
using Monitor = HardwareManager.model.entity.Monitor;



namespace HardwareManager.viewModel
{
    class SelectHardwareFromInventoryVM : ViewModelBase
    {
        ///TypicalActions
        ///TypicalValidations

        #region СВОЙСТВА

        private int tabSelectedIndex = 0;
        public int TabSelectedIndex
        {
            get => tabSelectedIndex;
            set => Set(ref tabSelectedIndex, value);
        }

        private HardwareView selectedHardware;
        public HardwareView SelectedHardware
        {
            get => selectedHardware;
            set => Set(ref selectedHardware, value);
        }
        private WireView selectedWire;
        public WireView SelectedWire
        {
            get => selectedWire;
            set => Set(ref selectedWire, value);
        }


        #endregion
        #region КОМАНДЫ


        public ICommand CancelCommand { get; }
        private void CancelCommandAction(object p)
        {
            TypicalActions.CloseWindow<SelectHardwareFromInventoryWin>();
        }
        public ICommand ApplyCommand { get; }
        private void ApplyCommandAction(object p)
        {
            if (SelectedHardware == null && TabSelectedIndex == 0 || SelectedWire == null && TabSelectedIndex == 1)
            {
                TypicalActions.ShowInformationalMessage("Необходимо выделить прикрепляемое оборудование в списке инвентаря");
                return;
            }
            if (SelectedHardware != null && TabSelectedIndex == 0)
            {
                SessionStorage.GetInstance().PutValue("SelectedHardware", SelectedHardware.Original);
            }
            else
            {
                SessionStorage.GetInstance().PutValue("SelectedWire", SelectedWire.Original);
            }
            TypicalActions.CloseWindow<SelectHardwareFromInventoryWin>();
        }

        public ICommand ViewHardwareCommand { get; }
        private void ViewHardwareCommandAction(object p)
        {
            if (p is HardwareView)
            {
                SessionStorage.GetInstance().PutValue("Hardware", (p as HardwareView).Original);
                TypicalActions.GoToDialogWindow<ViewHardwareWin>();
            }
            else
            {
                SessionStorage.GetInstance().PutValue("Wire", (p as WireView).Original);
                TypicalActions.GoToDialogWindow<ViewWireWin>();
            }
        }

        #endregion
        #region МОДЕЛЬ


        private ObservableCollection<HardwareView> mainHardwareList;
        public ObservableCollection<HardwareView> MainHardwareList
        {
            get => mainHardwareList;
            set => Set(ref mainHardwareList, value);
        }

        private ObservableCollection<WireView> wiresList;
        public ObservableCollection<WireView> WiresList
        {
            get => wiresList;
            set => Set(ref wiresList, value);
        }


        #endregion


        public SelectHardwareFromInventoryVM()
        {
            CancelCommand = new UnconditionalLambdaCommand(CancelCommandAction);
            ApplyCommand = new UnconditionalLambdaCommand(ApplyCommandAction);

            ViewHardwareCommand = new UnconditionalLambdaCommand(ViewHardwareCommandAction);

            try
            {
                MainHardwareList = [];
                WiresList = [];
                foreach (Hardware item in DataBaseService.GetAllMainHardwareInStock())
                {
                    MainHardwareList.Add(new HardwareView(item));
                }
                foreach (Wire item in DataBaseService.GetAllWiresInStock())
                {
                    WiresList.Add(new WireView(item));
                }
            }
            catch (Exception)
            {
                MainHardwareList =
                [
                    new HardwareView(new Computer()),
                    new HardwareView(new Monitor { Model = "Asshole 400 pro xl134 sm" })
                ];
                WiresList =
                [
                    new WireView(new Cable{Model = "Кабель Baseus High Definition Series 8K Adapter USB Type-C - DisplayPort (2 м, черный) "}),
                    new WireView(new AdapterCable { Model = "USB-C-USB-A" })
                ];
            }
        }
    }
}
