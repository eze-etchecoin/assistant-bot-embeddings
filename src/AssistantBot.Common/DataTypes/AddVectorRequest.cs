namespace AssistantBot.Common.DataTypes
{
    public class AddVectorRequest
    {
        public AddVectorRequest()
        {
            Vector = new EmbeddedTextVector();
        }

        public EmbeddedTextVector Vector { get; set; }
        public string? KeyComplementStr { get; set; }
    }
}
