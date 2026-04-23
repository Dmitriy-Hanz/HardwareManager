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
    class HardwareAddingOptionsVM : ViewModelBase
    {
        ///TypicalActions
        ///TypicalValidations

        #region СВОЙСТВА


        private object sampleProperty;
        public object SampleProperty
        {
            get => sampleProperty;
            set => Set(ref sampleProperty, value);
        }


        #endregion
        #region КОМАНДЫ

        public ICommand SelectCommand { get; }
        private void SelectCommandAction(object p)
        {
            SessionStorage.GetInstance().SetFlag(p.ToString());
            TypicalActions.CloseWindow<HardwareAddingOptionsWin>();
        }

        public ICommand CancelCommand { get; }
        private void CancelCommandAction(object p)
        {
            TypicalActions.CloseWindow<HardwareAddingOptionsWin>();
        }
        

        #endregion
        #region МОДЕЛЬ


        private object modelSample;
        public object ModelSample
        {
            get => modelSample;
            set => Set(ref modelSample, value);
        }


        #endregion


        public HardwareAddingOptionsVM()
        {
            SelectCommand = new UnconditionalLambdaCommand(SelectCommandAction);
            CancelCommand = new UnconditionalLambdaCommand(CancelCommandAction);
        }
    }
}
