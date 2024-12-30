using Microsoft.JSInterop;

namespace Logic.SessionLogic
{
    public static class SessionLogic
    {
        public static async Task SetItem(IJSRuntime jsRuntime, string key, string value)
        {
            var encryptedKey = Cryption.CryptionLogic.Encrypt(key);
            var encryptedValue = Cryption.CryptionLogic.Encrypt(value);
            await jsRuntime.InvokeVoidAsync("sessionStorage.setItem", encryptedKey, encryptedValue);
        }

        public static async Task<string> GetItem(IJSRuntime jsRuntime, string key)
        {
            var encryptedKey = Cryption.CryptionLogic.Encrypt(key);
            return Cryption.CryptionLogic.Decrypt(await jsRuntime.InvokeAsync<string>("sessionStorage.getItem", encryptedKey));
        }

        public static async Task SetDebugItem(IJSRuntime jsRuntime, string key, string value)
        {
            await jsRuntime.InvokeVoidAsync("sessionStorage.setItem",key, value);
        }

        public static async Task<string> GetDebugItem(IJSRuntime jsRuntime, string key)
        {
            return await jsRuntime.InvokeAsync<string>("sessionStorage.getItem",key);
        }
    }
}
