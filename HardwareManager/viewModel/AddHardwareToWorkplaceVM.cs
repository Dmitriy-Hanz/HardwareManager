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
using System.Windows;

namespace HardwareManager.viewModel
{
    class AddHardwareToWorkplaceVM : ViewModelBase
    {
        #region СВОЙСТВА

        private object prop;
        public object Prop
        {
            get => prop;
            set => Set(ref prop, value);
        }

        #endregion
        #region КОМАНДЫ

        public ICommand ExitCommand { get; }
        private void OnExitCommandExecuted(object p)
        {
            TypicalActions.CloseApplication();
        }

        public ICommand GoToCommand { get; }
        private void OnGoToCommandExecuted(object p)
        {
            //TypicalActions.GoToWindow<,>();
            //TypicalActions.GoToDialogWindow<>();
        }

        #endregion
        #region МОДЕЛЬ

        public object model { get; set; }

        #endregion


        public AddHardwareToWorkplaceVM()
        {
            ExitCommand = new UnconditionalLambdaCommand(OnExitCommandExecuted);
            GoToCommand = new UnconditionalLambdaCommand(OnGoToCommandExecuted);

        }
    }
}
