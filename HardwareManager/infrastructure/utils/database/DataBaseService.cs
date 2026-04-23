using HardwareManager.model;
using HardwareManager.model.entity;
using HardwareManager.viewModel;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Monitor = HardwareManager.model.entity.Monitor;
using Computer = HardwareManager.model.entity.Computer;
using Camera = HardwareManager.model.entity.Camera;
using System.Threading;
using System.Windows.Shell;
using System.Diagnostics.Metrics;
using HardwareManager.model.view;

namespace HardwareManager.infrastructure.utils.database
{
    public class DataBaseService
    {
        private static class SqlRequests
        {
            public static void InsertParam(StringBuilder target, object param)
            {
                for (int i = 0; i < target.Length; i++)
                {
                    if (target[i] == '?')
                    {
                        target.Remove(i, 1);
                        switch (param)
                        {
                            case null:
                                target.Insert(i, "NULL");
                                break;
                            case string:
                                target.Insert(i, "'" + param.ToString().Trim('\'') + "'");
                                break;
                            case bool:
                                target.Insert(i, (bool)param == true ? 1 : 0);
                                break;
                            default:
                                target.Insert(i, param);
                                break;
                        }
                        return;
                    }
                }
            }
        }

        private static string ToStr(object obj)
        {
            try
            {
                if (obj != null)
                {
                    return obj.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return null;
        }
        private static long ToLong(object obj)
        {
            try
            {
                if (obj != null && !obj.Equals(""))
                {
                    return long.Parse(obj.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }
        private static double ToDouble(object obj)
        {
            try
            {
                if (obj != null && !obj.Equals(""))
                {
                    return double.Parse(obj.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка каста ToDouble - DataBaseService.cs " + ex.Message);
            }
            return 0;
        }
        private static int ToInt(object obj)
        {
            try
            {
                if (obj != null)
                {
                    return int.Parse(obj.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка каста ToInt - DataBaseService.cs " + ex.Message);
            }
            return 0;
        }
        private static bool ToBool(object obj)
        {
            if (obj != null)
            {
                if (obj is bool)
                {
                    return (bool)obj;
                }
                if (obj.Equals("True") || obj.Equals("False"))
                {
                    return bool.Parse(obj.ToString());
                }
                int r;
                if (int.TryParse(obj.ToString(), out r))
                {
                    if (r < 1)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private static int ToBit(object obj)
        {
            if (obj != null)
            {
                if (obj is bool)
                {
                    return (bool)obj ? 1 : 0;
                }
                return 1;
            }
            return 0;
        }
        private static DateTime? ToDate(object obj)
        {
            if (obj != null)
            {
                return DateTime.Parse(obj.ToString());
            }
            return null;
        }
        private static T ToEnum<T>(object obj) where T : struct
        {
            return Enum.Parse<T>(obj as string);
        }
        

        private const string SAVE_ACCOUNT = "INSERT INTO Account (id_Role_f, id_workplace_f, username, [password]) VALUES (?,?,?,?);";
        private const string SAVE_CABINET = "INSERT INTO Cabinet ([name]) VALUES (?);";
        private const string SAVE_WORKPLACE = "INSERT INTO Workplace ([name], id_Cabinet_f) VALUES (?,?);";
        private const string SAVE_REQUEST = "INSERT INTO Request (id_Workplace_f, reasonType, requestedType, hardwareIN, requestStatus) VALUES (?,?,?,?,?);";

        private const string UPDATE_CABINET_BY_ID = "UPDATE Cabinet SET [name] = ? WHERE id_p = ?";
        private const string UPDATE_WORKPLACE_BY_ID = "UPDATE Workplace SET [name] = ? WHERE id_p = ?";

        private const string SELECT_KABINETS = "SELECT * FROM Cabinet";
        private const string SELECT_KABINETS_NAMES = "SELECT [name] FROM Cabinet";
        private const string SELECT_WORKPLACES_BY_KABINET_ID = "SELECT id_p, [name] FROM WorkPlace WHERE id_Cabinet_f = ?";
        private const string SELECT_WORKPLACE_BY_ID = "SELECT * FROM dbo.SelectWorkplaceById_F(?)";

        private const string SELECT_ALL_REQUESTS = "SELECT id_p, reasonType, requestedType, hardwareIN, requestStatus FROM Request";
        private const string SELECT_ALL_REQUESTS_WITH_WORKPLACE_NAMES = "SELECT id_p, reasonType, requestedType, hardwareIN, requestStatus, (SELECT [Name] FROM Workplace WHERE id_p = id_Workplace_f) FROM Request";
        private const string SELECT_ALL_REQUESTS_BY_WORKPLACE_ID = "SELECT id_p, reasonType, requestedType, hardwareIN, requestStatus FROM Request WHERE id_Workplace_f = ?";

        private const string SELECT_ALL_COMPUTERS_BY_INVENTORIAL_STATUS = "SELECT * FROM dbo.SelectAllComputersByInventorialStatus_F(?)";
        private const string SELECT_ALL_MONITORS_BY_INVENTORIAL_STATUS = "SELECT id_p, dbo.GetInventorialStatusValueById(id_InventorialStatus_f), id_Workplace_f, dbo.GetHardwareImageDataById(id_HardwareImage_f), inventorialNumber, model, diagonal, aspectRatio, matrix, frequency, resolution FROM Monitor WHERE id_InventorialStatus_f = ?";
        private const string SELECT_ALL_KEYBOARDS_BY_INVENTORIAL_STATUS = "SELECT id_p, dbo.GetInventorialStatusValueById(id_InventorialStatus_f), id_Workplace_f, dbo.GetHardwareImageDataById(id_HardwareImage_f), inventorialNumber, model, isWired FROM Keyboard WHERE id_InventorialStatus_f = ?";
        private const string SELECT_ALL_MOUSES_BY_INVENTORIAL_STATUS = "SELECT id_p, dbo.GetInventorialStatusValueById(id_InventorialStatus_f), id_Workplace_f, dbo.GetHardwareImageDataById(id_HardwareImage_f), inventorialNumber, model, isWired FROM Mouse WHERE id_InventorialStatus_f = ?";
        private const string SELECT_ALL_CAMERAS_BY_INVENTORIAL_STATUS = "SELECT id_p, dbo.GetInventorialStatusValueById(id_InventorialStatus_f), id_Workplace_f, dbo.GetHardwareImageDataById(id_HardwareImage_f), inventorialNumber, model, maxResolution, isRotatable, hasMicro FROM Camera WHERE id_InventorialStatus_f = ?";
        private const string SELECT_ALL_HEADPHONES_BY_INVENTORIAL_STATUS = "SELECT id_p, dbo.GetInventorialStatusValueById(id_InventorialStatus_f), id_Workplace_f, dbo.GetHardwareImageDataById(id_HardwareImage_f), inventorialNumber, model, isWired, hasMicro FROM Headphones WHERE id_InventorialStatus_f = ?";
        private const string SELECT_ALL_PRINTERS_BY_INVENTORIAL_STATUS = "SELECT id_p, dbo.GetInventorialStatusValueById(id_InventorialStatus_f), id_Workplace_f, dbo.GetHardwareImageDataById(id_HardwareImage_f), inventorialNumber, model, maxFormat, printTechnology, color, doubleSidedPrinting, hasScanner FROM Printer WHERE id_InventorialStatus_f = ?";
        private const string SELECT_ALL_WIRED_TELEPHONES_BY_INVENTORIAL_STATUS = "SELECT id_p, dbo.GetInventorialStatusValueById(id_InventorialStatus_f), id_Workplace_f, dbo.GetHardwareImageDataById(id_HardwareImage_f), inventorialNumber, model, connectionType, hasScreen FROM WiredTelephone WHERE id_InventorialStatus_f = ?";
        private const string SELECT_ALL_BACKUP_BATTERIES_BY_INVENTORIAL_STATUS = "SELECT id_p, dbo.GetInventorialStatusValueById(id_InventorialStatus_f), id_Workplace_f, dbo.GetHardwareImageDataById(id_HardwareImage_f), inventorialNumber, model, socketsCount, batteryLife, batteryType FROM BackupBattery WHERE id_InventorialStatus_f = ?";
        private const string SELECT_ALL_SURGE_PROTECTORS_BY_INVENTORIAL_STATUS = "SELECT id_p, dbo.GetInventorialStatusValueById(id_InventorialStatus_f), id_Workplace_f, dbo.GetHardwareImageDataById(id_HardwareImage_f), inventorialNumber, model, socketsCount, hasSwitcher, hasWire FROM SurgeProtector WHERE id_InventorialStatus_f = ?";

        private const string SELECT_ALL_CABLES_BY_INVENTORIAL_STATUS = "SELECT id_p, dbo.GetInventorialStatusValueById(id_InventorialStatus_f), id_Workplace_f, dbo.GetHardwareImageDataById(id_HardwareImage_f), model, itemCount, connectorType, isInputFirst, isInputSecond FROM Cable WHERE id_InventorialStatus_f = ?";
        private const string SELECT_ALL_ADAPTER_CABLES_BY_INVENTORIAL_STATUS = "SELECT id_p, dbo.GetInventorialStatusValueById(id_InventorialStatus_f), id_Workplace_f, dbo.GetHardwareImageDataById(id_HardwareImage_f), model, itemCount, firstConnectorType, secondConnectorType, isInputFirst, isInputSecond, hasWire FROM AdapterCable WHERE id_InventorialStatus_f = ?";

        private const string GET_STATISTIC = "SELECT * FROM dbo.GetStatistic_F()";


        private const string ATTACH_HARDWARE_TO_WORKPLACE = "UPDATE ? SET dbo.CheckInventorialNumber(?)";


        private const string SELECT_ALL_CPU_BY_COMPUTER_INVENTORIAL_STATUS = "SELECT id_p, id_Computer_f, [name], coreCount, cashMemoryType, cashMemoryValue FROM CPU c WHERE (SELECT id_InventorialStatus_f FROM Computer WHERE id_p = c.id_Computer_f) = ?";

        private const string GENERATE_INVENTORIAL_NUMBER_QUERY = "SELECT dbo.GenerateInventorialNumber()";
        private const string CHECK_INVENTORIAL_NUMBER_QUERY = "SELECT dbo.CheckInventorialNumber(?)";




        public static bool IfHistoryExists()
        {
            return ToBool(DBUtil.ExecuteReturnOneRow("SELECT COUNT(*) FROM History")[0]);
        }
        public static List<HistoryRecord> GetHistory()
        {
            List<string[]> tempList = DBUtil.ExecuteReturn("SELECT * FROM History");
            List<HistoryRecord> result = [];
            HistoryRecord temp;
            foreach (string[] record in tempList) {
                temp = new()
                {
                    Id = ToLong(record[0]),
                    OperationDate = ToDate(record[1]),
                    OperationType = record[2],
                    HardwareType = record[3],
                    InventorialNumber = ToInt(record[4])
                };
                result.Add(temp);
            }
            return result;
        }
        public static void SaveHistory(string opretation, string hardwareType = null, int? hardwareIN = null, string workplaceName = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO History (operationDate, operationType, hardwareType, inventorialNumber, WorkplaceName) VALUES (CAST(GETDATE() AS DATETIME), ?,?,?,?)");

            SqlRequests.InsertParam(sqlRequest, opretation);
            SqlRequests.InsertParam(sqlRequest, hardwareType);
            SqlRequests.InsertParam(sqlRequest, hardwareIN);
            SqlRequests.InsertParam(sqlRequest, workplaceName);

            DBUtil.Execute(sqlRequest.ToString());
        }
        public static void ClearHistory()
        {
            DBUtil.Execute("TRUNCATE TABLE History");
        }

        public static List<Cabinet> GetAllKabinets()
        {
            List<string[]> tempList = DBUtil.ExecuteReturn(SELECT_KABINETS);
            List<Cabinet> result = [];
            Cabinet temp;
            foreach (string[] record in tempList)
            {
                temp = new()
                {
                    Id = ToLong(record[0]),
                    Name = record[1]
                };
                temp.WorkPlaceList = GetWorkplacesByKabinetId(temp.Id);
                result.Add(temp);
            }
            return result;
        }
        public static List<string> GetAllKabinetsNames()
        {
            List<string[]> raw = DBUtil.ExecuteReturn(SELECT_KABINETS_NAMES);
            List<string> result = [];
            foreach (string[] record in raw)
            {
                result.Add(record[0]);
            }
            return result;
        }
        public static List<string> GetWorkplacesNamesByCabinetName(string cabinetName)
        {
            List<string[]> raw = DBUtil.ExecuteReturn($"SELECT [name] FROM WorkPlace WHERE id_Cabinet_f = (SELECT id_p FROM Cabinet WHERE [name] = '{cabinetName}')");
            List<string> result = [];
            foreach (string[] record in raw)
            {
                result.Add(record[0]);
            }
            return result;
        }

        public static List<WorkPlace> GetWorkplacesByKabinetId(long id)
        {
            StringBuilder sqlRequest = new(SELECT_WORKPLACES_BY_KABINET_ID);

            SqlRequests.InsertParam(sqlRequest, id);

            List<string[]> tempList = DBUtil.ExecuteReturn(sqlRequest.ToString());
            List<WorkPlace> result = [];
            WorkPlace temp;
            foreach (string[] record in tempList)
            {
                temp = new()
                {
                    Id = ToLong(record[0]),
                    Name = record[1]
                };
                result.Add(temp);
            }
            return result;
        }
        public static WorkPlace GetWorkplaceById(long id)
        {
            List<string[]> tempList = DBUtil.ExecuteReturn(SELECT_WORKPLACE_BY_ID.Replace("?",id.ToString()));
            if (tempList.Count == 0) return null;
            return ParceWorkplaceRawData(tempList);
        }
        public static WorkPlace GetWorkplaceByAccount(Account account)
        {
            long workplaceId = ToLong(DBUtil.ExecuteReturnScalar($"SELECT id_Workplace_f FROM Account WHERE id_p = {account.Id}"));
            List<string[]> tempList = DBUtil.ExecuteReturn(SELECT_WORKPLACE_BY_ID.Replace("?", workplaceId.ToString()));
            if (tempList.Count == 0) return null;
            return ParceWorkplaceRawData(tempList);
        }
        public static long GetWorkplaceIdByCabinetNameAndWorkplaceName(string cabinetName, string workplaceName)
        {
           return ToLong(DBUtil.ExecuteReturnScalar($"SELECT id_p FROM Workplace WHERE Workplace.[name] LIKE '{workplaceName}' AND (SELECT id_p FROM Cabinet WHERE Cabinet.[name] LIKE '{cabinetName}') = id_Cabinet_f"));
        }
        public static bool CheckCabinetName(string cabinetName)
        {
            return DBUtil.ExecuteReturnScalar($"SELECT id_p FROM Cabinet WHERE Cabinet.[name] LIKE '{cabinetName}'") != null;
        }
        public static bool CheckWorkplaceName(string workplaceName)
        {
            return DBUtil.ExecuteReturnScalar($"SELECT id_p FROM Workplace WHERE Workplace.[name] LIKE '{workplaceName}'") != null;
        }
        public static bool CheckAccountUsername(string username)
        {
            return (int)DBUtil.ExecuteReturnScalar($"SELECT COUNT(id_p) FROM Account WHERE username LIKE '{username}'") == 0;
        }
        public static bool CheckAccountWorkplaceData(string username, string workplaceName)
        {
            return (int)DBUtil.ExecuteReturnScalar($"SELECT COUNT(id_p) FROM Account WHERE id_Workplace_f = (SELECT id_p FROM Workplace WHERE [name] LIKE '{workplaceName}')") == 0;
        }

        public static Account GetAccount(string username, string password)
        {
            string[] temp = DBUtil.ExecuteReturnOneRow($"SELECT Account.id_p, username, password, [Role].id_p, roleName FROM Account INNER JOIN [Role] ON Account.id_Role_f = [Role].id_p WHERE username = '{username}' AND password = '{password}'");
            if (temp != null)
            {
                Account result = new();
                result.Id = ToLong(temp[0]);
                result.Username = temp[1];
                result.Password = temp[2];
                result.Role = new();
                result.Role.Id = ToLong(temp[3]);
                result.Role.RoleName = temp[4];
                return result;
            }
            return null;
        }
        public static void SaveAccount(Account account)
        {
            StringBuilder sqlRequest = new(SAVE_ACCOUNT);

            if (account == null)
            {
                return;
            }

            account.Role = GetRoleByName("User");

            SqlRequests.InsertParam(sqlRequest, account.Role.Id);
            SqlRequests.InsertParam(sqlRequest, null);
            SqlRequests.InsertParam(sqlRequest, account.Username);
            SqlRequests.InsertParam(sqlRequest, account.Password);

            account.Id = DBUtil.ExecuteInsert(sqlRequest.ToString());
        }
        public static void SaveAccount(Account account, string cabinetName, string workplaceName)
        {
            StringBuilder sqlRequest = new(SAVE_ACCOUNT);

            if (account == null)
            {
                return;
            }

            account.Role = GetRoleByName("User");
            SqlRequests.InsertParam(sqlRequest, account.Role.Id);
            SqlRequests.InsertParam(sqlRequest, GetWorkplaceIdByCabinetNameAndWorkplaceName(cabinetName, workplaceName));
            SqlRequests.InsertParam(sqlRequest, account.Username);
            SqlRequests.InsertParam(sqlRequest, account.Password);

            account.Id = DBUtil.ExecuteInsert(sqlRequest.ToString());
        }
        public static void SaveAccount(Account account, Role role)
        {
            StringBuilder sqlRequest = new(SAVE_ACCOUNT);

            if (account == null || role == null)
            {
                return;
            }

            role.Id = GetRoleByName(role.RoleName).Id;
            account.Role = role;

            SqlRequests.InsertParam(sqlRequest, account.Role.Id);
            SqlRequests.InsertParam(sqlRequest, account.Username);
            SqlRequests.InsertParam(sqlRequest, account.Password);

            account.Id = DBUtil.ExecuteInsert(sqlRequest.ToString());
        }
        public static void DeleteAccountByWorkplaceId(long workplaceId)
        {
            DBUtil.Execute($"DELETE FROM Account WHERE id_workplace_f = {workplaceId}");
        }

        public static Role GetRoleById(long id)
        {
            string[] temp = DBUtil.ExecuteReturnOneRow($"SELECT * FROM [Role] WHERE id_p = {id}");
            if (temp != null)
            {
                return new Role
                {
                    Id = ToLong(temp[0]),
                    RoleName = temp[1],
                };
            }
            return null;
        }
        public static Role GetRoleByName(string name)
        {
            string[] temp = DBUtil.ExecuteReturnOneRow($"SELECT * FROM [Role] WHERE roleName = '{name}'");
            if (temp != null)
            {
                return new Role
                {
                    Id = ToLong(temp[0]),
                    RoleName = temp[1],
                };
            }
            return null;
        }
        public static Role GetRole(long id, string name)
        {
            string[] temp = DBUtil.ExecuteReturnOneRow($"SELECT * FROM [Role] WHERE id_p = {id} OR roleName = '{name}'");
            if (temp != null)
            {
                return new Role
                {
                    Id = ToLong(temp[0]),
                    RoleName = temp[1],
                };
            }
            return null;
        }



        public static int GenerateInventorialNumber()
        {
            return (int)DBUtil.ExecuteReturnScalar(GENERATE_INVENTORIAL_NUMBER_QUERY);
        }
        public static bool CheckInventorialNumber(int inventorialNumber)
        {
            return (bool)DBUtil.ExecuteReturnScalar(CHECK_INVENTORIAL_NUMBER_QUERY.Replace("?", inventorialNumber.ToString()));
        }

        private static byte[] GetHardwareImage(long hardwareImageId)
        {
            return DBUtil.ExecuteReturnImage($"SELECT imageData FROM HardwareImage HIDT WHERE HIDT.id_p = {hardwareImageId}");
        }
        private static long SaveHardwareImage(byte[] image)
        {
            if (image != null)
            {
                return DBUtil.ExecuteInsertImage("INSERT INTO HardwareImage (imageData) VALUES (@Image)", "@Image", image);
            }
            return 0;
        }
        private static long UpdateHardwareImage(byte[] image, Entity hardware)
        {
            string hardwareType = hardware.GetType().Name;
            long hardwareImageId = ToLong(DBUtil.ExecuteReturnScalar($"SELECT id_HardwareImage_f FROM {hardwareType} WHERE id_p = '{hardware.Id}'"));
            if (image == null)
            {
                DBUtil.Execute($"UPDATE {hardwareType} SET id_HardwareImage_f = NULL WHERE id_p = {hardware.Id}");
                DBUtil.Execute($"DELETE FROM HardwareImage WHERE id_p = {hardwareImageId}");
                return 0;
            }
            if (image != null && hardwareImageId != 0)
            {
                DBUtil.ExecuteUpdateImage($"UPDATE HardwareImage SET imageData = @Image WHERE id_p = {hardwareImageId}", "@Image", image);
                return hardwareImageId;
            }
            if (image != null && hardwareImageId == 0)
            {
                hardwareImageId = SaveHardwareImage(image);
                return hardwareImageId;
            }
            return 0;
        }


        public static List<Request> GetAllRequests()
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_REQUESTS);

            List<Request> result = [];
            Request tempRequest;
            foreach (object[] row in tempList)
            {
                tempRequest = new();
                tempRequest.Id = ToLong(row[0]);
                tempRequest.ReasonType = ToStr(row[1]);
                tempRequest.RequestedType = ToStr(row[2]);
                tempRequest.HardwareIN = ToInt(row[3]);
                tempRequest.Status = ToStr(row[4]);
                result.Add(tempRequest);
            }
            return result;
        }
        public static List<RequestView> GetAllRequestsWithWorkPlaceNames()
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_REQUESTS_WITH_WORKPLACE_NAMES);

            List<RequestView> result = [];
            Request tempRequest;
            RequestView tempRequestView;
            foreach (object[] row in tempList)
            {
                tempRequest = new();
                tempRequest.Id = ToLong(row[0]);
                tempRequest.ReasonType = ToStr(row[1]);
                tempRequest.RequestedType = ToStr(row[2]);
                tempRequest.HardwareIN = ToInt(row[3]);
                tempRequest.Status = ToStr(row[4]);
                tempRequestView = new(tempRequest);
                tempRequestView.OwnerWorkPlaceName = ToStr(row[5]);
                result.Add(tempRequestView);
            }
            return result;
        }
        public static List<Request> GetAllRequestsByWorkplace(WorkPlace wp)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_REQUESTS_BY_WORKPLACE_ID.Replace("?", (wp.Id).ToString()));

            List<Request> result = [];
            Request tempRequest;
            foreach (object[] row in tempList)
            {
                tempRequest = new();
                tempRequest.Id = ToLong(row[0]);
                tempRequest.ReasonType = ToStr(row[1]);
                tempRequest.RequestedType = ToStr(row[2]);
                tempRequest.HardwareIN = ToInt(row[3]);
                tempRequest.Status = ToStr(row[4]);
                result.Add(tempRequest);
            }
            return result;
        }
        public static void SaveRequest(Request request, WorkPlace wp)
        {
            StringBuilder sqlRequest = new(SAVE_REQUEST);

            SqlRequests.InsertParam(sqlRequest, wp.Id);
            SqlRequests.InsertParam(sqlRequest, request.ReasonType);
            SqlRequests.InsertParam(sqlRequest, request.RequestedType);
            SqlRequests.InsertParam(sqlRequest, request.HardwareIN);
            SqlRequests.InsertParam(sqlRequest, request.Status);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), request);
        }
        public static void RevokeRequest(Request request)
        {
            DBUtil.ExecuteDelete(request, request.GetType().Name);
        }
        public static void DeleteRequest(Request request)
        {
            DBUtil.ExecuteDelete(request, request.GetType().Name);
        }
        public static void DenyRequest(Request request)
        {
            DBUtil.Execute("UPDATE Request SET requestStatus = 'Отменено' WHERE id_p = " + request.Id);
            request.Status = "Отменено";
        }
        public static void ApplyRequest(Request request)
        {
            DBUtil.Execute("UPDATE Request SET requestStatus = 'Одобрено' WHERE id_p = " + request.Id);
            request.Status = "Одобрено";
        }




        public static void SaveCabinet(Cabinet cabinet)
        {
            StringBuilder sqlRequest = new(SAVE_CABINET);

            SqlRequests.InsertParam(sqlRequest, cabinet.Name);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), cabinet);
        }
        public static void UpdateCabinet(Cabinet cabinet)
        {
            StringBuilder sqlRequest = new(UPDATE_CABINET_BY_ID);

            SqlRequests.InsertParam(sqlRequest, cabinet.Name);

            SqlRequests.InsertParam(sqlRequest, cabinet.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }
        public static void DeleteCabinet(Cabinet cabinet)
        {
            DBUtil.Execute($"EXEC dbo.DeleteCabinetById_P {cabinet.Id}");
        }


        public static void SaveWorkPlace(WorkPlace workPlace, Cabinet cabinet)
        {
            StringBuilder sqlRequest = new(SAVE_WORKPLACE);

            SqlRequests.InsertParam(sqlRequest, workPlace.Name);
            SqlRequests.InsertParam(sqlRequest, cabinet.Id);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), workPlace);
        }
        public static void UpdateWorkPlace(WorkPlace workPlace)
        {
            StringBuilder sqlRequest = new(UPDATE_WORKPLACE_BY_ID);

            SqlRequests.InsertParam(sqlRequest, workPlace.Name);

            SqlRequests.InsertParam(sqlRequest, workPlace.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }
        public static void DeleteWorkPlace(WorkPlace workPlace)
        {
            DBUtil.Execute($"EXEC dbo.DeleteWorkplaceById_F {workPlace.Id}");
        }
        public static long GetWorkPlaceAccountId(WorkPlace workPlace)
        {
            return ToLong(DBUtil.ExecuteReturnScalar($"SELECT id_p FROM Account WHERE id_Workplace_f = {workPlace.Id}"));
        }


        public static List<Computer> GetAllComputersByInventorialStatus(EInventorialStatus inventorialStatus)
        {
            StringBuilder sqlRequest = new(SELECT_ALL_COMPUTERS_BY_INVENTORIAL_STATUS);
            SqlRequests.InsertParam(sqlRequest, (int)inventorialStatus);
            List<string[]> tempList = DBUtil.ExecuteReturn(sqlRequest.ToString());

            return ParceComputerRawData(tempList);
        }
        private static List<Computer> ParceComputerRawData(List<string[]> raw)
        {
            int counter = 0;
            List<Computer> result = [];
            Computer tempComputer;
            CPU tempCPU;
            GraphicalAdapter tempGraphicalAdapter;
            PhisicalDisk tempPhisicalDisk;
            RamModule tempRamModule;
            while (counter < raw.Count && raw[counter][0].Equals("Computer"))
            {
                tempComputer = new();
                tempComputer.Id = ToLong(raw[counter][1]);
                tempComputer.InventorialStatus = raw[counter][2];
                //tempList[counter][3] - это workplace_id
                tempComputer.HardwareImage = GetHardwareImage(ToLong(raw[counter][4]));
                tempComputer.InventorialNumber = ToInt(raw[counter][5]);
                tempComputer.Model = raw[counter][6];
                counter++;

                if (counter < raw.Count && raw[counter][0].Equals("CPU"))
                {
                    tempCPU = new();
                    tempCPU.Id = ToLong(raw[counter][1]);
                    tempCPU.Name = raw[counter][2];
                    tempCPU.CoreCount = ToInt(raw[counter][3]);
                    tempCPU.CashMemoryType = raw[counter][4];
                    tempCPU.CashMemoryValue = ToDouble(raw[counter][5]);
                    tempComputer.Cpu = tempCPU;
                    counter++;
                }

                while (counter < raw.Count && raw[counter][0].Equals("GraphicalAdapter"))
                {
                    tempGraphicalAdapter = new();
                    tempGraphicalAdapter.Id = ToLong(raw[counter][1]);
                    tempGraphicalAdapter.Model = raw[counter][2];
                    tempGraphicalAdapter.GraphicalProcessor = raw[counter][3];
                    tempGraphicalAdapter.RamMemory = ToDouble(raw[counter][4]);
                    tempComputer.GraphicalAdapters.Add(tempGraphicalAdapter);
                    counter++;
                }

                while (counter < raw.Count && raw[counter][0].Equals("PhisicalDisk"))
                {
                    tempPhisicalDisk = new();
                    tempPhisicalDisk.Id = ToLong(raw[counter][1]);
                    tempPhisicalDisk.Model = raw[counter][2];
                    tempPhisicalDisk.DiskType = raw[counter][3];
                    tempPhisicalDisk.Interface = raw[counter][4];
                    tempPhisicalDisk.FormFactor = raw[counter][5];
                    tempPhisicalDisk.Memory = ToInt(raw[counter][6]);
                    tempComputer.PhisicalDisks.Add(tempPhisicalDisk);
                    counter++;
                }

                while (counter < raw.Count && raw[counter][0].Equals("RamModule"))
                {
                    tempRamModule = new();
                    tempRamModule.Id = ToLong(raw[counter][1]);
                    tempRamModule.Model = raw[counter][2];
                    tempRamModule.RamType = raw[counter][3];
                    tempRamModule.Memory = ToInt(raw[counter][4]);
                    tempComputer.RamModules.Add(tempRamModule);
                    counter++;
                }
                result.Add(tempComputer);
            }
            return result;
        }


        public static List<Monitor> GetAllMonitorsByInventorialStatus(EInventorialStatus inventorialStatus)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_MONITORS_BY_INVENTORIAL_STATUS.Replace("?", ((int)inventorialStatus).ToString()));
            
            List<Monitor> result = [];
            Monitor tempMonitor;
            foreach (object[] row in tempList)
            {
                tempMonitor = new();
                tempMonitor.Id = ToLong(row[0]);
                tempMonitor.InventorialStatus = ToStr(row[1]);
                //row[2] - это workplace_id
                tempMonitor.HardwareImage = (byte[])row[3];
                tempMonitor.InventorialNumber = ToInt(row[4]);
                tempMonitor.Model = ToStr(row[5]);
                tempMonitor.Diagonal = ToDouble(row[6]);
                tempMonitor.AspectRatio = ToStr(row[7]);
                tempMonitor.Matrix = ToStr(row[8]);
                tempMonitor.Frequency = ToInt(row[9]);
                tempMonitor.Resolution = ToStr(row[10]);
                result.Add(tempMonitor);
            }
            return result;
        }
        public static List<Keyboard> GetAllKeyboardsByInventorialStatus(EInventorialStatus inventorialStatus)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_KEYBOARDS_BY_INVENTORIAL_STATUS.Replace("?", ((int)inventorialStatus).ToString()));

