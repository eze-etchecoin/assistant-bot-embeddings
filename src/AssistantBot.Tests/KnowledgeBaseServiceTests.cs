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
    public class KnowledgeBaseServiceTests
    {
        private readonly KnowledgeBaseService _service;

        private readonly AssistantBotConfiguration _config = new(
            Options.Create(new AssistantBotConfigurationOptions
            {
                CustomCacheUrl = "https://localhost:44328",
                UploadedFilesFolderPath = Path.Combine(".", "UploadedFiles")
            }));

        private readonly ITestOutputHelper _outputHelper;

        public KnowledgeBaseServiceTests(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
            _service = new KnowledgeBaseService(
                new ChatBotServiceFactory(new InDiskCache<Dictionary<string, double[]>>())
                    .CreateService(ChatBotServiceOption.ChatGpt),

                //new RedisVectorStorageService<EmbeddedTextVector>("localhost:6379")
                new CustomMemoryStorageService<EmbeddedTextVector>(_config),
                _config);
        }

        [Fact]
        public async Task AddParagraphToKnowledgeBase_PopulateTestBase_Short()
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

        [Fact]
        public async Task AddParagraphToKnowledgeBase_PopulateTestBase_Long()
        {
            var paragraphs = new List<string>
            {
                "Había una vez un niño llamado Facundo, vivía en un pequeño pueblo en las sierras de Córdoba, Argentina.",
                "Su casa estaba rodeada de montañas altas, y desde pequeño, siempre sentía una conexión especial con la naturaleza.",
                "Un día, mientras jugaba en los campos que bordeaban las montañas, siguió a una mariposa de colores brillantes hasta que se dio cuenta de que estaba más adentro de lo que jamás había estado.",
                "La mariposa se esfumó y Facundo se encontró solo, perdido en la inmensidad de las sierras.",
                "Comenzó a caminar sin rumbo, tratando de encontrar el camino de vuelta.",
                "Pero las montañas parecían todas iguales. A medida que caía la noche, Facundo comenzó a sentir miedo.",
                "De repente, escuchó un ruido entre los arbustos.",
                "Un zorro de pelaje rojizo apareció ante él. Facundo, sorprendido, le explicó su situación.",
                "El zorro, con ojos sabios, le indicó que siguiera la dirección del viento que soplaba desde el este, pues siempre llevaba hacia el pueblo.",
                "Facundo agradeció al zorro y siguió su consejo.",
                "Caminó durante horas hasta que se encontró en un arroyo, pero no sabía si debía cruzarlo o seguir su curso.",
                "De las aguas emergió una nutria, quien le aseguró que cruzar el arroyo sería seguro y le acercaría a su hogar.",
                "Facundo, agotado pero determinado, cruzó el arroyo y continuó su viaje.",
                "Finalmente, se encontró frente a un gran peñasco.",
                "Sabía que su pueblo estaba más allá, pero no podía escalarlo.",
                "En ese momento, un cóndor majestuoso voló por encima de él y le indicó un sendero oculto que bordeaba el peñasco.",
                "Siguiendo el camino que el cóndor le había mostrado, Facundo finalmente vio las luces de su pueblo a lo lejos.",
                "Con lágrimas de alegría en los ojos, corrió hacia su hogar.",
                "Desde aquel día, Facundo nunca se aventuró lejos sin recordar las lecciones de los animales de las sierras de Córdoba.",
                "reció con un profundo respeto por la naturaleza y la sabiduría que albergaba, y se convirtió en un protector de las montañas y de todas las criaturas que en ellas habitaban.",
                "Y siempre que se perdía, sabía que los amigos que había hecho en la montaña estarían allí para guiarlo de regreso a casa."
            };

            foreach (var paragraph in paragraphs)
            {
                _ = await _service.AddParagraphToKnowledgeBase(paragraph);
            }
        }
    }
}
