using System;
using Reviewer.SharedModels;
using Xamarin.Forms;

namespace Reviewer.Core
{
    public partial class ReviewDetailPage : ContentPage
    {
        readonly ReviewDetailViewModel vm;

        public ReviewDetailPage(Review review, Business business) : this()
        {
            BindingContext = vm = new ReviewDetailViewModel(review, business)
            {
                Title = "Review Details"
            };
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public ReviewDetailPage() => InitializeComponent();
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected override void OnAppearing()
        {
            base.OnAppearing();
            videoList.SelectedItemChanged += VideoList_SelectedItemChanged;
            photoList.SelectedItemChanged += PhotoList_SelectedItemChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            videoList.SelectedItemChanged -= VideoList_SelectedItemChanged;
            photoList.SelectedItemChanged -= PhotoList_SelectedItemChanged;
        }

        async void VideoList_SelectedItemChanged(object sender, EventArgs e)
        {
            if (sender is not HorizontalList horizontalList)
                return;

            if (horizontalList.SelectedItem is not Video video)
                return;

            var videoPlayer = new NavigationPage(new VideoPlayerPage(video));

            await Navigation.PushModalAsync(videoPlayer);
        }

        async void PhotoList_SelectedItemChanged(object sender, EventArgs e)
        {
            if (sender is not HorizontalList horizontalList)
                return;

            if (horizontalList.SelectedItem is not string photoUrl)
                return;

            var photoViewer = new NavigationPage(new PhotoViewerPage(photoUrl));

            await Navigation.PushModalAsync(photoViewer);
        }

        async void Handle_EditClicked(object sender, EventArgs e)
        {
            var editPage = new EditReviewPage(vm.Review);

            await Navigation.PushAsync(editPage);
        }
    }
}
