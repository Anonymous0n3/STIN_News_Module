using System.Text.Json;

namespace STIN_News_Module.Logic.JsonModel
{
    public class JSONLogic
    {
        private static readonly Lazy<JSONLogic> _instance = new Lazy<JSONLogic>(() => new JSONLogic());
        private JSONLogic() { }

        public static JSONLogic Instance => _instance.Value;

        public string serializeJSON(List<DataModel> json)
        {
            return JsonSerializer.Serialize(json);
        }

        public List<DataModel> deserializeJSON(string json)
        {
            return JsonSerializer.Deserialize<List<DataModel>>(json);
        }
    }
}
