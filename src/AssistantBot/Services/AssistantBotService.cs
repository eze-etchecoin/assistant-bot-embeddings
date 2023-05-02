using AssistantBot.DataTypes;
using AssistantBot.Exceptions;
using AssistantBot.Models.AssistantBot;
using AssistantBot.Services.Interfaces;

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

            var questionEmbedding = await _chatBotService.GetEmbedding(question);

            var knowledgeBaseTopResults = _indexedVectorStorage.SearchDataBySimilarVector(
                new EmbeddedTextVector
                {
                    Values = questionEmbedding.ToArray()
                },
                5);

            if (!knowledgeBaseTopResults.Any())
                throw new AssistantBotException("Error at question processing.");

            var userQuestionPrompt = PromptTemplate.GetPromptFromTemplate(
                string.Join("\n", knowledgeBaseTopResults.Select(x => x.ToString())),
                question);

            var result = await _chatBotService.SendMessage(userQuestionPrompt);

            return result;
        }
    }
}
