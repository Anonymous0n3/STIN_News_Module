using STIN_News_Module.Logic.Filtering.Filters;
using STIN_News_Module.Logic.JsonModel;
using System.Collections.Generic;
using System.Linq;

public class DummyFilter : FilterBase
{
    public override List<DataModel> Execute(List<DataModel> data)
    {
        // Nastav všem položkám rating = 42 jako důkaz, že filtr byl spuštěn
        foreach (var item in data)
        {
            item.Rating = 42;
        }
        return data;
    }
}
