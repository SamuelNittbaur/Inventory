using Google.Cloud.Firestore;

namespace BusinessLogic.Shared
{
    /// <summary>
    /// Represents an inventory item stored in Firestore.
    /// </summary>
    [FirestoreData]
    public class InventoryItem
    {
        /// <summary>
        /// Unique identifier for the inventory item.
        /// </summary>
        public Guid id { get; set; } = Guid.Empty;

        /// <summary>
        /// Firestore-specific identifier for the inventory item.
        /// </summary>
        [FirestoreProperty("_id")]
        public string _id { get; set; } = string.Empty;

        /// <summary>
        /// Name of the inventory item.
        /// </summary>
        [FirestoreProperty("name")]
        public string name { get; set; } = string.Empty;

        /// <summary>
        /// Category of the inventory item.
        /// </summary>
        [FirestoreProperty("category")]
        public string category { get; set; } = string.Empty;

        /// <summary>
        /// User who inserted the item into the inventory.
        /// </summary>
        [FirestoreProperty("insertedFrom")]
        public string insertedFrom { get; set; } = string.Empty;

        /// <summary>
        /// Unit of measurement for the inventory item.
        /// </summary>
        [FirestoreProperty("unit")]
        public string unit { get; set; } = string.Empty;

        /// <summary>
        /// Current quantity of the inventory item.
        /// </summary>
        [FirestoreProperty("quantity")]
        public double quantity { get; set; } = 0;

        /// <summary>
        /// Indicates whether the item is marked as deleted.
        /// </summary>
        [FirestoreProperty("isDeleted")]
        public bool isDeleted { get; set; } = false;

        /// <summary>
        /// Desired quantity for the inventory item.
        /// </summary>
        [FirestoreProperty("shouldQuantity")]
        public double shouldQuantity { get; set; } = 0;

        /// <summary>
        /// Price per unit of the inventory item.
        /// </summary>
        [FirestoreProperty("singlePrice")]
        public double singlePrice { get; set; } = 0;

        /// <summary>
        /// Minimum quantity threshold for the inventory item.
        /// </summary>
        [FirestoreProperty("minQuantity")]
        public double minQuantity { get; set; } = 0;

        /// <summary>
        /// Firestore-specific join date of the inventory item.
        /// </summary>
        [FirestoreProperty("_joinDate")]
        public string _joinDate { get; set; } = string.Empty;

        /// <summary>
        /// Date when the item was added to the inventory.
        /// </summary>
        public DateTime joinDate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Indicates whether the item is active.
        /// </summary>
        [FirestoreProperty("active")]
        public bool active { get; set; } = true;

        /// <summary>
        /// Indicates whether notifications are enabled for the item.
        /// </summary>
        [FirestoreProperty("notification")]
        public bool notification { get; set; } = true;

        /// <summary>
        /// Status of the inventory item.
        /// </summary>
        [FirestoreProperty("status")]
        public Status status { get; set; } = Status.New;

        /// <summary>
        /// History of changes for the inventory item.
        /// </summary>
        [FirestoreProperty("history")]
        public List<HistoryItem> history { get; set; } = new List<HistoryItem>();

        /// <summary>
        /// Notifications associated with the inventory item.
        /// </summary>
        [FirestoreProperty("notifications")]
        public List<NotificationItem> notifications { get; set; } = new List<NotificationItem>();
    }

    /// <summary>
    /// Enum representing the status of an inventory item.
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// The item is empty.
        /// </summary>
        Empty,

        /// <summary>
        /// The item is new.
        /// </summary>
        New,

        /// <summary>
        /// The item is reserved.
        /// </summary>
        Reserverd,

        /// <summary>
        /// The item is full.
        /// </summary>
        Full
    }

    /// <summary>
    /// Represents a historical record for an inventory item.
    /// </summary>
    [FirestoreData]
    public class HistoryItem
    {
        /// <summary>
        /// Firestore-specific identifier for the history item.
        /// </summary>
        [FirestoreProperty("_id")]
        public string _id { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier for the history item.
        /// </summary>
        public Guid id { get; set; } = Guid.Empty;

        /// <summary>
        /// Identifier of the associated product.
        /// </summary>
        public Guid productId { get; set; } = Guid.Empty;

        /// <summary>
        /// Firestore-specific identifier for the associated product.
        /// </summary>
        [FirestoreProperty("_productId")]
        public string _productId { get; set; } = string.Empty;

        /// <summary>
        /// User who created the history record.
        /// </summary>
        [FirestoreProperty("insertedFrom")]
        public string insertedFrom { get; set; } = string.Empty;

        /// <summary>
        /// Firestore-specific date of the history record.
        /// </summary>
        [FirestoreProperty("_date")]
        public string _date { get; set; } = string.Empty;

        /// <summary>
        /// Date of the history record.
        /// </summary>
        public DateTime date { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Quantity associated with the history record.
        /// </summary>
        [FirestoreProperty("appointmquantityentCategories")]
        public double quantity { get; set; } = 0;
    }

    /// <summary>
    /// Represents a notification related to an inventory item.
    /// </summary>
    [FirestoreData]
    public class NotificationItem
    {
        /// <summary>
        /// Firestore-specific identifier for the notification.
        /// </summary>
        [FirestoreProperty("_id")]
        public string _id { get; set; } = string.Empty;

        /// <summary>
        /// Unique identifier for the notification.
        /// </summary>
        public Guid id { get; set; } = Guid.Empty;

        /// <summary>
        /// Firestore-specific date of the notification.
        /// </summary>
        [FirestoreProperty("_date")]
        public string _date { get; set; } = string.Empty;

        /// <summary>
        /// Date of the notification.
        /// </summary>
        public DateTime date { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Name of the notification.
        /// </summary>
        [FirestoreProperty("name")]
        public string name { get; set; } = string.Empty;
    }
}
