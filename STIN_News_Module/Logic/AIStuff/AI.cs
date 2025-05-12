using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STIN_News_Module.Logic.Logging;
using System.Text;

namespace STIN_News_Module.Logic.AIStuff
{
    public class AI
    {
        private readonly string apiKey;
        private readonly string apiUrl;
        private static readonly Lazy<AI> _instance = new Lazy<AI>(() => new AI());
        public static AI Instance => _instance.Value;

        private AI()
        {
            // Constructor
            apiKey = Environment.GetEnvironmentVariable("AI_API_KEY");
            apiUrl = Environment.GetEnvironmentVariable("AI_API_URL");
        }

        public async Task<int> GetClasification(string text)
        {
            LoggingService.AddLog("Getting AI classification for: " + text);
            Console.WriteLine(text);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var payload = new { inputs = text };
            var json = JsonConvert.SerializeObject(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(apiUrl, content);
                var result = await response.Content.ReadAsStringAsync();

                var resultJson = JsonConvert.DeserializeObject<List<Sentiment>>(result);

                double positive = 0;
                double negative = 0;

                foreach (var sentiment in resultJson)
                {
                    if (sentiment.label.ToLower() == "positive")
                        positive = sentiment.score;
                    else if (sentiment.label.ToLower() == "negative")
                        negative = sentiment.score;
                }

                if (positive > negative)
                {
                    Console.WriteLine("Sentiment: +1");
                    return 1;
                }
                else
                {
                    Console.WriteLine("Sentiment: -1");
                    return 0;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }

            return 0;
        }


    }
}
