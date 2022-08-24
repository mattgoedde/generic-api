namespace GenericApi.Data.Models.v1
{
    public record Person : Record
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
