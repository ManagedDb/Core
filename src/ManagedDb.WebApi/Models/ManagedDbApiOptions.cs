namespace ManagedDb.WebApi.Models
{
    public class ManagedDbApiOptions
    {
        public const string ConfigKey = "ManagedDbApi";

        public string? DbPath { get; set; }

        public string? EntityListPath { get; set; }
    }
}
