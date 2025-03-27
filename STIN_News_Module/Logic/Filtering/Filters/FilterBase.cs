using NewsAPI.Models;
using STIN_News_Module.Logic.JsonModel;

namespace STIN_News_Module.Logic.Filtering.Filters
{
    public abstract class FilterBase
    {

        public abstract List<DataModel> Execute(List<DataModel> data);

    }
}
