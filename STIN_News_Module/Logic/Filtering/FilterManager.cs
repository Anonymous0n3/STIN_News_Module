using STIN_News_Module.Logic.Filtering.Filters;
using STIN_News_Module.Logic.JsonModel;
using System.Reflection;

namespace STIN_News_Module.Logic.Filtering
{
    public class FilterManager
    {
        private List<FilterBase> filters = new List<FilterBase>();

        public FilterManager() {
            AutoRegisterFilters();
        }

        private void AutoRegisterFilters()
        {
            var filterTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(FilterBase)) && !t.IsAbstract);

            foreach (var type in filterTypes)
            {
                FilterBase filter = (FilterBase)Activator.CreateInstance(type);
                filters.Add(filter);
            }
        }

        public void ExecuteAllFilters(List<DataModel> data)
        {
            foreach (var filter in filters)
            {
                data = filter.Execute(data);
            }
        }
    }
}
