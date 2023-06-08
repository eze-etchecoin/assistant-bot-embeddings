namespace AssistantBot.Models.KnowledgeBase
{
    public class KnowledgeBaseFileInfo
    {
        public KnowledgeBaseFileInfo()
        {
            UploadedDateTime = DateTimeOffset.UtcNow;
            FileName = "";
        }

        public KnowledgeBaseFileInfo(string fileName, int totalParagraphs) : this()
        {
            FileName = fileName;
            TotalParagraphs = totalParagraphs;
            ProcessedParagraphs = 0;
        }

        public string FileName { get; set; }
        public DateTimeOffset UploadedDateTime { get; set; }
        public int TotalParagraphs { get; set; }
        public int ProcessedParagraphs { get; set; }
    }
}
