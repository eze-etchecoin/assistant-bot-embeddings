using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;
using AssistantBot.Common.Exceptions;
using AssistantBot.Services.DocumentConverter;
using AssistantBot.Models.KnowledgeBase;
using AssistantBot.Configuration;

namespace AssistantBot.Services
{
    public class KnowledgeBaseService
    {
        private readonly IChatBotService _chatBotService;
        private readonly IIndexedVectorStorage<EmbeddedTextVector> _indexedVectorStorage;
        private readonly AssistantBotConfiguration _config;

        private static Dictionary<string, KnowledgeBaseFileInfo> _documents;

        public KnowledgeBaseService(
            IChatBotService chatBotService,
            IIndexedVectorStorage<EmbeddedTextVector> indexedVectorStorage,
            AssistantBotConfiguration config)
        {
            _chatBotService = chatBotService;
            _indexedVectorStorage = indexedVectorStorage;
            _config = config;

            _documents ??= ReadUploadedFiles();
        }

        private Dictionary<string, KnowledgeBaseFileInfo> ReadUploadedFiles()
        {
            var result = new Dictionary<string, KnowledgeBaseFileInfo>();

            var files = Directory.GetFiles(_config.UploadedFilesFolderPath);

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);

                result[fileName] = new KnowledgeBaseFileInfo(
                    fileName,
                    0,
                    File.GetCreationTime(file));
            }

            return result;
        }

        public async Task<string> AddParagraphToKnowledgeBase(string paragraph)
        {
            if (string.IsNullOrEmpty(paragraph))
                return "";

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
            var keyComplementStr = fileName.Replace(".", "-");

            foreach (var paragraph in paragraphs)
            {
                try
                {
                    var embedding = await _chatBotService.GetEmbedding(paragraph.Text)
                    ?? throw new AssistantBotException("An error has occurred getting the embedding for given text.");

                    var storedHash = _indexedVectorStorage.AddVector(
                        new EmbeddedTextVector(
                            embedding.ToArray(),
                            new ParagraphWithPage(paragraph.Page, paragraph.Text)),
                        keyComplementStr);

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

        internal static KnowledgeBaseFileInfo? GetKnowledgeBaseFileInfo(string fileName)
        {
            if (_documents.ContainsKey(fileName) == false)
                return null;

            var docInfo = _documents[fileName];

            return docInfo;
        }

        internal static KnowledgeBaseFileInfo? GetLastUploadedFileInfo()
        {
            if (!_documents.Any())
                return null;

            return _documents
                .OrderByDescending(x => x.Value.UploadedDateTime)
                .FirstOrDefault()
                .Value;
        }

        internal static IEnumerable<KnowledgeBaseFileInfo> GetAllUploadedFilesInfo()
        {
            return _documents.Values.OrderBy(x => x.FileName);
        }
    }
}
