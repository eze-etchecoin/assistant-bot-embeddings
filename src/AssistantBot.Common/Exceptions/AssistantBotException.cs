namespace AssistantBot.Common.Exceptions
{
    public class AssistantBotException : Exception
    {
        public AssistantBotException(string message) 
            : base(message) { }

        public AssistantBotException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
