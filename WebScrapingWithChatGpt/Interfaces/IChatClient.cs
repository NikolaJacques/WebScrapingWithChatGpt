namespace WebScrapingWithChatGpt.Interfaces
{
    public interface IChatClient
    {
        public Task<string> GetStringResponseAsync(string prompt);
    }
}