            List<Keyboard> result = [];
            Keyboard tempKeyboard;
            foreach (object[] row in tempList)
            {
                tempKeyboard = new();
                tempKeyboard.Id = ToLong(row[0]);
                tempKeyboard.InventorialStatus = ToStr(row[1]);
                //row[2] - это workplace_id
                tempKeyboard.HardwareImage = (byte[])row[3];
                tempKeyboard.InventorialNumber = ToInt(row[4]);
                tempKeyboard.Model = ToStr(row[5]);
                tempKeyboard.IsWired = (bool)row[6];
                result.Add(tempKeyboard);
            }
            return result;
        }
        public static List<Mouse> GetAllMousesByInventorialStatus(EInventorialStatus inventorialStatus)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_MOUSES_BY_INVENTORIAL_STATUS.Replace("?", ((int)inventorialStatus).ToString()));

            List<Mouse> result = [];
            Mouse tempMouse;
            foreach (object[] row in tempList)
            {
                tempMouse = new();
                tempMouse.Id = ToLong(row[0]);
                tempMouse.InventorialStatus = ToStr(row[1]);
                //row[2] - это workplace_id
                tempMouse.HardwareImage = (byte[])row[3];
                tempMouse.InventorialNumber = ToInt(row[4]);
                tempMouse.Model = ToStr(row[5]);
                tempMouse.IsWired = (bool)row[6];
                result.Add(tempMouse);
            }
            return result;
        }        
        public static List<Camera> GetAllCamerasByInventorialStatus(EInventorialStatus inventorialStatus)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_CAMERAS_BY_INVENTORIAL_STATUS.Replace("?", ((int)inventorialStatus).ToString()));

            List<Camera> result = [];
            Camera tempCamera;
            foreach (object[] row in tempList)
            {
                tempCamera = new();
                tempCamera.Id = ToLong(row[0]);
                tempCamera.InventorialStatus = ToStr(row[1]);
                //row[2] - это workplace_id
                tempCamera.HardwareImage = (byte[])row[3];
                tempCamera.InventorialNumber = ToInt(row[4]);
                tempCamera.Model = ToStr(row[5]);
                tempCamera.MaxResolution = ToStr(row[6]);
                tempCamera.IsRotatable = (bool)row[7];
                tempCamera.HasMicro = (bool)row[8];
                result.Add(tempCamera);
            }
            return result;
        }
        public static List<Headphones> GetAllHeadphonesByInventorialStatus(EInventorialStatus inventorialStatus)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_HEADPHONES_BY_INVENTORIAL_STATUS.Replace("?", ((int)inventorialStatus).ToString()));

            List<Headphones> result = [];
            Headphones tempHeadphones;
            foreach (object[] row in tempList)
            {
                tempHeadphones = new();
                tempHeadphones.Id = ToLong(row[0]);
                tempHeadphones.InventorialStatus = ToStr(row[1]);
                //row[2] - это workplace_id
                tempHeadphones.HardwareImage = (byte[])row[3];
                tempHeadphones.InventorialNumber = ToInt(row[4]);
                tempHeadphones.Model = ToStr(row[5]);
                tempHeadphones.IsWired = (bool)row[6];
                tempHeadphones.HasMicro = (bool)row[7];
                result.Add(tempHeadphones);
            }
            return result;
        }
        public static List<Printer> GetAllPrintersByInventorialStatus(EInventorialStatus inventorialStatus)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_PRINTERS_BY_INVENTORIAL_STATUS.Replace("?", ((int)inventorialStatus).ToString()));

            List<Printer> result = [];
            Printer tempPrinter;
            foreach (object[] row in tempList)
            {
                tempPrinter = new();
                tempPrinter.Id = ToLong(row[0]);
                tempPrinter.InventorialStatus = ToStr(row[1]);
                //row[2] - это workplace_id
                tempPrinter.HardwareImage = (byte[])row[3];
                tempPrinter.InventorialNumber = ToInt(row[4]);
                tempPrinter.Model = ToStr(row[5]);
                tempPrinter.MaxFormat = ToStr(row[6]);
                tempPrinter.PrintTechnology = ToStr(row[7]);
                tempPrinter.Color = ToStr(row[8]);
                tempPrinter.DoubleSidedPrinting = (bool)row[9];
                tempPrinter.HasScanner = (bool)row[10];

                result.Add(tempPrinter);
            }
            return result;
        }
        public static List<WiredTelephone> GetAllWiredTelephonesByInventorialStatus(EInventorialStatus inventorialStatus)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_WIRED_TELEPHONES_BY_INVENTORIAL_STATUS.Replace("?", ((int)inventorialStatus).ToString()));

            List<WiredTelephone> result = [];
            WiredTelephone tempWiredTelephone;
            foreach (object[] row in tempList)
            {
                tempWiredTelephone = new();
                tempWiredTelephone.Id = ToLong(row[0]);
                tempWiredTelephone.InventorialStatus = ToStr(row[1]);
                //row[2] - это workplace_id
                tempWiredTelephone.HardwareImage = (byte[])row[3];
                tempWiredTelephone.InventorialNumber = ToInt(row[4]);
                tempWiredTelephone.Model = ToStr(row[5]);
                tempWiredTelephone.ConnectionType = ToStr(row[6]);
                tempWiredTelephone.HasScreen = (bool)row[7];

                result.Add(tempWiredTelephone);
            }
            return result;
        }
        public static List<BackupBattery> GetAllBackupBatteriesByInventorialStatus(EInventorialStatus inventorialStatus)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_BACKUP_BATTERIES_BY_INVENTORIAL_STATUS.Replace("?", ((int)inventorialStatus).ToString()));

            List<BackupBattery> result = [];
            BackupBattery tempBackupBattery;
            foreach (object[] row in tempList)
            {
                tempBackupBattery = new();
                tempBackupBattery.Id = ToLong(row[0]);
                tempBackupBattery.InventorialStatus = ToStr(row[1]);
                //row[2] - это workplace_id
                tempBackupBattery.HardwareImage = (byte[])row[3];
                tempBackupBattery.InventorialNumber = ToInt(row[4]);
                tempBackupBattery.Model = ToStr(row[5]);
                tempBackupBattery.SocketsCount = ToInt(row[6]);
                tempBackupBattery.BatteryLife = ToInt(row[7]);
                tempBackupBattery.BatteryType = ToStr(row[8]);

                result.Add(tempBackupBattery);
            }
            return result;
        }
        public static List<SurgeProtector> GetAllSurgeProtectorsByInventorialStatus(EInventorialStatus inventorialStatus)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_SURGE_PROTECTORS_BY_INVENTORIAL_STATUS.Replace("?", ((int)inventorialStatus).ToString()));

            List<SurgeProtector> result = [];
            SurgeProtector tempSurgeProtector;
            foreach (object[] row in tempList)
            {
                tempSurgeProtector = new();
                tempSurgeProtector.Id = ToLong(row[0]);
                tempSurgeProtector.InventorialStatus = ToStr(row[1]);
                //row[2] - это workplace_id
                tempSurgeProtector.HardwareImage = (byte[])row[3];
                tempSurgeProtector.InventorialNumber = ToInt(row[4]);
                tempSurgeProtector.Model = ToStr(row[5]);
                tempSurgeProtector.SocketsCount = ToInt(row[6]);
                tempSurgeProtector.HasSwitcher = (bool)row[7];
                tempSurgeProtector.HasWire = (bool)row[8];

                result.Add(tempSurgeProtector);
            }
            return result;
        }

        public static List<Hardware> GetAllMainHardwareInStock()
        {
            List<Hardware> result = [];
            result.AddRange(GetAllComputersByInventorialStatus(EInventorialStatus.InStock));
            result.AddRange(GetAllMonitorsByInventorialStatus(EInventorialStatus.InStock));
            result.AddRange(GetAllKeyboardsByInventorialStatus(EInventorialStatus.InStock));
            result.AddRange(GetAllMousesByInventorialStatus(EInventorialStatus.InStock));
            result.AddRange(GetAllCamerasByInventorialStatus(EInventorialStatus.InStock));
            result.AddRange(GetAllHeadphonesByInventorialStatus(EInventorialStatus.InStock));
            result.AddRange(GetAllPrintersByInventorialStatus(EInventorialStatus.InStock));
            result.AddRange(GetAllWiredTelephonesByInventorialStatus(EInventorialStatus.InStock));
            result.AddRange(GetAllBackupBatteriesByInventorialStatus(EInventorialStatus.InStock));
            result.AddRange(GetAllSurgeProtectorsByInventorialStatus(EInventorialStatus.InStock));

            result.AddRange(GetAllComputersByInventorialStatus(EInventorialStatus.Defective));
            result.AddRange(GetAllMonitorsByInventorialStatus(EInventorialStatus.Defective));
            result.AddRange(GetAllKeyboardsByInventorialStatus(EInventorialStatus.Defective));
            result.AddRange(GetAllMousesByInventorialStatus(EInventorialStatus.Defective));
            result.AddRange(GetAllCamerasByInventorialStatus(EInventorialStatus.Defective));
            result.AddRange(GetAllHeadphonesByInventorialStatus(EInventorialStatus.Defective));
            result.AddRange(GetAllPrintersByInventorialStatus(EInventorialStatus.Defective));
            result.AddRange(GetAllWiredTelephonesByInventorialStatus(EInventorialStatus.Defective));
            result.AddRange(GetAllBackupBatteriesByInventorialStatus(EInventorialStatus.Defective));
            result.AddRange(GetAllSurgeProtectorsByInventorialStatus(EInventorialStatus.Defective));
            return result;
        }
        public static List<Hardware> GetAllMainHardware()
        {
            List<Hardware> result = GetAllMainHardwareInStock();
            result.AddRange(GetAllComputersByInventorialStatus(EInventorialStatus.InWork));
            result.AddRange(GetAllMonitorsByInventorialStatus(EInventorialStatus.InWork));
            result.AddRange(GetAllKeyboardsByInventorialStatus(EInventorialStatus.InWork));
            result.AddRange(GetAllMousesByInventorialStatus(EInventorialStatus.InWork));
            result.AddRange(GetAllCamerasByInventorialStatus(EInventorialStatus.InWork));
            result.AddRange(GetAllHeadphonesByInventorialStatus(EInventorialStatus.InWork));
            result.AddRange(GetAllPrintersByInventorialStatus(EInventorialStatus.InWork));
            result.AddRange(GetAllWiredTelephonesByInventorialStatus(EInventorialStatus.InWork));
            result.AddRange(GetAllBackupBatteriesByInventorialStatus(EInventorialStatus.InWork));
            result.AddRange(GetAllSurgeProtectorsByInventorialStatus(EInventorialStatus.InWork));
            return result;
        }

        public static List<Cable> GetAllCablesByInventorialStatus(EInventorialStatus inventorialStatus)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_CABLES_BY_INVENTORIAL_STATUS.Replace("?", ((int)inventorialStatus).ToString()));

            List<Cable> result = [];
            Cable tempCable;
            foreach (object[] row in tempList)
            {
                tempCable = new();
                tempCable.Id = ToLong(row[0]);
                tempCable.InventorialStatus = ToStr(row[1]);
                //row[2] - это workplace_id
                tempCable.HardwareImage = (byte[])row[3];
                tempCable.Model = ToStr(row[4]);
                tempCable.ItemCount = ToInt(row[5]);
                tempCable.ConnectorType = ToStr(row[6]);
                tempCable.IsInputFirst = ToBool(row[7]);
                tempCable.IsInputSecond = ToBool(row[8]);
                result.Add(tempCable);
            }
            return result;
        }
        public static List<AdapterCable> GetAllAdapterCablesByInventorialStatus(EInventorialStatus inventorialStatus)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(SELECT_ALL_ADAPTER_CABLES_BY_INVENTORIAL_STATUS.Replace("?", ((int)inventorialStatus).ToString()));

            List<AdapterCable> result = [];
            AdapterCable tempAdapterCable;
            foreach (object[] row in tempList)
            {
                tempAdapterCable = new();
                tempAdapterCable.Id = ToLong(row[0]);
                tempAdapterCable.InventorialStatus = ToStr(row[1]);
                //row[2] - это workplace_id
                tempAdapterCable.HardwareImage = (byte[])row[3];
                tempAdapterCable.Model = ToStr(row[4]);
                tempAdapterCable.ItemCount = ToInt(row[5]);
                tempAdapterCable.FirstConnectorType = ToStr(row[6]);
                tempAdapterCable.SecondConnectorType = ToStr(row[7]);
                tempAdapterCable.IsInputFirst = ToBool(row[8]);
                tempAdapterCable.IsInputSecond = ToBool(row[9]);
                tempAdapterCable.HasWire = ToBool(row[10]);
                result.Add(tempAdapterCable);
            }
            return result;
        }

        public static List<Wire> GetAllWiresInStock()
        {
            List<Wire> result = [];
            result.AddRange(GetAllCablesByInventorialStatus(EInventorialStatus.InStock));
            result.AddRange(GetAllAdapterCablesByInventorialStatus(EInventorialStatus.InStock));
            return result;
        }
        public static List<Wire> GetAllWires()
        {
            List<Wire> result = GetAllWiresInStock();
            result.AddRange(GetAllCablesByInventorialStatus(EInventorialStatus.InWork));
            result.AddRange(GetAllAdapterCablesByInventorialStatus(EInventorialStatus.InWork));
            return result;
        }



        public static void SaveComputer(Computer computer, WorkPlace workPlace = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO Computer (id_InventorialStatus_f, id_Workplace_f, id_HardwareImage_f, inventorialNumber, model) VALUES ((SELECT id_p FROM InventorialStatus WHERE [value] = ?), ?, ?, ?, ?)");
            SqlRequests.InsertParam(sqlRequest, computer.InventorialStatus);
            SqlRequests.InsertParam(sqlRequest, workPlace?.Id);// это проверка на null. Если не null, то id, а если null - то и null с ним

            if (computer.HardwareImage != null)
            {
                long hardwareImageId = SaveHardwareImage(computer.HardwareImage);
                SqlRequests.InsertParam(sqlRequest, hardwareImageId);
            }
            else
            {
                SqlRequests.InsertParam(sqlRequest, null);
            }
            SqlRequests.InsertParam(sqlRequest, computer.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, computer.Model);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), computer);

            if (computer.Cpu != null)
            {
                SaveCPU(computer.Cpu, computer.Id);
            }

            for (int i = 0; i < computer.GraphicalAdapters.Count; i++)
            {
                SaveGraphicalAdapter(computer.GraphicalAdapters[i], computer.Id);
            }
            for (int i = 0; i < computer.PhisicalDisks.Count; i++)
            {
                SavePhisicalDisk(computer.PhisicalDisks[i], computer.Id);
            }
            for (int i = 0; i < computer.RamModules.Count; i++)
            {
                SaveRamModule(computer.RamModules[i], computer.Id);
            }
        }
        public static void UpdateComputer(Computer computer)
        {
            StringBuilder sqlRequest = new("UPDATE Computer SET id_InventorialStatus_f = dbo.GetInventorialStatusIdByValue(?), id_HardwareImage_f = ?, inventorialNumber = ?, model = ? WHERE id_p = ?");
            SqlRequests.InsertParam(sqlRequest, computer.InventorialStatus);
            long hardwareImageId = UpdateHardwareImage(computer.HardwareImage, computer);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, computer.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, computer.Model);
            SqlRequests.InsertParam(sqlRequest, computer.Id);

            DBUtil.Execute(sqlRequest.ToString());

            long cpuId = ToLong(DBUtil.ExecuteReturnScalar($"SELECT id_p FROM CPU WHERE id_Computer_f = '{computer.Id}'"));
            if (cpuId != 0 && computer.Cpu != null)
            {
                sqlRequest = new("UPDATE CPU SET id_Computer_f = ?, [name] = ?, coreCount = ?, cashMemoryType = ?, cashMemoryValue = ? WHERE id_p = ?");
                SqlRequests.InsertParam(sqlRequest, computer.Id);
                SqlRequests.InsertParam(sqlRequest, computer.Cpu.Name);
                SqlRequests.InsertParam(sqlRequest, computer.Cpu.CoreCount);
                SqlRequests.InsertParam(sqlRequest, computer.Cpu.CashMemoryType);
                SqlRequests.InsertParam(sqlRequest, computer.Cpu.CashMemoryValue);
                SqlRequests.InsertParam(sqlRequest, computer.Cpu.Id);
                DBUtil.Execute(sqlRequest.ToString());
            }
            if (cpuId == 0 && computer.Cpu != null)
            {
                SaveCPU(computer.Cpu, computer.Id);
            }
            if (cpuId != 0 && computer.Cpu == null)
            {
                DBUtil.Execute($"DELETE FROM CPU WHERE id_p = {cpuId}");
            }

            DBUtil.Execute($"DELETE FROM GraphicalAdapter WHERE id_Computer_f = '{computer.Id}'");
            for (int i = 0; i < computer.GraphicalAdapters.Count; i++)
            {
                SaveGraphicalAdapter(computer.GraphicalAdapters[i], computer.Id);
            }

            DBUtil.Execute($"DELETE FROM PhisicalDisk WHERE id_Computer_f = '{computer.Id}'");
            for (int i = 0; i < computer.PhisicalDisks.Count; i++)
            {
                SavePhisicalDisk(computer.PhisicalDisks[i], computer.Id);
            }

            DBUtil.Execute($"DELETE FROM RamModule WHERE id_Computer_f = '{computer.Id}'");
            for (int i = 0; i < computer.RamModules.Count; i++)
            {
                SaveRamModule(computer.RamModules[i], computer.Id);
            }
        }
        public static void DeleteComputer(Computer computer)
        {
            DBUtil.Execute($"DELETE FROM CPU WHERE id_Computer_f = {computer.Id}");
            DBUtil.Execute($"DELETE FROM GraphicalAdapter WHERE id_Computer_f = {computer.Id}");
            DBUtil.Execute($"DELETE FROM PhisicalDisk WHERE id_Computer_f = {computer.Id}");
            DBUtil.Execute($"DELETE FROM RamModule WHERE id_Computer_f = {computer.Id}");
            DBUtil.Execute($"UPDATE Computer SET id_HardwareImage_f = NULL WHERE id_p = {computer.Id}");
            DBUtil.Execute($"DELETE FROM HardwareImage WHERE id_p = (SELECT id_HardwareImage_f FROM Computer WHERE id_p = {computer.Id})");
            DBUtil.Execute($"DELETE FROM Computer WHERE id_p = {computer.Id}");
        }

        public static void SaveMonitor(Monitor monitor, WorkPlace workPlace = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO Monitor (id_InventorialStatus_f,id_Workplace_f,id_HardwareImage_f,inventorialNumber,model,diagonal,aspectRatio,matrix,frequency,resolution) VALUES ((SELECT id_p FROM InventorialStatus WHERE [value] = ?),?,?,?,?,?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, monitor.InventorialStatus);
            SqlRequests.InsertParam(sqlRequest, workPlace?.Id);// это проверка на null. Если не null, то id, а если null - то и null с ним
            long hardwareImageId = SaveHardwareImage(monitor.HardwareImage);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null: hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, monitor.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, monitor.Model);
            SqlRequests.InsertParam(sqlRequest, monitor.Diagonal);
            SqlRequests.InsertParam(sqlRequest, monitor.AspectRatio);
            SqlRequests.InsertParam(sqlRequest, monitor.Matrix);
            SqlRequests.InsertParam(sqlRequest, monitor.Frequency);
            SqlRequests.InsertParam(sqlRequest, monitor.Resolution);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), monitor);
        }
        public static void UpdateMonitor(Monitor monitor)
        {
            StringBuilder sqlRequest = new("UPDATE Monitor SET id_InventorialStatus_f = (SELECT id_p FROM InventorialStatus WHERE [value] = ?), id_HardwareImage_f = ?, inventorialNumber = ?, model = ?, diagonal = ?, aspectRatio = ?, matrix = ?,frequency = ?,resolution = ? WHERE id_p = ?");
            SqlRequests.InsertParam(sqlRequest, monitor.InventorialStatus);
            long hardwareImageId = UpdateHardwareImage(monitor.HardwareImage, monitor);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, monitor.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, monitor.Model);
            SqlRequests.InsertParam(sqlRequest, monitor.Diagonal);
            SqlRequests.InsertParam(sqlRequest, monitor.AspectRatio);
            SqlRequests.InsertParam(sqlRequest, monitor.Matrix);
            SqlRequests.InsertParam(sqlRequest, monitor.Frequency);
            SqlRequests.InsertParam(sqlRequest, monitor.Resolution);
            SqlRequests.InsertParam(sqlRequest, monitor.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }

        public static void SaveKeyboard(Keyboard keyboard, WorkPlace workPlace = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO Keyboard (id_InventorialStatus_f,id_Workplace_f,id_HardwareImage_f,inventorialNumber,model,isWired) VALUES ((SELECT id_p FROM InventorialStatus WHERE [value] = ?),?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, keyboard.InventorialStatus);
            SqlRequests.InsertParam(sqlRequest, workPlace?.Id);// это проверка на null. Если не null, то id, а если null - то и null с ним
            long hardwareImageId = SaveHardwareImage(keyboard.HardwareImage);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, keyboard.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, keyboard.Model);
            SqlRequests.InsertParam(sqlRequest, keyboard.IsWired);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), keyboard);
        }
        public static void UpdateKeyboard(Keyboard keyboard)
        {
            StringBuilder sqlRequest = new("UPDATE Keyboard SET id_InventorialStatus_f = (SELECT id_p FROM InventorialStatus WHERE [value] = ?), id_HardwareImage_f = ?, inventorialNumber = ?, model = ?, isWired = ? WHERE id_p = ?");
            SqlRequests.InsertParam(sqlRequest, keyboard.InventorialStatus);
            long hardwareImageId = UpdateHardwareImage(keyboard.HardwareImage, keyboard);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, keyboard.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, keyboard.Model);
            SqlRequests.InsertParam(sqlRequest, keyboard.IsWired);
            SqlRequests.InsertParam(sqlRequest, keyboard.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }

        public static void SaveMouse(Mouse mouse, WorkPlace workPlace = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO Mouse (id_InventorialStatus_f,id_Workplace_f,id_HardwareImage_f,inventorialNumber,model,isWired) VALUES ((SELECT id_p FROM InventorialStatus WHERE [value] = ?),?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, mouse.InventorialStatus);
            SqlRequests.InsertParam(sqlRequest, workPlace?.Id);// это проверка на null. Если не null, то id, а если null - то и null с ним
            long hardwareImageId = SaveHardwareImage(mouse.HardwareImage);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, mouse.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, mouse.Model);
            SqlRequests.InsertParam(sqlRequest, mouse.IsWired);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), mouse);
        }
        public static void UpdateMouse(Mouse mouse)
        {
            StringBuilder sqlRequest = new("UPDATE Mouse SET id_InventorialStatus_f = (SELECT id_p FROM InventorialStatus WHERE [value] = ?), id_HardwareImage_f = ?, inventorialNumber = ?, model = ?, isWired = ? WHERE id_p = ?");
            SqlRequests.InsertParam(sqlRequest, mouse.InventorialStatus);
            long hardwareImageId = UpdateHardwareImage(mouse.HardwareImage, mouse);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, mouse.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, mouse.Model);
            SqlRequests.InsertParam(sqlRequest, mouse.IsWired);
            SqlRequests.InsertParam(sqlRequest, mouse.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }

        public static void SaveCamera(Camera camera, WorkPlace workPlace = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO Camera (id_InventorialStatus_f,id_Workplace_f,id_HardwareImage_f,inventorialNumber,model, maxResolution, isRotatable, hasMicro) VALUES ((SELECT id_p FROM InventorialStatus WHERE [value] = ?),?,?,?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, camera.InventorialStatus);
            SqlRequests.InsertParam(sqlRequest, workPlace?.Id);// это проверка на null. Если не null, то id, а если null - то и null с ним
            long hardwareImageId = SaveHardwareImage(camera.HardwareImage);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, camera.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, camera.Model);
            SqlRequests.InsertParam(sqlRequest, camera.MaxResolution);
            SqlRequests.InsertParam(sqlRequest, camera.IsRotatable);
            SqlRequests.InsertParam(sqlRequest, camera.HasMicro);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), camera);
        }
        public static void UpdateCamera(Camera camera)
        {
            StringBuilder sqlRequest = new("UPDATE Camera SET id_InventorialStatus_f = (SELECT id_p FROM InventorialStatus WHERE [value] = ?), id_HardwareImage_f = ?, inventorialNumber = ?, model = ?, maxResolution = ?, isRotatable = ?, hasMicro = ? WHERE id_p = ?");
            SqlRequests.InsertParam(sqlRequest, camera.InventorialStatus);
            long hardwareImageId = UpdateHardwareImage(camera.HardwareImage, camera);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, camera.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, camera.Model);
            SqlRequests.InsertParam(sqlRequest, camera.MaxResolution);
            SqlRequests.InsertParam(sqlRequest, camera.IsRotatable);
            SqlRequests.InsertParam(sqlRequest, camera.HasMicro);
            SqlRequests.InsertParam(sqlRequest, camera.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }

        public static void SaveHeadphones(Headphones headphones, WorkPlace workPlace = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO Headphones (id_InventorialStatus_f,id_Workplace_f,id_HardwareImage_f,inventorialNumber,model, isWired, hasMicro) VALUES ((SELECT id_p FROM InventorialStatus WHERE [value] = ?),?,?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, headphones.InventorialStatus);
            SqlRequests.InsertParam(sqlRequest, workPlace?.Id);// это проверка на null. Если не null, то id, а если null - то и null с ним
            long hardwareImageId = SaveHardwareImage(headphones.HardwareImage);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, headphones.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, headphones.Model);
            SqlRequests.InsertParam(sqlRequest, headphones.IsWired);
            SqlRequests.InsertParam(sqlRequest, headphones.HasMicro);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), headphones);
        }
        public static void UpdateHeadphones(Headphones headphones)
        {
            StringBuilder sqlRequest = new("UPDATE Headphones SET id_InventorialStatus_f = (SELECT id_p FROM InventorialStatus WHERE [value] = ?), id_HardwareImage_f = ?, inventorialNumber = ?, model = ?, isWired = ?, hasMicro = ? WHERE id_p = ?");
            SqlRequests.InsertParam(sqlRequest, headphones.InventorialStatus);
            long hardwareImageId = UpdateHardwareImage(headphones.HardwareImage, headphones);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, headphones.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, headphones.Model);
            SqlRequests.InsertParam(sqlRequest, headphones.IsWired);
            SqlRequests.InsertParam(sqlRequest, headphones.HasMicro);
            SqlRequests.InsertParam(sqlRequest, headphones.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }

        public static void SavePrinter(Printer printer, WorkPlace workPlace = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO Printer (id_InventorialStatus_f,id_Workplace_f,id_HardwareImage_f,inventorialNumber,model, maxFormat, printTechnology, color, doubleSidedPrinting, hasScanner) VALUES (dbo.GetInventorialStatusIdByValue(?),?,?,?,?,?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, printer.InventorialStatus);
            SqlRequests.InsertParam(sqlRequest, workPlace?.Id);// это проверка на null. Если не null, то id, а если null - то и null с ним
            long hardwareImageId = SaveHardwareImage(printer.HardwareImage);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, printer.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, printer.Model);
            SqlRequests.InsertParam(sqlRequest, printer.MaxFormat);
            SqlRequests.InsertParam(sqlRequest, printer.PrintTechnology);
            SqlRequests.InsertParam(sqlRequest, printer.Color);
            SqlRequests.InsertParam(sqlRequest, ToBit(printer.DoubleSidedPrinting));
            SqlRequests.InsertParam(sqlRequest, ToBit(printer.HasScanner));

            DBUtil.ExecuteInsert(sqlRequest.ToString(), printer);
        }
        public static void UpdatePrinter(Printer printer)
        {
            StringBuilder sqlRequest = new("UPDATE Printer SET id_InventorialStatus_f = dbo.GetInventorialStatusIdByValue(?), id_HardwareImage_f = ?, inventorialNumber = ?, model = ?, maxFormat = ?, printTechnology = ?, color = ?, doubleSidedPrinting = ?, hasScanner = ? WHERE id_p = ?");
            SqlRequests.InsertParam(sqlRequest, printer.InventorialStatus);
            long hardwareImageId = UpdateHardwareImage(printer.HardwareImage, printer);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, printer.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, printer.Model);
            SqlRequests.InsertParam(sqlRequest, printer.MaxFormat);
            SqlRequests.InsertParam(sqlRequest, printer.PrintTechnology);
            SqlRequests.InsertParam(sqlRequest, printer.Color);
            SqlRequests.InsertParam(sqlRequest, ToBit(printer.DoubleSidedPrinting));
            SqlRequests.InsertParam(sqlRequest, ToBit(printer.HasScanner));
            SqlRequests.InsertParam(sqlRequest, printer.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }

        public static void SaveWiredTelephone(WiredTelephone wiredTelephone, WorkPlace workPlace = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO WiredTelephone (id_InventorialStatus_f,id_Workplace_f,id_HardwareImage_f,inventorialNumber,model, connectionType, hasScreen) VALUES (dbo.GetInventorialStatusIdByValue(?),?,?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, wiredTelephone.InventorialStatus);
            SqlRequests.InsertParam(sqlRequest, workPlace?.Id);// это проверка на null. Если не null, то id, а если null - то и null с ним
            long hardwareImageId = SaveHardwareImage(wiredTelephone.HardwareImage);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, wiredTelephone.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, wiredTelephone.Model);
            SqlRequests.InsertParam(sqlRequest, wiredTelephone.ConnectionType);
            SqlRequests.InsertParam(sqlRequest, ToBit(wiredTelephone.HasScreen));

            DBUtil.ExecuteInsert(sqlRequest.ToString(), wiredTelephone);
        }
        public static void UpdateWiredTelephone(WiredTelephone wiredTelephone)
        {
            StringBuilder sqlRequest = new("UPDATE WiredTelephone SET id_InventorialStatus_f = dbo.GetInventorialStatusIdByValue(?), id_HardwareImage_f = ?, inventorialNumber = ?, model = ?, connectionType = ?, hasScreen = ? WHERE id_p = ?");
            SqlRequests.InsertParam(sqlRequest, wiredTelephone.InventorialStatus);
            long hardwareImageId = UpdateHardwareImage(wiredTelephone.HardwareImage, wiredTelephone);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, wiredTelephone.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, wiredTelephone.Model);
            SqlRequests.InsertParam(sqlRequest, wiredTelephone.ConnectionType);
            SqlRequests.InsertParam(sqlRequest, ToBit(wiredTelephone.HasScreen));
            SqlRequests.InsertParam(sqlRequest, wiredTelephone.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }

        public static void SaveBackupBattery(BackupBattery backupBattery, WorkPlace workPlace = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO BackupBattery (id_InventorialStatus_f,id_Workplace_f,id_HardwareImage_f,inventorialNumber,model, socketsCount, batteryLife, batteryType) VALUES (dbo.GetInventorialStatusIdByValue(?),?,?,?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, backupBattery.InventorialStatus);
            SqlRequests.InsertParam(sqlRequest, workPlace?.Id);// это проверка на null. Если не null, то id, а если null - то и null с ним
            long hardwareImageId = SaveHardwareImage(backupBattery.HardwareImage);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, backupBattery.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, backupBattery.Model);
            SqlRequests.InsertParam(sqlRequest, backupBattery.SocketsCount);
            SqlRequests.InsertParam(sqlRequest, backupBattery.BatteryLife);
            SqlRequests.InsertParam(sqlRequest, backupBattery.BatteryType);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), backupBattery);
        }
        public static void UpdateBackupBattery(BackupBattery backupBattery)
        {
            StringBuilder sqlRequest = new("UPDATE BackupBattery SET id_InventorialStatus_f = dbo.GetInventorialStatusIdByValue(?), id_HardwareImage_f = ?, inventorialNumber = ?, model = ?, socketsCount = ?, batteryLife = ?, batteryType = ? WHERE id_p = ?");
            SqlRequests.InsertParam(sqlRequest, backupBattery.InventorialStatus);
            long hardwareImageId = UpdateHardwareImage(backupBattery.HardwareImage, backupBattery);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, backupBattery.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, backupBattery.Model);
            SqlRequests.InsertParam(sqlRequest, backupBattery.SocketsCount);
            SqlRequests.InsertParam(sqlRequest, backupBattery.BatteryLife);
            SqlRequests.InsertParam(sqlRequest, backupBattery.BatteryType);
            SqlRequests.InsertParam(sqlRequest, backupBattery.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }

        public static void SaveSurgeProtector(SurgeProtector surgeProtector, WorkPlace workPlace = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO SurgeProtector (id_InventorialStatus_f,id_Workplace_f,id_HardwareImage_f,inventorialNumber,model, socketsCount, hasSwitcher, hasWire) VALUES (dbo.GetInventorialStatusIdByValue(?),?,?,?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, surgeProtector.InventorialStatus);
            SqlRequests.InsertParam(sqlRequest, workPlace?.Id);// это проверка на null. Если не null, то id, а если null - то и null с ним
            long hardwareImageId = SaveHardwareImage(surgeProtector.HardwareImage);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, surgeProtector.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, surgeProtector.Model);
            SqlRequests.InsertParam(sqlRequest, surgeProtector.SocketsCount);
            SqlRequests.InsertParam(sqlRequest, surgeProtector.HasSwitcher);
            SqlRequests.InsertParam(sqlRequest, surgeProtector.HasWire);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), surgeProtector);
        }
        public static void UpdateSurgeProtector(SurgeProtector surgeProtector)
        {
            StringBuilder sqlRequest = new("UPDATE SurgeProtector SET id_InventorialStatus_f = dbo.GetInventorialStatusIdByValue(?), id_HardwareImage_f = ?, inventorialNumber = ?, model = ?, socketsCount = ?, hasSwitcher = ?, hasWire = ? WHERE id_p = ?");
            SqlRequests.InsertParam(sqlRequest, surgeProtector.InventorialStatus);
            long hardwareImageId = UpdateHardwareImage(surgeProtector.HardwareImage, surgeProtector);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, surgeProtector.InventorialNumber);
            SqlRequests.InsertParam(sqlRequest, surgeProtector.Model);
            SqlRequests.InsertParam(sqlRequest, surgeProtector.SocketsCount);
            SqlRequests.InsertParam(sqlRequest, surgeProtector.HasSwitcher);
            SqlRequests.InsertParam(sqlRequest, surgeProtector.HasWire);
            SqlRequests.InsertParam(sqlRequest, surgeProtector.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }


        public static void SaveHardware(Hardware hardware, WorkPlace workPlace = null)
        {
            switch (hardware)
            {
                case Computer:
                    SaveComputer(hardware as Computer, workPlace);
                    break;
                case Monitor:
                    SaveMonitor(hardware as Monitor, workPlace);
                    break;
                case Keyboard:
                    SaveKeyboard(hardware as Keyboard, workPlace);
                    break;
                case Mouse:
                    SaveMouse(hardware as Mouse, workPlace);
                    break;

                case Camera:
                    SaveCamera(hardware as Camera, workPlace);
                    break;
                case Headphones:
                    SaveHeadphones(hardware as Headphones, workPlace);
                    break;

                case Printer:
                    SavePrinter(hardware as Printer, workPlace);
                    break;
                case WiredTelephone:
                    SaveWiredTelephone(hardware as WiredTelephone, workPlace);
                    break;

                case BackupBattery:
                    SaveBackupBattery(hardware as BackupBattery, workPlace);
                    break;
                case SurgeProtector:
                    SaveSurgeProtector(hardware as SurgeProtector, workPlace);
                    break;
            }
            SaveHistory("Оборудование создано", StaticCatalogs.HardwareTypeToHardwareName(hardware.GetType().Name), hardware.InventorialNumber, workPlace?.Name);
        }
        public static void UpdateHardware(Hardware hardware)
        {
            switch (hardware)
            {
                case Computer:
                    UpdateComputer(hardware as Computer);
                    break;
                case Monitor:
                    UpdateMonitor(hardware as Monitor);
                    break;
                case Keyboard:
                    UpdateKeyboard(hardware as Keyboard);
                    break;
                case Mouse:
                    UpdateMouse(hardware as Mouse);
                    break;

                case Camera:
                    UpdateCamera(hardware as Camera);
                    break;
                case Headphones:
                    UpdateHeadphones(hardware as Headphones);
                    break;

                case Printer:
                    UpdatePrinter(hardware as Printer);
                    break;
                case WiredTelephone:
                    UpdateWiredTelephone(hardware as WiredTelephone);
                    break;

                case BackupBattery:
                    UpdateBackupBattery(hardware as BackupBattery);
                    break;
                case SurgeProtector:
                    UpdateSurgeProtector(hardware as SurgeProtector);
                    break;
            }
            SaveHistory("Оборудование изменено", StaticCatalogs.HardwareTypeToHardwareName(hardware.GetType().Name), hardware.InventorialNumber);
        }
        public static void WriteOffHardware(Hardware hardware)
        {
            if (hardware is Computer)
            {
                DeleteComputer(hardware as Computer);
                return;
            }
            DBUtil.Execute($"UPDATE {hardware.GetType().Name} SET id_HardwareImage_f = NULL WHERE id_p = {hardware.Id}");
            DBUtil.Execute($"DELETE FROM HardwareImage WHERE id_p = (SELECT id_HardwareImage_f FROM {hardware.GetType().Name} WHERE id_p = {hardware.Id})");
            DBUtil.Execute($"DELETE FROM {hardware.GetType().Name} WHERE id_p = {hardware.Id}");
            SaveHistory("Оборудование списано", StaticCatalogs.HardwareTypeToHardwareName(hardware.GetType().Name), hardware.InventorialNumber);
        }


        private static void SaveCPU(CPU cpu, long computerId) 
        {
            StringBuilder sqlRequest = new("INSERT INTO CPU (id_Computer_f, [name], coreCount, cashMemoryType, cashMemoryValue) VALUES (?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, computerId);
            SqlRequests.InsertParam(sqlRequest, cpu.Name);
            SqlRequests.InsertParam(sqlRequest, cpu.CoreCount);
            SqlRequests.InsertParam(sqlRequest, cpu.CashMemoryType);
            SqlRequests.InsertParam(sqlRequest, cpu.CashMemoryValue);
            DBUtil.ExecuteInsert(sqlRequest.ToString(), cpu);
        }
        private static void SaveGraphicalAdapter(GraphicalAdapter graphicalAdapter, long computerId)
        {
            StringBuilder sqlRequest = new("INSERT INTO GraphicalAdapter (id_Computer_f, model, graphicalProcessor, ramMemory) VALUES (?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, computerId);
            SqlRequests.InsertParam(sqlRequest, graphicalAdapter.Model);
            SqlRequests.InsertParam(sqlRequest, graphicalAdapter.GraphicalProcessor);
            SqlRequests.InsertParam(sqlRequest, graphicalAdapter.RamMemory);
            DBUtil.ExecuteInsert(sqlRequest.ToString(), graphicalAdapter);
        }
        private static void SavePhisicalDisk(PhisicalDisk phisicalDisk, long computerId)
        {
            StringBuilder sqlRequest = new("INSERT INTO PhisicalDisk (id_Computer_f, model, diskType, interface, formFactor, memory) VALUES (?,?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, computerId);
            SqlRequests.InsertParam(sqlRequest, phisicalDisk.Model);
            SqlRequests.InsertParam(sqlRequest, phisicalDisk.DiskType);
            SqlRequests.InsertParam(sqlRequest, phisicalDisk.Interface);
            SqlRequests.InsertParam(sqlRequest, phisicalDisk.FormFactor);
            SqlRequests.InsertParam(sqlRequest, phisicalDisk.Memory);
            DBUtil.ExecuteInsert(sqlRequest.ToString(), phisicalDisk);
        }
        private static void SaveRamModule(RamModule ramModule, long computerId)
        {
            StringBuilder sqlRequest = new("INSERT INTO RamModule(id_Computer_f, model, ramType, memory) VALUES (?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, computerId);
            SqlRequests.InsertParam(sqlRequest, ramModule.Model);
            SqlRequests.InsertParam(sqlRequest, ramModule.RamType);
            SqlRequests.InsertParam(sqlRequest, ramModule.Memory);
            DBUtil.ExecuteInsert(sqlRequest.ToString(), ramModule);
        }



        public static void SaveCable(Cable cable, WorkPlace wp = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO Cable (id_Workplace_f, id_InventorialStatus_f,id_HardwareImage_f, model, itemCount, connectorType, isInputFirst, isInputSecond) VALUES (?, dbo.GetInventorialStatusIdByValue(?),?,?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, wp == null ? null : wp.Id);
            SqlRequests.InsertParam(sqlRequest, cable.InventorialStatus);
            long hardwareImageId = SaveHardwareImage(cable.HardwareImage);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, cable.Model);
            SqlRequests.InsertParam(sqlRequest, cable.ItemCount);
            SqlRequests.InsertParam(sqlRequest, cable.ConnectorType);
            SqlRequests.InsertParam(sqlRequest, cable.IsInputFirst);
            SqlRequests.InsertParam(sqlRequest, cable.IsInputSecond);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), cable);
        }
        public static void UpdateCable(Cable cable)
        {
            StringBuilder sqlRequest = new("UPDATE Cable SET id_InventorialStatus_f = dbo.GetInventorialStatusIdByValue(?), id_HardwareImage_f = ?, model = ?, itemCount = ?, connectorType = ?, isInputFirst = ?, isInputSecond = ? WHERE id_p = ?");
            SqlRequests.InsertParam(sqlRequest, cable.InventorialStatus);
            long hardwareImageId = UpdateHardwareImage(cable.HardwareImage, cable);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, cable.Model);
            SqlRequests.InsertParam(sqlRequest, cable.ItemCount);
            SqlRequests.InsertParam(sqlRequest, cable.ConnectorType);
            SqlRequests.InsertParam(sqlRequest, cable.IsInputFirst);
            SqlRequests.InsertParam(sqlRequest, cable.IsInputSecond);
            SqlRequests.InsertParam(sqlRequest, cable.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }
        public static Cable SplitCable(Cable cable)
        {
            if (cable.ItemCount == 1) return cable;
            cable.ItemCount--;
            UpdateCable(cable);
            Cable newCable = new(cable);
            newCable.ItemCount = 1;
            SaveCable(newCable);
            return newCable;
        }
        public static long CheckCableDuplicates(CableView cableView)
        {
            StringBuilder sqlRequest = new("SELECT id_p FROM Cable WHERE connectorType LIKE ? AND isInputFirst = ? AND isInputSecond = ? AND id_InventorialStatus_f = 101 AND id_p != ?");
            SqlRequests.InsertParam(sqlRequest, cableView.ConnectorType);
            SqlRequests.InsertParam(sqlRequest, cableView.IsInputFirst);
            SqlRequests.InsertParam(sqlRequest, cableView.IsInputSecond);
            SqlRequests.InsertParam(sqlRequest, cableView.Original == null ? 0 : cableView.Original.Id);
            long flag = ToLong(DBUtil.ExecuteReturnScalar(sqlRequest.ToString()));
            return flag;
        }
        public static long CheckCableDuplicatesOnWorkplace(Cable cable, WorkPlace workPlace)
        {
            StringBuilder sqlRequest = new("SELECT id_p FROM Cable WHERE connectorType LIKE ? AND isInputFirst = ? AND isInputSecond = ? AND id_Workplace_f = ? AND id_p != ?");
            SqlRequests.InsertParam(sqlRequest, cable.ConnectorType);
            SqlRequests.InsertParam(sqlRequest, cable.IsInputFirst);
            SqlRequests.InsertParam(sqlRequest, cable.IsInputSecond);
            SqlRequests.InsertParam(sqlRequest, workPlace.Id);
            SqlRequests.InsertParam(sqlRequest, cable.Id);
            long flag = ToLong(DBUtil.ExecuteReturnScalar(sqlRequest.ToString()));
            return flag;
        }
        public static long CheckCableDuplicatesOnWorkplace(CableView cableView, WorkPlace workPlace)
        {
            StringBuilder sqlRequest = new("SELECT id_p FROM Cable WHERE connectorType LIKE ? AND isInputFirst = ? AND isInputSecond = ? AND id_Workplace_f = ? AND id_p != ?");
            SqlRequests.InsertParam(sqlRequest, cableView.ConnectorType);
            SqlRequests.InsertParam(sqlRequest, cableView.IsInputFirst);
            SqlRequests.InsertParam(sqlRequest, cableView.IsInputSecond);
            SqlRequests.InsertParam(sqlRequest, workPlace.Id);
            SqlRequests.InsertParam(sqlRequest, cableView.Original == null ? 0 : cableView.Original.Id);
            long flag = ToLong(DBUtil.ExecuteReturnScalar(sqlRequest.ToString()));
            return flag;
        }

        public static void SaveAdapterCable(AdapterCable adapterCable, WorkPlace wp = null)
        {
            StringBuilder sqlRequest = new("INSERT INTO AdapterCable (id_Workplace_f,id_InventorialStatus_f,id_HardwareImage_f, model, itemCount, firstСonnectorType, secondСonnectorType, isInputFirst, isInputSecond, hasWire) VALUES (?, dbo.GetInventorialStatusIdByValue(?),?,?,?,?,?,?,?,?)");
            SqlRequests.InsertParam(sqlRequest, wp == null ? null : wp.Id);
            SqlRequests.InsertParam(sqlRequest, adapterCable.InventorialStatus);
            long hardwareImageId = SaveHardwareImage(adapterCable.HardwareImage);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, adapterCable.Model);
            SqlRequests.InsertParam(sqlRequest, adapterCable.ItemCount);
            SqlRequests.InsertParam(sqlRequest, adapterCable.FirstConnectorType);
            SqlRequests.InsertParam(sqlRequest, adapterCable.SecondConnectorType);
            SqlRequests.InsertParam(sqlRequest, adapterCable.IsInputFirst);
            SqlRequests.InsertParam(sqlRequest, adapterCable.IsInputSecond);
            SqlRequests.InsertParam(sqlRequest, adapterCable.HasWire);

            DBUtil.ExecuteInsert(sqlRequest.ToString(), adapterCable);
        }
        public static void UpdateAdapterCable(AdapterCable adapterCable)
        {
            StringBuilder sqlRequest = new("UPDATE AdapterCable SET id_InventorialStatus_f = dbo.GetInventorialStatusIdByValue(?), id_HardwareImage_f = ?, model = ?, itemCount = ?, firstСonnectorType = ?, secondСonnectorType = ?, isInputFirst = ?, isInputSecond = ?, hasWire = ? WHERE id_p = ?");
            SqlRequests.InsertParam(sqlRequest, adapterCable.InventorialStatus);
            long hardwareImageId = UpdateHardwareImage(adapterCable.HardwareImage, adapterCable);
            SqlRequests.InsertParam(sqlRequest, hardwareImageId == 0 ? null : hardwareImageId);
            SqlRequests.InsertParam(sqlRequest, adapterCable.Model);
            SqlRequests.InsertParam(sqlRequest, adapterCable.ItemCount);
            SqlRequests.InsertParam(sqlRequest, adapterCable.FirstConnectorType);
            SqlRequests.InsertParam(sqlRequest, adapterCable.SecondConnectorType);
            SqlRequests.InsertParam(sqlRequest, adapterCable.IsInputFirst);
            SqlRequests.InsertParam(sqlRequest, adapterCable.IsInputSecond);
            SqlRequests.InsertParam(sqlRequest, adapterCable.HasWire);
            SqlRequests.InsertParam(sqlRequest, adapterCable.Id);

            DBUtil.Execute(sqlRequest.ToString());
        }
        public static AdapterCable SplitAdapterCable(AdapterCable adapterCable)
        {
            if (adapterCable.ItemCount == 1) return adapterCable;
            adapterCable.ItemCount--;
            UpdateAdapterCable(adapterCable);
            AdapterCable newAdapterCable = new(adapterCable);
            newAdapterCable.ItemCount = 1;
            SaveAdapterCable(newAdapterCable);
            return newAdapterCable;
        }
        public static long CheckAdapterCableDuplicates(AdapterCableView adapterCableView)
        {
            StringBuilder sqlRequest = new("SELECT id_p FROM AdapterCable WHERE firstConnectorType LIKE ? AND secondConnectorType LIKE ? AND isInputFirst = ? AND isInputSecond = ? AND hasWire = ? AND id_InventorialStatus_f = 101 AND id_p != ?");
            SqlRequests.InsertParam(sqlRequest, adapterCableView.FirstConnectorType);
            SqlRequests.InsertParam(sqlRequest, adapterCableView.SecondConnectorType);
            SqlRequests.InsertParam(sqlRequest, adapterCableView.IsInputFirst);
            SqlRequests.InsertParam(sqlRequest, adapterCableView.IsInputSecond);
            SqlRequests.InsertParam(sqlRequest, adapterCableView.HasWire);
            SqlRequests.InsertParam(sqlRequest, adapterCableView.Original == null ? 0 : adapterCableView.Original.Id);
            long flag = ToLong(DBUtil.ExecuteReturnScalar(sqlRequest.ToString()));
            return flag;
        }
        public static long CheckAdapterCableDuplicatesOnWorkplace(AdapterCable adapterCable, WorkPlace workPlace)
        {
            StringBuilder sqlRequest = new("SELECT id_p FROM AdapterCable WHERE firstConnectorType LIKE ? AND secondConnectorType LIKE ? AND isInputFirst = ? AND isInputSecond = ? AND hasWire = ? AND id_Workplace_f = ? AND id_p != ?");
            SqlRequests.InsertParam(sqlRequest, adapterCable.FirstConnectorType);
            SqlRequests.InsertParam(sqlRequest, adapterCable.SecondConnectorType);
            SqlRequests.InsertParam(sqlRequest, adapterCable.IsInputFirst);
            SqlRequests.InsertParam(sqlRequest, adapterCable.IsInputSecond);
            SqlRequests.InsertParam(sqlRequest, adapterCable.HasWire);
            SqlRequests.InsertParam(sqlRequest, workPlace.Id);
            SqlRequests.InsertParam(sqlRequest, adapterCable.Id);
            long flag = ToLong(DBUtil.ExecuteReturnScalar(sqlRequest.ToString()));
            return flag;
        }
        public static long CheckAdapterCableDuplicatesOnWorkplace(AdapterCableView adapterCableView, WorkPlace workPlace)
        {
            StringBuilder sqlRequest = new("SELECT id_p FROM AdapterCable WHERE firstConnectorType LIKE ? AND secondConnectorType LIKE ? AND isInputFirst = ? AND isInputSecond = ? AND hasWire = ? AND id_Workplace_f = ? AND id_p != ?");
            SqlRequests.InsertParam(sqlRequest, adapterCableView.FirstConnectorType);
            SqlRequests.InsertParam(sqlRequest, adapterCableView.SecondConnectorType);
            SqlRequests.InsertParam(sqlRequest, adapterCableView.IsInputFirst);
            SqlRequests.InsertParam(sqlRequest, adapterCableView.IsInputSecond);
            SqlRequests.InsertParam(sqlRequest, adapterCableView.HasWire);
            SqlRequests.InsertParam(sqlRequest, workPlace.Id);
            SqlRequests.InsertParam(sqlRequest, adapterCableView.Original == null ? 0 : adapterCableView.Original.Id);
            long flag = ToLong(DBUtil.ExecuteReturnScalar(sqlRequest.ToString()));
            return flag;
        }

        public static void SaveWire(Wire wire, WorkPlace workPlace = null)
        {
            switch (wire)
            {
                case Cable:
                    SaveCable(wire as Cable, workPlace);
                    break;
                case AdapterCable:
                    SaveAdapterCable(wire as AdapterCable, workPlace);
                    break;
            }
            SaveHistory("Оборудование создано", StaticCatalogs.HardwareTypeToHardwareName(wire.GetType().Name), null, workPlace?.Name);
        }
        public static void UpdateWire(Wire wire)
        {
            switch (wire)
            {
                case Cable:
                    UpdateCable(wire as Cable);
                    break;
                case AdapterCable:
                    UpdateAdapterCable(wire as AdapterCable);
                    break;
            }
            SaveHistory("Оборудование изменено", StaticCatalogs.HardwareTypeToHardwareName(wire.GetType().Name));
        }
        public static Wire SplitWire(Wire wire)
        {
            switch (wire)
            {
                case Cable:
                    return SplitCable(wire as Cable);
                case AdapterCable:
                    return SplitAdapterCable(wire as AdapterCable);
            }
            return null;
        }
        public static void WriteOffWire(Wire wire)
        {
            if (wire.ItemCount == 1)
            {
                DBUtil.Execute($"UPDATE {wire.GetType().Name} SET id_HardwareImage_f = NULL WHERE id_p = {wire.Id}");
                DBUtil.Execute($"DELETE FROM HardwareImage WHERE id_p = (SELECT id_HardwareImage_f FROM {wire.GetType().Name} WHERE id_p = {wire.Id})");
                DBUtil.Execute($"DELETE FROM {wire.GetType().Name} WHERE id_p = {wire.Id}");
            }
            else
            {
                DBUtil.Execute($"UPDATE {wire.GetType().Name} SET itemCount = itemCount - 1 WHERE id_p = {wire.Id}");
            }
            wire.ItemCount--;
            SaveHistory("Оборудование списано", StaticCatalogs.HardwareTypeToHardwareName(wire.GetType().Name));
        }
        public static long CheckWireDuplicates(Wire wire)
        {
            return wire switch
            {
                Cable => CheckCableDuplicates(new CableView(wire as Cable)),
                AdapterCable => CheckAdapterCableDuplicates(new AdapterCableView(wire as AdapterCable))
            };
        }
        public static long CheckWireDuplicatesOnWorkplace(Wire wire, WorkPlace workPlace)
        {
            return wire switch
            {
                Cable => CheckCableDuplicatesOnWorkplace(wire as Cable,workPlace),
                AdapterCable => CheckAdapterCableDuplicatesOnWorkplace(wire as AdapterCable, workPlace)
            };
        }




        public static void SetInventorialStatusToHardware(Hardware hardware, EInventorialStatus inventorialStatus)
        {
            DBUtil.Execute($"UPDATE [{hardware.GetType().Name}] SET id_InventorialStatus_f = {(int)inventorialStatus} WHERE id_p = {hardware.Id}");
            hardware.InventorialStatus = StaticCatalogs.EInventorialStatusToString(inventorialStatus);
        }
        public static void SetInventorialStatusToWire(Wire wire, EInventorialStatus inventorialStatus)
        {
            DBUtil.Execute($"UPDATE [{wire.GetType().Name}] SET id_InventorialStatus_f = {(int)inventorialStatus} WHERE id_p = {wire.Id}");
            wire.InventorialStatus = StaticCatalogs.EInventorialStatusToString(inventorialStatus);
        }

        public static void AttachHardwareToWorkplace(Hardware hardware, WorkPlace workplace)
        {
            DBUtil.Execute($"UPDATE {hardware.GetType().Name} SET id_Workplace_f = {workplace.Id}, id_InventorialStatus_f = 100 WHERE id_p = {hardware.Id}");
            hardware.InventorialStatus = StaticCatalogs.EInventorialStatusToString(EInventorialStatus.InWork);
            SaveHistory("Оборудование закреплено", StaticCatalogs.HardwareTypeToHardwareName(hardware.GetType().Name), hardware.InventorialNumber, workplace?.Name);
        }
        public static void DetachHardware(Hardware hardware)
        {
            DBUtil.Execute($"UPDATE {hardware.GetType().Name} SET id_Workplace_f = NULL, id_InventorialStatus_f = 101 WHERE id_p = {hardware.Id}");
            hardware.InventorialStatus = StaticCatalogs.EInventorialStatusToString(EInventorialStatus.InStock);
            SaveHistory("Оборудование откреплено", StaticCatalogs.HardwareTypeToHardwareName(hardware.GetType().Name), hardware.InventorialNumber);
        }

        public static void AttachWireToWorkplace(Wire wire, WorkPlace workplace)
        {
            long duplicateId = CheckWireDuplicatesOnWorkplace(wire, workplace);

            if (duplicateId == 0)
            {
                Wire temp = wire.Clone();
                temp.ItemCount = 1;
                temp.InventorialStatus = StaticCatalogs.EInventorialStatusToString(EInventorialStatus.InWork);
                SaveWire(temp, workplace);
                workplace.WireList.Add(temp);
            }
            else
            {
                DBUtil.Execute($"UPDATE {wire.GetType().Name} SET itemCount = itemCount + 1 WHERE id_p = {duplicateId}");
                workplace.WireList.Find((Wire obj) => obj.Id == duplicateId && obj.GetType().Name.Equals(wire.GetType().Name)).ItemCount++;
            }
            DBUtil.Execute($"UPDATE {wire.GetType().Name} SET itemCount = itemCount - 1 WHERE id_p = {wire.Id}");
            DBUtil.Execute($"DELETE FROM {wire.GetType().Name} WHERE id_p = {wire.Id} AND itemCount = 0");
            SaveHistory("Оборудование закреплено", StaticCatalogs.HardwareTypeToHardwareName(wire.GetType().Name), null, workplace?.Name);
        }
        public static void DetachWire(Wire wire)
        {
            long duplicateId = CheckWireDuplicates(wire);

            if (duplicateId == 0)
            {
                Wire temp = wire.Clone();
                temp.ItemCount = 1;
                temp.InventorialStatus = StaticCatalogs.EInventorialStatusToString(EInventorialStatus.InStock);
                SaveWire(temp);
            }
            else
            {
                DBUtil.Execute($"UPDATE {wire.GetType().Name} SET itemCount = itemCount + 1 WHERE id_p = {duplicateId}");
            }
            DBUtil.Execute($"UPDATE {wire.GetType().Name} SET itemCount = itemCount - 1 WHERE id_p = {wire.Id}");
            DBUtil.Execute($"DELETE FROM {wire.GetType().Name} WHERE id_p = {wire.Id} AND itemCount = 0");
            wire.ItemCount--;
        }
        public static WorkPlace ParceWorkplaceRawData(List<string[]> raw)
        {
            if (raw == null || raw.Count == 0) return null;

            int counter = 0;

            WorkPlace result = new();
            result.Id = ToLong(raw[counter][1]);
            result.Name = raw[counter][2];
            counter++;

            if (counter == raw.Count) return result;

            foreach (Computer item in ParceComputerRawData(raw.GetRange(1, raw.Count - 1)))
            {
                result.HardwareList.Add(item);
            }
            while ( raw[counter][0].Equals("Computer") || raw[counter][0].Equals("CPU") || raw[counter][0].Equals("GraphicalAdapter") || raw[counter][0].Equals("PhisicalDisk") || raw[counter][0].Equals("RamModule"))
            {
                counter++;
                if (counter == raw.Count) return result;
            }

            Monitor tempMonitor;
            while (counter < raw.Count && raw[counter][0].Equals("Monitor"))
            {
                tempMonitor = new();
                tempMonitor.Id = ToLong(raw[counter][1]);
                tempMonitor.InventorialStatus = raw[counter][2];
                tempMonitor.HardwareImage = GetHardwareImage(ToLong(raw[counter][3]));
                tempMonitor.InventorialNumber = ToInt(raw[counter][4]);
                tempMonitor.Model = raw[counter][5];

                tempMonitor.Diagonal = ToDouble(raw[counter][6]);
                tempMonitor.AspectRatio = raw[counter][7];
                tempMonitor.Matrix = raw[counter][8];
                tempMonitor.Frequency = ToInt(raw[counter][9]);
                tempMonitor.Resolution = raw[counter][10];

                result.HardwareList.Add(tempMonitor);
                counter++;
            }

            Keyboard tempKeyboard;
            while (counter < raw.Count && raw[counter][0].Equals("Keyboard"))
            {
                tempKeyboard = new();
                tempKeyboard.Id = ToLong(raw[counter][1]);
                tempKeyboard.InventorialStatus = raw[counter][2];
                tempKeyboard.HardwareImage = GetHardwareImage(ToLong(raw[counter][3]));
                tempKeyboard.InventorialNumber = ToInt(raw[counter][4]);
                tempKeyboard.Model = raw[counter][5];

                tempKeyboard.IsWired = ToBool(raw[counter][6]);

                result.HardwareList.Add(tempKeyboard);
                counter++;
            }

            Mouse tempMouse;
            while (counter < raw.Count && raw[counter][0].Equals("Mouse"))
            {
                tempMouse = new();
                tempMouse.Id = ToLong(raw[counter][1]);
                tempMouse.InventorialStatus = raw[counter][2];
                tempMouse.HardwareImage = GetHardwareImage(ToLong(raw[counter][3]));
                tempMouse.InventorialNumber = ToInt(raw[counter][4]);
                tempMouse.Model = raw[counter][5];

                tempMouse.IsWired = ToBool(raw[counter][6]);

                result.HardwareList.Add(tempMouse);
                counter++;
            }

            Camera tempCamera;
            while (counter < raw.Count && raw[counter][0].Equals("Camera"))
            {
                tempCamera = new();
                tempCamera.Id = ToLong(raw[counter][1]);
                tempCamera.InventorialStatus = raw[counter][2];
                tempCamera.HardwareImage = GetHardwareImage(ToLong(raw[counter][3]));
                tempCamera.InventorialNumber = ToInt(raw[counter][4]);
                tempCamera.Model = raw[counter][5];

                tempCamera.MaxResolution = raw[counter][6];
                tempCamera.IsRotatable = ToBool(raw[counter][7]);
                tempCamera.HasMicro = ToBool(raw[counter][8]);

                result.HardwareList.Add(tempCamera);
                counter++;
            }

            Headphones tempHeadphones;
            while (counter < raw.Count && raw[counter][0].Equals("Headphones"))
            {
                tempHeadphones = new();
                tempHeadphones.Id = ToLong(raw[counter][1]);
                tempHeadphones.InventorialStatus = raw[counter][2];
                tempHeadphones.HardwareImage = GetHardwareImage(ToLong(raw[counter][3]));
                tempHeadphones.InventorialNumber = ToInt(raw[counter][4]);
                tempHeadphones.Model = raw[counter][5];

                tempHeadphones.IsWired = ToBool(raw[counter][6]);
                tempHeadphones.HasMicro = ToBool(raw[counter][7]);

                result.HardwareList.Add(tempHeadphones);
                counter++;
            }

            Printer tempPrinter;
            while (counter < raw.Count && raw[counter][0].Equals("Printer"))
            {
                tempPrinter = new();
                tempPrinter.Id = ToLong(raw[counter][1]);
                tempPrinter.InventorialStatus = raw[counter][2];
                tempPrinter.HardwareImage = GetHardwareImage(ToLong(raw[counter][3]));
                tempPrinter.InventorialNumber = ToInt(raw[counter][4]);
                tempPrinter.Model = raw[counter][5];

                tempPrinter.MaxFormat = raw[counter][6];
                tempPrinter.PrintTechnology = raw[counter][7];
                tempPrinter.Color = raw[counter][8];
                tempPrinter.DoubleSidedPrinting = ToBool(raw[counter][9]);
                tempPrinter.HasScanner = ToBool(raw[counter][10]);

                result.HardwareList.Add(tempPrinter);
                counter++;
            }

            WiredTelephone tempWiredTelephone;
            while (counter < raw.Count && raw[counter][0].Equals("WiredTelephone"))
            {
                tempWiredTelephone = new();
                tempWiredTelephone.Id = ToLong(raw[counter][1]);
                tempWiredTelephone.InventorialStatus = raw[counter][2];
                tempWiredTelephone.HardwareImage = GetHardwareImage(ToLong(raw[counter][3]));
                tempWiredTelephone.InventorialNumber = ToInt(raw[counter][4]);
                tempWiredTelephone.Model = raw[counter][5];

                tempWiredTelephone.ConnectionType = raw[counter][6];
                tempWiredTelephone.HasScreen = ToBool(raw[counter][7]);

                result.HardwareList.Add(tempWiredTelephone);
                counter++;
            }

            BackupBattery tempBackupBattery;
            while (counter < raw.Count && raw[counter][0].Equals("BackupBattery"))
            {
                tempBackupBattery = new();
                tempBackupBattery.Id = ToLong(raw[counter][1]);
                tempBackupBattery.InventorialStatus = raw[counter][2];
                tempBackupBattery.HardwareImage = GetHardwareImage(ToLong(raw[counter][3]));
                tempBackupBattery.InventorialNumber = ToInt(raw[counter][4]);
                tempBackupBattery.Model = raw[counter][5];

                tempBackupBattery.SocketsCount = ToInt(raw[counter][6]);
                tempBackupBattery.BatteryLife = ToInt(raw[counter][7]);
                tempBackupBattery.BatteryType = raw[counter][8];

                result.HardwareList.Add(tempBackupBattery);
                counter++;
            }

            SurgeProtector tempSurgeProtector;
            while (counter < raw.Count && raw[counter][0].Equals("SurgeProtector"))
            {
                tempSurgeProtector = new();
                tempSurgeProtector.Id = ToLong(raw[counter][1]);
                tempSurgeProtector.InventorialStatus = raw[counter][2];
                tempSurgeProtector.HardwareImage = GetHardwareImage(ToLong(raw[counter][3]));
                tempSurgeProtector.InventorialNumber = ToInt(raw[counter][4]);
                tempSurgeProtector.Model = raw[counter][5];

                tempSurgeProtector.SocketsCount = ToInt(raw[counter][6]);
                tempSurgeProtector.HasSwitcher = ToBool(raw[counter][7]);
                tempSurgeProtector.HasWire = ToBool(raw[counter][8]);

                result.HardwareList.Add(tempSurgeProtector);
                counter++;
            }



            Cable tempCable;
            while (counter < raw.Count && raw[counter][0].Equals("Cable"))
            {
                tempCable = new();
                tempCable.Id = ToLong(raw[counter][1]);
                tempCable.InventorialStatus = raw[counter][2];
                tempCable.HardwareImage = GetHardwareImage(ToLong(raw[counter][3]));
                tempCable.Model = raw[counter][4];
                tempCable.ItemCount = ToInt(raw[counter][5]);
                tempCable.ConnectorType = raw[counter][6];
                tempCable.IsInputFirst = ToBool(raw[counter][7]);
                tempCable.IsInputSecond = ToBool(raw[counter][8]);

                result.WireList.Add(tempCable);
                counter++;
            }

            AdapterCable tempAdapterCable;
            while (counter < raw.Count && raw[counter][0].Equals("Cable"))
            {
                tempAdapterCable = new();
                tempAdapterCable.Id = ToLong(raw[counter][1]);
                tempAdapterCable.InventorialStatus = raw[counter][2];
                tempAdapterCable.HardwareImage = GetHardwareImage(ToLong(raw[counter][3]));
                tempAdapterCable.Model = raw[counter][4];
                tempAdapterCable.ItemCount = ToInt(raw[counter][5]);
                tempAdapterCable.FirstConnectorType = raw[counter][6];
                tempAdapterCable.SecondConnectorType = raw[counter][7];
                tempAdapterCable.IsInputFirst = ToBool(raw[counter][8]);
                tempAdapterCable.IsInputSecond = ToBool(raw[counter][9]);
                tempAdapterCable.HasWire = ToBool(raw[counter][10]);

                result.WireList.Add(tempAdapterCable);
                counter++;
            }


            return result;
        }



        public static void GetStatistic(Statistic statistic)
        {
            List<object[]> tempList = DBUtil.ExecuteReturnOld(GET_STATISTIC);

            statistic.HardwareInInventory = ToInt(tempList[1][1]) + ToInt(tempList[2][1]);
            statistic.HardwareOnWorkplaces = ToInt(tempList[0][1]);
            statistic.WireInInventory = ToInt(tempList[5][1]);
            statistic.WireOnWorkplaces = ToInt(tempList[4][1]);
            statistic.AllInInventory = statistic.HardwareInInventory + statistic.WireInInventory;
            statistic.AllOnWorkplaces = statistic.HardwareOnWorkplaces + statistic.WireOnWorkplaces;
            statistic.AllDefective = ToInt(tempList[2][1]);
            statistic.HardwareAll = statistic.HardwareInInventory + statistic.HardwareOnWorkplaces + statistic.AllDefective;
            statistic.WireAll = statistic.WireInInventory + statistic.WireOnWorkplaces;
            statistic.All = statistic.HardwareAll + statistic.WireAll;
        }
    }
}
