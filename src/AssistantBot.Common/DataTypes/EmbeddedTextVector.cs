using AssistantBot.Common.Interfaces;

namespace AssistantBot.Common.DataTypes
{
    public class EmbeddedTextVector : IVectorWithObject
    {
        public ParagraphWithPage ParagraphWithPage { get; set; }
        public double[] Values { get; set; }


        object IVectorWithObject.Data 
        { 
            get => ParagraphWithPage; 
            set => ParagraphWithPage = (ParagraphWithPage)value; 
        }
    }
}
