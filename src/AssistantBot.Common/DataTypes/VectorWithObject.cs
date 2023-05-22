using AssistantBot.Common.Interfaces;

namespace AssistantBot.Common.DataTypes
{
    public class VectorWithObject : IVectorWithObject
    {
        public object Data { get; set; }
        public double[] Values { get; set; }
    }
}
