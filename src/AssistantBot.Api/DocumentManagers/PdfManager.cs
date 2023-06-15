using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf;
using System.Text;
using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;

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

        public static IEnumerable<IParagraphWithPage> ExtractTextInParagraphsFromDocument(string filePath, bool skipEmpty)
        {
            var result = Enumerable.Empty<IParagraphWithPage>();

            var reader = new PdfReader(filePath);

            IParagraphWithPage? lastResultParagraph = null;

            using (var document = new PdfDocument(reader))
            {
                for (int page = 1; page <= document.GetNumberOfPages(); page++)
                {
                    var strategy = new SimpleTextExtractionStrategy();
                    var text = PdfTextExtractor.GetTextFromPage(document.GetPage(page), strategy);

                    var paragraphs = text
                        .Replace("\n", "")
                        .Split('.')
                        .SelectMany(x => x.Split("  ")) // Split by double space.
                        .Select(x => new ParagraphWithPage
                        {
                            Page = page,
                            Text = x
                        } as IParagraphWithPage);

                    // Concatenate the first paragraph of the current page with the last added paragraph to result.
                    if (page > 1)
                    {
                        var firstParagraph = paragraphs.FirstOrDefault();
                        if (firstParagraph != null && lastResultParagraph != null)
                        {
                            lastResultParagraph.Text += firstParagraph.Text;
                            paragraphs = paragraphs.Skip(1);
                        }
                    }

                    if (skipEmpty)
                        paragraphs = paragraphs.Where(x => !string.IsNullOrEmpty(x.Text.Trim()));

                    result = result.Concat(paragraphs).ToList();
                    lastResultParagraph = result.LastOrDefault();
                }
            }

            return result;
        }
    }
}
