using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace AssistantBot.Tests
{
    public class ExamplesTests
    {
        private readonly List<string> stringListToOrder;
        private readonly List<int> intListToOrder;

        private readonly ITestOutputHelper _outputHelper;

        public ExamplesTests(ITestOutputHelper outputHelper)
        {
            stringListToOrder = new List<string>
            {
                "300", "3456", "34", "3555", "30", "35", "351"
            };

            intListToOrder = stringListToOrder
                .Select(x => int.Parse(x))
                .ToList(); // 300,3456...

            _outputHelper = outputHelper;
        }

        [Fact]
        public void StringOrdering()
        {
            var orderedStrings = stringListToOrder.OrderBy(x => x);

            foreach(var @string in orderedStrings)
            {
                _outputHelper.WriteLine(@string);
            }
        }

        [Fact]
        public void NumericOrdering()
        {
            var orderedNumbers = intListToOrder.OrderBy(x => x);

            foreach (var @int in orderedNumbers)
            {
                _outputHelper.WriteLine(@int.ToString());
            }
        }

        [Fact]
        public void IntVectorToBytesConversion()
        {
            int[] vector = { 18, 169, 245, 108 };
            byte[] bytes = new byte[vector.Length];

            for (int i = 0; i < vector.Length; i++)
            {
                bytes[i] = (byte)vector[i];
            }

            string hexadecimal = BitConverter.ToString(bytes);

            _outputHelper.WriteLine(hexadecimal);
        }

        [Fact]
        public void DoubleVectorToBytesConversion()
        {
            double[] vector = { 18.5, 169.2, 245.8, 108.3 };
            byte[] bytes = new byte[vector.Length * sizeof(double)];

            Buffer.BlockCopy(vector, 0, bytes, 0, bytes.Length);

            string hexadecimal = BitConverter.ToString(bytes);

            _outputHelper.WriteLine(hexadecimal);

            var sbX = new StringBuilder();
            foreach (byte b in bytes)
            {
                sbX.Append("\\x" + b.ToString("X2"));
            }
            _outputHelper.WriteLine(sbX.ToString());
        }
    }
}
