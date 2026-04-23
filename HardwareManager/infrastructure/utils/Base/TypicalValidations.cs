using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HardwareManager.infrastructure.utils.Base
{
    public class TypicalValidations
    {
        public static void ShowValidationMessageBox(string message)
        {
            MessageBox.Show(message, "Ошибка валидации", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        public static bool ValidateTextField(string input, string fieldName)
        {
            if (input == null || input.Trim().Equals(""))
            {
                ShowValidationMessageBox($"Поле «{fieldName}» обязательное для заполнения.");
                return false;
            }
            return true;
        }
        
        public static bool ValidateIntegerField(int? input, string fieldName, bool allowNegative = true, bool allowZero = true)
        {
            if (input == null)
            {
                ShowValidationMessageBox($"Поле «{fieldName}» обязательное для заполнения.");
                return false;
            }
            if (allowNegative == false && input < 0 )
            {
                ShowValidationMessageBox($"Поле «{fieldName}» не должно иметь отрицательное значение.");
                return false;
            }
            if (allowZero == false && input == 0)
            {
                ShowValidationMessageBox($"Поле «{fieldName}» не должно иметь нулевое значение.");
                return false;
            }
            return true;
        }

        public static bool ValidateFloatField(double? input, string fieldName, bool allowNegative = true, bool allowZero = true)
        {
            if (input == null)
            {
                ShowValidationMessageBox($"Поле «{fieldName}» обязательное для заполнения.");
                return false;
            }
            if (allowNegative == false && input < 0)
            {
                ShowValidationMessageBox($"Поле «{fieldName}» не должно иметь отрицательное значение.");
                return false;
            }
            if (allowZero == false && input == 0)
            {
                ShowValidationMessageBox($"Поле «{fieldName}» не должно иметь нулевое значение.");
                return false;
            }
            return true;
        }



        public static bool ValidateLogin(string input)
        {
            if (input == null || input.Equals(""))
            {
                ShowValidationMessageBox($"Поле «Логин» обязательное для заполнения.");
                return false;
            }
            return true;
        }
        public static bool ValidateLogin(string input, int minSymbolsCount, int maxSymbolsCount)
        {
            if (input == null || input.Equals(""))
            {
                ShowValidationMessageBox($"Поле «Логин» обязательное для заполнения.");
                return false;
            }
            if (input.Length < minSymbolsCount)
            {
                ShowValidationMessageBox($"Логин слишком короткий. Он должен быть размером в {minSymbolsCount}-{maxSymbolsCount} символов.");
                return false;
            }
            if (input.Length > maxSymbolsCount)
            {
                ShowValidationMessageBox($"Логин слишком длинный. Он должен быть размером в {minSymbolsCount}-{maxSymbolsCount} символов.");
                return false;
            }
            return true;
        }        
        public static bool ValidateUsername(string input)
        {
            if (input == null || input.Equals(""))
            {
                ShowValidationMessageBox($"Имя пользователя обязательно для заполнения.");
                return false;
            }
            return true;
        }
        public static bool ValidateUsername(string input, int minSymbolsCount, int maxSymbolsCount)
        {
            if (input == null || input.Equals(""))
            {
                ShowValidationMessageBox($"Имя пользователя обязательно для заполнения.");
                return false;
            }
            if (input.Length < minSymbolsCount)
            {
                ShowValidationMessageBox($"Имя пользователя слишком короткое. Оно должно быть размером в {minSymbolsCount}-{maxSymbolsCount} символов.");
                return false;
            }
            if (input.Length > maxSymbolsCount)
            {
                ShowValidationMessageBox($"Имя пользователя слишком длинное. Оно должно быть размером в {minSymbolsCount}-{maxSymbolsCount} символов.");
                return false;
            }
            return true;
        }
        public static bool ValidatePassword(string input)
        {
            if (input == null || input.Equals(""))
            {
                ShowValidationMessageBox("Поле \"Пароль\" обязательное для заполнения.");
                return false;
            }

            return true;
        }

        public static bool ValidatePassword(string input, int minSymbolsCount, int maxSymbolsCount)
        {
            if (input == null || input.Equals(""))
            {
                ShowValidationMessageBox("Поле \"Пароль\" обязательное для заполнения.");
                return false;
            }
            if (input.Length < minSymbolsCount)
            {
                ShowValidationMessageBox($"Длинна пароля слишком мала. Пароль должен содержать {minSymbolsCount}-{maxSymbolsCount} символов.");
                return false;
            }
            if (input.Length > maxSymbolsCount)
            {
                ShowValidationMessageBox($"Длинна пароля слишком велика. Пароль должен содержать {minSymbolsCount}-{maxSymbolsCount} символов.");
                return false;
            }

            return true;
        }

        public static bool ValidateRepeatedPassword(string password, string repeatedPassword)
        {
            if (repeatedPassword == null || repeatedPassword.Equals("") || !repeatedPassword.Equals(password))
            {
                ShowValidationMessageBox("Введенные пароли не совпадают.");
                return false;
            }
            return true;
        }







        public static bool CustomValidateField(bool check, string message)
        {
            if (!check)
            {
                ShowValidationMessageBox(message);
                return false;
            }
            return true;
        }
    }
}
