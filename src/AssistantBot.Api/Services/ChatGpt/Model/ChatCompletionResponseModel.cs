using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AssistantBot.Services.ChatGpt.Model
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ChatCompletionResponseModel
    {
        public ChatCompletionResponseModel()
        {
            Choices = Enumerable.Empty<ChoiceModel>();
        }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("choices")]
        public IEnumerable<ChoiceModel> Choices { get; set; }

        [JsonProperty("usage")]
        public UsageModel Usage { get; set; }
    }
}
