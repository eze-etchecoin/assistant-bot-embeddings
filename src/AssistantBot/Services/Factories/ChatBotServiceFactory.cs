using AssistantBot.Configuration;
using AssistantBot.Common.Interfaces;
using AssistantBot.Common.Exceptions;
using AssistantBot.Services.ChatGpt;
using AssistantBot.Services.Cache;

namespace AssistantBot.Services.Factories
{
    public class ChatBotServiceFactory : IChatBotServiceFactory
    {
        private readonly InDiskCache<Dictionary<string, byte[]>> _inDiskCache;

        public ChatBotServiceFactory(InDiskCache<Dictionary<string, byte[]>> inDiskCache)
        {
            _inDiskCache = inDiskCache;
        }

        public IChatBotService CreateService(ChatBotServiceOption chatBotServiceOption)
        {
            return chatBotServiceOption switch
            {
                ChatBotServiceOption.ChatGpt => new ChatGptService(
                    Environment.GetEnvironmentVariable(StartupEnvironmentVariables.OpenAIApiKey) ?? "",
                    _inDiskCache),
                _ => throw new AssistantBotException("No ChatBotServiceOption specified.")
            };
        }
    }
}
