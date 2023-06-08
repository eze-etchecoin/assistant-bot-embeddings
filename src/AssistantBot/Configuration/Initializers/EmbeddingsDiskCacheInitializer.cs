using AssistantBot.Common.DataTypes;
using AssistantBot.Common.Interfaces;
using AssistantBot.Services.Cache;
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
                    Values = CompressedByteToDoubleArray(embedding.Value)
                });
            }
        }
        private static double[] CompressedByteToDoubleArray(byte[] compressedByte)
        {
            var doubleList = new List<double>();
            using (var memoryStream = new MemoryStream(compressedByte))
            {
                using var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
                using var binaryReader = new BinaryReader(gZipStream);
                while (gZipStream.Position < gZipStream.Length)
                    doubleList.Add(binaryReader.ReadDouble());
            }
            return doubleList.ToArray();
        }
    }
}
