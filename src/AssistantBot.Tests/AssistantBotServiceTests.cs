using AssistantBot.Common.DataTypes;
using AssistantBot.Configuration;
using AssistantBot.Services;
using AssistantBot.Services.Cache;
using AssistantBot.Services.Factories;
using AssistantBot.Services.Integrations;
using Microsoft.Extensions.Options;
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
                new ChatBotServiceFactory(new InDiskCache<Dictionary<string, byte[]>>())
                    .CreateService(ChatBotServiceOption.ChatGpt),

                //new RedisVectorStorageService<EmbeddedTextVector>("localhost:6379")
                new CustomMemoryStorageService<EmbeddedTextVector>(
                    new AssistantBotConfiguration(
                        Options.Create(new AssistantBotConfigurationOptions
                        {
                            CustomCacheUrl = "https://localhost:44328"
                        }))));
        }

        [Fact]
        public async Task AskToKnowledgeBase_ValidQuestion()
        {
            var result = await _service.AskToKnowledgeBase("En dónde vivía Facundo?");

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            _outputHelper.WriteLine(result);
        }
    }
}
