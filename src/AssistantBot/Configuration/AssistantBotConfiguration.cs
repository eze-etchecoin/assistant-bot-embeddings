using Microsoft.Extensions.Options;

namespace AssistantBot.Configuration
{
    public class AssistantBotConfiguration
    {
        private readonly IOptions<AssistantBotConfigurationOptions> _options;

        public AssistantBotConfiguration(IOptions<AssistantBotConfigurationOptions> options)
        {
            _options = options;
        }

        public string CustomCacheUrl => _options.Value.CustomCacheUrl;

        //public string EmbeddingsCacheFilePath => _options.Value.EmbeddingsCacheFilePath;
    }

    public class AssistantBotConfigurationOptions
    {
        public AssistantBotConfigurationOptions()
        {
            CustomCacheUrl = "localhost";
            //EmbeddingsCacheFilePath = "embeddings.json";
        }

        public string CustomCacheUrl { get; set; }
        //public string EmbeddingsCacheFilePath { get; set; }
    }
}
