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
    class UserViewWorkplaceVM : ViewModelBase
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
            TypicalActions.CloseWindow<UserViewWorkplaceWin>();
        }
        public ICommand ExitAccountCommand { get; }
        private void ExitAccountCommandAction(object p)
        {
            TypicalActions.GoToWindow<UserViewWorkplaceWin,LoginWin>();
        }


        public ICommand MailCommand { get; }
        private void MailCommandAction(object p)
        {
            SessionStorage.GetInstance().PutValue("CurrentAccount", Account);
            SessionStorage.GetInstance().PutValue("CurrentWorkPlaceView", WorkPlaceView);
            TypicalActions.GoToDialogWindow<UserMailWin>();
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

        Account Account { get; set; }

        #endregion


        public UserViewWorkplaceVM()
        {
            ExitCommand = new UnconditionalLambdaCommand(ExitCommandAction);
            ExitAccountCommand = new UnconditionalLambdaCommand(ExitAccountCommandAction);

            MailCommand = new UnconditionalLambdaCommand(MailCommandAction);
            ViewHardwareCommand = new UnconditionalLambdaCommand(ViewHardwareCommandAction);

            try
            {
                Account = SessionStorage.GetInstance().GetValue("currentUser") as Account; 
                WorkPlaceView = new WorkPlaceView(DataBaseService.GetWorkplaceByAccount(Account));
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
                WorkPlaceView = new();
                WorkPlaceView.Name = "СКибидИ";
            }
        }
    }
}
