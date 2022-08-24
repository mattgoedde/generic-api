namespace GenericApi.Api.Models.v1
{
    public class RestResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
        public IEnumerable<T> SuccessfulRecords { get; set; } = Enumerable.Empty<T>();
        public IEnumerable<T> FailedRecords { get; set; } = Enumerable.Empty<T>();
        public IEnumerable<string> Exceptions { get; set; } = Enumerable.Empty<string>();
    }
}