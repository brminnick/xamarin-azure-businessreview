using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Reviewer.SharedModels
{
    public class Review : ObservableObject
    {
        string id = string.Empty;
        string businessId = string.Empty;
        string businessName = string.Empty;
        string author = string.Empty;
        string authorId = string.Empty;
        string reviewText = string.Empty;

        DateTime date = DateTime.Now;

        List<string> photos = Enumerable.Empty<string>().ToList();
        List<Video> videos = Enumerable.Empty<Video>().ToList();

        [JsonProperty("id")]
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        [JsonProperty("businessId")]
        public string BusinessId
        {
            get => businessId;
            set => SetProperty(ref businessId, value);
        }

        [JsonProperty("businessName")]
        public string BusinessName
        {
            get => businessName;
            set => SetProperty(ref businessName, value);
        }

        [JsonProperty("author")]
        public string Author
        {
            get => author;
            set => SetProperty(ref author, value);
        }

        [JsonProperty("authorId")]
        public string AuthorId
        {
            get => authorId;
            set => SetProperty(ref authorId, value);
        }

        [JsonProperty("reviewText")]
        public string ReviewText
        {
            get => reviewText;
            set => SetProperty(ref reviewText, value);
        }

        [JsonProperty("date")]
        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }

        [JsonProperty("photos")]
        public List<string> Photos
        {
            get => photos;
            set => SetProperty(ref photos, value);
        }

        public List<Video> Videos
        {
            get => videos;
            set => SetProperty(ref videos, value);
        }

        int rating = 1;
        [JsonProperty("rating")]
        public int Rating
        {
            get => rating;
            set => SetProperty(ref rating, value);
        }
    }
}

