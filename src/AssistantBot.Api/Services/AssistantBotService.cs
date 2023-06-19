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

        private List<IChatBotMessage> _messages;

        public AssistantBotService(
            IChatBotService chatBotService,
            IIndexedVectorStorage<EmbeddedTextVector> indexedVectorStorage)
        {
            _chatBotService = chatBotService;
            _indexedVectorStorage = indexedVectorStorage;
        }

        public async Task<string> SendMessage(SendMessageRequest request)
        {
            _messages.Add(
                new AssistantBotMessage(request.User, AssistantBotRole.User, request.Message));

            var result = await _chatBotService.SendMessage(request.Message, _messages);
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
                20);

            if (!knowledgeBaseTopResults.Any())
                throw new AssistantBotException("Error at question processing.");

            var userQuestionPrompt = PromptTemplate.GetPromptFromTemplate(
                string.Join("\n", knowledgeBaseTopResults.Select(x => x.Text)),
                request.Question);

            var result = await _chatBotService.SendMessage(userQuestionPrompt);

            return result;
        }
    }
}
