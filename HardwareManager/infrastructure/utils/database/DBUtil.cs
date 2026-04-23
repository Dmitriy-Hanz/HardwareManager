using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows;
using System.Windows.Resources;
using System.IO;
using System.Threading;
using Microsoft.Data.SqlClient;
using System.Collections;
using System.Windows.Threading;
using System.Windows.Controls;
using HardwareManager.view;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HardwareManager.infrastructure.utils.Base;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Media3D;
using System.Windows.Input;
using System.Windows.Media.Animation;
using HardwareManager.model;
using System.Security.Cryptography;

namespace HardwareManager.infrastructure.utils.database
{
    class DBUtil
    {
        private class ConnectionWindow
        {
            public enum InterfaceState
            {
                SearchingForSqlServer,
                ConnectionToSystemDB,
                ConnectionToMainDB,
                CreationOfMainDB,
                SecondConnectionToMainDB,
                Finish
            }
            public static Window ConnectionWin { get; set; }
            public static Dispatcher ConnectionWinDispatcher { get; set; }
            public static InterfaceState CurrentInterfaceState { get; set; }
            private static List<TextBlock> InformTextBlocks { get; set; } = new List<TextBlock>();
            private static ScrollViewer ScrollViewer { get; set; }

            public static Window CreateConnectionWindow()
            {
                Window w = new()
                {
                    WindowStyle = WindowStyle.None,
                    ResizeMode = ResizeMode.NoResize,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Height = 70,
                    Width = 350,
                };
                Canvas canvas = new();
                w.Content = canvas;

                Image image = new Image();
                image.Source = new BitmapImage(new Uri("pack://application:,,,/infrastructure/utils/database/AnimatedLoadingImg.png"));
                RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality);
                image.Width = image.Height = 50;
                image.RenderTransform = new RotateTransform { CenterX = 25, CenterY = 25 };
                DoubleAnimation imageAnim = new DoubleAnimation();
                imageAnim.RepeatBehavior= RepeatBehavior.Forever;
                imageAnim.By = 360;
                imageAnim.Duration = TimeSpan.FromSeconds(0.9);
                imageAnim.FillBehavior = FillBehavior.HoldEnd;
                imageAnim.DecelerationRatio = 1;
                image.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, imageAnim);

                canvas.Children.Add(image);

                Canvas.SetTop(image, 10);
                Canvas.SetLeft(image, 10);

                Border border = new()
                {
                    Height = 50,
                    Width = 270,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(1),
                };
                Canvas.SetTop(border, 10);
                Canvas.SetLeft(border, 70);
                canvas.Children.Add(border);

                ScrollViewer = new();
                border.Child = ScrollViewer;

                StackPanel stackPanel = new();
                ScrollViewer.Content = stackPanel;

                TextBlock searchingForSqlServer_TB = new()
                {
                    Visibility = Visibility.Collapsed,
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(3,0,3,0),
                    Text = "Обнаружение SQL Server",
                    Name = "SearchingForSqlServer_TB"
                };

                TextBlock connectionToSystemDB_TB = new()
                {
                    Visibility = Visibility.Collapsed,
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(3, 0, 3, 0),
                    Text = "Подключение к системной базе данных",
                    Name = "ConnectionToSystemDB_TB"
                };

                TextBlock connectionToMainDB_TB = new()
                {
                    Visibility = Visibility.Collapsed,
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(3, 0, 3, 0),
                    Text = "Подключение к основной базе данных",
                    Name = "ConnectionToMainDB_TB"
                };

                TextBlock creationOfMainDB_TB = new()
                {
                    Visibility = Visibility.Collapsed,
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(3, 0, 3, 0),
                    Text = "Создание основной базы данных",
                    Name = "CreationOfMainDB_TB"
                };

                TextBlock secondConnectionToMainDB_TB = new()
                {
                    Visibility = Visibility.Collapsed,
                    FontSize = 12,
                    FontWeight = FontWeights.Bold,
                    Margin = new Thickness(3, 0, 3, 0),
                    Text = "Подключение к основной базе данных",
                    Name = "SecondConnectionToMainDB_TB"
                };

