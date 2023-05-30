using Newtonsoft.Json;

namespace AssistantBot.Services.ChatGpt.Model
{
    public class EmbeddingsRequestModel
    {
        /// <summary>
        /// ID of the model to use. You can use the List models API to see all of your available models, or see our Model overview for descriptions of them.
        /// </summary>
        [JsonProperty("model")]
        public string Model { get; set; }

        /// <summary>
        /// Input text to get embeddings for, encoded as a string or array of tokens. 
        /// To get embeddings for multiple inputs in a single request, pass an array of strings or array of token arrays. 
        /// Each input must not exceed 8192 tokens in length.
        /// </summary>
        [JsonProperty("input")]
        public object Input { get; set; }
    }
}
