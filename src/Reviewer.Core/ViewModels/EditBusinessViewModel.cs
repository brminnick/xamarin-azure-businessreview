using System;
using System.Windows.Input;
using Reviewer.SharedModels;
using System.Threading.Tasks;
using Xamarin.Forms;
using Reviewer.Services;
using AsyncAwaitBestPractices.MVVM;

namespace Reviewer.Core
{
    class EditBusinessViewModel : BaseViewModel
    {
        readonly AsyncAwaitBestPractices.WeakEventManager weakEventManager = new();

        bool isNew;
        Business business;

        public EditBusinessViewModel(Business? theBusiness = null)
        {
            if (theBusiness is null)
            {
                IsNew = true;
                business = new Business() { Id = Guid.NewGuid().ToString() };
                Business.Address = new Address() { Id = Guid.NewGuid().ToString() };
            }
            else
            {
                business = theBusiness;
                IsNew = false;
            }

            SaveCommand = new AsyncCommand(ExecuteSaveCommand);
        }

        public event EventHandler SaveComplete
        {
            add => weakEventManager.AddEventHandler(value);
            remove => weakEventManager.RemoveEventHandler(value);
        }

        public bool IsNotNew => !IsNew;

        public ICommand SaveCommand { get; }

        public Business Business
        {
            get => business;
            set => SetProperty(ref business, value);
        }

        public bool IsNew
        {
            get => isNew;
            set => SetProperty(ref isNew, value, () => OnPropertyChanged(nameof(IsNotNew)));
        }

        async Task ExecuteSaveCommand()
        {
            if (IsBusy)
                return;

            try
            {
                var dataService = DependencyService.Get<IDataService>();

                if (IsNew)
                    await dataService.InsertBusiness(Business);
                else
                    await dataService.UpdateBusiness(Business);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"*** ERROR: {ex}");
            }
            finally
            {
                IsBusy = false;
            }

            OnSaveComplete();
        }

        void OnSaveComplete() => weakEventManager.RaiseEvent(this, EventArgs.Empty, nameof(SaveComplete));
    }
}
