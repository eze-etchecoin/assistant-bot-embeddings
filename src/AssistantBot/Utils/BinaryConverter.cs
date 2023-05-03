namespace AssistantBot.Utils
{
    public static class BinaryConverter
    {
        public static byte[] ConvertDoubleArrayToBinary(double[] array)
        {
            byte[] binaryArray;

            using (MemoryStream memoryStream = new())
            {
                foreach (var num in array)
                {
                    byte[] binaryDouble = BitConverter.GetBytes(num);
                    memoryStream.Write(binaryDouble, 0, binaryDouble.Length);
                }
                binaryArray = memoryStream.ToArray();
            }

            return binaryArray;
        }
    }
}
