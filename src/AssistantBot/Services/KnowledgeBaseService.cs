using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;
using AssistantBot.Common.Exceptions;

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

        public async Task<int> AddParagraphToKnowledgeBase(string paragraph)
        {
            if (string.IsNullOrEmpty(paragraph))
                return 0;

            var embedding = await _chatBotService.GetEmbedding(paragraph) 
                ?? throw new AssistantBotException("An error has ocurred getting the embedding for given text.");

            var storedHash = _indexedVectorStorage.AddVector(new EmbeddedTextVector
            {
                Values = embedding.ToArray(),
                ParagraphWithPage = new ParagraphWithPage(1, paragraph)
            });

            return storedHash;
        }
    }
}
