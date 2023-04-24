namespace AssistantBot.Services.Interfaces
{
    public interface ITrainingDataModel
    {
        string Prompt { get; set; }
        string IdealAnswer { get; set; }
    }
}
