using STIN_News_Module.Logic.JsonModel;

namespace STIN_News_Module.Logic.Filtering.Filters
{
    public class NumOfArticles : FilterBase
    {
        public override List<DataModel> Execute(List<DataModel> data)
        {
            List<DataModel> returnData = new List<DataModel>();
            for (int i = data.Count - 1; i >= 0; i--)
            {
                Console.WriteLine("Num of articles: " + data[i].getarticleNum());
                if (data[i].getarticleNum() >= 10)
                {
                    returnData.Add(data[i]);
                }
            }
            return returnData;
        }
    }
}
