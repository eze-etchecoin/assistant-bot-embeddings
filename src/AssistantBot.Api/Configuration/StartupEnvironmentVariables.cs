namespace AssistantBot.Configuration
{
    public static class StartupEnvironmentVariables
    {
        private const string _preffix = "QA_BOT_";
        // ------------------------------------- //
        public static string OpenAIApiKey => _preffix + "OPENAI_APIKEY"; // QA_BOT_OPENAI_APIKEY
        public static string RedisServerUrl => _preffix + "REDIS_URL";
        // Add more variables if needed, ...
    }
}
