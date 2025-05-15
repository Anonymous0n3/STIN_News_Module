using NewsAPI.Models;
using STIN_News_Module.Logic.AIStuff;
using STIN_News_Module.Logic.Filtering;
using STIN_News_Module.Logic.JsonModel;
using STIN_News_Module.Logic.Logging;
using STIN_News_Module.Logic.News;
using System.Net.Http.Json;
using System.Text.Json;

namespace STIN_News_Module.Logic
{
    public class Utils
    {
        private readonly int MAX_ARTICLES;
        private readonly INewsGetting _news;
        private readonly IAI _ai;
        private readonly IFilterManager _filterManager;
        private readonly IJSONLogic _jsonLogic;
        private readonly HttpClient _httpClient;

        // ✅ Konstruktor pro Dependency Injection
        public Utils(INewsGetting news, IAI ai, IFilterManager filter, IJSONLogic jsonLogic, HttpClient httpClient)
        {
            _news = news;
            _ai = ai;
            _filterManager = filter;
            _jsonLogic = jsonLogic;
            _httpClient = httpClient;

            // 🟢 Bezpečné načtení s fallbackem
            if (!int.TryParse(Environment.GetEnvironmentVariable("MAX_ARTICLES"), out MAX_ARTICLES))
            {
                MAX_ARTICLES = 10;
            }

            Console.WriteLine("Max articles: " + MAX_ARTICLES);
        }

        // ✅ Konstruktor pro starý způsob (Singletony)
        public Utils() : this(
            News_Getting.Instance,
            AI.Instance,
            FilterManager.Instance,
            JSONLogic.Instance,
            new HttpClient())
        { }

        public async Task<List<DataModel>> doAllLogic(List<DataModel> data, int daysBehind)
        {
            LoggingService.AddLog("Doing all logic with " + data.Count + " items");

            foreach (var item in data)
            {
                double rating = item.Rating;
                List<Article> articles = _news.returnNews(item.Name, daysBehind);
                item.setarticleNum(articles?.Count ?? 0);

                if (articles == null || articles.Count == 0)
                    continue;

                if (articles.Count > MAX_ARTICLES)
                {
                    articles.RemoveRange(10, articles.Count - 10);
                }

                if (Environment.GetEnvironmentVariable("DEVELOPMENT_MODE") == "False")
                {
                    LoggingService.AddLog("Rating " + item.Name + " with " + articles.Count + " articles");
                    foreach (var article in articles)
                    {
                        rating += await _ai.GetClasification(article.Description);
                    }
                }

                LoggingService.AddLog("Rating: " + rating);
                item.Rating = LimitToRange((int)Math.Round(rating), -10, 10);
            }

            List<DataModel> filteredData = _filterManager.ExecuteAllFilters(data);
            string? burzaBaseUrl = Environment.GetEnvironmentVariable("BURZA_URL");

            if (string.IsNullOrWhiteSpace(burzaBaseUrl))
            {
                LoggingService.AddLog("BURZA_URL environment variable is not set.");
                return sell(filteredData);
            }

            try
            {
                string fullUrl = $"{burzaBaseUrl.TrimEnd('/')}/salestock";
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(fullUrl, filteredData);
                var json = await response.Content.ReadAsStringAsync();
                List<DataModel>? result = _jsonLogic.deserializeJSON(json);

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
            if (value < inclusiveMinimum) return inclusiveMinimum;
            if (value > inclusiveMaximum) return inclusiveMaximum;
            return value;
        }

        public async Task<List<DataModel>> GetFromBurzaAsync()
        {
            string? burzaBaseUrl = Environment.GetEnvironmentVariable("BURZA_URL");

            if (string.IsNullOrWhiteSpace(burzaBaseUrl))
                throw new InvalidOperationException("BURZA_URL environment variable is not set.");

            string fullUrl = $"{burzaBaseUrl.TrimEnd('/')}/liststock";

            HttpResponseMessage response = await _httpClient.PostAsync(fullUrl, null);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Request to Burza failed with status code {response.StatusCode}");

            string json = await response.Content.ReadAsStringAsync();
            List<DataModel>? result = _jsonLogic.deserializeJSON(json);

            return result ?? new List<DataModel>();
        }
    }
}
