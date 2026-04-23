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
using System.IO;
using System.Windows.Forms;



namespace HardwareManager.viewModel
{
    class ViewWireVM : ViewModelBase
    {
        ///TypicalActions
        ///TypicalValidations

        #region СВОЙСТВА

        private WireView wireView;
        public WireView WireView
        {
            get => wireView;
            set => Set(ref wireView, value);
        }

        private CableView cableView;
        public CableView CableView
        {
            get => cableView;
            set => Set(ref cableView, value);
        }
        private AdapterCableView adapterCableView;
        public AdapterCableView AdapterCableView
        {
            get => adapterCableView;
            set => Set(ref adapterCableView, value);
        }

        #endregion
        #region КОМАНДЫ

        public ICommand ExitCommand { get; }
        private void ExitCommandAction(object p)
        {
            TypicalActions.CloseWindow<ViewWireWin>();
        }

        #endregion
        #region МОДЕЛЬ



        #endregion


        public ViewWireVM()
        {
            ExitCommand = new UnconditionalLambdaCommand(ExitCommandAction);

            try
            {
                WireView = new WireView(SessionStorage.GetInstance().GetValue<Wire>("Wire"));
                switch (WireView.WireType)
                {
                    case "Cable":
                        CableView = new CableView(WireView.Original as Cable);
                        break;
                    case "AdapterCable":
                        AdapterCableView = new AdapterCableView(WireView.Original as AdapterCable);
                        break;
                };
            }
            catch (Exception)
            {
                WireView = new("Cable");
            }
        }
    }
}
