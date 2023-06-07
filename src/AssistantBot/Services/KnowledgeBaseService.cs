using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;
using AssistantBot.Common.Exceptions;
using AssistantBot.DocumentManagers;
using AssistantBot.Services.DocumentConverter;

namespace AssistantBot.Services
{
    public class KnowledgeBaseService
    {
        private readonly IChatBotService _chatBotService;
        private readonly IIndexedVectorStorage<EmbeddedTextVector> _indexedVectorStorage;

        private Dictionary<string, (int,int)> _documentProcessingProgress = new Dictionary<string, (int,int)>();

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

        public async Task LoadFileToKnowledgeBase(string filePath)
        {
            if(File.Exists(filePath) == false)
                throw new AssistantBotException($"File {filePath} does not exist.");

            var documentConverterService = new DocumentConverterService();
            var paragraphs = documentConverterService.GetParagraphsTextWithPageNumber(filePath);

            var totalParagraphs = paragraphs.Count();
            var currentParagraph = 0;

            _documentProcessingProgress[filePath] = (currentParagraph, totalParagraphs);

            foreach (var paragraph in paragraphs)
            {
                var embedding = await _chatBotService.GetEmbedding(paragraph.Text)
                    ?? throw new AssistantBotException("An error has ocurred getting the embedding for given text.");

                var storedHash = _indexedVectorStorage.AddVector(new EmbeddedTextVector
                {
                    Values = embedding.ToArray(),
                    ParagraphWithPage = new ParagraphWithPage(paragraph.Page, paragraph.Text)
                });

                _documentProcessingProgress[filePath] = (++currentParagraph, totalParagraphs);
            }
        }

        public int GetDocumentProcessingStatus(string filePath)
        {
            if (_documentProcessingProgress.ContainsKey(filePath) == false)
                return 0;

            var (currentParagraph, totalParagraphs) = _documentProcessingProgress[filePath];

            return Convert.ToInt32((decimal)currentParagraph / totalParagraphs * 100);
        }
    }
}
