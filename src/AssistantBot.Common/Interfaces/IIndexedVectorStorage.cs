namespace AssistantBot.Common.Interfaces
{
    public interface IIndexedVectorStorage<T> : IRemoteMemoryStorage<T> where T : IVectorWithObject
    {
        int VectorSize { get; }

        string AddVector(T vector);
        void RemoveVector(T vector);
        bool Contains(T vector);
        IList<TResult> SearchDataBySimilarVector<TResult>(T vectorToSearch, int numResults = 1);
    }
}
