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
    }
}
