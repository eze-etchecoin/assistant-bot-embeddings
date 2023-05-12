using AssistantBot.Common.Interfaces;
using AssistantBot.Services.DocumentConverter;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace AssistantBot.Tests
{
    public class DocumentToTextConverterTests
    {
        private readonly IDocumentToTextConverter _converter;
        private readonly ITestOutputHelper _output;

        public DocumentToTextConverterTests(ITestOutputHelper output)
        {
            _converter = new DocumentConverterService();
            _output = output;
        }

        [Fact]
        public void ConvertPdfToPlainText()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "TestFiles", "pedrito-test.pdf");

            var text = _converter.ConvertDocumentToString(filePath);

            Assert.NotNull(text);
            Assert.NotEmpty(text);

            _output.WriteLine(text);

            Assert.True(
                text.Split(" ")
                    .Count(x => x == "Pedrito") == 2);
        }

        [Fact]
        public void GetParagraphsFromPdfSkippingEmpty()
        {
            var filePath = Path.Combine(AppContext.BaseDirectory, "TestFiles", "pedrito-test-paragraphs.pdf");

            var paragraphs = _converter.GetParagraphsTextWithPageNumber(filePath);

            Assert.NotNull(paragraphs);
            Assert.NotEmpty(paragraphs);

            _output.WriteLine(string.Join("\n", paragraphs.Select(x => $"({x.Page}) {x.Text}")));

            Assert.True(paragraphs.Count() == 7);

            // All paragraphs with text.
            Assert.True(paragraphs.Select(x => x.Text).All(x => !string.IsNullOrEmpty(x)));

            // Assert number of paragraphs per page.
            Assert.True(paragraphs.Where(x => x.Page == 1).Count() == 4);
            Assert.True(paragraphs.Where(x => x.Page == 2).Count() == 2);
            Assert.True(paragraphs.Where(x => x.Page == 3).Count() == 1);
        }
    }
}
