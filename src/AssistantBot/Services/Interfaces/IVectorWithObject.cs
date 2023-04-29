namespace AssistantBot.Services.Interfaces
{
    public interface IVectorWithObject : IVector
    {
        object Data { get; set; }
    }
}
