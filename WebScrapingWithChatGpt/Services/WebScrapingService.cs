using System.Text;
using WebScrapingWithChatGpt.Interfaces;
using System.Text.Json;

namespace WebScrapingWithChatGpt.Services
{
    public class WebScrapingService : IWebScrapingService
    {
        private readonly IChatClient _chatClient;
        public WebScrapingService(IChatClient chatClient)
        {
            _chatClient = chatClient;
        }

        public async Task<T?> ScrapeData<T>(string[] html)
        {
            var prompt = new StringBuilder();

            prompt.Append("Scrape the html content and return a json object with the following data: ");

            var properties = typeof(T).GetProperties()
                                      .Select(p => p.Name)
                                      .ToList();

            var propString = properties.Skip(1)
                                       .Aggregate($"{{ {properties[0]}", (total, current) => total + $", {current}") + "}";

            prompt.Append(propString);

            prompt.Append("Here are the types in order: ");

            var types = typeof(T).GetProperties()
                                 .Select(p => p.PropertyType.Name)
                                 .ToList();

            var typeString = types.Skip(1)
                                  .Aggregate($"{{ {types[0]}", (total, current) => total + $", {current}") + "}";

            prompt.Append(typeString);

            prompt.Append("Return only the object.");

            prompt.Append($"Here is the html to draw the data from: {html[0]}");

            if (html.Length > 1)
            {
                foreach (var item in html.Skip(1))
                {
                    prompt.Append($"If you don't find the information in the html above, try supplementing it with information from the following: ");
                    prompt.Append(item);
                    prompt.Append(" (don't replace any information you already have).");
                }
            }

            string result = await _chatClient.GetStringResponseAsync(prompt.ToString());

            return JsonSerializer.Deserialize<T>(result);
        }
    }
}
