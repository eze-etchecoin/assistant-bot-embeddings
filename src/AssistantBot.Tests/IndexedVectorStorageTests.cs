using AssistantBot.Common.DataTypes;
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
        private readonly IIndexedVectorStorage<EmbeddedTextVector> _service;
        private readonly ITestOutputHelper _testOutput;

        public IndexedVectorStorageTests(ITestOutputHelper testOutput)
        {
            //_service = new RedisVectorStorageService<TestVector>("localhost:6379");
            _service = new CustomMemoryStorageService<EmbeddedTextVector>(
                new AssistantBotConfiguration(Options.Create(new AssistantBotConfigurationOptions
                {
                    CustomCacheUrl = "https://localhost:44328"
                })));

            _testOutput = testOutput;
        }

        //[Fact]
        //public void AddVector_StringData()
        //{
        //    var testData = "hello world";
            
        //    var vector = new TestVector
        //    {
        //        Data = testData,
        //        Values = GetVectorValues()
        //    };

        //    var storedKey = _service.AddVector(vector);

        //    var storedData = _service.GetDataByKey(storedKey);
        //    Assert.NotNull(storedData);

        //    Assert.Equal(testData, storedData);
        //}

        [Fact]
        public void AddVector_EmbeddedTextVector()
        {
            var rnd = new Random();
            
            var vector = new EmbeddedTextVector
            {
                ParagraphWithPage = new ParagraphWithPage(1, $"Test{rnd.Next(1_000_000)}"),
                Values = GetVectorValues()
            };

            var storedHash = _service.AddVector(vector);

            var storedData = _service.GetDataByKey(storedHash);
            var jsonObject = JsonConvert.DeserializeObject<ParagraphWithPage>(storedData);

            Assert.NotNull(jsonObject);

            Assert.Equal(vector.ParagraphWithPage.Text, jsonObject.Text);
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
                _testOutput.WriteLine(key.ToString());
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
