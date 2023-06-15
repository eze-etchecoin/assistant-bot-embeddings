using System.IO.Compression;

namespace AssistantBot.Utils
{
    public static class CompressedDataHelper
    {
        public static byte[] DoubleArrayToCompressedByte(double[] doubleArray)
        {
            using var memoryStream = new MemoryStream();
            using var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress);
            using var binaryWriter = new BinaryWriter(gZipStream);

            for (int i = 0; i < doubleArray.Length; i++)
            {
                binaryWriter.Write(doubleArray[i]);
            }

            return memoryStream.ToArray();
        }

        public static double[] CompressedByteToDoubleArray(byte[] compressedByte)
        {
            using var memoryStream = new MemoryStream(compressedByte);
            using var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
            using var binaryReader = new BinaryReader(gZipStream);

            var resultList = new List<double>();
            while (true)
            {
                try
                {
                    var read = binaryReader.ReadDouble();
                    resultList.Add(read);
                }
                catch(EndOfStreamException)
                {
                    break;
                }
            }
            return resultList.ToArray();
        }
    }
}
