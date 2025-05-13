using NewsAPI.Models;

namespace STIN_News_Module.Logic.News
{
    public interface INews_Getting
    {
        List<Article> returnNews(string q, int days);
    }
}
