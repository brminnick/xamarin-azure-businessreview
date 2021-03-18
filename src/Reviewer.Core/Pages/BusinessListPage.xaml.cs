using System;
using Reviewer.SharedModels;
using Xamarin.Forms;

namespace Reviewer.Core
{
    public partial class BusinessListPage : ContentPage
    {
        readonly BusinessListViewModel viewModel;

        public BusinessListPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new BusinessListViewModel();

            allBusList.ItemTapped += (sender, args) => allBusList.SelectedItem = null;
            allBusList.ItemSelected += listItemSelected;
            addNewReview.Clicked += HandleAddNewClicked;

            viewModel.Title = "Businesses";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await viewModel.RefreshCommand.ExecuteAsync();

            if (!viewModel.IsLoggedIn)
                await viewModel.CheckLoginStatus();
        }

        async void HandleAddNewClicked(object sender, EventArgs eventArgs)
        {
            var editPage = new NavigationPage(new EditBusinessPage());

            await Navigation.PushModalAsync(editPage);
        }

        protected async void listItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var business = args.SelectedItem as Business;

            if (business == null)
                return;

            await Navigation.PushAsync(new BusinessReviewsPage(business));
        }
    }
}
