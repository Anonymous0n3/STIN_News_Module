using NewsAPI.Models;
using STIN_News_Module.Logic.AIStuff;
using STIN_News_Module.Logic.Filtering;
using STIN_News_Module.Logic.JsonModel;
using STIN_News_Module.Logic.Logging;
using STIN_News_Module.Logic.News;
using System.Text.Json;

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

                if (Environment.GetEnvironmentVariable("DEVELOPMENT_MODE") == "False") {
                    LoggingService.AddLog("Rating " + item.Name + " with " + articles.Count + " articles");
                    foreach (var article in articles)
                    {
                        rating += await AI.Instance.GetClasification(article.Description);
                    }
                }
                LoggingService.AddLog("Rating: " + rating);
                //rating = (rating / MAX_ARTICLES) * 10;
                item.Rating = LimitToRange((int)Math.Round(rating), -10, 10);
            }

            List<DataModel> filteredData = FilterManager.Instance.ExecuteAllFilters(data);
                string? burzaBaseUrl = Environment.GetEnvironmentVariable("BURZA_URL");

                if (string.IsNullOrWhiteSpace(burzaBaseUrl))
                {
                    LoggingService.AddLog("BURZA_URL environment variable is not set.");
                    return sell(filteredData);
                }

                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        string fullUrl = $"{burzaBaseUrl.TrimEnd('/')}/salestock";
                        HttpResponseMessage response = await client.PostAsJsonAsync(fullUrl, filteredData);
                        var json = await response.Content.ReadAsStringAsync();
                        List<DataModel>? result = JSONLogic.Instance.deserializeJSON(json);
                        

                    if (response.IsSuccessStatusCode)
                        {
                            LoggingService.AddLog("Data successfully sent to burza.");
                            return sell(result);
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

        public async Task<List<DataModel>> GetFromBurzaAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                string? burzaBaseUrl = Environment.GetEnvironmentVariable("BURZA_URL");

                if (string.IsNullOrWhiteSpace(burzaBaseUrl))
                    throw new InvalidOperationException("BURZA_URL environment variable is not set.");

                string fullUrl = $"{burzaBaseUrl.TrimEnd('/')}/liststock";

                HttpResponseMessage response = await client.PostAsync(fullUrl, null);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Request to Burza failed with status code {response.StatusCode}");
                }

                string json = await response.Content.ReadAsStringAsync();

                List<DataModel>? result = JSONLogic.Instance.deserializeJSON(json);

                return result ?? new List<DataModel>();
            }
        }

    }
}
