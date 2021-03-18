using System;
using Reviewer.SharedModels;
using Xamarin.Forms;

namespace Reviewer.Core
{
    public partial class BusinessReviewsPage : ContentPage
    {
        readonly BusinessReviewViewModel viewModel;

        public BusinessReviewsPage(Business business)
        {
            InitializeComponent();

            BindingContext = viewModel = new BusinessReviewViewModel(business);

            reviewList.ItemTapped += (sender, args) => reviewList.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            reviewList.ItemSelected += ReviewList_ItemSelected;
            addNewReview.Clicked += AddNewReview_Clicked;

            viewModel.RefreshCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            reviewList.ItemSelected -= ReviewList_ItemSelected;
            addNewReview.Clicked -= AddNewReview_Clicked;
        }

        async void AddNewReview_Clicked(object sender, EventArgs e)
        {
            var editPage = new EditReviewPage(viewModel.Business.Id, viewModel.Business.Name);

            await Navigation.PushModalAsync(new NavigationPage(editPage));
        }

        async void ReviewList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var review = e.SelectedItem as Review;
            if (review == null)
                return;

            await Navigation.PushAsync(new ReviewDetailPage(review, viewModel.Business));
        }
    }
}
