using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using Microsoft.Identity.Client;
using Reviewer.SharedModels;
using Xamarin.Forms;

namespace Reviewer.Core
{
    class AccountViewModel : BaseViewModel
    {
        const string notLoggedInInfo = "Sign in to unlock the wonderful world of reviews!";
        const string loggedInInfo = "Hiya {user}! Here are your reviews so far!";

        readonly AsyncAwaitBestPractices.WeakEventManager weakEventManager = new();
        readonly IIdentityService identityService = DependencyService.Get<IIdentityService>();

        bool loggedIn = false;
        bool notLoggedIn = true;

        string info = notLoggedInInfo;

        List<Review> reviews = Enumerable.Empty<Review>().ToList();

        AuthenticationResult? authResult;

        public AccountViewModel()
        {
            SignInCommand = new AsyncCommand(ExecuteSignInCommand);
            RefreshCommand = new AsyncCommand(ExecuteRefreshCommand);
            SignOutCommand = new Command(ExecuteSignOutCommand);

            CheckLoginStatus().SafeFireAndForget();
        }

        public event EventHandler SuccessfulSignIn
        {
            add => weakEventManager.AddEventHandler(value);
            remove => weakEventManager.RemoveEventHandler(value);
        }

        public event EventHandler UnsuccessfulSignIn
        {
            add => weakEventManager.AddEventHandler(value);
            remove => weakEventManager.RemoveEventHandler(value);
        }

        public ICommand SignInCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand SignOutCommand { get; }

        public bool NotLoggedIn
        {
            get => notLoggedIn;
            set => SetProperty(ref notLoggedIn, value);
        }

        public bool LoggedIn
        {
            get => loggedIn;
            set
            {
                SetProperty(ref loggedIn, value);
                NotLoggedIn = !LoggedIn;
            }
        }

        public List<Review> Reviews
        {
            get => reviews;
            set => SetProperty(ref reviews, value);
        }

        public string Info
        {
            get => info;
            set => SetProperty(ref info, value);
        }

        void ExecuteSignOutCommand()
        {
            if (IsBusy)
                return;

            if (NotLoggedIn)
                return;

            try
            {
                IsBusy = true;

                identityService.Logout();

                LoggedIn = false;
                Info = notLoggedInInfo;
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task ExecuteSignInCommand()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                authResult = await identityService.Login();
            }
            finally
            {
                IsBusy = false;
            }

            if (authResult?.User == null)
            {
                LoggedIn = false;
                Info = notLoggedInInfo;

                OnUnsuccessfulSignIn();
            }
            else
            {
                LoggedIn = true;
                Info = loggedInInfo.Replace("{user}", identityService.DisplayName);

                await ExecuteRefreshCommand();

                OnSuccessfulSignIn();
            }
        }

        async Task ExecuteRefreshCommand()
        {
            if (IsBusy)
                return;

            if (NotLoggedIn)
                return;

            try
            {
                IsBusy = true;

                var apiService = DependencyService.Get<IAPIService>();
                Reviews = await apiService.GetReviewsForAuthor(authResult.UniqueId, authResult.AccessToken);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task CheckLoginStatus()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                authResult = await identityService.GetCachedSignInToken();

                if (authResult?.User != null)
                {
                    LoggedIn = true;

                    Title = identityService.DisplayName;
                    Info = loggedInInfo.Replace("{user}", identityService.DisplayName);
                }
                else
                {
                    Title = "Account";
                    LoggedIn = false;
                }
            }
            finally
            {
                IsBusy = false;
            }
        }

        void OnSuccessfulSignIn() => weakEventManager.RaiseEvent(this, EventArgs.Empty, nameof(SuccessfulSignIn));
        void OnUnsuccessfulSignIn() => weakEventManager.RaiseEvent(this, EventArgs.Empty, nameof(UnsuccessfulSignIn));
    }
}
