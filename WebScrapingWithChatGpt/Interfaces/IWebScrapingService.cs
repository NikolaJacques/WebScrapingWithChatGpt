namespace WebScrapingWithChatGpt.Interfaces
{
    public interface IWebScrapingService
    {
        public Task<T?> ScrapeData<T>(string[] html);
    }
}