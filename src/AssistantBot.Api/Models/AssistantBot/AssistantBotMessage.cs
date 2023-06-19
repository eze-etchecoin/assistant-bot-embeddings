using AssistantBot.Common.Interfaces;

namespace AssistantBot.Api.Models.AssistantBot
{
    public record AssistantBotMessage : IChatBotMessage
    {
        public AssistantBotMessage(string user,string role, string content)
        {
            DateTime = System.DateTime.UtcNow;
            User = user;
            Role = role;
            Content = content;
        }

        public DateTimeOffset DateTime { get; }
        public string User { get; }
        public string Role { get; }
        public string Content { get; }
    }
}
