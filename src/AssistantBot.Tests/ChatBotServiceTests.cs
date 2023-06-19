using AssistantBot.Services.Factories;
using AssistantBot.Common.Interfaces;
using Xunit.Abstractions;
using AssistantBot.Services.Cache;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using AssistantBot.Api.Models.AssistantBot;

namespace AssistantBot.Tests
{
    public class ChatBotServiceTests
    {
        private readonly IChatBotService _service;
        private readonly ITestOutputHelper _output;


        public ChatBotServiceTests(ITestOutputHelper output)
        {
            _service = new ChatBotServiceFactory(new InDiskCache<Dictionary<string, double[]>>())
                .CreateService(ChatBotServiceOption.ChatGpt);

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

            var result = await _service.GetEmbedding(text);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.All(x => x != 0));

            _output.WriteLine(
                string.Join(", ", result.Take(3).Select(x => x.ToString())) + 
                ", ...");
        }

        [Fact]
        public async Task SendMessageWithContextTest()
        {
            var message = "Cómo es mi nombre? Lo recuerdas?";

            var answer = await _service.SendMessage(message, new List<AssistantBotMessage>
            {
                new AssistantBotMessage("", AssistantBotRole.User, "Mi nombre es Roberto"),
                new AssistantBotMessage("", AssistantBotRole.Assistant, "Hola Roberto, un gusto conocerte"),
                new AssistantBotMessage("", AssistantBotRole.User, "Tengo 30 años y vivo en Argentina."),
                new AssistantBotMessage("", AssistantBotRole.Assistant, "Argentina es un país muy lindo sin dudas.")
            });

            Assert.NotNull(answer);
            Assert.NotEmpty(answer);

            _output.WriteLine($"Message: {message}");
            _output.WriteLine($"Answer: {answer}");
        }

        [Fact]
        public async Task SendMessageWithSystemContextTest()
        {
            var message = "No gracias. Te acuerdas cómo es mi nombre? Me lo puedes decir?";

            var answer = await _service.SendMessage(message, new List<AssistantBotMessage>
            {
                new AssistantBotMessage("", AssistantBotRole.System, "Contestarás las preguntas del usuario con un tono humorístico."),
                new AssistantBotMessage("", AssistantBotRole.User, "Buenos días! Cómo estas? Mi nombre es Roberto."),
                new AssistantBotMessage("", AssistantBotRole.Assistant, "¡Hola Roberto! ¿Cómo crees que estoy? Soy una inteligencia artificial, no tengo emociones... pero gracias por preguntar igualmente. ¿En qué puedo ayudarte?"),
                new AssistantBotMessage("", AssistantBotRole.User, "Pueden las inteligencias artificiales cantar?"),
                new AssistantBotMessage("", AssistantBotRole.Assistant, "¡Por supuesto! ¿Quieres escuchar una canción interpretada por una inteligencia artificial? Pero no te prometo que sea un éxito en las listas de éxitos. Tal vez la IA tenga una futura carrera como cantante de karaoke, pero aún no estará llenando estadios.")
            });

            Assert.NotNull(answer);
            Assert.NotEmpty(answer);

            _output.WriteLine($"Message: {message}");
            _output.WriteLine($"Answer: {answer}");
        }
    }
}