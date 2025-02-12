namespace Logic.Shared
{
    public record class InventoryItem
    {
        public Guid id { get; set; } = Guid.Empty;
        public string name { get; set; } = String.Empty;
        public string insertedFrom { get; set; } = String.Empty;
        public string category { get; set; } = String.Empty;
        public string unit { get; set; } = String.Empty;
        public double quantity { get; set; } = 0;
        public double shouldQuantity { get; set; } = 0;
        public double singlePrice { get; set; } = 0;
        public bool isDeleted { get; set; } = false;
        public double sum() => quantity * singlePrice;
        public double minQuantity { get; set; } = 0;
        public DateTime joinDate { get; set; } = DateTime.MinValue;
        public bool active { get; set; } = true;
        public bool notification { get; set; } = true;
        public Status status { get; set; } = Status.New;
        public List<HistoryItem> history { get; set; } = new List<HistoryItem>();
        public List<NotificationItem> notifications { get; set; } = new List<NotificationItem>();
    }
   

    public enum Status
    {
        Empty,
        New,
        Reserverd,
        Full
    }

    public record class HistoryItem
    {
        public Guid id { get; set; } = Guid.Empty;
        public Guid productId { get; set; } = Guid.Empty;
        public string insertedFrom { get; set; } = String.Empty;
        public DateTime date { get; set; } = DateTime.MinValue;
        public double quantity { get; set; } = 0;
    }

    public record class NotificationItem
    {
        public Guid id { get; set; } = Guid.Empty;
        public DateTime date { get; set; } = DateTime.MinValue;
        public string name { get; set; } = String.Empty;
    }
}
