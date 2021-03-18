using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Reviewer.SharedModels
{
    public class Address : ObservableObject
    {
        string id = string.Empty;
        string line1 = string.Empty;
        string city = string.Empty;
        string state = string.Empty;
        string zip = string.Empty;

        [JsonProperty("id")]
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        [JsonProperty("line1")]
        public string Line1
        {
            get => line1;
            set => SetProperty(ref line1, value);
        }

        [JsonProperty("city")]
        public string City
        {
            get => city;
            set => SetProperty(ref city, value);
        }

        [JsonProperty("state")]
        public string State
        {
            get => state;
            set => SetProperty(ref state, value);
        }

        [JsonProperty("zip")]
        public string Zip
        {
            get => zip;
            set => SetProperty(ref zip, value);
        }
    }
}
