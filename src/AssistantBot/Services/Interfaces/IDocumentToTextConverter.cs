using AssistantBot.DataTypes;

namespace AssistantBot.Services.Interfaces
{
    public interface IDocumentToTextConverter
    {
        string ConvertDocumentToString(string filePath);
        IEnumerable<ParagraphWithPage> GetParagraphsTextWithPageNumber(string filePath, bool skipEmpty = true);
    }
}
