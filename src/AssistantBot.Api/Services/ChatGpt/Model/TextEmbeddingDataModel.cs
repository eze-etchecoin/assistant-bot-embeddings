using Newtonsoft.Json;

namespace AssistantBot.Services.ChatGpt.Model
{
    public class TextEmbeddingDataModel
    {
        [JsonProperty("object")]
        public string Object { get; set; }

        [JsonProperty("embedding")]
        public IEnumerable<double> Embedding { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }
    }
}
