namespace AssistantBot.Services.ChatGpt.Model
{
    public class UsageModel
    {
        public int Prompt_tokens { get; set; }
        public int Completion_tokens { get; set; }
        public int Total_tokens { get; set; }
    }
}