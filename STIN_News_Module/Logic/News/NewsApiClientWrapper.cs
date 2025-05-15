using NewsAPI;
using NewsAPI.Models;

namespace STIN_News_Module.Logic.News
{

    public class NewsApiClientWrapper : INewsApiClient
    {
        private readonly NewsApiClient _client;

        public NewsApiClientWrapper(string apiKey)
        {
            _client = new NewsApiClient(apiKey);
        }

        public ArticlesResult GetEverything(EverythingRequest request)
        {
            return _client.GetEverything(request);
        }
    }

}
