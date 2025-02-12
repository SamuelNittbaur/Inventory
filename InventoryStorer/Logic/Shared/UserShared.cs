namespace Logic.Shared
{
    public record class UserConfiguration
    {
        public Guid id { get; set; } = Guid.Empty;
        public string firstName { get; set; } = String.Empty;
        public string lastName { get; set; } = String.Empty;
        public string email { get; set; } = string.Empty;
        public string userName { get; set; } = string.Empty;
        public string paymentId { get; set; } = string.Empty;
        public List<string> categories { get; set; } = new List<string>();
        public string company { get; set; } = string.Empty;
        public bool paid { get; set; } = false;
        public DateTime joinDate { get; set; } = DateTime.MinValue;
        public DateTime lastPasswordChange { get; set; } = DateTime.MinValue;
    }
}
