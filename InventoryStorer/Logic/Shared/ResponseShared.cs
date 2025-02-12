namespace Logic.Shared
{
    public enum ReturnValue
    {
        Success,
        Warning,
        Error
    }

    public record class Loginresponse
    {
        public Guid userId { get; set; } = Guid.Empty;
        public string jwtToken { get; set; } = String.Empty;
    }

    public record class RegisterResponse
    {
        public Guid userId { get; set; } = Guid.Empty;
        public string jwtToken { get; set; } = String.Empty;
    }
}
