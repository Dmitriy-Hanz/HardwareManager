using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.viewModel.Base;
using HardwareManager.infrastructure.commands.Base;
using HardwareManager.model.entity;
using HardwareManager.infrastructure.utils.database;
using HardwareManager.view;
using System.Windows;
using System.Windows.Input;
using HardwareManager.model.view;
using System.Collections.ObjectModel;
using HardwareManager.model;
using Azure.Core;



namespace HardwareManager.viewModel
{
    class AdminMailVM : ViewModelBase
    {
        ///TypicalActions
        ///TypicalValidations

        #region СВОЙСТВА

        #endregion
        #region КОМАНДЫ


        public ICommand ExitCommand { get; }
        private void ExitCommandAction(object p)
        {
            TypicalActions.CloseWindow<AdminMailWin>();
        }

        public ICommand ApplyRequestCommand { get; }
        private void ApplyRequestCommandAction(object p)
        {
            DataBaseService.ApplyRequest((p as RequestView).Original);
            (p as RequestView).Status = (p as RequestView).Original.Status;
            Update(ref requests, "Requests");
        }
        public ICommand DenyRequestCommand { get; }
        private void DenyRequestCommandAction(object p)
        {
            DataBaseService.DenyRequest((p as RequestView).Original);
            (p as RequestView).Status = (p as RequestView).Original.Status;
            Update(ref requests, "Requests");
        }
        public ICommand DeleteRequestCommand { get; }
        private void DeleteRequestCommandAction(object p)
        {
            DataBaseService.DeleteRequest((p as RequestView).Original);
            Requests.Remove(p as RequestView);
        }


        #endregion
        #region МОДЕЛЬ

        private ObservableCollection<RequestView> requests;
        public ObservableCollection<RequestView> Requests
        {
            get => requests;
            set => Set(ref requests, value);
        }

        #endregion


        public AdminMailVM()
        {
            ExitCommand = new UnconditionalLambdaCommand(ExitCommandAction);

            ApplyRequestCommand = new UnconditionalLambdaCommand(ApplyRequestCommandAction);
            DenyRequestCommand = new UnconditionalLambdaCommand(DenyRequestCommandAction);
            DeleteRequestCommand = new UnconditionalLambdaCommand(DeleteRequestCommandAction);

            try
            {
                Requests = [];
                foreach (RequestView item in DataBaseService.GetAllRequestsWithWorkPlaceNames())
                {
                    Requests.Add(item);
                }
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message + "\n\n" + e.StackTrace);
                Requests = [];
                Requests.Add(new RequestView());
                Requests[0].ReasonType = "DefectiveHardware";
                Requests[0].ReasonName = "Неисправное оборудование";
                Requests[0].RequestedType = "Computer";
                Requests[0].RequestedTypeName = "Компьютер";
                Requests[0].Status = "В обработке";
                Requests[0].HardwareIN = 123456789;
                Requests[0].OwnerWorkPlaceName = "Скибиди";
            }
        }
    } 
}
