using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;
using AssistantBot.Common.Exceptions;
using AssistantBot.Services.DocumentConverter;
using AssistantBot.Models.KnowledgeBase;
using static iText.Svg.SvgConstants;

namespace AssistantBot.Services
{
    public class KnowledgeBaseService
    {
        private readonly IChatBotService _chatBotService;
        private readonly IIndexedVectorStorage<EmbeddedTextVector> _indexedVectorStorage;

        private static Dictionary<string, KnowledgeBaseFileInfo> _documents;

        public KnowledgeBaseService(
            IChatBotService chatBotService,
            IIndexedVectorStorage<EmbeddedTextVector> indexedVectorStorage)
        {
            _chatBotService = chatBotService;
            _indexedVectorStorage = indexedVectorStorage;

            _documents ??= new Dictionary<string, KnowledgeBaseFileInfo>();
        }

        public async Task<int> AddParagraphToKnowledgeBase(string paragraph)
        {
            if (string.IsNullOrEmpty(paragraph))
                return 0;

            var embedding = await _chatBotService.GetEmbedding(paragraph) 
                ?? throw new AssistantBotException("An error has ocurred getting the embedding for given text.");

            var storedHash = _indexedVectorStorage.AddVector(
                new EmbeddedTextVector(embedding.ToArray(), new ParagraphWithPage(1, paragraph)));

            return storedHash;
        }

        public async Task LoadFileToKnowledgeBase(string filePath, string fileName)
        {
            if(File.Exists(filePath) == false)
                throw new AssistantBotException($"File {filePath} does not exist.");

            var documentConverterService = new DocumentConverterService();
            var paragraphs = documentConverterService.GetParagraphsTextWithPageNumber(filePath);

            var totalParagraphs = paragraphs.Count();

            _documents[fileName] = new KnowledgeBaseFileInfo(fileName, totalParagraphs);

            foreach (var paragraph in paragraphs)
            {
                try
                {
                    var embedding = await _chatBotService.GetEmbedding(paragraph.Text)
                    ?? throw new AssistantBotException("An error has occurred getting the embedding for given text.");

                    var storedHash = _indexedVectorStorage.AddVector(
                        new EmbeddedTextVector(
                            embedding.ToArray(),
                            new ParagraphWithPage(paragraph.Page, paragraph.Text)));

                }
                catch (Exception ex)
                {
                    _documents[fileName].ErrorMessage = $"An error has ocurred getting the embedding for given text: {ex.Message}";
                }

                _documents[fileName].ProcessedParagraphs++;
            }
        }

        //public KnowledgeBaseFileInfo? GetDocumentProcessingStatus(string fileName)
        //{
        //    if (_documents.ContainsKey(fileName) == false)
        //        return null;

        //    var docInfo = _documents[fileName];

        //    return docInfo;

        //    //return Convert.ToInt32((decimal)docInfo.ProcessedParagraphs / docInfo.TotalParagraphs * 100);
        //}

        internal KnowledgeBaseFileInfo? GetKnowledgeBaseFileInfo(string fileName)
        {
            if (_documents.ContainsKey(fileName) == false)
                return null;

            var docInfo = _documents[fileName];

            return docInfo;
        }

        internal KnowledgeBaseFileInfo? GetLastUploadedFileInfo()
        {
            if (!_documents.Any())
                return null;

            return _documents
                .OrderBy(x => x.Value.UploadedDateTime)
                .LastOrDefault()
                .Value;
        }
    }
}
