using STIN_News_Module.Logic.JsonModel;
using STIN_News_Module.Logic.Logging;

namespace STIN_News_Module.Logic.Filtering.Filters
{
    public class NumOfArticles : FilterBase
    {
        public override List<DataModel> Execute(List<DataModel> data)
        {
            string MIN_ARTICLE_NUM = Environment.GetEnvironmentVariable("MIN_ARTICLE_NUM");
           int minArticleNum = string.IsNullOrWhiteSpace(MIN_ARTICLE_NUM)
        ? 10
        : int.Parse(MIN_ARTICLE_NUM);
            LoggingService.AddLog("Executing NumOfArticles filter");
            List<DataModel> returnData = new List<DataModel>();
            for (int i = data.Count - 1; i >= 0; i--)
            {
                Console.WriteLine("Num of articles: " + data[i].getarticleNum());
                if (data[i].getarticleNum() >= minArticleNum)
                {
                    returnData.Add(data[i]);
                }
            }
            return returnData;
        }
    }
}
