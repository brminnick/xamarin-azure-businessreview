using System;
using Reviewer.SharedModels;
using Xamarin.Forms;

namespace Reviewer.Core
{
    public partial class AccountPage : ContentPage
    {
        readonly AccountViewModel viewModel;

        public AccountPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new AccountViewModel();

            viewModel.UnsuccessfulSignIn += UnsuccessfulSignIn;
            authorReviewList.ItemSelected += ListItemSelected;
            authorReviewList.ItemTapped += (sender, e) => authorReviewList.SelectedItem = null;
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
