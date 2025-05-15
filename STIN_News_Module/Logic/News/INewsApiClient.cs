using NewsAPI.Models;

public interface INewsApiClient
{
    ArticlesResult GetEverything(EverythingRequest request);
}