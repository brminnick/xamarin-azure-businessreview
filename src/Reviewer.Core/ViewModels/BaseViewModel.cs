using Reviewer.SharedModels;

namespace Reviewer.Core
{
    class BaseViewModel : ObservableObject
    {
        bool isBusy;
        string title = string.Empty;

        public bool IsNotBusy => !IsBusy;

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value, () => OnPropertyChanged(nameof(IsNotBusy)));
        }
    }
}
