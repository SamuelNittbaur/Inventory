using Logic.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;
using System.Xml.Linq;

namespace Logic
{
    public static class Root
    {
        public static string apiURL = "https://webcardapi.online";// "https://localhost:7012"; 
        public static string jwtToken = String.Empty;
        public static bool loggedIn = false;
        public static bool isMobile = false;
        public static Guid userId = Guid.Empty;
        public static string userName = String.Empty;
        public static UserConfiguration userConfiguration = new UserConfiguration();
        public static bool unsafePageExit = false;
        public static List<HistoryItem> history = new List<HistoryItem>();
        public static InventoryItem itemToEdit = new InventoryItem();
        public static List<InventoryItem> inventory = new List<InventoryItem>();
        public static List<Appointment> appointments = new List<Appointment>();

    }

    public static class FeatureFlip
    {
        public static bool insertedFrom = false;
    }

}


