using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;
using System.Text;
using Xceed.Words.NET;

namespace AssistantBot.Api.DocumentManagers
{
    // This class uses DocX library to extract text from .docx files.
    public class DocManager
    {
        public static string ExtractTextFromDocument(string filePath)
        {
            var result = new StringBuilder();

            using var document = DocX.Load(filePath);
            foreach (var paragraph in document.Paragraphs)
            {
                result.AppendLine(paragraph.Text);
            }

            return result.ToString();
        }

        public static IEnumerable<IParagraphWithPage> ExtractTextInParagraphsFromDocument(string filePath, bool skipEmpty)
        {
            var result = Enumerable.Empty<IParagraphWithPage>();

            using var document = DocX.Load(filePath);

            foreach (var paragraph in document.Paragraphs)
            {
                var text = paragraph.Text;

                if (skipEmpty && string.IsNullOrWhiteSpace(text.Trim()))
                    continue;

                result = result.Append(new ParagraphWithPage
                {
                    Page = 0,
                    Text = text
                });
            }

            return result;
        }
    }
}
