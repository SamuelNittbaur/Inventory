using Logic.Shared;

namespace Logic.Shared
{
    public class RegisterRequest
    {
        public UserConfiguration configuration { get; set; } = new UserConfiguration();
        public Guid userId { get; set; } = Guid.Empty;
        public String userName { get; set; } = String.Empty;
        public String password { get; set; } = String.Empty;
    }

    public class LoginRequest
    {
        public String userName { get; set; } = String.Empty;
        public String password { get; set; } = String.Empty;
    }

    public class GeneralRequest
    {
        public Guid userId { get; set; } = Guid.Empty;
        public String userName { get; set; } = String.Empty;
        public String company { get; set; } = String.Empty;
    }

   

    public class FileRequest
    {
        public byte[] fileStream { get; set; } = new byte[0];
        public string fileName { get; set; } = String.Empty;
        public string company { get; set; } = String.Empty;
    }

   
    
    public class ChangePasswordRequest
    {
        public string userName { get; set; } = String.Empty;
        public string oldPassword { get; set; } = String.Empty;
        public string newPassword { get; set; } = String.Empty;

    }

    public class InventoryRequest
    {
        public InventoryItem item { get; set; } = new InventoryItem();
        public String company { get; set; } = String.Empty;
    }

    public class AppointmentRequest
    {
        public Appointment item { get; set; } = new Appointment();
        public String company { get; set; } = String.Empty;
    }
    public class ResetPasswordRequest
    {
        public Guid userId { get; set; } = Guid.Empty;
        public String userName { get; set; } = String.Empty;
        public String password { get; set; } = String.Empty;
    }
}
