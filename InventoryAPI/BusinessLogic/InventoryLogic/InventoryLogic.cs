using BusinessLogic.Shared;
using Google.Cloud.Firestore;
using System.Net;

namespace BusinessLogic.InventoryLogic
{
    /// <summary>
    /// Stellt die Geschäftslogik für Inventaroperationen bereit.
    /// </summary>
    public static class InventoryLogic
    {
        /// <summary>
        /// Fügt einen neuen Inventargegenstand hinzu.
        /// </summary>
        /// <param name="request">Die Anfrage mit den Informationen des neuen Inventargegenstands.</param>
        /// <returns>Gibt true zurück, wenn der Vorgang erfolgreich war, andernfalls false.</returns>
        public static async Task<bool> InsertNewItem(InventoryRequest request)
        {
            try
            {
                var response = await DataBase.InventoryDataBase.InsertNewInventory(request);
                return response;
            }
            catch (Exception exeption)
            {
                // Fehlerbehandlung
                return false;
            }
        }

        /// <summary>
        /// Ruft eine Liste von Inventargegenständen ab.
        /// </summary>
        /// <param name="request">Die Anfrage mit den allgemeinen Parametern wie Unternehmen.</param>
        /// <returns>Eine Liste von <see cref="InventoryItem"/>, oder eine leere Liste im Fehlerfall.</returns>
        public static async Task<List<InventoryItem>> GetItems(GeneralRequest request)
        {
            try
            {
                var response = await DataBase.InventoryDataBase.GetInventory(request.company);
                return response;
            }
            catch (Exception exeption)
            {
                // Fehlerbehandlung
                return new List<InventoryItem>();
            }
        }
    }
}
