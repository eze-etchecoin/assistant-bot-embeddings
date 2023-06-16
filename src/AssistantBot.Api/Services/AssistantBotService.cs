using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Exceptions;
using AssistantBot.Models.AssistantBot;
using AssistantBot.Common.Interfaces;

namespace AssistantBot.Services
{
    public class AssistantBotService
    {
        private readonly IChatBotService _chatBotService;
        private readonly IIndexedVectorStorage<EmbeddedTextVector> _indexedVectorStorage;

        public AssistantBotService(
            IChatBotService chatBotService,
            IIndexedVectorStorage<EmbeddedTextVector> indexedVectorStorage)
        {
            _chatBotService = chatBotService;
            _indexedVectorStorage = indexedVectorStorage;
        }

        public async Task<string> SendMessage(string message)
        {
            var result = await _chatBotService.SendMessage(message);
            return result;
        }

        public async Task<string> AskToKnowledgeBase(string? question)
        {
            if (string.IsNullOrEmpty(question?.Trim()))
            {
                return "Empty text.";
            }

            var questionEmbedding = await _chatBotService.GetEmbedding(question, ignoreCache: true);

            var knowledgeBaseTopResults = _indexedVectorStorage.SearchDataBySimilarVector<ParagraphWithPage>(
                new EmbeddedTextVector(questionEmbedding.ToArray(), default(ParagraphWithPage)),
                20);

            if (!knowledgeBaseTopResults.Any())
                throw new AssistantBotException("Error at question processing.");

            var userQuestionPrompt = PromptTemplate.GetPromptFromTemplate(
                string.Join("\n", knowledgeBaseTopResults.Select(x => x.Text)),
                question);

            var result = await _chatBotService.SendMessage(userQuestionPrompt);

            return result;
        }
    }
}
