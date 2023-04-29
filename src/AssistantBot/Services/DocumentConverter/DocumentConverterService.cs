
using iText.Kernel.Pdf;
using AssistantBot.DataTypes;
using AssistantBot.DocumentManagers;
using AssistantBot.Services.Interfaces;
using AssistantBot.Exceptions;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;

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

        public IEnumerable<ParagraphWithPage> GetParagraphsTextWithPageNumber(string filePath, bool skipEmpty = true)
        {
            CheckFileExtension(filePath);

            return Path.GetExtension(filePath) switch
            {
                PdfExtension => PdfManager.ExtractTextInParagraphsFromDocument(filePath, skipEmpty),
                DocXExtension => Enumerable.Empty<ParagraphWithPage>(),
                DocExtension => Enumerable.Empty<ParagraphWithPage>(),
                _ => Enumerable.Empty<ParagraphWithPage>(),
            };

            throw new NotImplementedException();
        }

        private static void CheckFileExtension(string filePath)
        {
            var fileExtension = Path.GetExtension(filePath)?.ToLower() ?? "";
            var validExtensions = new[] { PdfExtension, DocExtension, DocXExtension };

            if (validExtensions.All(x => x != fileExtension))
                throw new QAAssistantBotException($"File extension {fileExtension} not supported.");
        }
    }
}
