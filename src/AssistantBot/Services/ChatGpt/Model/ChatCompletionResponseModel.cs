using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace AssistantBot.Services.ChatGpt.Model
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class ChatCompletionResponseModel
    {
        public ChatCompletionResponseModel()
        {
            Choices = Enumerable.Empty<ChoiceModel>();
        }
        
        public string Id { get; set; }
        public string Object { get; set; }
        public long Created { get; set; }
        public IEnumerable<ChoiceModel> Choices { get; set; }
        public UsageModel Usage { get; set; }
    }
}
