using AssistantBot.Common.Interfaces;

namespace AssistantBot.Services.Factories
{
    public interface IChatBotServiceFactory
    {
        IChatBotService? CreateService(ChatBotServiceOption chatBotServiceOption);
    }

    public enum ChatBotServiceOption
    {
        ChatGpt
    }
}
