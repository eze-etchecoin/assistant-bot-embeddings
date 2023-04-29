namespace AssistantBot.Services.Interfaces
{
    public interface IIndexedVectorStorage<T> : IRemoteMemoryStorage where T : IVectorWithObject
    {
        int VectorSize { get; }

        string AddVector(T vector);
        void RemoveVector(T vector);
        bool Contains(T vector);
        IList<object> SearchDataBySimilarVector(T vectorToSearch, int numResults = 1);
    }
}
