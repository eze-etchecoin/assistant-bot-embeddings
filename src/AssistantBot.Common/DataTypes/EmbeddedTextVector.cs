using AssistantBot.Common.Interfaces;
using Newtonsoft.Json;

namespace AssistantBot.Common.DataTypes
{
    public class EmbeddedTextVector : IVectorWithObject
    {
        public EmbeddedTextVector()
        {
            Values = Array.Empty<double>();
            ParagraphWithPage = new ParagraphWithPage();
        }

        public EmbeddedTextVector(double[] values, ParagraphWithPage paragraphWithPage)
        {
            Values = values;
            ParagraphWithPage = paragraphWithPage;
        }

        public EmbeddedTextVector(double[] values, string jsonParagraphWithPage)
        {
            Values = values;
            ParagraphWithPage = JsonConvert.DeserializeObject<ParagraphWithPage>(jsonParagraphWithPage);
        }

        public ParagraphWithPage ParagraphWithPage { get; set; }
        public double[] Values { get; set; }


        object IVectorWithObject.Data 
        { 
            get => ParagraphWithPage; 
            set => ParagraphWithPage = (ParagraphWithPage)value; 
        }
    }
}
