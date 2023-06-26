using AssistantBot.DocumentManagers;
using AssistantBot.Common.Interfaces;
using AssistantBot.Common.Exceptions;
using AssistantBot.Api.DocumentManagers;

namespace AssistantBot.Services.DocumentConverter
{
    public class DocumentConverterService : IDocumentToTextProcessor
    {
        private const string PdfExtension = ".pdf";
        private const string DocExtension = ".doc";
        private const string DocXExtension = ".docx";

        public string ConvertDocumentToString(string filePath)
        {
            CheckFileExtension(filePath);

            return Path.GetExtension(filePath) switch
            {
                PdfExtension => PdfManager.ExtractTextFromDocument(filePath),
                DocXExtension => DocManager.ExtractTextFromDocument(filePath),
                DocExtension => DocManager.ExtractTextFromDocument(filePath),
                _ => "",
            };
        }

        public IEnumerable<IParagraphWithPage> GetParagraphsTextWithPageNumber(string filePath, bool skipEmpty = true)
        {
            CheckFileExtension(filePath);

            return Path.GetExtension(filePath) switch
            {
                PdfExtension => PdfManager.ExtractTextInParagraphsFromDocument(filePath, skipEmpty),
                DocXExtension => DocManager.ExtractTextInParagraphsFromDocument(filePath, skipEmpty),
                DocExtension => DocManager.ExtractTextInParagraphsFromDocument(filePath, skipEmpty),
                _ => Enumerable.Empty<IParagraphWithPage>(),
            };

            throw new NotImplementedException();
        }

        public int GetNumberOfParagraphs(string filePath)
        {
            CheckFileExtension(filePath);

            return Path.GetExtension(filePath) switch
            {
                PdfExtension => PdfManager.GetNumberOfParagraphs(filePath),
                DocXExtension => DocManager.GetNumberOfParagraphs(filePath),
                DocExtension => DocManager.GetNumberOfParagraphs(filePath),
                _ => 0,
            };
        }

        private static void CheckFileExtension(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath)?.ToLower() ?? "";
            var validExtensions = new[] { PdfExtension, DocExtension, DocXExtension };

            if (validExtensions.All(x => x != fileExtension))
                throw new AssistantBotException($"File extension {fileExtension} not supported.");
        }
    }
}
