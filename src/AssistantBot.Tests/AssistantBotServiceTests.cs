using AssistantBot.DataTypes;
using AssistantBot.Services;
using AssistantBot.Services.Factories;
using AssistantBot.Services.RedisStorage;
using Xunit.Abstractions;

namespace AssistantBot.Tests
{
    public class AssistantBotServiceTests
    {
        private readonly AssistantBotService _service;
        private readonly ITestOutputHelper _outputHelper;

        public AssistantBotServiceTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _service = new AssistantBotService(
                new ChatBotServiceFactory().CreateService(ChatBotServiceOption.ChatGpt),
                new RedisVectorStorageService<EmbeddedTextVector>("localhost:6379"));
        }

        [Fact]
        public async Task AskToKnowledgeBase_ValidQuestion()
        {
            var result = await _service.AskToKnowledgeBase("de qué color son las rosas?");

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            _outputHelper.WriteLine(result);
        }
    }
}
