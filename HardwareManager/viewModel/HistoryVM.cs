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
using System.Data;
using System.IO;
using HardwareManager.infrastructure.utils.database;
using HardwareManager.model.entity;
using System.Collections.ObjectModel;

namespace HardwareManager.viewModel
{
    class HistoryVM : ViewModelBase
    {
        #region СВОЙСТВА

        private DateTime? searchDate;
        public DateTime? SearchDate
        {
            get => searchDate;
            set => Set(ref searchDate, value);
        }
        private string searchOperation;
        public string SearchOperation
        {
            get => searchOperation;
            set => Set(ref searchOperation, value);
        }

        #endregion
        #region КОМАНДЫ

        public ICommand CloseCommand { get; }
        private void OnCloseCommandExecuted(object p)
        {
            TypicalActions.CloseWindow<HistoryWin>();
        }

        public ICommand ClearHistoryCommand { get; }
        private void OnClearHistoryCommandExecuted(object p)
        {
            if (MessageBox.Show("Вы уверены, что хотите удалить все записи?", "Предупреждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes) 
            {
                HistoryRecords.Clear();
                DataBaseService.ClearHistory();
                Update(ref historyRecords, "historyRecords");
            }
        }

        public ICommand SearchCommand { get; }
        private void SearchCommandAction(object p)
        {
            HistoryRecords.Clear();
            foreach (HistoryRecord item in HistoryRecordsSource)
            {
                if (SearchDate != null && ((DateTime)item.OperationDate).Date != ((DateTime)SearchDate).Date) continue;
                if (SearchOperation != null && !SearchOperation.Equals("") && !SearchOperation.Equals(item.OperationType)) continue;
                HistoryRecords.Add(item);
            }
        }
        public ICommand ClearFiltersCommand { get; }
        private void ClearFiltersCommandAction(object p)
        {
            SearchDate = null;
            SearchOperation = null;
            HistoryRecords.Clear();
            foreach (HistoryRecord item in HistoryRecordsSource)
            {
                HistoryRecords.Add(item);
            }
        }

        #endregion
        #region МОДЕЛЬ

        private ObservableCollection<HistoryRecord> historyRecords;
        public ObservableCollection<HistoryRecord> HistoryRecords
        {
            get => historyRecords;
            set => Set(ref historyRecords, value);
        }

        private List<HistoryRecord> historyRecordsSource;
        public List<HistoryRecord> HistoryRecordsSource
        {
            get => historyRecordsSource;
            set => Set(ref historyRecordsSource, value);
        }

        #endregion


        public HistoryVM()
        {
            CloseCommand = new UnconditionalLambdaCommand(OnCloseCommandExecuted);
            ClearHistoryCommand = new UnconditionalLambdaCommand(OnClearHistoryCommandExecuted);
            SearchCommand = new UnconditionalLambdaCommand(SearchCommandAction);
            ClearFiltersCommand = new UnconditionalLambdaCommand(ClearFiltersCommandAction);

            try
            {
                HistoryRecordsSource = DataBaseService.GetHistory();
                HistoryRecords = [];
                foreach (HistoryRecord item in HistoryRecordsSource)
                {
                    HistoryRecords.Add(item);
                }
            }
            catch (Exception)
            {
                HistoryRecords = [];
            }
            
        }
    }
}