                stackPanel.Children.Add(searchingForSqlServer_TB);
                stackPanel.Children.Add(connectionToSystemDB_TB);
                stackPanel.Children.Add(connectionToMainDB_TB);
                stackPanel.Children.Add(creationOfMainDB_TB);
                stackPanel.Children.Add(secondConnectionToMainDB_TB);

                InformTextBlocks.AddRange(stackPanel.Children.Cast<TextBlock>());

                return w;
            }

            public static void StartConnectionWindow()
            {
                Thread t = new Thread(() =>
                {
                    ConnectionWinDispatcher = Dispatcher.CurrentDispatcher;
                    ConnectionWin = CreateConnectionWindow();
                    Dispatcher.Run();
                });
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                t.Start();
                Thread.Sleep(1000);
            }

            public static void ChangeInterfaceState(InterfaceState interfaceState)
            {
                ConnectionWinDispatcher.BeginInvoke(() =>
                {
                    CurrentInterfaceState = interfaceState;
                    switch (CurrentInterfaceState)
                    {
                        case InterfaceState.SearchingForSqlServer:
                            GetTextBlockByName("SearchingForSqlServer_TB").Visibility = Visibility.Visible;
                            break;
                        case InterfaceState.ConnectionToSystemDB:
                            GetTextBlockByName("SearchingForSqlServer_TB").Foreground = Brushes.DarkGreen;
                            GetTextBlockByName("ConnectionToSystemDB_TB").Visibility = Visibility.Visible;
                            break;
                        case InterfaceState.ConnectionToMainDB:
                            GetTextBlockByName("ConnectionToSystemDB_TB").Foreground = Brushes.DarkGreen;
                            GetTextBlockByName("ConnectionToMainDB_TB").Visibility = Visibility.Visible;
                            break;
                        case InterfaceState.CreationOfMainDB:
                            ScrollViewer.ScrollToEnd();
                            GetTextBlockByName("ConnectionToMainDB_TB").Foreground = Brushes.Yellow;
                            GetTextBlockByName("CreationOfMainDB_TB").Visibility = Visibility.Visible;
                            break;
                        case InterfaceState.SecondConnectionToMainDB:
                            ScrollViewer.ScrollToEnd();
                            GetTextBlockByName("CreationOfMainDB_TB").Foreground = Brushes.DarkGreen;
                            GetTextBlockByName("SecondConnectionToMainDB_TB").Visibility = Visibility.Visible;
                            break;
                        case InterfaceState.Finish:
                            ScrollViewer.ScrollToEnd();
                            GetTextBlockByName("SecondConnectionToMainDB_TB").Foreground = Brushes.DarkGreen;
                            GetTextBlockByName("ConnectionToMainDB_TB").Foreground = Brushes.DarkGreen;
                            break;
                    }
                });
                Thread.Sleep(1000);
            }

