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
    }

    public class AssistantBotConfigurationOptions
    {
        public AssistantBotConfigurationOptions()
        {
            CustomCacheUrl = "localhost";
        }

        public string CustomCacheUrl { get; set; }
    }
}
