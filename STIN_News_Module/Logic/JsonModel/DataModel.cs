using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace STIN_News_Module.Logic.JsonModel
{
    public class DataModel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("date")]
        public int Date { get; set; }

        [JsonPropertyName("rating")]
        public int Rating { get; set; }

        [JsonPropertyName("sale")]
        public int Sale { get; set; }

        private int numOfArticles = 0;

        public int getarticleNum() { return numOfArticles; }
        public void setarticleNum(int value) { numOfArticles = value; }
    }
}
