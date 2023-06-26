namespace AssistantBot.Common.Interfaces
{
    public interface IDocumentToTextProcessor
    {
        string ConvertDocumentToString(string filePath);
        IEnumerable<IParagraphWithPage> GetParagraphsTextWithPageNumber(string filePath, bool skipEmpty = true);
        int GetNumberOfParagraphs(string filePath);
    }
}
