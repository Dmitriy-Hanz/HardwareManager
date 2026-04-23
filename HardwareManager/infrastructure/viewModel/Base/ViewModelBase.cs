using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HardwareManager.infrastructure.viewModel.Base
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) { return false; }
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }

        protected virtual bool Update<T>(ref T field, string PropertyName)
        {
            T temp = field;
            field = default;
            OnPropertyChanged(PropertyName);
            field = temp;
            OnPropertyChanged(PropertyName);
            return true;
        }
    }
}
