using AssistantBot.Services.Factories;
using AssistantBot.Services.Interfaces;
using Xunit.Abstractions;

namespace AssistantBot.Tests
{
    public class ChatBotServiceTests
    {
        private readonly IChatBotService _service;
        private readonly ITestOutputHelper _output;


        public ChatBotServiceTests(ITestOutputHelper output)
        {
            _service = new ChatBotServiceFactory().CreateService(ChatBotServiceOption.ChatGpt);

            _output = output;
        }

        [Fact]
        public async Task SendMessageTest()
        {
            // data preparation
            var message = "hola, como estás?";

            // method execution
            var answer = await _service.SendMessage(message);

            // assertions
            Assert.NotNull(answer);
            Assert.NotEmpty(answer);

            _output.WriteLine($"Message: {message}");
            _output.WriteLine($"Answer: {answer}");
        }

        [Fact]
        public async Task GetEmbeddingsTest()
        {
            var text = "El veloz murciélago hindú comía feliz cardillo y kiwi. " +
                "La cigüeña toca el saxofón detrás del palenque de paja. " +
                "1234567890";

            var result = await _service.GetEmbeddings(text);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x != 0));

            _output.WriteLine(
                string.Join(", ", result.Take(3).Select(x => x.ToString())) + 
                ", ...");
        }
    }
}