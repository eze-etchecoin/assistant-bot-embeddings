using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace QAAssistantBot.Services.ChatGpt.Model
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ChatMessageModel
    {
        public ChatMessageModel(string role, string content)
        {
            Role = role;
            Content = content;
        }

        public string Role { get; set; }
        public string Content { get; set; }
    }

    public static class ChatMessageRoles
    {
        public static string System => "system";
        public static string User => "user";
        public static string Assistant => "assistant";
    }
}
