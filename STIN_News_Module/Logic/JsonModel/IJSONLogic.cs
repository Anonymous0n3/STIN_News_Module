namespace STIN_News_Module.Logic.JsonModel
{
    public interface IJSONLogic
    {
        List<DataModel>? deserializeJSON(string json);
    }
}