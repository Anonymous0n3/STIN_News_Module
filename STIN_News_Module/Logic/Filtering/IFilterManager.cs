using STIN_News_Module.Logic.JsonModel;

namespace STIN_News_Module.Logic.Filtering
{
    public interface IFilterManager
    {
        public List<DataModel> ExecuteAllFilters(List<DataModel> data);
    }
}
