namespace STIN_News_Module.Logic.AIStuff
{
    public interface IAI
    {
        public Task<int> GetClasification(string text);
    }
}
