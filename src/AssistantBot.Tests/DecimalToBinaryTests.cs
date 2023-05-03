using Xunit.Abstractions;

namespace AssistantBot.Tests
{
    public class DecimalToBinaryTests
    {
        private readonly ITestOutputHelper _outputHelper;

        public DecimalToBinaryTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public void ConvertDoubleArrayToBinaryString()
        {
            var array = new[] { 0.123, 0.333, -0.123, 1 };
            byte[] binaryArray;

            using (MemoryStream memoryStream = new())
            {
                foreach (var num in array)
                {
                    byte[] binaryDouble = BitConverter.GetBytes(num);
                    memoryStream.Write(binaryDouble, 0, binaryDouble.Length);
                }
                binaryArray = memoryStream.ToArray();
            }

            string hexString = BitConverter.ToString(binaryArray);

            _outputHelper.WriteLine(hexString);
        }
    }
}
