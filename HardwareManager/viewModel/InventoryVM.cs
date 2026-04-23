using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.viewModel.Base;
using HardwareManager.infrastructure.commands.Base;
using HardwareManager.view;
using HardwareManager.model;
using HardwareManager.model.entity;
using System.Windows.Input;
using System.Windows;
using Monitor = HardwareManager.model.entity.Monitor;
using HardwareManager.model.view;
using HardwareManager.infrastructure.utils.database;
using System.Collections.ObjectModel;


namespace HardwareManager.viewModel
{
    class InventoryVM : ViewModelBase
    {
        #region СВОЙСТВА

        private Visibility hardwareInformationStackPanelVisibility;
        public Visibility HardwareInformationStackPanelVisibility
        {
            get => hardwareInformationStackPanelVisibility;
            set => Set(ref hardwareInformationStackPanelVisibility, value);
        }

        private SearchCriteria searchCriteria;
        public SearchCriteria SearchCriteria
        {
            get => searchCriteria;
            set => Set(ref searchCriteria, value);
        }

        private Statistic statistic;
        public Statistic Statistic
        {
            get => statistic;
            set => Set(ref statistic, value);
        }

        #endregion
        #region КОМАНДЫ

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
        public ICommand CreateNewHardwareCommand { get; }
        private void CreateNewHardwareCommandAction(object p)
        {
            TypicalActions.GoToDialogWindow<ChooseNewHardwareTypeWin>();
            if (SessionStorage.GetInstance().Contains("HardwareType"))
            {
                TypicalActions.GoToDialogWindow<EditHardwareWin>();
                if (SessionStorage.GetInstance().Contains("NewHardware"))
                {
                    Hardware temp = SessionStorage.GetInstance().GetValue("NewHardware") as Hardware;
                    DataBaseService.SetInventorialStatusToHardware(temp, EInventorialStatus.InStock);
                    MainHardwareListSource.Add(new HardwareView(temp));
                    MainHardwareList.Add(MainHardwareListSource.Last());
                }
                return;
            }
            if (SessionStorage.GetInstance().Contains("WireType"))
            {
                TypicalActions.GoToDialogWindow<EditWireWin>();
                if (SessionStorage.GetInstance().Contains("NewWire"))
                {
                    Wire temp = SessionStorage.GetInstance().GetValue("NewWire") as Wire;
                    WiresListSource.Add(new WireView(temp));
                    WiresList.Add(WiresListSource.Last());
                }
            }
            Statistic.Load();
            Update(ref statistic, "statistic");
        }
        public ICommand EditHardwareCommand { get; }
        private void EditHardwareCommandAction(object p)
        {
            if (p is HardwareView)
            {
                SessionStorage.GetInstance().PutValue("EditedHardware", (p as HardwareView).Original);
                TypicalActions.GoToDialogWindow<EditHardwareWin>();
                if (SessionStorage.GetInstance().Contains("EditedHardware"))
                {
                    SessionStorage.GetInstance().GetValue("EditedHardware");
                    (p as HardwareView).Originalize();
                    Update(ref mainHardwareList, "mainHardwareList");
                }
            }
            else
            {
                SessionStorage.GetInstance().PutValue("EditedWire", (p as WireView).Original);
                TypicalActions.GoToDialogWindow<EditWireWin>();
                if (SessionStorage.GetInstance().CheckFlag("EditedWire"))
                {
                    (p as WireView).Originalize();
                    Update(ref wiresList, "wiresList");
                }
            }
            Statistic.Load();
            Update(ref statistic, "statistic");
        }
        public ICommand WriteOffHardwareCommand { get; }
        private void WriteOffHardwareCommandAction(object p)
        {
            if (TypicalActions.ShowQuestionAnswerableMessage("Вы уверены что хотите списать данное оборудование?") == MessageBoxResult.Yes)
            {
                if (p is HardwareView)
                {
                    DataBaseService.WriteOffHardware((p as HardwareView).Original);
                    MainHardwareList.Remove(p as HardwareView);
                    MainHardwareListSource.Remove(p as HardwareView);
                }
                else
                {
                    DataBaseService.WriteOffWire((p as WireView).Original);
                    if ((p as WireView).Original.ItemCount == 0)
                    {
                        WiresList.Remove(p as WireView);
                        WiresListSource.Remove(p as WireView);
                    }
                    else
                    {
                        (p as WireView).Originalize();
                    }
                    Update(ref wiresList, "wiresList");
                }
            }
            Statistic.Load();
            Update(ref statistic, "statistic");
        }


