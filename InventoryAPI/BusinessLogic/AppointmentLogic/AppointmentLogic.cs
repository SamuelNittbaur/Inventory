using BusinessLogic.Shared;
using Google.Cloud.Firestore;
using System.Net;

namespace BusinessLogic.AppointmentLogic
{
    /// <summary>
    /// Enthält die Logik für Terminoperationen.
    /// </summary>
    public static class AppointmentLogic
    {
        /// <summary>
        /// Fügt einen neuen Termin in die Datenbank ein.
        /// </summary>
        /// <param name="request">Das <see cref="AppointmentRequest"/>-Objekt mit den Details des neuen Termins.</param>
        /// <returns>
        /// Ein <see cref="bool"/>-Wert, der angibt, ob die Einfügung erfolgreich war.
        /// </returns>
        public static async Task<bool> InsertNewItem(AppointmentRequest request)
        {
            try
            {
                var response = await DataBase.AppointmentDatabase.InsertNew(request);
                return response;
            }
            catch (Exception exeption)
            {
                return false;
            }
        }

        /// <summary>
        /// Löscht einen vorhandenen Termin aus der Datenbank.
        /// </summary>
        /// <param name="request">Das <see cref="AppointmentRequest"/>-Objekt mit den Details des zu löschenden Termins.</param>
        /// <returns>
        /// Ein <see cref="bool"/>-Wert, der angibt, ob die Löschung erfolgreich war.
        /// </returns>
        public static async Task<bool> DeleteItem(AppointmentRequest request)
        {
            try
            {
                var response = await DataBase.AppointmentDatabase.DeleteAppointment(request);
                return response;
            }
            catch (Exception exeption)
            {
                return false;
            }
        }

        /// <summary>
        /// Ruft eine Liste von Terminen aus der Datenbank ab.
        /// </summary>
        /// <param name="request">Das <see cref="GeneralRequest"/>-Objekt mit allgemeinen Anfragenparametern, wie der Firmen-ID.</param>
        /// <returns>
        /// Eine Liste von <see cref="Appointment"/>, die alle relevanten Termine enthält.
        /// </returns>
        public static async Task<List<Appointment>> GetItems(GeneralRequest request)
        {
            try
            {
                var response = await DataBase.AppointmentDatabase.GetAppointment(request.company);
                return response;
            }
            catch (Exception exeption)
            {
                return new List<Appointment>();
            }
        }
    }
}
