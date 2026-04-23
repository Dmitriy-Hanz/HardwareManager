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
    class SearchVM : ViewModelBase
    {
        ///TypicalActions
        ///TypicalValidations

        #region СВОЙСТВА


        private int hardwareTabSelectedIndex;
        public int HardwareTabSelectedIndex
        {
            get => hardwareTabSelectedIndex;
            set => Set(ref hardwareTabSelectedIndex, value);
        }

        private SearchCriteria searchCriteria;
        public SearchCriteria SearchCriteria
        {
            get => searchCriteria;
            set => Set(ref searchCriteria, value);
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
                }
                else
                {
                    DataBaseService.WriteOffWire((p as WireView).Original);
                    if ((p as WireView).Original.ItemCount == 0)
                    {
                        WiresList.Remove(p as WireView);
                    }
                    else
                    {
                        (p as WireView).Originalize();
                    }
                    Update(ref wiresList, "wiresList");
                }
            }
        }

        public ICommand BackToMainMenuCommand { get; }
        private void BackToMainMenuCommandAction(object p)
        {
            TypicalActions.GoToWindow<SearchWin, MainMenuWin>();
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


        public SearchVM()
        {
            ViewHardwareCommand = new UnconditionalLambdaCommand(ViewHardwareCommandAction);
            EditHardwareCommand = new UnconditionalLambdaCommand(EditHardwareCommandAction);
            WriteOffHardwareCommand = new UnconditionalLambdaCommand(WriteOffHardwareCommandAction);

            BackToMainMenuCommand = new UnconditionalLambdaCommand(BackToMainMenuCommandAction);
            SearchCommand = new UnconditionalLambdaCommand(SearchCommandAction);
            ClearFiltersCommand = new UnconditionalLambdaCommand(ClearFiltersCommandAction);

            try
            {
                SearchCriteria = new();
                MainHardwareList = [];
                WiresList = [];
                MainHardwareListSource = [];
                WiresListSource = [];
                foreach (Hardware item in DataBaseService.GetAllMainHardware())
                {
                    MainHardwareListSource.Add(new HardwareView(item));
                }
                foreach (Wire item in DataBaseService.GetAllWires())
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
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
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
                SearchCriteria = new();
                SearchCriteria.HardwareType = "Cable";
            }
        }
    }
}
