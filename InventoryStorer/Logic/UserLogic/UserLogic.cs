using System.Text;
using System;
using Logic.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using BusinessLogic.Shared;

namespace Logic.UserLogic
{
    public static class UserLogic
    {
        public static async Task<RegisterResponse> RegisterUser(RegisterRequest register)
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

                        string jsonPayload = Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(register));
                        CryptionElement cryptionElement = new CryptionElement() { data = jsonPayload };
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(cryptionElement), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/user/register", content);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseData = await response.Content.ReadAsStringAsync();
                            CryptionElement decryptedElement = JsonConvert.DeserializeObject<CryptionElement>(responseData);
                            string decryptData = Cryption.CryptionLogic.Decrypt(decryptedElement.data);
                            return JsonConvert.DeserializeObject<RegisterResponse>(decryptData);
                        }
                        else
                        {
                            return new RegisterResponse();
                        }
                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        return new RegisterResponse();
                    }
                }
            }
            catch (Exception exception)
            {
                return new RegisterResponse();
            }
        }

        public static async Task<string> GetToken(GeneralRequest register)
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

                        string jsonPayload = Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(register));
                        CryptionElement cryptionElement = new CryptionElement() { data = jsonPayload };
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(cryptionElement), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/user/ksdjhfpiu", content);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseData = await response.Content.ReadAsStringAsync();
                            CryptionElement decryptedElement = JsonConvert.DeserializeObject<CryptionElement>(responseData);
                            string decryptData = Cryption.CryptionLogic.Decrypt(decryptedElement.data);
                            return decryptData;
                        }
                        else
                        {
                            return String.Empty;
                        }
                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        return String.Empty;
                    }
                }
            }
            catch (Exception exception)
            {
                return String.Empty;
            }
        }

        public static async Task<Loginresponse> Login(LoginRequest register)
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

                        string jsonPayload = Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(register));
                        CryptionElement cryptionElement = new CryptionElement() { data = jsonPayload };
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(cryptionElement), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/user/login", content);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseData = await response.Content.ReadAsStringAsync();
                            CryptionElement decryptedElement = JsonConvert.DeserializeObject<CryptionElement>(responseData);
                            string decryptData = Cryption.CryptionLogic.Decrypt(decryptedElement.data);
                            return JsonConvert.DeserializeObject<Loginresponse>(decryptData);
                        }
                        else
                        {
                            return new Loginresponse();
                        }
                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        return new Loginresponse();
                    }
                }
            }
            catch (Exception exception)
            {
                return new Loginresponse();
            }
        }

        public static async Task<UserConfiguration> GetUserConfiguration(GeneralRequest request)
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

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/user/getUserConfiguration", content);

                        if (response.IsSuccessStatusCode)
                        {
                            string responseData = await response.Content.ReadAsStringAsync();
                            CryptionElement decryptedElement = JsonConvert.DeserializeObject<CryptionElement>(responseData);
                            string decryptData = Cryption.CryptionLogic.Decrypt(decryptedElement.data);
                            return JsonConvert.DeserializeObject<UserConfiguration>(decryptData);
                        }
                        else
                        {
                            return new UserConfiguration();
                        }
                    }
                    catch (HttpRequestException httpRequestException)
                    {
                        return new UserConfiguration();
                    }
                }
            }
            catch (Exception exception)
            {
                return new UserConfiguration();
            }
        }

        


            public static async Task<bool> SendPasswordEmail(GeneralRequest request)
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
                        string jsonPayload = Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(request));
                        CryptionElement cryptionElement = new CryptionElement() { data = jsonPayload };
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(cryptionElement), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/user/sendPasswordEmail", content);

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


        public static async Task<bool> ResetPassword(ResetPasswordRequest request)
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
                        string jsonPayload = Cryption.CryptionLogic.Encrypt(JsonConvert.SerializeObject(request));
                        CryptionElement cryptionElement = new CryptionElement() { data = jsonPayload };
                        HttpContent content = new StringContent(JsonConvert.SerializeObject(cryptionElement), Encoding.UTF8, "application/json");

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/user/resetPassword", content);

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

        public static async Task<bool> ChangePassword(ResetPasswordRequest request)
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

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/user/changePassword", content);

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

        public static async Task<bool> CreateUserConfiguration(UserConfiguration request)
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

                        HttpResponseMessage response = await client.PostAsync($"{Logic.Root.apiURL}/user/createUserConfiguration", content);

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
    }
}
