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
using HardwareManager.model.entity;
using HardwareManager.model.view;
using HardwareManager.infrastructure.utils.database;

namespace HardwareManager.viewModel
{
    class EditCabinetVM : ViewModelBase
    {
        #region СВОЙСТВА

        private CabinetView cabinetView;
        public CabinetView CabinetView
        {
            get => cabinetView;
            set => Set(ref cabinetView, value);
        }


        private Visibility newCabinetViewElementsVisibility;
        public Visibility NewCabinetViewElementsVisibility
        {
            get => newCabinetViewElementsVisibility;
            set => Set(ref newCabinetViewElementsVisibility, value);
        }


        #endregion
        #region КОМАНДЫ

        public ICommand CancelCommand { get; }
        private void CancelCommandAction(object p)
        {
            TypicalActions.CloseWindow<EditCabinetWin>();
        }

        public ICommand SaveCommand { get; }
        private void SaveCommandAction(object p)
        {
            if (!CabinetView.Validate()) return;
            if (Cabinet == null)
            {
                Cabinet = CabinetView.Convert();
                DataBaseService.SaveCabinet(Cabinet);
                SessionStorage.GetInstance().PutValue("NewCabinet", Cabinet);
            }
            else
            {
                CabinetView.Merge(Cabinet);
                DataBaseService.UpdateCabinet(Cabinet);
                SessionStorage.GetInstance().PutValue("EditedCabinet", Cabinet);
            }
            TypicalActions.CloseWindow<EditCabinetWin>();
        }

        #endregion
        #region МОДЕЛЬ

        Cabinet Cabinet { get; set; }

        #endregion


        public EditCabinetVM()
        {
            CancelCommand = new UnconditionalLambdaCommand(CancelCommandAction);
            SaveCommand = new UnconditionalLambdaCommand(SaveCommandAction);

            if (SessionStorage.GetInstance().Contains("EditedCabinet"))
            {
                Cabinet = SessionStorage.GetInstance().GetValue("EditedCabinet") as Cabinet;
                CabinetView = new CabinetView(Cabinet);
                NewCabinetViewElementsVisibility = Visibility.Collapsed;
            }
            else
            {
                CabinetView = new();
                NewCabinetViewElementsVisibility = Visibility.Visible;
            }
        }
    }
}
