using Google.Cloud.Firestore;

namespace BusinessLogic.Shared
{
    /// <summary>
    /// Repräsentiert einen Termin mit Eigenschaften, die für die Planung und Speicherung benötigt werden.
    /// </summary>
    [FirestoreData]
    public class Appointment
    {
        /// <summary>
        /// Eindeutige Kennung des Termins.
        /// </summary>
        [FirestoreProperty("id")]
        public string id { get; set; } = string.Empty;

        /// <summary>
        /// Startzeit des Termins im String-Format.
        /// </summary>
        [FirestoreProperty("start")]
        public string start { get; set; }

        /// <summary>
        /// Endzeit des Termins im String-Format.
        /// </summary>
        [FirestoreProperty("end")]
        public string end { get; set; }

        /// <summary>
        /// Beschreibung oder Titel des Termins.
        /// </summary>
        [FirestoreProperty("text")]
        public string text { get; set; }

        /// <summary>
        /// Zusätzliche Kommentare oder Anmerkungen zum Termin.
        /// </summary>
        [FirestoreProperty("comment")]
        public string comment { get; set; } = string.Empty;

        /// <summary>
        /// Startzeit des Termins als <see cref="DateTime"/>.
        /// </summary>
        public DateTime _start { get; set; }

        /// <summary>
        /// Endzeit des Termins als <see cref="DateTime"/>.
        /// </summary>
        public DateTime _end { get; set; }
    }
}
