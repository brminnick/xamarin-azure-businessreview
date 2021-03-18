using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Reviewer.SharedModels
{
    public class Business : ObservableObject
    {
        string id = string.Empty;
        string name = string.Empty;
        string phone = string.Empty;
        List<Review> recentReviews = Enumerable.Empty<Review>().ToList();

        Address address = new Address { Id = Guid.NewGuid().ToString() };

        [JsonIgnore()]
        public string FirstInitial => Name.Substring(0, 1);

        [JsonProperty("id")]
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        [JsonProperty("name")]
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        [JsonProperty("address")]
        public Address Address
        {
            get => address;
            set => SetProperty(ref address, value);
        }

        [JsonProperty("phone")]
        public string Phone
        {
            get => phone;
            set => SetProperty(ref phone, value);
        }

        [JsonProperty("recentReviews")]
        public List<Review> RecentReviews
        {
            get => recentReviews;
            set => SetProperty(ref recentReviews, value);
        }
    }
}
