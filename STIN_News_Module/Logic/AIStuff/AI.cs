using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            Console.WriteLine(text);
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try { 
                var response = await client.PostAsync(apiUrl, new StringContent(text));
                var result = await response.Content.ReadAsStringAsync();

                var resultJson = JsonConvert.DeserializeObject<List<List<Sentiment>>>(result);

                double positive = 0;
                double negative = 0;
                //Read all labels from resultJson
                for (int i = 0; i < resultJson.Count; i++)
                {
                    for (int j = 0; j < resultJson[i].Count; j++)
                    {
                        
                        if (resultJson[i][j].label.ToLower() == "positive")
                        {
                            positive = resultJson[i][j].score;
                        }
                        else if (resultJson[i][j].label.ToLower() == "negative")
                        {
                            negative = resultJson[i][j].score;
                        }
                    }
                }

                if (positive > negative)
                {
                    Console.WriteLine("Sentiment: +1");
                    return 1 ;
                }
                else
                {
                    Console.WriteLine("Sentiment: -1");
                    return 0;
                }



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;
        }

    }
}
