using AssistantBot.DocumentManagers;
using AssistantBot.Common.Interfaces;
using AssistantBot.Common.Exceptions;

namespace AssistantBot.Services.DocumentConverter
{
    public class DocumentConverterService : IDocumentToTextConverter
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
                DocXExtension => "",
                DocExtension => "",
                _ => "",
            };
        }

        public IEnumerable<IParagraphWithPage> GetParagraphsTextWithPageNumber(string filePath, bool skipEmpty = true)
        {
            CheckFileExtension(filePath);

            return Path.GetExtension(filePath) switch
            {
                PdfExtension => PdfManager.ExtractTextInParagraphsFromDocument(filePath, skipEmpty),
                DocXExtension => Enumerable.Empty<IParagraphWithPage>(),
                DocExtension => Enumerable.Empty<IParagraphWithPage>(),
                _ => Enumerable.Empty<IParagraphWithPage>(),
            };

            throw new NotImplementedException();
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
