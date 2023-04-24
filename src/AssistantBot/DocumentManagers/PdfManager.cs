using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System.Text;
using AssistantBot.DataTypes;

namespace AssistantBot.DocumentManagers
{
    public class PdfManager
    {
        public static string ExtractTextFromDocument(string filePath)
        {
            var result = new StringBuilder();

            var reader = new PdfReader(filePath);
            using (var document = new PdfDocument(reader))
            {
                // Loop through each page in the PDF file and extract the text
                for (int page = 1; page <= document.GetNumberOfPages(); page++)
                {
                    var strategy = new SimpleTextExtractionStrategy();
                    var text = PdfTextExtractor.GetTextFromPage(document.GetPage(page), strategy);

                    result.Append(text);
                }
            }

            return result.ToString();
        }

        public static IEnumerable<ParagraphWithPage> ExtractTextInParagraphsFromDocument(string filePath, bool skipEmpty)
        {
            var result = Enumerable.Empty<ParagraphWithPage>();

            var reader = new PdfReader(filePath);
            using (var document = new PdfDocument(reader))
            {
                for (int page = 1; page <= document.GetNumberOfPages(); page++)
                {
                    var strategy = new SimpleTextExtractionStrategy();
                    var text = PdfTextExtractor.GetTextFromPage(document.GetPage(page), strategy);

                    var paragraphs = text.Split("\n").Select(p => new ParagraphWithPage(page, p));

                    if (skipEmpty)
                        paragraphs = paragraphs.Where(x => !string.IsNullOrEmpty(x.Text.Trim()));

                    result = result.Concat(paragraphs).ToList();
                }
            }

            return result;
        }
    }
}
