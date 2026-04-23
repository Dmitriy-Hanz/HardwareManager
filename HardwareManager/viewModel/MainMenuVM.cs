using HardwareManager.infrastructure.commands.Base;
using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.viewModel.Base;
using System.Windows.Input;
using System.Windows;
using System.IO;
using HardwareManager.view;
using HardwareManager.infrastructure.utils.database;
using System.Diagnostics;

namespace HardwareManager.viewModel
{
    class MainMenuVM : ViewModelBase
    {
        #region СВОЙСТВА

        private Visibility testDifficultyPanelVisibility;
        public Visibility TestDifficultyPanelVisibility
        {
            get => testDifficultyPanelVisibility;
            set => Set(ref testDifficultyPanelVisibility, value);
        }

        #endregion
        #region КОМАНДЫ

        public ICommand ExitCommand { get; }
        private void OnExitCommandExecuted(object p)
        {
            TypicalActions.CloseApplication();
        }

        public ICommand ManualCommand { get; }
        private void OnManualCommandExecuted(object p)
        {
            if (!File.Exists($"{Environment.CurrentDirectory}\\HardwareManagerManual.chm"))
            {
                FileStream fs = new FileStream($"{Environment.CurrentDirectory}//HardwareManagerManual.chm", FileMode.CreateNew);
                Application.GetResourceStream(new Uri("HardwareManagerManual.chm", UriKind.Relative)).Stream.CopyTo(fs);
                fs.Close();
            }
            Process.Start("explorer", $"{Environment.CurrentDirectory}\\HardwareManagerManual.chm");
        }

        public ICommand HistoryCommand { get; }
        private void OnHistoryCommandExecuted(object p)
        {
            if (!DataBaseService.IfHistoryExists())
            { 
                MessageBox.Show("Журнал движений пуст", "Оповещение", MessageBoxButton.OK, MessageBoxImage.Information); 
                return; 
            }
            TypicalActions.GoToDialogWindow<HistoryWin>();
        }
        public ICommand OrganizationCommand { get; }
        private void OrganizationCommandAction(object p)
        {
            TypicalActions.GoToWindow<MainMenuWin,OrganizationWin>();
        }
        public ICommand InventoryCommand { get; }
        private void OnInventoryCommandExecuted(object p)
        {
            TypicalActions.GoToWindow<MainMenuWin,InventoryWin>();
        }
        public ICommand FinderCommand { get; }
        private void OnFinderCommandExecuted(object p)
        {
            TypicalActions.GoToWindow<MainMenuWin, SearchWin>();
        }

        public ICommand MailCommand { get; }
        private void MailCommandAction(object p)
        {
            TypicalActions.GoToDialogWindow<AdminMailWin>();
        }

        #endregion
        #region МОДЕЛЬ

        #endregion


        public MainMenuVM()
        {
            OrganizationCommand = new UnconditionalLambdaCommand(OrganizationCommandAction);
            InventoryCommand = new UnconditionalLambdaCommand(OnInventoryCommandExecuted);
            FinderCommand = new UnconditionalLambdaCommand(OnFinderCommandExecuted);
            MailCommand = new UnconditionalLambdaCommand(MailCommandAction);
            HistoryCommand = new UnconditionalLambdaCommand(OnHistoryCommandExecuted);
            ManualCommand = new UnconditionalLambdaCommand(OnManualCommandExecuted);

            ExitCommand = new UnconditionalLambdaCommand(OnExitCommandExecuted);
        }

    }
}
