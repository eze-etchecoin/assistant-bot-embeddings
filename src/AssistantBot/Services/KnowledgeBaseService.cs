using AssistantBot.DataTypes;
using AssistantBot.Exceptions;
using AssistantBot.Common.Interfaces;

namespace AssistantBot.Services
{
    public class KnowledgeBaseService
    {
        private readonly IChatBotService _chatBotService;
        private readonly IIndexedVectorStorage<EmbeddedTextVector> _indexedVectorStorage;

        public KnowledgeBaseService(
            IChatBotService chatBotService,
            IIndexedVectorStorage<EmbeddedTextVector> indexedVectorStorage)
        {
            _chatBotService = chatBotService;
            _indexedVectorStorage = indexedVectorStorage;
        }

        public async Task<string> AddParagraphToKnowledgeBase(string paragraph)
        {
            if (string.IsNullOrEmpty(paragraph))
                return "Empty text.";

            var embedding = await _chatBotService.GetEmbedding(paragraph) 
                ?? throw new AssistantBotException("An error has ocurred getting the embedding for given text.");

            var storedKey = _indexedVectorStorage.AddVector(new EmbeddedTextVector
            {
                Values = embedding.ToArray(),
                ParagraphWithPage = new ParagraphWithPage(1, paragraph)
            });

            return storedKey;
        }
    }
}
