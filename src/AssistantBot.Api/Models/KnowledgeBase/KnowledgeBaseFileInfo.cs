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

        public KnowledgeBaseFileInfo(string fileName, int totalParagraphs, DateTimeOffset uploadedDateTime) 
            : this(fileName, totalParagraphs)
        {
            UploadedDateTime = uploadedDateTime;
        }

        public string FileName { get; set; }
        public DateTimeOffset UploadedDateTime { get; set; }
        public int TotalParagraphs { get; set; }
        public int ProcessedParagraphs { get; set; }
        public string? ErrorMessage { get; set; }

        public int Progress
        { 
            get => TotalParagraphs == 0 
                ? 100 
                : (int)Math.Round((double)ProcessedParagraphs / TotalParagraphs * 100);
            set { }
        }
    }
}
