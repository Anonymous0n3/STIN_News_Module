using NewsAPI.Models;
using STIN_News_Module.Logic.AIStuff;
using STIN_News_Module.Logic.Filtering;
using STIN_News_Module.Logic.JsonModel;
using STIN_News_Module.Logic.Logging;
using STIN_News_Module.Logic.News;

namespace STIN_News_Module.Logic
{
    public class Utils
    {
        private readonly int MAX_ARTICLES;

        public Utils() 
        {
            int.TryParse(Environment.GetEnvironmentVariable("MAX_ARTICLES"), out this.MAX_ARTICLES);
            Console.WriteLine("Max articles: " + MAX_ARTICLES);
        }


        // Využívá singletony a nemůže být proto testován
        public async Task<List<DataModel>> doAllLogic(List<DataModel> data, int daysBehind)
        {
            LoggingService.AddLog("Doing all logic with " + data.Count + " items");

            foreach (var item in data)
            {
                double rating = item.Rating;
                List<Article> articles = News_Getting.Instance.returnNews(item.Name, daysBehind);
                item.setarticleNum(articles.Count);

                if (articles.Count > MAX_ARTICLES)
                {
                    articles.RemoveRange(10, articles.Count - 10);
                }

                foreach (var article in articles)
                {
                    rating += await AI.Instance.GetClasification(article.Description);
                }

                Console.WriteLine("Rating: " + rating);
                rating = (rating / MAX_ARTICLES) * 10;
                item.Rating = LimitToRange((int)Math.Round(rating), -10, 10);
            }

            List<DataModel> filteredData = FilterManager.Instance.ExecuteAllFilters(data);

            if (Environment.GetEnvironmentVariable("DEVELOPMENT_MODE") == "True")
            {
                LoggingService.AddLog("Development mode is on, skipping sending data to burza");
                return sell(filteredData);
            }
            else
            {
                string? burzaBaseUrl = Environment.GetEnvironmentVariable("BURZA_URL");
                string? port = Environment.GetEnvironmentVariable("PORT");

                if (string.IsNullOrWhiteSpace(burzaBaseUrl) || string.IsNullOrWhiteSpace(port))
                {
                    LoggingService.AddLog("BURZA_URL or PORT environment variable is not set.");
                    return sell(filteredData);
                }

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string fullUrl = $"{burzaBaseUrl.TrimEnd('/')}:{port}/salestock";
                        HttpResponseMessage response = await client.PostAsJsonAsync(fullUrl, filteredData);

                        if (response.IsSuccessStatusCode)
                        {
                            LoggingService.AddLog("Data successfully sent to burza.");
                        }
                        else
                        {
                            LoggingService.AddLog("Failed to send data to burza. Status code: " + response.StatusCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LoggingService.AddLog("Error while sending data to burza: " + ex.Message);
                }
            }
            return sell(filteredData);
        }


        public List<DataModel> sell(List<DataModel> data)
        {
            LoggingService.AddLog("Selling items");
            return data.Where(item => item.Sale != 1).ToList();
        }

        public int LimitToRange(int value, int inclusiveMinimum, int inclusiveMaximum)
        {
            LoggingService.AddLog("Limiting value " + value + " to range [" + inclusiveMinimum + ", " + inclusiveMaximum + "]");
            if (value < inclusiveMinimum) { return inclusiveMinimum; }
            if (value > inclusiveMaximum) { return inclusiveMaximum; }
            return value;
        }
    }
}
