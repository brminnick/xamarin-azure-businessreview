using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Reviewer.Services;
using Reviewer.SharedModels;

namespace Reviewer.Core
{
    public class MockAPIService : IAPIService
    {
        readonly static HttpClient webClient = new();

        public async Task<List<Review>> GetReviewsForBusiness(string businessId)
        {
            var reviewJson = await webClient.GetStringAsync($"{APIKeys.WebAPIUrl}review/business/{businessId}");

            return JsonConvert.DeserializeObject<List<Review>>(reviewJson);
        }

        public Task InsertReview(Review review, string token) => Task.CompletedTask;

        public Task UpdateReview(Review review, string token) => Task.CompletedTask;

        public Task<List<Review>> GetReviewsForAuthor(string authorId, string token) => Task.FromResult(new List<Review>());

        public Task<string> GetContainerWriteSasToken() => Task.FromResult("");

        public Task WritePhotoInfoToQueue(string reviewId, string photoUrl) => Task.CompletedTask;
    }
}
