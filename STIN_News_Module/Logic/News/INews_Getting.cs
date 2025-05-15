using NewsAPI.Models;

namespace STIN_News_Module
{
    public interface INewsGetting
    {
        List<Article> returnNews(string q, int days);
    }
}