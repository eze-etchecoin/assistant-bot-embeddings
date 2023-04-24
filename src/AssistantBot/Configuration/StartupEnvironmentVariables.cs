namespace AssistantBot.Configuration
{
    public static class StartupEnvironmentVariables
    {
        private const string _preffix = "QA_BOT_";
        // ------------------------------------- //
        public static string OpenAIApiKey => _preffix + "OPENAI_APIKEY"; // QA_BOT_OPENAI_APIKEY

        // Add more variables if needed, ...
    }
}
