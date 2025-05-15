using NewsAPI.Constants;
using NewsAPI.Models;
using NewsAPI;
using STIN_News_Module.Logic.Logging;
using STIN_News_Module.Logic.News;

namespace STIN_News_Module{
    public class News_Getting : INewsGetting
    {
        private readonly INewsApiClient _newsApiClient;

        private static readonly Lazy<News_Getting> _instance = new Lazy<News_Getting>(() =>
            new News_Getting(new NewsApiClientWrapper(Environment.GetEnvironmentVariable("NEWS_API_KEY")))
        );

        public static News_Getting Instance => _instance.Value;

        // Konstruktor pro injekci
        public News_Getting(INewsApiClient client)
        {
            _newsApiClient = client;
        }

        public List<Article> returnNews(string q, int days)
        {
            LoggingService.AddLog("Getting news for " + q + " for " + days + " days");

            var request = new EverythingRequest
            {
                Q = q,
                SortBy = NewsAPI.Constants.SortBys.Popularity,
                Language = NewsAPI.Constants.Languages.EN,
                From = DateTime.Now.AddDays(-days),
            };

            var articlesResponse = _newsApiClient.GetEverything(request);

            if (articlesResponse.Status == NewsAPI.Constants.Statuses.Ok)
            {
                LoggingService.AddLog("Returning " + articlesResponse.TotalResults + " articles");
                return articlesResponse.Articles;
            }

            return null;
        }
    }
}
