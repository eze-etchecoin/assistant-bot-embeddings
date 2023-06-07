using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;
using AssistantBot.Services.Cache;

namespace AssistantBot.Configuration.Initializers
{
    public static class EmbeddingsDiskCacheInitializer
    {
        public static async Task LoadEmbeddingsIntoCache(IServiceProvider serviceProvider)
        {
            var inDiskCache = serviceProvider.GetService<InDiskCache<Dictionary<string, double[]>>>();

            var embeddings = await inDiskCache.LoadAsync();
            if (embeddings.Count == 0)
                return;

            var customCache = serviceProvider.GetService<IIndexedVectorStorage<EmbeddedTextVector>>();

            foreach(var embedding in embeddings)
            {
                customCache.AddVector(new EmbeddedTextVector
                {
                    ParagraphWithPage = new ParagraphWithPage(1, embedding.Key),
                    Values = embedding.Value
                });
            }
        }
    }
}
