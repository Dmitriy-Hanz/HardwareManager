using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.viewModel.Base;
using HardwareManager.infrastructure.commands.Base;
using HardwareManager.model.entity;
using HardwareManager.infrastructure.utils.database;
using HardwareManager.view;
using System.Windows;
using System.Windows.Input;



namespace HardwareManager.viewModel
{
    class OrganizationVM : ViewModelBase
    {
        #region СВОЙСТВА

        private Cabinet selectedCabinet;
        public Cabinet SelectedCabinet
        {
            get => selectedCabinet;
            set
            {
                if (value != null)
                {
                    CreateNewWorkPlaceButtonVisibility = Visibility.Visible;
                }
                else
                {
                    if (CreateNewWorkPlaceButtonVisibility != Visibility.Visible)
                    {
                        CreateNewWorkPlaceButtonVisibility = Visibility.Visible;
                    }
                }
                Set(ref selectedCabinet, value);
            }

        }
        private WorkPlace selectedWorkplace;
        public WorkPlace SelectedWorkplace
        {
            get => selectedWorkplace;
            set => Set(ref selectedWorkplace, value);
        }


        private Visibility createNewWorkPlaceButtonVisibility;
        public Visibility CreateNewWorkPlaceButtonVisibility
        {
            get => createNewWorkPlaceButtonVisibility;
            set => Set(ref createNewWorkPlaceButtonVisibility, value);
        }

        #endregion
        #region КОМАНДЫ

        public ICommand ExitCommand { get; }
        private void OnExitCommandExecuted(object p)
        {
            TypicalActions.CloseApplication();
        }
        public ICommand BackToMainMenuCommand { get; }
        private void BackToMainMenuCommandAction(object p)
        {
            TypicalActions.GoToWindow<OrganizationWin,MainMenuWin>();
        }


        public ICommand CreateNewCabinetCommand { get; }
        private void CreateNewCabinetCommandAction(object p)
        {
            TypicalActions.GoToDialogWindow<EditCabinetWin>();
            if (SessionStorage.GetInstance().Contains("NewCabinet"))
            {
                CabinetList.Add(SessionStorage.GetInstance().GetValue("NewCabinet") as Cabinet);
                Update(ref cabinetList, "cabinetList");
            }
        }
        public ICommand EditCabinetCommand { get; }
        private void EditCabinetCommandAction(object p)
        {
            SessionStorage.GetInstance().PutValue("EditedCabinet", p as Cabinet);
            TypicalActions.GoToDialogWindow<EditCabinetWin>();
            if (SessionStorage.GetInstance().Contains("EditedCabinet"))
            {
                SessionStorage.GetInstance().GetValue("EditedCabinet");
                Update(ref cabinetList, "cabinetList");
            }
        }
        public ICommand DeleteCabinetCommand { get; }
        private void DeleteCabinetCommandAction(object p)
        {
            if (TypicalActions.ShowQuestionAnswerableMessage("Вы действительно хотите удалить выбранный кабинет?") == MessageBoxResult.Yes)
            {
                foreach (WorkPlace item in (p as Cabinet).WorkPlaceList)
                {
                    DataBaseService.DeleteAccountByWorkplaceId(item.Id);
                    //if (DataBaseService.GetWorkPlaceAccountId(item) != 0)
                    //{
                    //    TypicalActions.ShowInformationalMessage("Нельзя удалить рабочее место за которым закреплен сотрудник");
                    //    return;
                    //}
                }
                DataBaseService.DeleteCabinet(p as Cabinet);
                CabinetList.Remove(p as Cabinet);
                Update(ref cabinetList, "cabinetList");
            }
        }


