using AssistantBot.Services.ChatGpt.Model;
using AssistantBot.Services.Interfaces;
using Newtonsoft.Json;
using AssistantBot.Exceptions;
using AssistantBot.Services.ChatGpt.Model;
using RestSharp;

namespace AssistantBot.Services.ChatGpt
{
    public class ChatGptService : IChatBotService
    {
        private readonly string _apiKey;
        private readonly string _chatModel = ChatCompletionModelOption.Gpt_3_5_Turbo;

        private const string BaseUrl = "https://api.openai.com";

        private readonly RestClient _client;

        public ChatGptService(string apiKey)
        {
            _apiKey = apiKey;
            _client = new RestClient(BaseUrl);
        }

        public async Task<string> SendMessage(string message)
        {
            // Request is built here, pointing to OpenAI corresponding endpoint
            var request = GetRequest("/v1/chat/completions", Method.Post);

            // Content type application/json is built here
            request.AddJsonBody(new ChatCompletionRequestModel
            {
                Messages = new[] 
                {
                    new ChatMessageModel(ChatMessageRoles.User, message)
                },
                Model = _chatModel,
                Max_tokens = 256
            });

            // Request is executed, and a response must be received
            var response = await _client.ExecuteAsync(request);

            var chatResponse = GetResponse<ChatCompletionResponseModel>(response);

            if (!chatResponse.Choices.Any())
                throw new QAAssistantBotException($"Assistant is not available.");

            // The first (or only) value is returned
            return chatResponse.Choices.First().Message.Content;
        }


        public async Task<string> SendTrainingInput(ITrainingDataModel dataModel)
        {
            return await SendTrainingInput(new[] { dataModel });
        }

        public Task<string> SendTrainingInput(IEnumerable<ITrainingDataModel> dataModels)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<double>> GetEmbeddings(string textToTransform)
        {
            var request = GetRequest("/v1/embeddings", Method.Post);
            request.AddJsonBody(new EmbeddingsRequestModel
            {
                Model = TextEmbeddingModels.Text_Embedding_Ada_002,
                Input = textToTransform
            });

            var response = await _client.ExecuteAsync(request);

            var embeddingsResponse = GetResponse<EmbeddingsResponseModel>(response);

            return embeddingsResponse.Data.First().Embedding;
        }

        private RestRequest GetRequest(string url, Method method)
        {
            var request = new RestRequest(url, method);
            request.AddHeader("Authorization", $"Bearer {_apiKey}");

            return request;
        }

        private T GetResponse<T>(RestResponse restResponse)
        {
            if (!restResponse.IsSuccessful)
                throw new QAAssistantBotException(restResponse.ErrorMessage ?? restResponse.Content);

            // The response content is deserialized (it comes in JSON format)
            var response = JsonConvert.DeserializeObject<T>(restResponse.Content);

            return response ?? Activator.CreateInstance<T>();
        }
    }
}
