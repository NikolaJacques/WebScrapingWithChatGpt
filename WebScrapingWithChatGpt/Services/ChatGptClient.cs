namespace WebScrapingWithChatGpt.Services
{
    using Azure;
    using Azure.AI.OpenAI;
    using WebScrapingWithChatGpt.Interfaces;

        public class ChatGptClient : IChatClient
        {
            private readonly OpenAIClient _client;
            private readonly IConfiguration _configuration;
            public ChatGptClient(IConfiguration configuration)
            {
                var key = configuration.GetValue<string>("ChatGptApiKey");
                _client = new OpenAIClient(key);
                _configuration = configuration;
            }

            public async Task<string> GetStringResponseAsync(string prompt)
            {
                var chatCompletionsOptions = new ChatCompletionsOptions()
                {
                    DeploymentName = _configuration.GetValue<string>("ChatGptModel"),
                    Messages = { new ChatRequestUserMessage(prompt) }
                };
                Response<ChatCompletions> response = await _client.GetChatCompletionsAsync(chatCompletionsOptions);
                if (response.HasValue)
                {
                    return response.Value.Choices[0].Message.Content;
                }
                else
                {
                    throw new Exception($"Request failed on chatGptClient.");
                }
            }
        }
}
