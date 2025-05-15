using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STIN_News_Module.Logic.Logging;
using System.Text;

//Nebude se testovat kvůli singletonu a tomu že to má limit
namespace STIN_News_Module.Logic.AIStuff
{
    public class AI :IAI
    {
        private readonly string apiKey;
        private readonly string apiUrl;
        private readonly HttpClient httpClient;

        // Singleton - nezměněno pro použití v aplikaci
        private static readonly Lazy<AI> _instance = new Lazy<AI>(() => new AI());
        public static AI Instance => _instance.Value;

        // Konstruktor pro produkční použití
        private AI()
        {
            apiKey = Environment.GetEnvironmentVariable("AI_API_KEY");
            apiUrl = Environment.GetEnvironmentVariable("AI_API_URL");
            httpClient = new HttpClient();
        }

        // Konstruktor pro testování
        public AI(HttpClient client, string key, string url)
        {
            httpClient = client;
            apiKey = key;
            apiUrl = url;
        }

        public async Task<int> GetClasification(string text)
        {
            var payload = new { inputs = text };
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            httpClient.DefaultRequestHeaders.Clear();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try
            {
                var response = await httpClient.PostAsync(apiUrl, content);
                var result = await response.Content.ReadAsStringAsync();

                var resultJson = JsonConvert.DeserializeObject<List<List<Sentiment>>>(result);
                var sentiments = resultJson.FirstOrDefault();
                if (sentiments == null || sentiments.Count == 0) return 0;

                double positive = 0, negative = 0;
                foreach (var sentiment in sentiments)
                {
                    if (sentiment.label.ToLower() == "positive")
                        positive = sentiment.score;
                    else if (sentiment.label.ToLower() == "negative")
                        negative = sentiment.score;
                }

                return positive > negative ? 1 : -1;
            }
            catch
            {
                return 0;
            }
        }
    }
}
