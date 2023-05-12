namespace AssistantBot.Common.Interfaces
{
    public interface IDocumentToTextConverter
    {
        string ConvertDocumentToString(string filePath);
        IEnumerable<IParagraphWithPage> GetParagraphsTextWithPageNumber(string filePath, bool skipEmpty = true);
    }
}
