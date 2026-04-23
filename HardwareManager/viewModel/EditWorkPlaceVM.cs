using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.viewModel.Base;
using HardwareManager.infrastructure.commands.Base;
using HardwareManager.model.entity;
using HardwareManager.infrastructure.utils.database;
using HardwareManager.view;
using System.Windows;
using System.Windows.Input;
using HardwareManager.model.view;



namespace HardwareManager.viewModel
{
    class EditWorkPlaceVM : ViewModelBase
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


        private Visibility newWorkPlaceViewElementsVisibility;
        public Visibility NewWorkPlaceViewElementsVisibility
        {
            get => newWorkPlaceViewElementsVisibility;
            set => Set(ref newWorkPlaceViewElementsVisibility, value);
        }


        #endregion
        #region КОМАНДЫ

        public ICommand CancelCommand { get; }
        private void CancelCommandAction(object p)
        {
            TypicalActions.CloseWindow<EditWorkPlaceWin>();
        }

        public ICommand SaveCommand { get; }
        private void SaveCommandAction(object p)
        {
            if (!WorkPlaceView.Validate()) return;
            if (WorkPlace == null)
            {
                WorkPlace = WorkPlaceView.Convert();
                DataBaseService.SaveWorkPlace(WorkPlace,SelectedCabinet);
                SessionStorage.GetInstance().PutValue("NewWorkPlace", WorkPlace);
            }
            else
            {
                WorkPlaceView.Merge(WorkPlace);
                DataBaseService.UpdateWorkPlace(WorkPlace);
                SessionStorage.GetInstance().PutValue("EditedWorkPlace", WorkPlace);
            }
            TypicalActions.CloseWindow<EditWorkPlaceWin>();
        }

        #endregion
        #region МОДЕЛЬ
        
        WorkPlace WorkPlace { get; set; }
        Cabinet SelectedCabinet { get; set; }

        #endregion


        public EditWorkPlaceVM()
        {
            CancelCommand = new UnconditionalLambdaCommand(CancelCommandAction);
            SaveCommand = new UnconditionalLambdaCommand(SaveCommandAction);

            if (SessionStorage.GetInstance().Contains("EditedWorkPlace"))
            {
                WorkPlace = SessionStorage.GetInstance().GetValue("EditedWorkPlace") as WorkPlace;
                WorkPlaceView = new WorkPlaceView(WorkPlace);
                NewWorkPlaceViewElementsVisibility = Visibility.Collapsed;
            }
            else
            {
                SelectedCabinet = SessionStorage.GetInstance().GetValue("SelectedCabinet") as Cabinet;
                WorkPlaceView = new();
                NewWorkPlaceViewElementsVisibility = Visibility.Visible;
            }
        }
    }
}
