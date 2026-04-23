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



namespace HardwareManager.viewModel
{
    class EditRequestVM : ViewModelBase
    {
        ///TypicalActions
        ///TypicalValidations

        #region СВОЙСТВА

        private RequestView requestView;
        public RequestView RequestView
        {
            get => requestView;
            set => Set(ref requestView, value);
        }

        private string requestHeaderText;
        public string RequestHeaderText
        {
            get => requestHeaderText;
            set => Set(ref requestHeaderText, value);
        }

        #endregion
        #region КОМАНДЫ

        public ICommand ExitCommand { get; }
        private void ExitCommandAction(object p)
        {
            TypicalActions.CloseWindow<EditRequestWin>();
        }

        public ICommand SaveCommand { get; }
        private void SaveCommandAction(object p)
        {
            if (!RequestView.Validate()) return;
            foreach (Hardware item in WorkPlace.HardwareList)
            {
                if (RequestView.HardwareIN == item.InventorialNumber && !StaticCatalogs.HardwareNameToHardwareType(RequestView.RequestedTypeName).Equals(item.GetType().Name))
                {
                    TypicalActions.ShowInformationalMessage("Инвентарный номер оборудования не соответствует его типу");
                    return;
                }
            }
            RequestView.Status = "В обработке";
            RequestView.Original = new();
            RequestView.Merge(RequestView.Original);
            DataBaseService.SaveRequest(RequestView.Original, WorkPlace);
            SessionStorage.GetInstance().PutValue("NewRequest", RequestView.Original);
            TypicalActions.CloseWindow<EditRequestWin>();
        }


        #endregion
        #region МОДЕЛЬ

        private WorkPlace WorkPlace { get; set; }
        public List<int> WorkPlaceHardwareINs { get; set; }

        #endregion


        public EditRequestVM()
        {
            ExitCommand = new UnconditionalLambdaCommand(ExitCommandAction);
            SaveCommand = new UnconditionalLambdaCommand(SaveCommandAction);

            try
            {
                WorkPlace = SessionStorage.GetInstance().GetValue<WorkPlace>("targetWorkplace");
                RequestView = new();
                RequestHeaderText = "Новый запрос";
                WorkPlaceHardwareINs = [];
                foreach (Hardware item in WorkPlace.HardwareList)
                {
                    WorkPlaceHardwareINs.Add(item.InventorialNumber);
                }
            }
            catch (Exception)
            {
                RequestHeaderText = "Новый запрос";
            }
        }

    }
}
