namespace Common.Contracts
{
    public class CommonAppSettings
    {
        public IReadOnlyList<string> RedactedFields { get; set; } // redacted fields
        public string ServerID { get; set; }
        public string ServiceID { get; set; }
    }
}
