using Newtonsoft.Json;

namespace Reviewer.SharedModels
{
    public class Video : ObservableObject
    {
        string hlsUrl = string.Empty;
        string smoothStreamUrl = string.Empty;
        string mpegDashUrl = string.Empty;
        string thumbnailUrl = string.Empty;

        [JsonProperty("hlsUrl")]
        public string HLSUrl
        {
            get => hlsUrl;
            set => SetProperty(ref hlsUrl, value);
        }

        [JsonProperty("smoothStreamUrl")]
        public string SmoothStreamUrl
        {
            get => smoothStreamUrl;
            set => SetProperty(ref smoothStreamUrl, value);
        }

        [JsonProperty("mpegDashUrl")]
        public string MPEGDashUrl
        {
            get => mpegDashUrl;
            set => SetProperty(ref mpegDashUrl, value);
        }

        [JsonProperty("thumbnailUrl")]
        public string ThumbnailUrl
        {
            get => thumbnailUrl;
            set => SetProperty(ref thumbnailUrl, value);
        }
    }
}
