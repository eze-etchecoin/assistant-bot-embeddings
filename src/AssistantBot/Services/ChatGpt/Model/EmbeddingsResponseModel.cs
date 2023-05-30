using Newtonsoft.Json;

namespace AssistantBot.Services.ChatGpt.Model
{
    public class EmbeddingsResponseModel
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("data")]
        public IEnumerable<TextEmbeddingDataModel> Data { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("usage")]
        public UsageModel Usage { get; set; }

    }
}
