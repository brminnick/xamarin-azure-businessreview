using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AsyncAwaitBestPractices;

namespace Reviewer.SharedModels
{
    public class ObservableObject : INotifyPropertyChanged
    {
        readonly WeakEventManager propertyChangedEventManager = new();

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => propertyChangedEventManager.AddEventHandler(value);
            remove => propertyChangedEventManager.RemoveEventHandler(value);
        }

        public void SetProperty<T>(ref T backingStore, T value, Action? onChanged = null, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged(string propertyName = "") =>
            propertyChangedEventManager?.RaiseEvent(this, new PropertyChangedEventArgs(propertyName), nameof(INotifyPropertyChanged.PropertyChanged));
    }
}
