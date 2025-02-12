namespace Logic.Shared
{
    public record class Appointment
    {
        public string id { get; set; } = String.Empty;
        public string start { get; set; }
        public string end { get; set; }
        public string text { get; set; }
        public string comment { get; set; } = String.Empty;
        public DateTime _start { get; set; }
        public DateTime _end { get; set; }
    }
}
