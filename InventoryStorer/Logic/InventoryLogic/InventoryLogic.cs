using Logic.Shared;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Logic.InventoryLogic
{
    public static class InventoryLogic
    {
        public static async Task<bool> InsertNewMaschine(InventoryRequest request)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        var (securityKey, securityContent) = Logic.Cryption.CryptionLogic.CreateSecurityHeader();
                        client.DefaultRequestHeaders.Add("X-Security-Key", securityContent);
                        client.DefaultRequestHeaders.Add("X-Security-Value", securityKey);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Logic.Root.jwtToken);
                        string jsonPayload = Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(request));
                        CryptionElement cryptionElement = new CryptionElement() { data = jsonPayload };
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(cryptionElement), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/inventory/insertNewItem", content);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseData = await response.Content.ReadAsStringAsync();
                            CryptionElement decryptedElement = JsonConvert.DeserializeObject<CryptionElement>(responseData);
                            string decryptData = Cryption.CryptionLogic.Decrypt(decryptedElement.data);
                            return JsonConvert.DeserializeObject<bool>(decryptData);
                        }
                        else
                        {
                            return false;
                        }
                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        return false;
                    }
                }
            }
            catch (Exception exception)
            {
                return false;
            }
        }

        public static async Task<List<InventoryItem>> GetInventory(GeneralRequest request)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    try
                    {
                        var (securityKey, securityContent) = Logic.Cryption.CryptionLogic.CreateSecurityHeader();
                        client.DefaultRequestHeaders.Add("X-Security-Key", securityContent);
                        client.DefaultRequestHeaders.Add("X-Security-Value", securityKey);
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Logic.Root.jwtToken);
                        string jsonPayload = Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(request));
                        CryptionElement cryptionElement = new CryptionElement() { data = jsonPayload };
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(cryptionElement), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/inventory/getInventory", content);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseData = await response.Content.ReadAsStringAsync();
                            CryptionElement decryptedElement = JsonConvert.DeserializeObject<CryptionElement>(responseData);
                            string decryptData = Cryption.CryptionLogic.Decrypt(decryptedElement.data);
                            return JsonConvert.DeserializeObject<List<InventoryItem>>(decryptData);
                        }
                        else
                        {
                            return new List<InventoryItem>();
                        }
                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        return new List<InventoryItem>();
                    }
                }
            }
            catch (Exception exception)
            {
                return new List<InventoryItem>();
            }
        }
    }
}
