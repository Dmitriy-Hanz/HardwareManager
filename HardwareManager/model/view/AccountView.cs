using HardwareManager.infrastructure.utils.Base;
using HardwareManager.infrastructure.utils.database;
using HardwareManager.model.entity;
using Microsoft.Identity.Client.NativeInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.model.view
{
    public class AccountView
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RepeatedPassword { get; set; }
        public string Cabinet { get; set; }
        public string Workplace { get; set; }


        public AccountView() { }
        public AccountView(Account account) 
        {
            Id = account.Id;
            Username = account.Username;
            Password = account.Password;
        }

        public Account Convert()
        {
            return new Account
            {
                Id = Id,
                Username = Username,
                Password = Password
            };
        }

        public void Merge(Account target)
        {
            target.Username = Username;
            target.Password = Password;
        }

        public bool ValidateLogin()
        {
            if (!TypicalValidations.ValidateTextField(Username, "Имя пользователя")) { return false; }
            if (!TypicalValidations.ValidateTextField(Password, "Пароль")) { return false; }
            return true;
        }
        public bool ValidateRegistration()
        {
            if (!TypicalValidations.ValidateUsername(Username, 7, 30)) { return false; }
            if (!DataBaseService.CheckAccountUsername(Username))
            {
                TypicalActions.ShowInformationalMessage("Пользователь с данным именем уже существует");
                return false;
            }
            if (!TypicalValidations.ValidatePassword(Password, 10, 20)) { return false; }
            if (!TypicalValidations.ValidateRepeatedPassword(Password, RepeatedPassword)) { return false; }
            if (!TypicalValidations.ValidateTextField(Cabinet, "Кабинет")) return false;
            if (!DataBaseService.CheckCabinetName(Cabinet))
            {
                TypicalActions.ShowInformationalMessage("Не удается найти указанный кабинет");
                return false;
            }
            if (!TypicalValidations.ValidateTextField(Workplace, "Рабочее место")) return false;
            if (!DataBaseService.CheckWorkplaceName(Workplace))
            {
                TypicalActions.ShowInformationalMessage("Не удается найти указанное рабочее место");
                return false;
            }
            if (!DataBaseService.CheckAccountWorkplaceData(Username, Workplace))
            {
                TypicalActions.ShowInformationalMessage("За данным рабочим местом уже закреплен работник");
                return false;
            }

            return true;
        }
    }
}
