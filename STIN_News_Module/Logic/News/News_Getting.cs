
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using System.Net;

namespace STIN_News_Module.Logic.News
{
    public class News_Getting
    {
        private readonly string news_Api;
        private static readonly Lazy<News_Getting> _instance = new Lazy<News_Getting>(() => new News_Getting());
        public static News_Getting Instance => _instance.Value;

        private News_Getting()
        {
            this.news_Api = Environment.GetEnvironmentVariable("NEWS_API_KEY");
        }

        public List<Article> returnNews(string q,int days)
        {
            var newsApiClient = new NewsApiClient(news_Api);
            var articlesResponse = newsApiClient.GetEverything(new EverythingRequest
            {
                Q = q,
                SortBy = SortBys.Popularity,
                Language = Languages.EN,
                From = DateTime.Now.AddDays(-days),
            });

            if (articlesResponse.Status == Statuses.Ok) 
            {
                Console.WriteLine("Total Articels:" + articlesResponse.TotalResults);
                
                return articlesResponse.Articles;
            }

            return null;
        }
    }
}
