using Newtonsoft.Json;

namespace AssistantBot.Services.ChatGpt.Model
{
    public class UsageModel
    {

        [JsonProperty("prompt_tokens")] 
        public int PromptTokens { get; set; }

        [JsonProperty("completion_tokens")]
        public int CompletionTokens { get; set; }

        [JsonProperty("total_tokens")]
        public int TotalTokens { get; set; }
    }
}