namespace AssistantBot.Common.Helpers
{
    public static class HashCodeHelper
    {
        public static int CalculateHashCode(IEnumerable<double> values) =>
            values.Aggregate(
                new HashCode(),
                (hash, value) =>
                {
                    hash.Add(value);
                    return hash;
                })
                .ToHashCode();
    }
}
