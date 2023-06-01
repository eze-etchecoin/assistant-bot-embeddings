namespace AssistantBot.Common.Interfaces
{
    public interface IDiskPersistor<T> where T : class
    {
        Task SaveAsync(T obj);
        Task<T> LoadAsync();
    }
}
