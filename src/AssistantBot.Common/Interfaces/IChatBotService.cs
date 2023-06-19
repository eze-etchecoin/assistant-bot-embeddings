namespace AssistantBot.Common.Interfaces
{
    public interface IChatBotService
    {
        Task<string> SendMessage(string message);
        Task<string> SendMessage(string message, IEnumerable<IChatBotMessage> contextMessages);

        Task<string> SendTrainingInput(ITrainingDataModel dataModel);
        Task<string> SendTrainingInput(IEnumerable<ITrainingDataModel> dataModels);

        Task<IEnumerable<double>> GetEmbedding(string textToTransform, bool ignoreCache = false);
    }
}
