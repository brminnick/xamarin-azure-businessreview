using System;
using Reviewer.SharedModels;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;

namespace Reviewer.Core
{
    class ReviewDetailViewModel : BaseViewModel
    {
        bool editable;

        Review review;
        Business business;

        public ReviewDetailViewModel(Review review, Business business)
        {
            this.review = review;
            this.business = business;

            Title = "Details";

            Initialize().SafeFireAndForget();
        }

        public Review Review
        {
            get => review;
            set => SetProperty(ref review, value);
        }

        public Business Business
        {
            get => business;
            set => SetProperty(ref business, value);
        }

        public bool Editable
        {
            get => editable;
            set => SetProperty(ref editable, value);
        }

        async Task Initialize()
        {
            var cachedResult = await DependencyService.Get<IIdentityService>().GetCachedSignInToken();

            if (Review?.AuthorId is not null && cachedResult?.UniqueId == Review.AuthorId)
                Editable = true;
            else
                Editable = false;
        }
    }
}
