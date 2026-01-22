namespace edson268_landing_page_api.Core.Entities
{
    public class Certificate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Institution { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string CredentialUrl { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Certificate()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
