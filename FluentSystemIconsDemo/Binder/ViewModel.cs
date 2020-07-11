using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FluentSystemIconsDemo.Binder
{
    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SetValue<T>(ref T property, T value, Action changedAction, [CallerMemberName] string propertyName = "")
        {
            if (Equals(property, value))
                return;
            property = value;
            OnChanged(propertyName);
            changedAction?.Invoke();
        }

        public void SetValue<T>(ref T property, T value, [CallerMemberName] string propertyName = "")
        {
            SetValue(ref property, value, null, propertyName);
        }
    }
}