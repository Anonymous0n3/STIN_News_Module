using NewsAPI.Models;
using STIN_News_Module.Logic.AIStuff;
using STIN_News_Module.Logic.Filtering;
using STIN_News_Module.Logic.JsonModel;
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



        public List<DataModel> doAllLogic(List<DataModel> data, int daysBehind)
        {
            foreach (var item in data)
            {
                double rating = item.Rating;
                List<Article> articles = News_Getting.Instance.returnNews(item.Name, daysBehind);
                item.setarticleNum(articles.Count);

                //Max articles per item is 10
                if (articles.Count > MAX_ARTICLES)
                {
                    articles.RemoveRange(10, articles.Count - 10);
                }
                foreach (var article in articles)
                {
                    rating += AI.Instance.GetClasification(article.Description).Result;
                }
                Console.WriteLine("Rating: " + rating);
                rating = (rating / MAX_ARTICLES)*10;

                item.Rating = LimitToRange((int)Math.Round(rating), -10, 10);
            }

            List<DataModel> filteredData = FilterManager.Instance.ExecuteAllFilters(data);
            return filteredData;
        }

        public List<DataModel> saleRating(List<DataModel> data, int minRating)
        {
            foreach (var item in data)
            {
                if (item.Rating >= minRating)
                {
                    item.Sale = 1;
                }
                else
                {
                    item.Sale = 0;
                }
            }
            return data;
        }

        public int LimitToRange(int value, int inclusiveMinimum, int inclusiveMaximum)
        {
            if (value < inclusiveMinimum) { return inclusiveMinimum; }
            if (value > inclusiveMaximum) { return inclusiveMaximum; }
            return value;
        }
    }
}
