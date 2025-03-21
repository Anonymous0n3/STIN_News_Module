using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace STIN_News_Module.Logic.AIStuff
{
    public class AI
    {
        private readonly string apiKey;
        private readonly string apiUrl;

        public AI()
        {
            // Constructor
            apiKey = Environment.GetEnvironmentVariable("AI_API_KEY");
            apiUrl = Environment.GetEnvironmentVariable("AI_API_URL");
        }

        public async Task<int> GetClasification(string text)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            try { 
                var response = await client.PostAsync(apiUrl, new StringContent(text));
                var result = await response.Content.ReadAsStringAsync();

                var resultJson = JsonConvert.DeserializeObject<List<List<Sentiment>>>(result);

                double sentimentValue = 0;
                //Read all labels from resultJson
                for (int i = 0; i < resultJson.Count; i++)
                {
                    for (int j = 0; j < resultJson[i].Count; j++)
                    {
                        if (resultJson[i][j].label.ToLower() == "positive")
                        {
                            sentimentValue += resultJson[i][j].score;
                        }
                        else if (resultJson[i][j].label.ToLower() == "negative")
                        {
                            sentimentValue -= resultJson[i][j].score;
                        }
                    }
                }

                sentimentValue *= 10;

                return (int)Math.Round(sentimentValue);

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return 0;
        }

    }
}
