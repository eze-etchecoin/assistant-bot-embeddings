using AssistantBot.Services.ChatGpt.Model;

namespace AssistantBot.Services.ChatGpt.Model
{
    public class EmbeddingsResponseModel
    {
        public string Object { get; set; }
        public IEnumerable<TextEmbeddingDataModel> Data { get; set; }
        public string Model { get; set; }
        public UsageModel Usage { get; set; }

    }
}
