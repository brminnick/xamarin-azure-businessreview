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
            allBusList.ItemSelected += ListItemSelected;
            addNewReview.Clicked += HandleAddNewClicked;

            viewModel.Title = "Businesses";
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            if (!viewModel.IsLoggedIn)
                await viewModel.CheckLoginStatus();

            await viewModel.RefreshCommand.ExecuteAsync();
        }

        async void HandleAddNewClicked(object sender, EventArgs eventArgs)
        {
            var editPage = new NavigationPage(new EditBusinessPage());

            await Navigation.PushModalAsync(editPage);
        }

        protected async void ListItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            if (args.SelectedItem is not Business business)
                return;

            await Navigation.PushAsync(new BusinessReviewsPage(business));
        }
    }
}
