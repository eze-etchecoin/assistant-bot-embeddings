using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace AssistantBot.Services.ChatGpt.Model
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ChatMessageModel
    {
        public ChatMessageModel(string role, string content)
        {
            Role = role;
            Content = content;
        }

        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public static class ChatMessageRoles
    {
        public static string System => "system";
        public static string User => "user";
        public static string Assistant => "assistant";
    }
}
