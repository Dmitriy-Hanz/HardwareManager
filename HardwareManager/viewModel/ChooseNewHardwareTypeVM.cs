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
    class ChooseNewHardwareTypeVM : ViewModelBase
    {
        #region СВОЙСТВА

        #endregion
        #region КОМАНДЫ

        public ICommand SelectHardwareTypeCommand { get; }
        private void SelectHardwareTypeCommandAction(object p)
        {
            SessionStorage.GetInstance().PutValue("HardwareType",p);
            TypicalActions.CloseWindow<ChooseNewHardwareTypeWin>();
        }
        public ICommand SelectWireTypeCommand { get; }
        private void SelectWireTypeCommandAction(object p)
        {
            SessionStorage.GetInstance().PutValue("WireType", p);
            TypicalActions.CloseWindow<ChooseNewHardwareTypeWin>();
        }

        public ICommand CancelCommand { get; }
        private void CancelCommandAction(object p)
        {
            TypicalActions.CloseWindow<ChooseNewHardwareTypeWin>();
        }


        #endregion
        #region МОДЕЛЬ


        #endregion


        public ChooseNewHardwareTypeVM()
        {
            SelectHardwareTypeCommand = new UnconditionalLambdaCommand(SelectHardwareTypeCommandAction);
            SelectWireTypeCommand = new UnconditionalLambdaCommand(SelectWireTypeCommandAction);
            CancelCommand = new UnconditionalLambdaCommand(CancelCommandAction);

        }
    }
}
