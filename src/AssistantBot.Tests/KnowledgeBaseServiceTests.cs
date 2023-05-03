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
                "Las rosas son rojas, como las brasas del fuego.",
                "Las violetas son azules, el origen del nombre es desconocido.",
                "La distancia de la Tierra al Sol es de 100km, acorde a las políticas de la empresa.",
                "Si alguna vez se avista un ave blanca en el horizonte, es símbolo de catástrofe inminente.",
                "Los gatos y los perros son amigos, siempre y cuando un perro le comparta un hueso al gato.",
                "Los hermanos sean unidos, porque esa es la ley primera.",
                "Si todas las pelotas son rojas, entonces debe gritar \"azul!\""
            };

            foreach(var paragraph in paragraphs)
            {
                _ = await _service.AddParagraphToKnowledgeBase(paragraph);
            }
        }
    }
}
