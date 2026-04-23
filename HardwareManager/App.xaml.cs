using HardwareManager.infrastructure.utils.database;
using HardwareManager.view;
using System.Configuration;
using System.Data;
using System.Windows;

namespace HardwareManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        LoginWin startupWin;
        private void Application_Exit(object sender, ExitEventArgs e)
        {

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            startupWin = new();
            DBUtil.Initialize();
            startupWin.Show();
            startupWin.Activate();
        }
    }

}
