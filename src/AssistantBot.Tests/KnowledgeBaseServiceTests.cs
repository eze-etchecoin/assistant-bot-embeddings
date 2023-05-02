using AssistantBot.DataTypes;
using AssistantBot.Services;
using AssistantBot.Services.Factories;
using AssistantBot.Services.RedisStorage;
using Xunit.Abstractions;

namespace AssistantBot.Tests
{
    public class KnowledgeBaseServiceTests
    {
        private readonly KnowledgeBaseService _service;
        private readonly ITestOutputHelper _outputHelper;

        public KnowledgeBaseServiceTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _service = new KnowledgeBaseService(
                new ChatBotServiceFactory().CreateService(ChatBotServiceOption.ChatGpt),
                new RedisVectorStorageService<EmbeddedTextVector>("localhost:6379"));
        }

        [Fact]
        public async Task AddParagraphToKnowledgeBase_PopulateTestBase()
        {
            var paragraphs = new List<string>
            {
                // enter text here
            };

            foreach(var paragraph in paragraphs)
            {
                _ = await _service.AddParagraphToKnowledgeBase(paragraph);
            }
        }
    }
}
