﻿using STIN_News_Module.Logic.Logging;
using System.Text.Json;

namespace STIN_News_Module.Logic.JsonModel
{
    public class JSONLogic : IJSONLogic
    {
        private static readonly Lazy<JSONLogic> _instance = new Lazy<JSONLogic>(() => new JSONLogic());
        private JSONLogic() { }

        public static JSONLogic Instance => _instance.Value;

        public string serializeJSON(List<DataModel> json)
        {
            LoggingService.AddLog("Serializing JSON");
            return JsonSerializer.Serialize(json);
        }

        public List<DataModel> deserializeJSON(string json)
        {
            try
            {
                LoggingService.AddLog("Deserializing JSON");
                return JsonSerializer.Deserialize<List<DataModel>>(json);
            } catch (Exception e) 
            {
                LoggingService.AddLog("Error deserializing JSON: " + e.Message);
                return null;   
            }
            
        }
    }
}