        public ICommand CreateNewWorkPlaceCommand { get; }
        private void CreateNewWorkPlaceCommandAction(object p)
        {
            SessionStorage.GetInstance().PutValue("SelectedCabinet", SelectedCabinet);
            TypicalActions.GoToDialogWindow<EditWorkPlaceWin>();
            if (SessionStorage.GetInstance().Contains("NewWorkPlace"))
            {
                SelectedCabinet.WorkPlaceList.Add(SessionStorage.GetInstance().GetValue("NewWorkPlace") as WorkPlace);
                Update(ref selectedCabinet, "selectedCabinet");
            }
        }
        public ICommand EditWorkPlaceCommand { get; }
        private void EditWorkPlaceCommandAction(object p)
        {
            SessionStorage.GetInstance().PutValue("EditedWorkPlace", p as WorkPlace);
            TypicalActions.GoToDialogWindow<EditWorkPlaceWin>();
            if (SessionStorage.GetInstance().Contains("EditedWorkPlace"))
            {
                SessionStorage.GetInstance().GetValue("EditedWorkPlace");
                Update(ref selectedCabinet, "selectedCabinet");
            }
        }
        public ICommand DeleteWorkPlaceCommand { get; }
        private void DeleteWorkPlaceCommandAction(object p)
        {
            if (TypicalActions.ShowQuestionAnswerableMessage("Вы действительно хотите удалить выбранное рабочее место?") == MessageBoxResult.Yes)
            {
                DataBaseService.DeleteAccountByWorkplaceId((p as WorkPlace).Id);
                //if (DataBaseService.GetWorkPlaceAccountId((p as WorkPlace)) != 0)
                //{
                //    TypicalActions.ShowInformationalMessage("Нельзя удалить рабочее место за которым закреплен сотрудник");
                //    return;
                //}
                DataBaseService.DeleteWorkPlace(p as WorkPlace);
                selectedCabinet.WorkPlaceList.Remove(p as WorkPlace);
                Update(ref selectedCabinet, "selectedCabinet");
            }
        }
        public ICommand AddNewHardwareToWorkplaceCommand { get; }
        private void AddNewHardwareToWorkplaceCommandAction(object p)
        {
            TypicalActions.GoToDialogWindow<ChooseNewHardwareTypeWin>();

        }
        public ICommand ViewWorkplaceCommand { get; }
        private void ViewWorkplaceCommandAction(object p)
        {
            SessionStorage.GetInstance().PutValue("SelectedWorkplace", p);
            TypicalActions.GoToDialogWindow<ViewWorkplaceWin>();
        }

        #endregion
        #region МОДЕЛЬ

        private List<Cabinet> cabinetList;
        public List<Cabinet> CabinetList
        {
            get => cabinetList;
            set => Set(ref cabinetList, value);
        }

        #endregion


        public OrganizationVM()
        {
            ExitCommand = new UnconditionalLambdaCommand(OnExitCommandExecuted);
            BackToMainMenuCommand = new UnconditionalLambdaCommand(BackToMainMenuCommandAction);
            AddNewHardwareToWorkplaceCommand = new UnconditionalLambdaCommand(AddNewHardwareToWorkplaceCommandAction);

            CreateNewCabinetCommand = new UnconditionalLambdaCommand(CreateNewCabinetCommandAction);
            EditCabinetCommand = new UnconditionalLambdaCommand(EditCabinetCommandAction);
            DeleteCabinetCommand = new UnconditionalLambdaCommand(DeleteCabinetCommandAction);

            CreateNewWorkPlaceCommand = new UnconditionalLambdaCommand(CreateNewWorkPlaceCommandAction);
            ViewWorkplaceCommand = new UnconditionalLambdaCommand(ViewWorkplaceCommandAction);
            EditWorkPlaceCommand = new UnconditionalLambdaCommand(EditWorkPlaceCommandAction);
            DeleteWorkPlaceCommand = new UnconditionalLambdaCommand(DeleteWorkPlaceCommandAction);

            CreateNewWorkPlaceButtonVisibility = Visibility.Collapsed;

            try
            {
                CabinetList = DataBaseService.GetAllKabinets();
            }
            catch (Exception)
            {
                //CabinetList ??= [];
                CabinetList ??=
                [
                    new (1,"Скибиди")
                ];
                CabinetList[0].WorkPlaceList = 
                [
                    new (1,"Скибиди рабочее место")
                ];
            }
            
        }
    }
}
