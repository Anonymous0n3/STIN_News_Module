using NewsAPI.Models;
using STIN_News_Module.Logic.JsonModel;
using STIN_News_Module.Logic.News;

namespace STIN_News_Module.Logic
{
    public class Utils
    {
        public Utils() { }

        public List<DataModel> addScore(List<DataModel> data)
        {
            foreach (var item in data)
            {
                
            }
            return data;
        }

        public List<Article> getArticlesForItem(DataModel item)
        {
            return News_Getting.Instance.returnNews(item.Name, 1);
        }
    }
}
