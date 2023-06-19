using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Exceptions;
using AssistantBot.Models.AssistantBot;
using AssistantBot.Common.Interfaces;
using AssistantBot.Api.Models.AssistantBot;

namespace AssistantBot.Services
{
    public class AssistantBotService
    {
        private readonly IChatBotService _chatBotService;
        private readonly IIndexedVectorStorage<EmbeddedTextVector> _indexedVectorStorage;
        
        private readonly List<IChatBotMessage> _messages = new();

        public AssistantBotService(
            IChatBotService chatBotService,
            IIndexedVectorStorage<EmbeddedTextVector> indexedVectorStorage)
        {
            _chatBotService = chatBotService;
            _indexedVectorStorage = indexedVectorStorage;
        }

        public async Task<string> SendMessage(SendMessageRequest request)
        {
            AddMessageToHistory(request.User, AssistantBotRole.User, request.Message);

            string result;
            try
            {
                result = await _chatBotService.SendMessage(
                    GetMessagesHistory(request.User));
            }
            catch
            {
                RemoveLastMessageFromHistory(request.User);
                throw;
            }

            AddMessageToHistory(request.User, AssistantBotRole.Assistant, result);

            return result;
        }

        public async Task<string> AskToKnowledgeBase(AskToKnowledgeBaseRequest request)
        {
            if (string.IsNullOrEmpty(request.Question?.Trim()))
            {
                return "Empty text.";
            }

            var questionEmbedding = await _chatBotService.GetEmbedding(request.Question, ignoreCache: true);

            var knowledgeBaseTopResults = _indexedVectorStorage.SearchDataBySimilarVector<ParagraphWithPage>(
                new EmbeddedTextVector(questionEmbedding.ToArray(), default(ParagraphWithPage)),
                15);

            if (!knowledgeBaseTopResults.Any())
                throw new AssistantBotException("Error at question processing.");

            var basePrompt = PromptTemplate.GetPromptFromTemplate(
                string.Join("\n", knowledgeBaseTopResults.Select(x => x.Text)));

            AddMessageToHistory(request.User, AssistantBotRole.User, request.Question);

            var messages = new List<IChatBotMessage>
            {
                new AssistantBotMessage(request.User, AssistantBotRole.System, basePrompt)
            };

            messages.AddRange(GetMessagesHistory(request.User));

            string result;
            try
            {
                result = await _chatBotService.SendMessage(messages);
            }
            catch
            {
                RemoveLastMessageFromHistory(request.User);
                throw;
            }

            AddMessageToHistory(request.User, AssistantBotRole.Assistant, result);

            return result;
        }

        private void AddMessageToHistory(string user, string role, string content)
        {
            _messages.Add(new AssistantBotMessage(user, role, content));
        }

        private void RemoveLastMessageFromHistory(string user) =>
            _messages.Remove(_messages.Last(x => x.User == user));

        private List<IChatBotMessage> GetMessagesHistory(string user, int numResults = 11) =>
            _messages.Where(x => x.User == user)
                .OrderBy(x => x.DateTime)
                .Take(numResults)
                .ToList();
    }
}
