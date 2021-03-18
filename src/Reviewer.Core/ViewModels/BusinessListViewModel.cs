using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using Reviewer.Services;
using Reviewer.SharedModels;
using Xamarin.Forms;

namespace Reviewer.Core
{
    class BusinessListViewModel : BaseViewModel
    {
        readonly IDataService dataService = DependencyService.Get<IDataService>();

        bool isLoggedIn;
        List<Business> businesses = Enumerable.Empty<Business>().ToList();

        public BusinessListViewModel()
        {
            RefreshCommand = new AsyncCommand(ExecuteRefreshCommand);
        }

        public IAsyncCommand RefreshCommand { get; }

        public List<Business> Businesses
        {
            get => businesses;
            set => SetProperty(ref businesses, value);
        }

        public bool IsLoggedIn
        {
            get => isLoggedIn;
            set => SetProperty(ref isLoggedIn, value);
        }

        public async Task CheckLoginStatus()
        {
            var idService = DependencyService.Get<IIdentityService>();

            var authResult = await idService.GetCachedSignInToken();

            IsLoggedIn = authResult?.Account != null;
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                Businesses = await dataService.GetBusinesses();
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
