namespace edson268_landing_page_api.Core.DTOs
{
    public record CreateCertificateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string CredentialUrl { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
    }
}
