using AssistantBot.Common.Interfaces;
using AssistantBot.Configuration;
using AssistantBot.Services.Integrations;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Xunit.Abstractions;

namespace AssistantBot.Tests
{
    public class IndexedVectorStorageTests
    {
        private readonly IIndexedVectorStorage<TestVector> _service;
        private readonly ITestOutputHelper _testOutput;

        public IndexedVectorStorageTests(ITestOutputHelper testOutput)
        {
            //_service = new RedisVectorStorageService<TestVector>("localhost:6379");
            _service = new CustomMemoryStorageService<TestVector>(
                new AssistantBotConfiguration(Options.Create(new AssistantBotConfigurationOptions
                {
                    CustomCacheUrl = "http://localhost:61226"
                })));

            _testOutput = testOutput;
        }

        [Fact]
        public void AddVector_StringData()
        {
            var testData = "hello world";
            
            var vector = new TestVector
            {
                Data = testData,
                Values = GetVectorValues()
            };

            var storedKey = _service.AddVector(vector);

            var storedData = _service.GetDataByKey(storedKey);
            Assert.NotNull(storedData);

            Assert.Equal(testData, storedData);
        }

        [Fact]
        public void AddVector_JsonData()
        {
            var testObject = new DataTestClass
            {
                Prop1 = "value1",
                Prop2 = "value2",
                Prop3 = "value3"
            };

            var testJson = JsonConvert.SerializeObject(testObject);

            var vector = new TestVector
            {
                Data = testJson,
                Values = GetVectorValues()
            };

            var storedKey = _service.AddVector(vector);

            var storedData = _service.GetDataByKey(storedKey);
            var jsonObject = JsonConvert.DeserializeObject<DataTestClass>(storedData);

            Assert.NotNull(jsonObject);

            Assert.Equal(testObject.Prop1, jsonObject.Prop1);
            Assert.Equal(testObject.Prop2, jsonObject.Prop2);
            Assert.Equal(testObject.Prop3, jsonObject.Prop3);
        }

        [Fact]
        public void ConnectionTest()
        {
            var result = _service.TestConnection();
            _testOutput.WriteLine(result);
        }

        [Fact]
        public void GetKeysTest()
        {
            var result = _service.GetKeys();

            foreach(var key in result)
            {
                _testOutput.WriteLine(key);
            }
        }

        [Fact]
        public void DeleteAllKeysTest()
        {
            _service.DeleteAllKeys();
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

    public class DataTestClass
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
        public string Prop3 { get; set; }
    }
}
