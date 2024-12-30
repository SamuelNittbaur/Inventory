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
using BusinessLogic.Shared;
using static Google.Rpc.Context.AttributeContext.Types;
using System.Globalization;

namespace DataBase
{
    /// <summary>
    /// Diese Klasse verwaltet die Interaktionen mit der Firebase Firestore-Datenbank und speichert sowie abruft Termine.
    /// </summary>
    public static class AppointmentDatabase 
    {
        private static string format = "dd.MM.yyyy HH:mm:ss"; // Datumsformat für die Speicherung und Abfrage von Terminen
        public static string storageBucketInventory = Environment.GetEnvironmentVariable("inventoryBucket"); // Firebase Storage-Bucket

        /// <summary>
        /// Fügt einen neuen Termin in die Firestore-Datenbank ein.
        /// </summary>
        /// <param name="request">Die Anfrage mit den Details des Termins.</param>
        /// <returns>True, wenn der Termin erfolgreich eingefügt wurde, andernfalls False.</returns>
        public static async Task<bool> InsertNew(AppointmentRequest request)
        {
            try
            {
                // Umwandlung der Start- und Endzeiten in das gewünschte Format
                request.item.end = request.item._end.ToString(format);
                request.item.start = request.item._start.ToString(format);

                // Zugriff auf die Firestore-Datenbank und Hinzufügen des Termins
                FirestoreDb db = FirestoreDb.Create(Environment.GetEnvironmentVariable("inventoryDB"));
                Dictionary<string, object> items = new Dictionary<string, object>()
                {
                    {request.item.id.ToString(), request.item }
                };
                DocumentReference docRef = db.Collection(request.company).Document("Appointment").Collection("AppointmentItems").Document(request.item.id);

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
        /// Löscht einen Termin aus der Firestore-Datenbank.
        /// </summary>
        /// <param name="request">Die Anfrage mit den Details des zu löschenden Termins.</param>
        /// <returns>True, wenn der Termin erfolgreich gelöscht wurde, andernfalls False.</returns>
        public static async Task<bool> DeleteAppointment(AppointmentRequest request)
        {
            try
            {
                // Zugriff auf die Firestore-Datenbank und Löschen des Termins
                FirestoreDb db = FirestoreDb.Create(Environment.GetEnvironmentVariable("inventoryDB"));
                DocumentReference docRef = db.Collection(request.company).Document("Appointment").Collection("AppointmentItems").Document(request.item.id);

                // Löschen des Dokuments
                await docRef.DeleteAsync();
                return true;
            }
            catch (Exception exeption)
            {
                // Fehlerbehandlung
                return false;
            }
        }

        /// <summary>
        /// Ruft alle Termine eines Unternehmens aus der Firestore-Datenbank ab.
        /// </summary>
        /// <param name="company">Der Name des Unternehmens, dessen Termine abgerufen werden sollen.</param>
        /// <returns>Eine Liste von Terminen für das angegebene Unternehmen.</returns>
        public static async Task<List<Appointment>> GetAppointment(string company)
        {
            try
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                List<Appointment> appointments = new List<Appointment>();
                FirestoreDb db = FirestoreDb.Create(Environment.GetEnvironmentVariable("inventoryDB"));

                // Zugriff auf die Dokumentensammlung für Termine
                DocumentReference docRef = db.Collection(company).Document("Appointment");
                CollectionReference colRef = docRef.Collection("AppointmentItems");

                // Abrufen der Termine
                var snapshot = await colRef.GetSnapshotAsync();

                // Durchlaufen der Ergebnisse und Umwandlung der Termine
                foreach (var document in snapshot.Documents)
                {
                    Appointment item = document.ConvertTo<Appointment>();
                    item._end = DateTime.ParseExact(item.end, format, provider);
                    item._start = DateTime.ParseExact(item.start, format, provider);

                    appointments.Add(item);
                }

                return appointments;
            }
            catch (Exception exeption)
            {
                // Fehlerbehandlung
                return new List<Appointment>();
            }
        }
    }
}
