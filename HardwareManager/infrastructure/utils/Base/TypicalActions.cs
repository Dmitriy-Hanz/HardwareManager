using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HardwareManager.infrastructure.utils.Base
{
    class TypicalActions
    {
        public static void GoToWindow<OwnerType, TargetType>(bool hideOwner = false) where OwnerType : Window where TargetType : Window, new()
        {
            OwnerType owner = null;
            TargetType target = null;
            foreach (Window item in Application.Current.Windows)
            {
                if (item is OwnerType)
                {
                    owner = item as OwnerType;
                }
                if (item is TargetType)
                {
                    target = item as TargetType;
                }
            }
            if (target == null)
            {
                target = new TargetType();
            }
            target.Show();
            owner.Hide();
            target.Owner = null;
            owner.Owner = target;
            if (!hideOwner)
            {
                owner.Close();
            }
        }

        public static void GoToDialogWindow<TargetType>() where TargetType : Window, new()
        {
            TargetType win = new TargetType();
            win.ShowDialog();
        }
        public static void CloseWindow<TargetType>() where TargetType : Window
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item is TargetType)
                {
                    item.Close();
                }
            }
        }

        public static void HideWindow<TargetType>() where TargetType : Window
        {
            foreach (Window item in Application.Current.Windows)
            {
                if (item is TargetType)
                {
                    item.Hide();
                }
            }
        }


        public static MessageBoxResult ShowAlertAnswerableMessage(string message)
        {
            return MessageBox.Show(message, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No);
        }
        public static MessageBoxResult ShowQuestionAnswerableMessage(string message)
        {
            return MessageBox.Show(message, "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
        }
        public static MessageBoxResult ShowInformationalAnswerableMessage(string message)
        {
            return MessageBox.Show(message, "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Information, MessageBoxResult.No);
        }
        public static void ShowInformationalMessage(string message)
        {
            MessageBox.Show(message, "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        public static void CloseApplication()
        {
            Application.Current.Shutdown();
        }
        public static void ShutdownWithError(Exception ex)
        {
            MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
            Application.Current.Shutdown();
        }
    }
}
