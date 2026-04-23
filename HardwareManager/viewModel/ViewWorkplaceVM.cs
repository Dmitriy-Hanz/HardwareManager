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



namespace HardwareManager.viewModel
{
    class ViewWorkplaceVM : ViewModelBase
    {
        ///TypicalActions
        ///TypicalValidations

        #region СВОЙСТВА


        private WorkPlaceView workPlaceView;
        public WorkPlaceView WorkPlaceView
        {
            get => workPlaceView;
            set => Set(ref workPlaceView, value);
        }



        #endregion
        #region КОМАНДЫ


        public ICommand ExitCommand { get; }
        private void ExitCommandAction(object p)
        {
            TypicalActions.CloseWindow<ViewWorkplaceWin>();
        }
        
        public ICommand AddHardwareCommand { get; }
        private void AddHardwareCommandAction(object p)
        {
            TypicalActions.GoToDialogWindow<HardwareAddingOptionsWin>();
            if (SessionStorage.GetInstance().CheckFlag("CreateNew") && !SessionStorage.GetInstance().CheckFlag("FromInventory"))
            {
                TypicalActions.GoToDialogWindow<ChooseNewHardwareTypeWin>();
                if (SessionStorage.GetInstance().Contains("HardwareType"))
                {
                    TypicalActions.GoToDialogWindow<EditHardwareWin>();
                    if (SessionStorage.GetInstance().Contains("NewHardware"))
                    {
                        Hardware temp = SessionStorage.GetInstance().GetValue("NewHardware") as Hardware;
                        DataBaseService.AttachHardwareToWorkplace(temp, WorkPlaceView.Original);
                        WorkPlaceView.HardwareList.Add(new HardwareView(temp));
                        Update(ref workPlaceView, "workPlaceView");
                        return;
                    }
                }
                if (SessionStorage.GetInstance().Contains("WireType"))
                {
                    SessionStorage.GetInstance().PutValue("targetWorkplace", WorkPlaceView.Original);
                    TypicalActions.GoToDialogWindow<EditWireWin>();
                    if (SessionStorage.GetInstance().Contains("NewWire"))
                    {
                        Wire temp = SessionStorage.GetInstance().GetValue("NewWire") as Wire;
                        DataBaseService.AttachWireToWorkplace(temp, WorkPlaceView.Original);
                        WorkPlaceView.WireList.Add(new WireView(temp));
                        Update(ref workPlaceView, "workPlaceView");
                        return;
                    }
                }
            }
            if (SessionStorage.GetInstance().CheckFlag("FromInventory") && !SessionStorage.GetInstance().CheckFlag("CreateNew"))
            {
                TypicalActions.GoToDialogWindow<SelectHardwareFromInventoryWin>();
                if (SessionStorage.GetInstance().Contains("SelectedHardware"))
                {
                    Hardware temp = SessionStorage.GetInstance().GetValue("SelectedHardware") as Hardware;
                    DataBaseService.AttachHardwareToWorkplace(temp, WorkPlaceView.Original);
                    WorkPlaceView.HardwareList.Add(new HardwareView(temp));
                    Update(ref workPlaceView, "workPlaceView");
                    return;
                }
                if (SessionStorage.GetInstance().Contains("SelectedWire"))
                {
                    Wire temp = SessionStorage.GetInstance().GetValue("SelectedWire") as Wire;
                    DataBaseService.AttachWireToWorkplace(temp, WorkPlaceView.Original);
                    if (WorkPlaceView.Original.WireList.Count > WorkPlaceView.WireList.Count)
                    {
                        WorkPlaceView.WireList.Add(new WireView(WorkPlaceView.Original.WireList.Last()));
                    }
                    else
                    {
                        WorkPlaceView.OriginalizeWires();
                    }
                    Update(ref workPlaceView, "workPlaceView");
                    return;
                }
            }
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
                }
            }
            else
            {
                SessionStorage.GetInstance().PutValue("EditedWire", (p as WireView).Original);
                SessionStorage.GetInstance().PutValue("targetWorkplace", WorkPlaceView.Original);
                TypicalActions.GoToDialogWindow<EditWireWin>();
                if (SessionStorage.GetInstance().CheckFlag("EditedWire"))
                {
                    (p as WireView).Originalize();
                }
            }
            Update(ref workPlaceView, "workPlaceView");
        }
        public ICommand DetachHardwareCommand { get; }
        private void DetachHardwareCommandAction(object p)
        {
            if (p is Hardware)
            {
                DataBaseService.DetachHardware((p as HardwareView).Original);
                WorkPlaceView.HardwareList.Remove(p as HardwareView);
            }
            else
            {
                DataBaseService.DetachWire((p as WireView).Original);
                if ((p as WireView).Original.ItemCount == 0)
                {
                    (p as WireView).Original.ItemCount++; //ЭТА СТРОЧКА НАДА, БЕЗ НЕЕ REMOVE РАБОТАТЬ НЕ БУДЕТ (НАВЕРНОЕ)
                    WorkPlaceView.WireList.Remove(p as WireView);
                }
                else
                {
                    (p as WireView).Originalize();
                }
            }
            Update(ref workPlaceView, "workPlaceView");
        }
        public ICommand WriteOffHardwareCommand { get; }
        private void WriteOffHardwareCommandAction(object p)
        {
            if (TypicalActions.ShowQuestionAnswerableMessage("Вы уверены что хотите списать данное оборудование?") == MessageBoxResult.Yes)
            {
                if (p is Hardware)
                {
                    DataBaseService.WriteOffHardware((p as HardwareView).Original);
                    WorkPlaceView.HardwareList.Remove(p as HardwareView);
                }
                else
                {
                    DataBaseService.WriteOffWire((p as WireView).Original);
                    if ((p as WireView).Original.ItemCount == 0)
                    {
                        WorkPlaceView.WireList.Remove(p as WireView);
                    }
                    else
                    {
                        (p as WireView).Originalize();
                    }
                }
                Update(ref workPlaceView, "workPlaceView");
            }
        }
        

        #endregion
        #region МОДЕЛЬ

        #endregion


        public ViewWorkplaceVM()
        {
            ExitCommand = new UnconditionalLambdaCommand(ExitCommandAction);

            AddHardwareCommand = new UnconditionalLambdaCommand(AddHardwareCommandAction);

            ViewHardwareCommand = new UnconditionalLambdaCommand(ViewHardwareCommandAction);
            EditHardwareCommand = new UnconditionalLambdaCommand(EditHardwareCommandAction);
            DetachHardwareCommand = new UnconditionalLambdaCommand(DetachHardwareCommandAction);
            WriteOffHardwareCommand = new UnconditionalLambdaCommand(WriteOffHardwareCommandAction);




            try
            {
                WorkPlace selectedWorkplace = SessionStorage.GetInstance().GetValue("SelectedWorkplace") as WorkPlace;
                WorkPlaceView = new WorkPlaceView(DataBaseService.GetWorkplaceById(selectedWorkplace.Id));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
                WorkPlaceView = new();
                WorkPlaceView.Name = "СКибидИ";
            }
        }
    }
}
