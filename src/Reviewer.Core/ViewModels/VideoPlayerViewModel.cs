using Reviewer.SharedModels;

namespace Reviewer.Core
{
    class VideoPlayerViewModel : BaseViewModel
    {
        Video video;
        string videoUrl;

        public VideoPlayerViewModel(Video video)
        {
            this.video = video;
            videoUrl = Video.HLSUrl.Replace("http:", "https:");
        }

        public Video Video
        {
            get => video;
            set => SetProperty(ref video, value);
        }

        public string VideoUrl
        {
            get => videoUrl;
            set => SetProperty(ref videoUrl, value);
        }
    }
}
