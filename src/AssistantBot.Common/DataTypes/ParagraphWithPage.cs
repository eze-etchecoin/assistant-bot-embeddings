using AssistantBot.Common.Interfaces;

namespace AssistantBot.Common.DataTypes
{
    public struct ParagraphWithPage : IParagraphWithPage
    {
        public ParagraphWithPage(int page, string text)
        {
            Page = page;
            Text = text;
        }

        public int Page { get; set; }
        public string Text { get; set; }
    }
}
