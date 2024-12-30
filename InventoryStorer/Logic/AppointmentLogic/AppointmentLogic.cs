using Logic.Shared;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace Logic.AppointmentLogic
{
    public static class AppointmentLogic
    {
        public static async Task<bool> InsertNewAppointment(AppointmentRequest request)
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

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/appointment/insertNewItem", content);

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

        public static async Task<bool> DeleteAppointment(AppointmentRequest request)
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

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/appointment/deleteAppointment", content);

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

        public static async Task<List<Appointment>> GetAppointment(GeneralRequest request)
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

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/appointment/getAppointment", content);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseData = await response.Content.ReadAsStringAsync();
                            CryptionElement decryptedElement = JsonConvert.DeserializeObject<CryptionElement>(responseData);
                            string decryptData = Cryption.CryptionLogic.Decrypt(decryptedElement.data);
                            return JsonConvert.DeserializeObject<List<Appointment>>(decryptData);
                        }
                        else
                        {
                            return new List<Appointment>();
                        }
                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        return new List<Appointment>();
                    }
                }
            }
            catch (Exception exception)
            {
                return new List<Appointment>();
            }
        }
    }
}
