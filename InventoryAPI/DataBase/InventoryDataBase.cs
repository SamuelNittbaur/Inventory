using BusinessLogic.Cryption;
using BusinessLogic.Shared;
using Firebase.Storage;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Net;
using System.Globalization;
using BusinessLogic.Shared;
using static Google.Rpc.Context.AttributeContext.Types;

namespace DataBase
{
    /// <summary>
    /// Diese Klasse verwaltet die Interaktionen mit der Firebase Firestore-Datenbank und ermöglicht das Hinzufügen, Abrufen und Hochladen von Inventarobjekten.
    /// </summary>
    public static class InventoryDataBase
    {
        public static string storageBucketInventory = Environment.GetEnvironmentVariable("inventoryStorageBucket"); // Firebase Storage-Bucket

        /// <summary>
        /// Fügt ein neues Inventarobjekt in die Firestore-Datenbank ein.
        /// </summary>
        /// <param name="request">Die Anfrage mit den Inventardaten.</param> 
        /// <returns>True, wenn das Inventar erfolgreich eingefügt wurde, andernfalls False.</returns>
        public static async Task<bool> InsertNewInventory(InventoryRequest request)
        {
            try
            {
                // Umwandlung der Daten in das gewünschte Format
                request.item._id = request.item.id.ToString();
                request.item._joinDate = request.item.joinDate.ToString("dd.MM.yyyy HH:mm:ss");

                foreach (var _historyItem in request.item.history)
                {
                    _historyItem._id = _historyItem.id.ToString();
                    _historyItem._productId = _historyItem.productId.ToString();
                    _historyItem._date = _historyItem.date.ToString("dd.MM.yyyy HH:mm:ss");
                }

                foreach (var _notificationItem in request.item.notifications)
                {
                    _notificationItem._id = _notificationItem.id.ToString();
                    _notificationItem._date = _notificationItem.date.ToString("dd.MM.yyyy HH:mm:ss");
                }

                // Zugriff auf Firestore-Datenbank und Speichern des Inventars
                FirestoreDb db = FirestoreDb.Create(Environment.GetEnvironmentVariable("inventoryDB"));
                Dictionary<string, object> items = new Dictionary<string, object>()
                {
                    {request.item.id.ToString(), request.item }
                };
                request.item._id = request.item.id.ToString();
                DocumentReference docRef = db.Collection(request.company).Document("Inventory").Collection("InventoryItems").Document(request.item._id);

                // Dokument in der Firestore-Datenbank speichern
                await docRef.SetAsync(request.item);
                return true;
            }
            catch (Exception exeption)
            {
                // Fehlerbehandlung
                return false;
            }
        }

        /// <summary>
        /// Ruft alle Inventarobjekte eines Unternehmens aus der Firestore-Datenbank ab.
        /// </summary>
        /// <param name="company">Der Name des Unternehmens, dessen Inventar abgerufen werden soll.</param>
        /// <returns>Eine Liste von Inventarobjekten für das angegebene Unternehmen.</returns>
        public static async Task<List<InventoryItem>> GetInventory(string company)
        {
            try
            {
                List<InventoryItem> inventory = new List<InventoryItem>();
                FirestoreDb db = FirestoreDb.Create(Environment.GetEnvironmentVariable("inventoryDB"));

                DocumentReference docRef = db.Collection(company).Document("Inventory");
                CollectionReference colRef = docRef.Collection("InventoryItems");

                // Abrufen der Inventargegenstände
                var snapshot = await colRef.GetSnapshotAsync();
                string format = "dd.MM.yyyy HH:mm:ss";
                CultureInfo provider = CultureInfo.InvariantCulture;

                // Durchlaufen der Ergebnisse und Umwandlung der Inventargegenstände
                foreach (var document in snapshot.Documents)
                {
                    InventoryItem item = document.ConvertTo<InventoryItem>();
                    item.id = Guid.Parse(item._id);
                    item.joinDate = DateTime.ParseExact(item._joinDate, format, provider);

                    foreach (var _historyItem in item.history)
                    {
                        _historyItem.id = Guid.Parse(_historyItem._id);
                        _historyItem.productId = Guid.Parse(_historyItem._productId);
                        _historyItem.date = DateTime.ParseExact(_historyItem._date, format, provider);
                    }

                    foreach (var _notificationItem in item.notifications)
                    {
                        _notificationItem.id = Guid.Parse(_notificationItem._id);
                        _notificationItem.date = DateTime.ParseExact(_notificationItem._date, format, provider);
                    }

                    // Hinzufügen des Inventarobjekts zur Liste
                    inventory.Add(item);
                }

                // Rückgabe der Inventargegenstände, die nicht gelöscht wurden
                return inventory.FindAll(_inventory => !_inventory.isDeleted);
            }
            catch (Exception exception)
            {
                // Fehlerbehandlung, Rückgabe einer Liste mit einer Fehlermeldung
                return new List<InventoryItem>()
                {
                    new InventoryItem()
                    {
                        name = exception.Message
                    }
                };
            }
        }

        /// <summary>
        /// Lädt ein Bild eines Inventarobjekts in Firebase Storage hoch.
        /// </summary>
        /// <param name="request">Die Anfrage mit den Bilddaten des Inventars.</param>
        /// <returns>Die URL des hochgeladenen Bildes, falls erfolgreich, andernfalls eine leere Zeichenkette.</returns>
        public static async Task<string> UploadInventoryPicture(FileRequest request)
        {
            try
            {
                // Erstellen eines MemoryStreams für das Bild
                MemoryStream memoryStream = new MemoryStream(request.fileStream);
                memoryStream.Position = 0;

                Stream stream = memoryStream;

                // Hochladen des Bildes in Firebase Storage
                var task = new FirebaseStorage(storageBucketInventory, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true
                })
                    .Child(request.company)
                    .Child("InventoryPics")
                    .Child(request.fileName)
                    .PutAsync(stream);

                task.Progress.ProgressChanged += (s, args) => { };
                var result = await task;
                return result; // Rückgabe der URL des hochgeladenen Bildes
            }
            catch (Exception exception)
            {
                // Fehlerbehandlung
                return String.Empty;
            }
        }
    }
}
