namespace GenericApi.Data.Models.v1
{
    public interface IRecord : IEquatable<IRecord>
    {
        public int Id { get; set; }
    }

    public abstract record Record : IRecord
    {
        public int Id { get; set; }

        public bool Equals(IRecord? other)
        {
            return other switch
            {
                Record r => Id == r.Id,
                _ => false,
            };
        }

        public static bool Equals(Record left, Record right) => left.Equals(right);
    }
}
