using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.viewModel.Base;
using HardwareManager.infrastructure.commands.Base;
using HardwareManager.view;
using System.Windows.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HardwareManager.model;
using System.Windows;
using HardwareManager.infrastructure.utils.database;
using HardwareManager.model.view;

namespace HardwareManager.viewModel
{
    class LoginVM : ViewModelBase
    {
        #region СВОЙСТВА

        private Visibility loginFormVisibility;
        public Visibility LoginFormVisibility
        {
            get => loginFormVisibility;
            set
            {
                if (value == Visibility.Visible) 
                {
                    RegistrationFormVisibility = Visibility.Collapsed;
                    SuccessFormVisibility = Visibility.Collapsed;
                }
                Set(ref loginFormVisibility, value);
            }
        }

        private Visibility registrationFormVisibility;
        public Visibility RegistrationFormVisibility
        {
            get => registrationFormVisibility;
            set 
            {
                if (value == Visibility.Visible)
                {
                    LoginFormVisibility = Visibility.Collapsed;
                    SuccessFormVisibility = Visibility.Collapsed;
                }
                Set(ref registrationFormVisibility, value);
            }
        }

        private Visibility successFormVisibility;
        public Visibility SuccessFormVisibility
        {
            get => successFormVisibility;
            set 
            {
                if (value == Visibility.Visible)
                {
                    LoginFormVisibility = Visibility.Collapsed;
                    RegistrationFormVisibility = Visibility.Collapsed;
                }
                Set(ref successFormVisibility, value); 
            }
        }




        private Visibility loginErrorPanelVisibility;
        public Visibility LoginErrorPanelVisibility
        {
            get => loginErrorPanelVisibility;
            set => Set(ref loginErrorPanelVisibility, value);
        }

        private Visibility loginHeaderPanelVisibility;
        public Visibility LoginHeaderPanelVisibility
        {
            get => loginHeaderPanelVisibility;
            set => Set(ref loginHeaderPanelVisibility, value);
        }

        #endregion
        #region КОМАНДЫ

        public ICommand ExitCommand { get; }
        private void OnExitCommandExecuted(object p)
        {
            TypicalActions.CloseApplication();
        }

        public ICommand GoToLoginFormCommand { get; }
        private void GoToLoginFormCommandAction(object p)
        {
            LoginHeaderPanelVisibility = Visibility.Visible;
            LoginFormVisibility = Visibility.Visible;
            AccountView = new();
            CabinetNameCBSelectedValue = null;
        }

        public ICommand GoToRegistrationFormCommand { get; }
        private void GoToRegistrationFormCommandAction(object p)
        {
            if (CabinetsNames == null)
            {
                CabinetsNames = DataBaseService.GetAllKabinetsNames();
            }
            LoginErrorPanelVisibility = Visibility.Collapsed;
            LoginHeaderPanelVisibility = Visibility.Collapsed;
            RegistrationFormVisibility = Visibility.Visible;
            AccountView = new();
            CabinetNameCBSelectedValue = null;
        }

        public ICommand LoginCommand { get; }
        private void LoginCommandAction(object p)
        {
            if (!AccountView.ValidateLogin()) return;

            Account = DataBaseService.GetAccount(AccountView.Username, AccountView.Password);

            if (Account != null) {
                SessionStorage.GetInstance().PutValue("currentUser", Account);

                if (Account.Role.RoleName.Equals("Admin"))
                {
                    TypicalActions.GoToWindow<LoginWin, MainMenuWin>();
                }
                else
                {
                    TypicalActions.GoToWindow<LoginWin, UserViewWorkplaceWin>();
                }
            }
            LoginErrorPanelVisibility = Visibility.Visible;
        }

        public ICommand RegistrationCommand { get; }
        private void RegistrationCommandAction(object p)
        {
            if (!AccountView.ValidateRegistration()) return;
            
            DataBaseService.SaveAccount(AccountView.Convert(),AccountView.Cabinet, AccountView.Workplace);

            SuccessFormVisibility = Visibility.Visible;
        }
        public ICommand CancelRegistrationCommand { get; }
        private void CancelRegistrationCommandAction(object p)
        {
            AccountView = new();
            CabinetNameCBSelectedValue = null;
            LoginHeaderPanelVisibility = Visibility.Visible;
            LoginFormVisibility = Visibility.Visible;
        }

        #endregion
        #region МОДЕЛЬ

        private Account account;
        public Account Account
        {
            get => account;
            set => Set(ref account, value);
        }

        private AccountView accountView;
        public AccountView AccountView
        {
            get => accountView;
            set => Set(ref accountView, value);
        }

        private List<string> cabinetsNames;
        public List<string> CabinetsNames
        {
            get => cabinetsNames;
            set => Set(ref cabinetsNames, value);
        }        
        private List<string> workplacesNames;
        public List<string> WorkplacesNames
        {
            get => workplacesNames;
            set => Set(ref workplacesNames, value);
        }
        private string cabinetNameCBSelectedValue;
        public string CabinetNameCBSelectedValue
        {
            get => cabinetNameCBSelectedValue;
            set
            {
                AccountView.Cabinet = value;
                WorkplacesNames = DataBaseService.GetWorkplacesNamesByCabinetName(value);
                Set(ref cabinetNameCBSelectedValue, value);
            }
        }

        #endregion


        public LoginVM()
        {                           
            ExitCommand = new UnconditionalLambdaCommand(OnExitCommandExecuted);
            GoToLoginFormCommand = new UnconditionalLambdaCommand(GoToLoginFormCommandAction);
            GoToRegistrationFormCommand = new UnconditionalLambdaCommand(GoToRegistrationFormCommandAction);

            LoginCommand = new UnconditionalLambdaCommand(LoginCommandAction);
            RegistrationCommand = new UnconditionalLambdaCommand(RegistrationCommandAction);
            CancelRegistrationCommand = new UnconditionalLambdaCommand(CancelRegistrationCommandAction);


            LoginFormVisibility = Visibility.Visible;
            RegistrationFormVisibility = Visibility.Collapsed;
            SuccessFormVisibility = Visibility.Collapsed;

            LoginErrorPanelVisibility = Visibility.Collapsed;
            LoginHeaderPanelVisibility = Visibility.Visible;
            WorkplacesNames = [];

            AccountView = new();
            //AccountView.Username = AccountView.Password = "admin";
        }
    }
}