        public ICommand BackToMainMenuCommand { get; }
        private void BackToMainMenuCommandAction(object p)
        {
            TypicalActions.GoToWindow<InventoryWin, MainMenuWin>();
        }

        public ICommand SearchCommand { get; }
        private void SearchCommandAction(object p)
        {
            if (!SearchCriteria.HardwareName.Equals("Кабель") && !SearchCriteria.HardwareName.Equals("Адаптер"))
            {
                MainHardwareList.Clear();
                foreach (HardwareView item in MainHardwareListSource)
                {
                    SearchCriteria.FilterHardware(SearchCriteria, item, MainHardwareList);
                }
            }
            else
            {
                WiresList.Clear();
                foreach (WireView item in WiresListSource)
                {
                    SearchCriteria.FilterWire(SearchCriteria, item, WiresList);
                }
            }
        }
        public ICommand ClearFiltersCommand { get; }
        private void ClearFiltersCommandAction(object p)
        {
            SearchCriteria = new();
            MainHardwareList.Clear();
            WiresList.Clear();
            foreach (HardwareView item in MainHardwareListSource)
            {
                MainHardwareList.Add(item);
            }
            foreach (WireView item in WiresListSource)
            {
                WiresList.Add(item);
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

        private List<HardwareView> MainHardwareListSource { get; set; }
        private List<WireView> WiresListSource { get; set; }

        #endregion


        public InventoryVM()
        {
            BackToMainMenuCommand = new UnconditionalLambdaCommand(BackToMainMenuCommandAction);

            ViewHardwareCommand = new UnconditionalLambdaCommand(ViewHardwareCommandAction);
            CreateNewHardwareCommand = new UnconditionalLambdaCommand(CreateNewHardwareCommandAction);
            EditHardwareCommand = new UnconditionalLambdaCommand(EditHardwareCommandAction);
            WriteOffHardwareCommand = new UnconditionalLambdaCommand(WriteOffHardwareCommandAction);

            SearchCommand = new UnconditionalLambdaCommand(SearchCommandAction);
            ClearFiltersCommand = new UnconditionalLambdaCommand(ClearFiltersCommandAction);
            
            HardwareInformationStackPanelVisibility = Visibility.Hidden;

            try
            {
                SearchCriteria = new();
                Statistic = new();
                MainHardwareList = [];
                WiresList = [];
                MainHardwareListSource = [];
                WiresListSource = [];
                foreach (Hardware item in DataBaseService.GetAllMainHardwareInStock())
                {
                    MainHardwareListSource.Add(new HardwareView(item));
                }
                foreach (Wire item in DataBaseService.GetAllWiresInStock())
                {
                    WiresListSource.Add(new WireView(item));
                }

                foreach (HardwareView item in MainHardwareListSource)
                {
                    MainHardwareList.Add(item);
                }
                foreach (WireView item in WiresListSource)
                {
                    WiresList.Add(item);
                }
                Statistic.Load();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
                MainHardwareList =
                [
                    new HardwareView(new Computer()),
                    new HardwareView(new Monitor { Model = "Asshole 400 pro xl134 sm" })
                ];
                WiresList =
                [
                    new WireView(new Cable{Model = "Apple USB 3.2 Gen2 Type-C - Lightning (1 м, белый) ", ItemCount=3}),
                    new WireView(new AdapterCable { Model = "USB-C-USB-A" })
                ];
                Statistic = new();
            }
        }
    }
}
