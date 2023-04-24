namespace AssistantBot.Services.ChatGpt.Model
{
    public class TextEmbeddingDataModel
    {
        public string Object { get; set; }
        public IEnumerable<double> Embedding { get; set; }
        public int Index { get; set; }
    }
}
