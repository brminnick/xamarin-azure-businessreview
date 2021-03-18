namespace Reviewer.Core
{
    class PhotoViewerViewModel : BaseViewModel
    {
        string photoUrl;

        public PhotoViewerViewModel(string url) => photoUrl = url;

        public string PhotoUrl
        {
            get => photoUrl;
            set => SetProperty(ref photoUrl, value);
        }
    }
}
