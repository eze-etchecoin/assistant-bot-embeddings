namespace AssistantBot.Common.Interfaces
{
    public interface IChatBotMessage
    {
        DateTimeOffset DateTime { get; }
        string Role { get; }
        string Content { get; }
        string User { get; }
    }
}
