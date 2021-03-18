using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using Reviewer.SharedModels;
using Xamarin.Forms;

namespace Reviewer.Core
{
    class BusinessReviewViewModel : BaseViewModel
    {
        readonly IAPIService apiService = DependencyService.Get<IAPIService>();

        Business business;
        bool isLoggedIn = false;
        List<Review> reviews = Enumerable.Empty<Review>().ToList();

        public BusinessReviewViewModel(Business business)
        {
            this.business = business;

            RefreshCommand = new Command(async () => await ExecuteRefreshCommand());

            Title = Business.Name;

            CheckLoginStatus().SafeFireAndForget();
        }

        public ICommand RefreshCommand { get; }

        public Business Business
        {
            get => business;
            set => SetProperty(ref business, value);
        }

        public List<Review> Reviews
        {
            get => reviews;
            set => SetProperty(ref reviews, value);
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

            IsLoggedIn = authResult?.User != null;
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                Reviews = await apiService.GetReviewsForBusiness(Business.Id);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
