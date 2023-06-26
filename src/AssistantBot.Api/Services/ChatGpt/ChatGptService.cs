using AssistantBot.Services.ChatGpt.Model;
using AssistantBot.Common.Interfaces;
using AssistantBot.Common.Exceptions;
using RestSharp;
using AssistantBot.Common.Helpers;
using AssistantBot.Services.Cache;
using System.Diagnostics;

namespace AssistantBot.Services.ChatGpt
{
    public class ChatGptService : IChatBotService
    {
        private readonly string _apiKey;
        private readonly string _chatModel = ChatCompletionModelOption.Gpt_3_5_Turbo;

        private const string BaseUrl = "https://api.openai.com";

        private readonly RestClient _client;
        private readonly InDiskCache<Dictionary<string, double[]>> _embeddingsDiskCache;

        private static readonly object _embeddingsLockObject = new();

        public ChatGptService(string apiKey, InDiskCache<Dictionary<string, double[]>> inDiskCache)
        {
            _apiKey = apiKey;
            _client = new RestClient(BaseUrl);
            _embeddingsDiskCache = inDiskCache;
        }

        public async Task<string> SendMessage(string message)
        {
            // Request is built here, pointing to OpenAI corresponding endpoint
            var restSharpHelper = new RestSharpJsonHelper<ChatCompletionRequestModel, ChatCompletionResponseModel>(_client);

            var chatResponse = await restSharpHelper.ExecuteRequestAsync(
                url: "/v1/chat/completions",
                method: Method.Post,
                body: new ChatCompletionRequestModel
                {
                    Messages = new[]
                    {
                        new ChatMessageModel(ChatMessageRoles.User, message)
                    },
                    Model = _chatModel,
                    MaxTokens = 256
                },
                new[] { AuthorizationHeader });

            if (!chatResponse.Choices.Any())
                throw new AssistantBotException($"Assistant is not available.");

            // The first (or only) value is returned
            return chatResponse.Choices.First().Message.Content;
        }

        public async Task<string> SendMessage(IEnumerable<IChatBotMessage> messages)
        {
            // Request is built here, pointing to OpenAI corresponding endpoint
            var restSharpHelper = new RestSharpJsonHelper<ChatCompletionRequestModel, ChatCompletionResponseModel>(_client);

            var messagesList = new List<ChatMessageModel>();
            messagesList.AddRange(
                messages.Select(x => new ChatMessageModel(MapRole(x.Role), x.Content)).ToList());

            //messagesList.Add(new ChatMessageModel(ChatMessageRoles.User, message));

            var chatResponse = await restSharpHelper.ExecuteRequestAsync(
                url: "/v1/chat/completions",
                method: Method.Post,
                body: new ChatCompletionRequestModel
                {
                    Messages = messagesList,
                    Model = _chatModel,
                    MaxTokens = 256
                },
                new[] { AuthorizationHeader });

            if (!chatResponse.Choices.Any())
                throw new AssistantBotException($"Assistant is not available.");

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

        public async Task<IEnumerable<double>> GetEmbedding(string textToTransform, bool ignoreCache = false)
        {
            //var cachedEmbeddings = await _embeddingsDiskCache.LoadAsync();

            //if (!ignoreCache)
            //{
            //    if (cachedEmbeddings.TryGetValue(textToTransform, out var cachedEmbedding))
            //    {
            //        //var uncompressedCachedEmbedding = CompressedDataHelper.CompressedByteToDoubleArray(cachedEmbedding);
            //        return cachedEmbedding;
            //    }
            //}

            var restSharpHelper = new RestSharpJsonHelper<EmbeddingsRequestModel, EmbeddingsResponseModel>(_client);

            EmbeddingsResponseModel? embeddingsResponse;
            lock (_embeddingsLockObject)
            {
                var stopwatch = Stopwatch.StartNew();

                embeddingsResponse = restSharpHelper.ExecuteRequestAsync(
                    url: "/v1/embeddings",
                    method: Method.Post,
                    body: new EmbeddingsRequestModel
                    {
                        Model = TextEmbeddingModels.Text_Embedding_Ada_002,
                        Input = textToTransform
                    },
                    headers: new[] { AuthorizationHeader }).Result;

                stopwatch.Stop();
                var elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
                if (elapsedMilliseconds < 1000)
                {
                    var millisecondsToSleep = 1000 - (int)elapsedMilliseconds;
                    Thread.Sleep(millisecondsToSleep);
                }
            }

            var embedding = embeddingsResponse.Data.First().Embedding;

            //if (!ignoreCache)
            //{
            //    //cachedEmbeddings[textToTransform] = CompressedDataHelper.DoubleArrayToCompressedByte(embedding.ToArray());

            //    cachedEmbeddings[textToTransform] = embedding.ToArray();
            //    await _embeddingsDiskCache.SaveAsync(cachedEmbeddings);
            //}

            return embedding;
        }

        private (string, string) AuthorizationHeader => ("Authorization", $"Bearer {_apiKey}");

        private string MapRole(string role) => role.ToLower() switch
        {
            "user" => ChatMessageRoles.User,
            "assistant" => ChatMessageRoles.Assistant,
            "system" => ChatMessageRoles.System,
            _ => throw new AssistantBotException($"Unknown role: {role}")
        };
    }
}
