namespace QAAssistantBot.Services.ChatGpt.Model
{
    public class ChoiceModel
    {
        public int Index { get; set; }
        public ChatMessageModel Message { get; set; }
        public string finish_reason { get; set; }
    }
}