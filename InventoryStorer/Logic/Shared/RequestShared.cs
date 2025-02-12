namespace Logic.Shared
{
    public record class RegisterRequest
    {
        public UserConfiguration configuration { get; set; } = new UserConfiguration();
        public Guid userId { get; set; } = Guid.Empty;
        public String userName { get; set; } = String.Empty;
        public String password { get; set; } = String.Empty;
    }

    public record class LoginRequest
    {
        public String userName { get; set; } = String.Empty;
        public String password { get; set; } = String.Empty;
    }

    public record class GeneralRequest
    {
        public Guid userId { get; set; } = Guid.Empty;
        public String userName { get; set; } = String.Empty;
        public String company { get; set; } = String.Empty;
    }



    public record class FileRequest
    {
        public byte[] fileStream { get; set; } = new byte[0];
        public string fileName { get; set; } = String.Empty;
        public string company { get; set; } = String.Empty;
    }



    public record class ChangePasswordRequest
    {
        public string userName { get; set; } = String.Empty;
        public string oldPassword { get; set; } = String.Empty;
        public string newPassword { get; set; } = String.Empty;

    }

    public record class InventoryRequest
    {
        public InventoryItem item { get; set; } = new InventoryItem();
        public String company { get; set; } = String.Empty;
    }

    public record class AppointmentRequest
    {
        public Appointment item { get; set; } = new Appointment();
        public String company { get; set; } = String.Empty;
    }
    public record class ResetPasswordRequest
    {
        public Guid userId { get; set; } = Guid.Empty;
        public String userName { get; set; } = String.Empty;
        public String password { get; set; } = String.Empty;
    }
}
