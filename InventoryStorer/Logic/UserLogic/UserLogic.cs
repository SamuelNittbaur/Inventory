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
        /// <summary>
        /// Registers a new user with the provided registration data.
        /// </summary>
        /// <param name="register">The registration data.</param>
        /// <returns>A <see cref="RegisterResponse"/> containing the registration result.</returns>
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
                    catch (HttpRequestException)
                    {
                        return new RegisterResponse();
                    }
                }
            }
            catch (Exception)
            {
                return new RegisterResponse();
            }
        }

        /// <summary>
        /// Retrieves a token using the provided general request data.
        /// </summary>
        /// <param name="register">The general request data.</param>
        /// <returns>A string containing the token, or an empty string if unsuccessful.</returns>
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
                            return string.Empty;
                        }
                    }
                    catch (HttpRequestException)
                    {
                        return string.Empty;
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Logs in a user with the provided login request data.
        /// </summary>
        /// <param name="register">The login request data.</param>
        /// <returns>A <see cref="Loginresponse"/> containing the login result.</returns>
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
                    catch (HttpRequestException)
                    {
                        return new Loginresponse();
                    }
                }
            }
            catch (Exception)
            {
                return new Loginresponse();
            }
        }

        /// <summary>
        /// Retrieves the user configuration based on the provided request data.
        /// </summary>
        /// <param name="request">The request data containing necessary details for retrieving the user configuration.</param>
        /// <returns>
        /// A <see cref="UserConfiguration"/> object containing the user configuration details.
        /// Returns a default <see cref="UserConfiguration"/> object if the operation fails.
        /// </returns>
        /// <exception cref="HttpRequestException">
        /// Thrown if there is an error while sending the HTTP request.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown if there is a general exception during the operation.
        /// </exception>
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





        /// <summary>
        /// Sends a password reset email based on the provided request data.
        /// </summary>
        /// <param name="request">The request data containing the necessary details to send the password reset email.</param>
        /// <returns>
        /// A <see cref="bool"/> indicating the success of the operation.
        /// Returns <c>true</c> if the email was sent successfully, otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="HttpRequestException">
        /// Thrown if there is an error while sending the HTTP request.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown if a general exception occurs during the operation.
        /// </exception>
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



        /// <summary>
        /// Resets the password for a user based on the provided reset password request data.
        /// </summary>
        /// <param name="request">The request data containing the details required to reset the password.</param>
        /// <returns>
        /// A <see cref="bool"/> indicating the success of the operation.
        /// Returns <c>true</c> if the password reset was successful, otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="HttpRequestException">
        /// Thrown if there is an error while sending the HTTP request.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown if a general exception occurs during the operation.
        /// </exception>
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


        /// <summary>
        /// Changes the password for a user based on the provided reset password request data.
        /// </summary>
        /// <param name="request">The request data containing the details required to change the password.</param>
        /// <returns>
        /// A <see cref="bool"/> indicating whether the password change was successful.
        /// Returns <c>true</c> if the password was successfully changed, otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="HttpRequestException">
        /// Thrown if there is an error while sending the HTTP request.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown if a general exception occurs during the operation.
        /// </exception>
        /// <remarks>
        /// This method uses a security header and a bearer token for authentication.
        /// The request data is encrypted before being sent to the server, and the response is decrypted upon receipt.
        /// </remarks>
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


        /// <summary>
        /// Creates or updates a user configuration based on the provided configuration data.
        /// </summary>
        /// <param name="request">The user configuration data that will be sent to the server to create or update the user's configuration.</param>
        /// <returns>
        /// A <see cref="bool"/> indicating whether the operation was successful.
        /// Returns <c>true</c> if the configuration was successfully created or updated, otherwise <c>false</c>.
        /// </returns>
        /// <exception cref="HttpRequestException">
        /// Thrown if there is an error while sending the HTTP request.
        /// </exception>
        /// <exception cref="Exception">
        /// Thrown if a general exception occurs during the operation.
        /// </exception>
        /// <remarks>
        /// This method sends the user configuration data to the server, where it is encrypted before transmission.
        /// The server's response is also encrypted, and the data is decrypted upon receipt.
        /// The method uses a security header and a bearer token for authentication.
        /// </remarks>
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
