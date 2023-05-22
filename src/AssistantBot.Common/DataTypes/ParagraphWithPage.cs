using AssistantBot.Common.Interfaces;

namespace AssistantBot.Common.DataTypes
{
    public struct ParagraphWithPage : IParagraphWithPage
    {
        private readonly int _page;
        private readonly string _text;

        public ParagraphWithPage(int page, string text)
        {
            _page = page;
            _text = text;
        }

        public int Page { get { return _page; } }
        public string Text { get { return _text; } }
    }
}
