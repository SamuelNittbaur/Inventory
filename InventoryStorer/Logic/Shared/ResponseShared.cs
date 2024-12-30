namespace Logic.Shared
{
    public enum ReturnValue
    {
        Success,
        Warning,
        Error
    }

    public class Loginresponse
    {
        public Guid userId { get; set; } = Guid.Empty;
        public string jwtToken { get; set; } = String.Empty;
    }

    public class RegisterResponse
    {
        public Guid userId { get; set; } = Guid.Empty;
        public string jwtToken { get; set; } = String.Empty;
    }
}
