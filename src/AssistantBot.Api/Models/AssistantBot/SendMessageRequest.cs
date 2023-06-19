using Newtonsoft.Json;

namespace AssistantBot.Models.AssistantBot
{
    public class SendMessageRequest
    {
        public string User { get; set; }
        public string Message { get; set; }
    }
}
