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

        public string DemoUrl => _options.Value.DemoUrl;
    }

    public class AssistantBotConfigurationOptions
    {
        public AssistantBotConfigurationOptions()
        {
            CustomCacheUrl = "localhost";
            DemoUrl = "";
        }

        public string CustomCacheUrl { get; set; }
        public string DemoUrl { get; set; }
    }
}
