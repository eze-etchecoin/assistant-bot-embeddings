using Newtonsoft.Json;

namespace AssistantBot.Services.ChatGpt.Model
{
    public class ChoiceModel
    {
        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("message")]
        public ChatMessageModel Message { get; set; }

        [JsonProperty("finish_reason")]
        public string FinishReason { get; set; }
    }
}