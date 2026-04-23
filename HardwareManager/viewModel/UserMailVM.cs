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
using System.Collections.ObjectModel;



namespace HardwareManager.viewModel
{
    class UserMailVM : ViewModelBase
    {
        ///TypicalActions
        ///TypicalValidations

        #region СВОЙСТВА

        #endregion
        #region КОМАНДЫ


        public ICommand ExitCommand { get; }
        private void ExitCommandAction(object p)
        {
            TypicalActions.CloseWindow<UserMailWin>();
        }

        public ICommand CreateNewRequestCommand { get; }
        private void CreateNewRequestCommandAction(object p)
        {
            SessionStorage.GetInstance().PutValue("targetWorkplace", WorkPlaceView.Original);
            TypicalActions.GoToDialogWindow<EditRequestWin>();
            if (SessionStorage.GetInstance().Contains("NewRequest"))
            {
                Requests.Add(new RequestView(SessionStorage.GetInstance().GetValue<Request>("NewRequest")));
            }
        }


        public ICommand RevokeRequestCommand { get; }
        private void RevokeRequestCommandAction(object p)
        {
            DataBaseService.RevokeRequest((p as RequestView).Original);
            Requests.Remove(p as RequestView);
        }
        public ICommand DeleteRequestCommand { get; }
        private void DeleteRequestCommandAction(object p)
        {
            DataBaseService.DeleteRequest((p as RequestView).Original);
            Requests.Remove(p as RequestView);
        }


        #endregion
        #region МОДЕЛЬ

        private WorkPlaceView workPlaceView;
        public WorkPlaceView WorkPlaceView
        {
            get => workPlaceView;
            set => Set(ref workPlaceView, value);
        }

        private ObservableCollection<RequestView> requests;
        public ObservableCollection<RequestView> Requests
        {
            get => requests;
            set => Set(ref requests, value);
        }

        Account Account { get; set; }

        #endregion


        public UserMailVM()
        {
            ExitCommand = new UnconditionalLambdaCommand(ExitCommandAction);
            CreateNewRequestCommand = new UnconditionalLambdaCommand(CreateNewRequestCommandAction);
            RevokeRequestCommand = new UnconditionalLambdaCommand(RevokeRequestCommandAction);
            DeleteRequestCommand = new UnconditionalLambdaCommand(DeleteRequestCommandAction);

            try
            {
                Account = SessionStorage.GetInstance().GetValue<Account>("CurrentAccount");
                WorkPlaceView = SessionStorage.GetInstance().GetValue<WorkPlaceView>("CurrentWorkPlaceView");
                Requests = [];
                foreach (Request item in DataBaseService.GetAllRequestsByWorkplace(WorkPlaceView.Original))
                {
                    Requests.Add(new RequestView(item));
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
                WorkPlaceView = new();
                WorkPlaceView.Name = "СКибидИ";
                WorkPlaceView.Requests = [];
                WorkPlaceView.Requests.Add(new RequestView());
                WorkPlaceView.Requests[0].ReasonType = "DefectiveHardware";
                WorkPlaceView.Requests[0].ReasonName = "Неисправное оборудование";
                WorkPlaceView.Requests[0].RequestedType = "Computer";
                WorkPlaceView.Requests[0].RequestedTypeName = "Компьютер";
                WorkPlaceView.Requests[0].Status = "В обработке";
                WorkPlaceView.Requests[0].HardwareIN = 123456789;
            }
        }
    }
}
