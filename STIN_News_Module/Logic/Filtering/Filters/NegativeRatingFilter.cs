using STIN_News_Module.Logic.JsonModel;
using STIN_News_Module.Logic.Logging;

namespace STIN_News_Module.Logic.Filtering.Filters
{
    public class NegativeRatingFilter : FilterBase
    {
        public override List<DataModel> Execute(List<DataModel> data)
        {
            LoggingService.AddLog("Executing NegativeRatingFilter");
            List<DataModel> returnData  = new List<DataModel>();
                foreach (var item in data)
                {
                    if (item.Rating > 0)
                    {
                        returnData.Add(item);
                }
                }
                return returnData;
        }
    }
}
