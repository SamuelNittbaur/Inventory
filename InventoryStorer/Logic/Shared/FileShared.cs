namespace BusinessLogic.Shared
{
    public class UploadFile
    {
        public string userName { get; set; } = String.Empty;
        public byte[] fileStream { get; set; } = new byte[0];
        public string fileName { get; set; } = String.Empty;
    }

    public class DeleteFileRequest
    {
        public string userName { get; set; } = String.Empty;
        public string fileName { get; set; } = String.Empty;
    }

}
