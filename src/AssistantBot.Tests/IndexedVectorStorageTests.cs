using AssistantBot.Services.Interfaces;
using AssistantBot.Services.RedisStorage;
using Xunit.Abstractions;

namespace AssistantBot.Tests
{
    public class IndexedVectorStorageTests
    {
        private readonly IIndexedVectorStorage<TestVector> _service;
        private readonly ITestOutputHelper _testOutput;

        public IndexedVectorStorageTests(ITestOutputHelper testOutput)
        {
            _service = new RedisVectorStorageService<TestVector>("localhost:6379");
            _testOutput = testOutput;
        }

        [Fact]
        public void AddVector_ValidVector()
        {
            var vector = new TestVector
            {
                Data = "hello world",
                Values = GetVectorValues()
            };

            var storedKey = _service.AddVector(vector);

            var storedData = _service.GetDataByKey(storedKey);
            Assert.NotNull(storedData);

            var storedVector = (TestVector)storedData;
            Assert.Equal(vector.Data, storedVector.Data);
        }

        [Fact]
        public void ConnectionTest()
        {
            var result = _service.TestConnection();
            _testOutput.WriteLine(result);
        }

        private double[] GetVectorValues()
        {
            var rnd = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray()));
            return Enumerable.Range(0, _service.VectorSize)
                .Select(x => rnd.NextDouble())
                .ToArray();
        }
    }

    public class TestVector : IVectorWithObject
    {
        public object Data { get; set; }
        public double[] Values { get; set; }
    }
}
