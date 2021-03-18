using System;
using Reviewer.SharedModels;
using Xamarin.Forms;

namespace Reviewer.Core
{
    public partial class AccountPage : ContentPage
    {
        readonly AccountViewModel vm;

        public AccountPage()
        {
            InitializeComponent();

            vm = new AccountViewModel();

            BindingContext = vm;

            authorReviewList.ItemTapped += (sender, e) => authorReviewList.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            vm.UnsuccessfulSignIn += UnsuccessfulSignIn;
            authorReviewList.ItemSelected += ListItemSelected;

            vm.RefreshCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            vm.UnsuccessfulSignIn -= UnsuccessfulSignIn;
            authorReviewList.ItemSelected -= ListItemSelected;
        }

        protected async void ListItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (args.SelectedItem is not Review review)
                return;

            await Navigation.PushAsync(new EditReviewPage(review));
        }

        async void UnsuccessfulSignIn(object sender, EventArgs e) => await DisplayAlert("Error", "Couldn't log in, try again!", "OK");
    }
}
