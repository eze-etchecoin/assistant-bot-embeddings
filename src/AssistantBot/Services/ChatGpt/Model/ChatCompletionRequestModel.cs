using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace AssistantBot.Services.ChatGpt.Model
{
    public class ChatCompletionRequestModel
    {
        public ChatCompletionRequestModel()
        {
            Model = ChatCompletionModelOption.Gpt_3_5_Turbo;
            Messages = Enumerable.Empty<ChatMessageModel>();
        }

        /// <summary>
        /// ID of the model to use. You can use the List models API to see all of your available models, 
        /// or see our Model overview for descriptions of them.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// The messages to generate chat completions for, in the chat format.
        /// </summary>
        public IEnumerable<ChatMessageModel> Messages { get; set; }

        /// <summary>
        /// The maximum number of tokens to generate in the completion.
        /// </summary>
        public int Max_tokens { get; set; } = 16;

        /// <summary>
        /// What sampling temperature to use, between 0 and 2. 
        /// Higher values like 0.8 will make the output more random, 
        /// while lower values like 0.2 will make it more focused and deterministic.
        /// </summary>
        public decimal Temperature { get; set; } = 1;

        /// <summary>
        /// An alternative to sampling with temperature, called nucleus sampling, 
        /// where the model considers the results of the tokens with top_p probability mass. 
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// 
        /// We generally recommend altering this or <code>Temperature</code> but not both.
        /// </summary>
        public decimal Top_p { get; set; } = 1;
    }
}