            public static void SetFailed()
            {
                switch (CurrentInterfaceState)
                {
                    case InterfaceState.SearchingForSqlServer:
                        MessageBox.Show("Не удалось обнаружить экземпляр SQL Server SQL. Убедитесь, что на вашем ПК установлено соответствующее ПО.", "Ошибка обнаружения!", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case InterfaceState.ConnectionToSystemDB:
                        MessageBox.Show("Не удалось установить соединение с сервером SQL.", "Ошибка подключения!", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case InterfaceState.CreationOfMainDB:
                        MessageBox.Show("Не удалось создать файлы и подключить основную БД к SQL Server.", "Ошибка создания!", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case InterfaceState.ConnectionToMainDB:
                        MessageBox.Show("Не удалось подключится к основной БД.", "Ошибка подключения!", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    case InterfaceState.SecondConnectionToMainDB:
                        MessageBox.Show("Не удалось подключится к основной БД.", "Ошибка подключения!", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
                CloseWindow();
            }

            public static void CloseWindow()
            {
                ConnectionWinDispatcher.BeginInvoke(() =>
                {
                    ConnectionWin.Close();
                });
            }

            public static void ShowWindow()
            {
                ConnectionWinDispatcher.BeginInvoke(() =>
                {
                    ConnectionWin.Show();
                });
            }


            private static TextBlock GetTextBlockByName(string name)
            {
                foreach (TextBlock item in InformTextBlocks)
                {
                    if (item.Name.Equals(name))
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        public const string DATABASE_NAME = "HardwareManagerDB";
        private const string DEFAULT_FAILED_SQL_SERVER_EXEMPLAR_NAME = ".MSSQLSERVER";
        private static ArrayList SQL_SERVER_EXEMPLARS;
        public static string SQL_SERVER_EXEMPLAR_NAME;
        public static bool LOCALE_MODE;
        public static string NO_LOCALE_CONNECTION_STRING;
        private static SqlConnection con;
        private static SqlDataAdapter sda;
        private static bool isDatabaseConnected;
        public static string CurrentDataBaseFilepath { get; private set; }

        private static ArrayList FindSqlServerExemplars()
        {
            ArrayList sqlExemplarsNames = new();

            ArrayList allFilesNames = new();
            if (Directory.Exists("C:\\Program Files"))
            {
                allFilesNames.AddRange(Directory.GetDirectories("C:\\Program Files", "*"));
            }
            if (Directory.Exists("C:\\Program Files (x86)"))
            {
                allFilesNames.AddRange(Directory.GetDirectories("C:\\Program Files (x86)", "*"));
            }
            DriveInfo[] logicalDriversObjects = DriveInfo.GetDrives();
            foreach (DriveInfo item in logicalDriversObjects)
            {
                if (!item.Name.Equals("C:\\") && item.IsReady)
                {
                    allFilesNames.AddRange(Directory.GetDirectories(item.Name, "*"));
                }
            }
            List<string> sqlFilesNames = new();
            foreach (string item in allFilesNames)
            {
                if (item.Contains("sql", StringComparison.OrdinalIgnoreCase))
                {
                    sqlFilesNames.Add(item);
                }
            }

            foreach (string item in sqlFilesNames)
            {
                foreach (string directoryPath in Directory.GetDirectories(item, "*", SearchOption.AllDirectories))
                {
                    if (directoryPath.Contains("DATA"))
                    {
                        string temp = directoryPath[(item.Length + 1)..];
                        temp = temp.Remove(temp.IndexOf('\\'));

                        if (!temp.Contains(DEFAULT_FAILED_SQL_SERVER_EXEMPLAR_NAME))
                        {
                            sqlExemplarsNames.Add(".\\" + temp.Substring(temp.IndexOf('.') + 1));
                            continue;
                        }
                        sqlExemplarsNames.Add(".");
                        break;
                    }
                }
            }

            return sqlExemplarsNames;
        }
        private static string GetSqlServerExemplarName()
        {
            object[,] sqlConnectionResults = new object[5, SQL_SERVER_EXEMPLARS.Count];
            for (int i = 0; i < SQL_SERVER_EXEMPLARS.Count; i++)
            {
                sqlConnectionResults[0, i] = i;
                sqlConnectionResults[1, i] = null;
                sqlConnectionResults[2, i] = new SqlConnection($"Data Source={SQL_SERVER_EXEMPLARS[i]};Initial Catalog=master;Database=master;Integrated Security=True;TrustServerCertificate=True");
                sqlConnectionResults[3, i] = SQL_SERVER_EXEMPLARS[i];
                sqlConnectionResults[4, i] = new Task((object index) =>
                {
                    try
                    {
                        (sqlConnectionResults[2, (int)index] as SqlConnection).Open();
                        (sqlConnectionResults[2, (int)index] as SqlConnection).Close();
                        sqlConnectionResults[1, (int)index] = true;
                    }
                    catch (Exception)
                    {
                        sqlConnectionResults[1, (int)index] = false;
                        return;
                    }
                }, i);
                (sqlConnectionResults[4, i] as Task).Start();
            }

            int failedFlag = 0;
            while (true)
            {
                Thread.Sleep(1000);
                for (int i = 0; i < SQL_SERVER_EXEMPLARS.Count; i++)
                {
                    if (sqlConnectionResults[1, i] != null)
                    {
                        if ((bool)sqlConnectionResults[1, i])
                        {
                            return sqlConnectionResults[3, i].ToString();
                        }
                        else
                        {
                            failedFlag++;
                        }
                    }
                }
                if (failedFlag == SQL_SERVER_EXEMPLARS.Count)
                {
                    return "";
                }
                failedFlag = 0;
            }
        }

        public static void LoadCfg()
        {
            if (!File.Exists($"{Environment.CurrentDirectory}\\dbcfg.ini"))
            {
                StreamResourceInfo fileResourse = Application.GetResourceStream(new Uri($"infrastructure/utils/database/dbcfg.ini", UriKind.Relative));
                FileStream fs = new FileStream($"{Environment.CurrentDirectory}/dbcfg.ini", FileMode.Create);
                fileResourse.Stream.CopyTo(fs);
                fs.Close();
                fileResourse.Stream.Close();
                Thread.Sleep(1000);
            }
            List<string> cfg = File.ReadLines($"{Environment.CurrentDirectory}\\dbcfg.ini").ToList();
            LOCALE_MODE = bool.Parse(cfg[0].Replace("LocaleMode = ", ""));
            NO_LOCALE_CONNECTION_STRING = cfg[1].Replace("DataBaseConnectionString = ", "");
        }
        public static bool InitializeLocale()
        {
            ConnectionWindow.StartConnectionWindow();
            ConnectionWindow.ShowWindow();
            ConnectionWindow.ChangeInterfaceState(ConnectionWindow.InterfaceState.SearchingForSqlServer);

            if (SQL_SERVER_EXEMPLARS.Count == 0)
            {
                ConnectionWindow.SetFailed();
                App.Current.Shutdown();
                return isDatabaseConnected = false;
            }

            if (SQL_SERVER_EXEMPLAR_NAME == "")
            {
                ConnectionWindow.SetFailed();
                App.Current.Shutdown();
                return isDatabaseConnected = false;
            }

            ConnectionWindow.ChangeInterfaceState(ConnectionWindow.InterfaceState.ConnectionToSystemDB);

            sda = new SqlDataAdapter();
            StringBuilder connectionString = new StringBuilder($"Data Source={SQL_SERVER_EXEMPLAR_NAME};Initial Catalog=master;Database=master;Integrated Security=True;TrustServerCertificate=True");
            con = new SqlConnection(connectionString.ToString());
            try
            {
                Execute($"USE master");
            }
            catch (Exception)
            {
                ConnectionWindow.SetFailed();
                App.Current.Shutdown();
                return isDatabaseConnected = false;
            }

            ConnectionWindow.ChangeInterfaceState(ConnectionWindow.InterfaceState.ConnectionToMainDB);

            CurrentDataBaseFilepath = ExecuteReturnOld("SELECT physical_name FROM sys.master_files WHERE database_id = DB_ID(N'master') AND type_desc = N'ROWS' ")[0][0].ToString().Replace("\\master.mdf", "");

            connectionString.Replace("master", DATABASE_NAME);
            con = new SqlConnection(connectionString.ToString());
            try
            {
                Execute($"USE {DATABASE_NAME}");
                ConnectionWindow.ChangeInterfaceState(ConnectionWindow.InterfaceState.Finish);
                ConnectionWindow.CloseWindow();
                return isDatabaseConnected = true;
            }
            catch
            {
                ConnectionWindow.ChangeInterfaceState(ConnectionWindow.InterfaceState.CreationOfMainDB);
                connectionString.Replace(DATABASE_NAME, "master");
                con = new SqlConnection(connectionString.ToString());
                if (File.Exists($"{Environment.CurrentDirectory}\\{DATABASE_NAME}.mdf") == false)
                {
                    if (File.Exists($"{Environment.CurrentDirectory}\\{DATABASE_NAME}_log.ldf"))
                    {
                        File.Delete($"databaseFiles/{DATABASE_NAME}_log.ldf");
                    }
                    StreamResourceInfo dbFileResourse = Application.GetResourceStream(new Uri($"resources/databaseFiles/{DATABASE_NAME}.mdf", UriKind.Relative));
                    StreamResourceInfo dbLogResourse = Application.GetResourceStream(new Uri($"resources/databaseFiles/{DATABASE_NAME}_log.ldf", UriKind.Relative));
                    FileStream fs1 = new FileStream($"{Environment.CurrentDirectory}/{DATABASE_NAME}.mdf", FileMode.Create);
                    FileStream fs2 = new FileStream($"{Environment.CurrentDirectory}/{DATABASE_NAME}_log.ldf", FileMode.Create);
                    dbFileResourse.Stream.CopyTo(fs1);
                    dbLogResourse.Stream.CopyTo(fs2);
                    fs1.Close();
                    fs2.Close();
                    dbFileResourse.Stream.Close();
                    dbLogResourse.Stream.Close();
                    Thread.Sleep(1000);
                }

                try
                {
                    AttachCurrentDatabase();
                }
                catch (Exception ex)
                {
                    ConnectionWindow.SetFailed();
                    App.Current.Shutdown();
                    return isDatabaseConnected = false;
                }
                ConnectionWindow.ChangeInterfaceState(ConnectionWindow.InterfaceState.SecondConnectionToMainDB);
                
                connectionString.Replace("master", DATABASE_NAME);
                try
                {
                    con = new SqlConnection(connectionString.ToString());
                    Execute($"USE {DATABASE_NAME}");
                    ConnectionWindow.ChangeInterfaceState(ConnectionWindow.InterfaceState.Finish);
                    ConnectionWindow.CloseWindow();
                    return isDatabaseConnected = true;
                }
                catch (Exception ex)
                {
                    ConnectionWindow.SetFailed();
                    App.Current.Shutdown();
                    return isDatabaseConnected = false;
                }
            }
        }
        public static bool InitializeServer(string databaseName = DATABASE_NAME)
        {
            ConnectionWindow.StartConnectionWindow();
            ConnectionWindow.ShowWindow();

            ConnectionWindow.ChangeInterfaceState(ConnectionWindow.InterfaceState.ConnectionToSystemDB);

            sda = new SqlDataAdapter();
            StringBuilder connectionString = new StringBuilder(NO_LOCALE_CONNECTION_STRING);
            con = new SqlConnection(connectionString.ToString());
            try
            {
                Execute($"USE Master");
            }
            catch (Exception)
            {
                ConnectionWindow.SetFailed();
                App.Current.Shutdown();
                return isDatabaseConnected = false;
            }

            ConnectionWindow.ChangeInterfaceState(ConnectionWindow.InterfaceState.ConnectionToMainDB);

            connectionString.Replace("master", databaseName);
            con = new SqlConnection(connectionString.ToString());
            try
            {
                Execute($"USE {databaseName}");
            }
            catch (Exception)
            {
                ConnectionWindow.SetFailed();
                App.Current.Shutdown();
                return isDatabaseConnected = false;
            }

            ConnectionWindow.ChangeInterfaceState(ConnectionWindow.InterfaceState.Finish);
            ConnectionWindow.CloseWindow();
            return isDatabaseConnected = true;
        }
        public static bool Initialize()
        {
            LoadCfg();
            if (LOCALE_MODE)
            {
                SQL_SERVER_EXEMPLARS = FindSqlServerExemplars();
                SQL_SERVER_EXEMPLAR_NAME = GetSqlServerExemplarName();
                return InitializeLocale();
            }
            else
            {
                return InitializeServer();
            }
        }


        public static void Execute(string com)
        {
            con.Open();

            sda.SelectCommand = new SqlCommand(com, con);
            try
            {
                sda.SelectCommand.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
        }

        public static void Execute(string com, DataTable target)
        {
            con.Open();
            sda.SelectCommand = new SqlCommand(com, con);
            sda.Fill(target);
            con.Close();
        }

        public static long ExecuteInsert(string com)
        {
            con.Open();
            string tableName = com.Split(' ')[2].Split('(')[0];
            long id = 0;
            sda.SelectCommand = new SqlCommand(com + $"\r\nSELECT IDENT_CURRENT('{tableName}')", con);
            try
            {
                id = long.Parse(sda.SelectCommand.ExecuteScalar().ToString());
            }
            finally
            {
                con.Close();
            }
            return id;
        }

        public static long ExecuteInsertImage(string com, string imageParamName, byte[] image)
        {
            con.Open();
            string tableName = com.Split(' ')[2].Split('(')[0];
            long id = 0;
            sda.SelectCommand = new SqlCommand(com + $"\r\nSELECT IDENT_CURRENT('{tableName}')", con);
            sda.SelectCommand.Parameters.Add(imageParamName, SqlDbType.Image);
            sda.SelectCommand.Parameters[imageParamName].Value = image;
            try
            {
                id = long.Parse(sda.SelectCommand.ExecuteScalar().ToString());
            }
            finally
            {
                con.Close();
            }
            return id;
        }
        public static void ExecuteUpdateImage(string com, string imageParamName, byte[] image)
        {
            con.Open();
            sda.SelectCommand = new SqlCommand(com, con);
            sda.SelectCommand.Parameters.Add(imageParamName, SqlDbType.Image);
            sda.SelectCommand.Parameters[imageParamName].Value = image;
            try
            {
                sda.SelectCommand.ExecuteNonQuery();
            }
            finally
            {
                con.Close();
            }
            return;
        }
        public static byte[] ExecuteReturnImage(string com)
        {
            DataTable result = new();
            con.Open();
            sda.SelectCommand = new SqlCommand(com, con);
            sda.Fill(result);
            con.Close();

            if (result.Rows.Count != 0)
            {
                return result.Rows[0][0] as byte[];
            }
            return null;
        }


        public static void ExecuteInsert<T>(string com, T entity) where T : Entity
        {
            con.Open();
            string tableName = com.Split(' ')[2].Split('(')[0];
            sda.SelectCommand = new SqlCommand(com + $"\r\nSELECT IDENT_CURRENT('{tableName}')", con);
            try
            {
                entity.Id = long.Parse(sda.SelectCommand.ExecuteScalar().ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public static void ExecuteDelete<T>(T entity, string tableName) where T : Entity
        {
            con.Open();
            sda.SelectCommand = new SqlCommand($"DELETE FROM {tableName} WHERE id_p = {entity.Id}", con);
            sda.SelectCommand.ExecuteNonQuery();
            con.Close();
        }


        public static List<object[]> ExecuteReturnOld(string com)
        {
            DataTable result = new();
            con.Open();
            sda.SelectCommand = new SqlCommand(com, con);
            sda.Fill(result);
            con.Close();

            List<object[]> resultList = new();
            foreach (DataRow item in result.Rows)
            {
                resultList.Add(item.ItemArray);
            }
            foreach (object[] item in resultList)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i].Equals(DBNull.Value))
                    {
                        item[i] = null;
                    }
                }
            }

            return resultList;
        }
        public static List<string[]> ExecuteReturn(string com)
        {
            DataTable result = new();
            con.Open();
            sda.SelectCommand = new SqlCommand(com, con);
            sda.Fill(result);
            con.Close();

            List<string[]> resultList = new();
            if (result.Rows.Count != 0)
            {
                string[] temp;
                foreach (DataRow item in result.Rows)
                {
                    temp = new string[result.Rows[0].ItemArray.Length];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        if (item.ItemArray[i] != DBNull.Value)
                        {
                            temp[i] = item.ItemArray[i].ToString();
                        }
                        else
                        {
                            temp[i] = null;
                        }
                    }
                    resultList.Add(temp);
                }
            }
            return resultList;
        }

        public static string[] ExecuteReturnOneRow(string com)
        {
            DataTable result = new();
            con.Open();
            sda.SelectCommand = new SqlCommand(com, con);
            sda.Fill(result);
            con.Close();

            string[] resultArray = null;
            if (result.Rows.Count != 0)
            {
                resultArray = new string[result.Rows[0].ItemArray.Length];
                for (int i = 0; i < result.Rows[0].ItemArray.Length; i++)
                {
                    resultArray[i] = result.Rows[0].ItemArray[i].ToString();
                }
            }
            return resultArray;
        }

        public static object ExecuteReturnScalar(string com)
        {
            DataTable result = new();
            con.Open();
            sda.SelectCommand = new SqlCommand(com, con);
            sda.Fill(result);
            con.Close();

            if (result.Rows.Count != 0 && !result.Rows[0][0].Equals(DBNull.Value))
            {
                return result.Rows[0][0];
            }
            return null;
        }

        public static void DetachCurrentDatabase()
        {
            if (isDatabaseConnected)
            {
                Execute($"use master ALTER DATABASE {DATABASE_NAME} set single_user with rollback immediate {Environment.NewLine} ALTER DATABASE {DATABASE_NAME} SET OFFLINE {Environment.NewLine} EXEC sp_detach_db '{DATABASE_NAME}'");
            }
            if (File.Exists(CurrentDataBaseFilepath + "\\" + DATABASE_NAME + ".mdf"))
            {
                File.Move(CurrentDataBaseFilepath + "\\" + DATABASE_NAME + ".mdf", $"{Environment.CurrentDirectory}/{DATABASE_NAME}.mdf", true);
            }
            if (File.Exists(CurrentDataBaseFilepath + "\\" + DATABASE_NAME + "_log.ldf"))
            {
                File.Move(CurrentDataBaseFilepath + "\\" + DATABASE_NAME + "_log.ldf", $"{Environment.CurrentDirectory}/{DATABASE_NAME}_log.ldf", true);
            }
        }
        public static void AttachCurrentDatabase()
        {
            File.Copy($"{Environment.CurrentDirectory}/{DATABASE_NAME}.mdf", CurrentDataBaseFilepath + "\\" + DATABASE_NAME + ".mdf", true);
            File.Copy($"{Environment.CurrentDirectory}/{DATABASE_NAME}_log.ldf", CurrentDataBaseFilepath + "\\" + DATABASE_NAME + "_log.ldf", true);

            Execute($"CREATE DATABASE {DATABASE_NAME} ON PRIMARY(FILENAME='{CurrentDataBaseFilepath}\\{DATABASE_NAME}.mdf'),(FILENAME ='{CurrentDataBaseFilepath}\\{DATABASE_NAME}_log.ldf') FOR ATTACH {Environment.NewLine} ALTER DATABASE {DATABASE_NAME} SET ONLINE");
            Thread.Sleep(8000);
        }

        public static void BackupCurrentDatabase(string filepath, string backupName)
        {
            
            Execute
                (
                    $"USE {DATABASE_NAME} " + Environment.NewLine +
                    $"BACKUP DATABASE {DATABASE_NAME} " +
                    $"TO DISK = '{CurrentDataBaseFilepath}\\{backupName}.bak' " +
                    "WITH FORMAT, " +
                    "MEDIANAME = 'SQLServerBackups', " +
                    $"NAME = '{backupName}';"
                );

            File.Move(CurrentDataBaseFilepath + '\\' + backupName + ".bak", filepath + '\\' + backupName + ".bak", true);
        }

        public static void RestoreDatabase(string filepath, string backupName)
        {
            File.Copy(filepath + '\\' + backupName + ".bak", CurrentDataBaseFilepath + '\\' + backupName + ".bak", true);

            Execute
                (
                    $"USE Master " + Environment.NewLine +
                    $"RESTORE DATABASE {DATABASE_NAME} " +
                    $"FROM DISK = '{CurrentDataBaseFilepath}\\{backupName}.bak' " +
                    "WITH REPLACE"
                );
        }
    }
}
