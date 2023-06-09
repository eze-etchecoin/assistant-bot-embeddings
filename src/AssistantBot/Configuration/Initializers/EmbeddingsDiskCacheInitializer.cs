﻿using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;
using AssistantBot.Services.Cache;
using AssistantBot.Utils;
using System.IO.Compression;

namespace AssistantBot.Configuration.Initializers
{
    public static class EmbeddingsDiskCacheInitializer
    {
        public static async Task LoadEmbeddingsIntoCache(IServiceProvider serviceProvider)
        {
            var inDiskCache = serviceProvider.GetService<InDiskCache<Dictionary<string, byte[]>>>();

            var embeddings = await inDiskCache.LoadAsync();
            if (embeddings.Count == 0)
                return;

            var customCache = serviceProvider.GetService<IIndexedVectorStorage<EmbeddedTextVector>>();

            foreach(var embedding in embeddings)
            {
                customCache.AddVector(new EmbeddedTextVector
                {
                    ParagraphWithPage = new ParagraphWithPage(1, embedding.Key),
                    Values = CompressedDataHelper.CompressedByteToDoubleArray(embedding.Value)
                });
            }
        }
    }
}
