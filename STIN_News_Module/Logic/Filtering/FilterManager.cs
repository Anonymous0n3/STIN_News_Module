using STIN_News_Module.Logic.Filtering.Filters;
using STIN_News_Module.Logic.JsonModel;
using STIN_News_Module.Logic.Logging;
using System.Reflection;

namespace STIN_News_Module.Logic.Filtering
{
    public class FilterManager : IFilterManager
    {
        private List<FilterBase> filters = new List<FilterBase>();
        private static readonly Lazy<FilterManager> _instance = new Lazy<FilterManager>(() => new FilterManager());

        public static FilterManager Instance => _instance.Value;

        private FilterManager() {
            AutoRegisterFilters();
        }

        private void AutoRegisterFilters()
        {
            LoggingService.AddLog("Auto registering filters");
            var filterTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(FilterBase)) && !t.IsAbstract);

            foreach (var type in filterTypes)
            {
                FilterBase filter = (FilterBase)Activator.CreateInstance(type);
                filters.Add(filter);
            }
        }

        public List<DataModel> ExecuteAllFilters(List<DataModel> data)
        {
            LoggingService.AddLog("Executing all filters");
            foreach (var filter in filters)
            {
                data = filter.Execute(data);
            }
            return data;
        }
    }
}
