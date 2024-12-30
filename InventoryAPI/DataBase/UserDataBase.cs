using BusinessLogic.Cryption;
using BusinessLogic.Shared;
using Firebase.Storage;
using Google.Cloud.Firestore;
using Newtonsoft.Json;
using System.Text;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Security.AccessControl;
using Firebase.Auth.Providers;
using static Google.Rpc.Context.AttributeContext.Types;
using Firebase.Auth;
using Firebase.Auth.Repository;
using System;

namespace DataBase
{
    public static class UserDataBase
    {

        public static StorageClient storageClient;
        public static Firebase.Auth.UserCredential credentials;

        public static string storageBucketUser = Environment.GetEnvironmentVariable("inventoryStorageBucket");
        public static async Task CreateFireBaseSecureLogin()
        {
            //try
            //{
            var config = new FirebaseAuthConfig
            {
                ApiKey = Environment.GetEnvironmentVariable("apiKey"),
                AuthDomain = Environment.GetEnvironmentVariable("authDomain"),
                Providers = new FirebaseAuthProvider[] {
                        new EmailProvider()
                    }

            };
            var client = new FirebaseAuthClient(config);
            //var result = await client.SignInWithEmailAndPasswordAsync("samuel.nittbaur@sn-dev.de", "Kaffee1234"); Kaffee12345
            credentials = await client.SignInWithEmailAndPasswordAsync(Environment.GetEnvironmentVariable("fbCredentialUserName"), Environment.GetEnvironmentVariable("fbCredentialPasswort"));
            //}
            //catch(Exception exception)
            //{
            //    Console.WriteLine(exception.Message);
            //}
        }
        public static async Task<Guid> LoadUserId(string userName, string password)
        {
            //try
            //{
            await CreateFireBaseSecureLogin();
            var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
            {
                ThrowOnCancel = true,
                AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken),
            })
            .Child(userName)
            .Child($"{password}.txt");
            var gsreference = await task.GetDownloadUrlAsync();

            string fileContent = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                fileContent = await httpClient.GetStringAsync(gsreference);
            }
            byte[] bytes = Encoding.Default.GetBytes(fileContent);
            fileContent = Encoding.UTF32.GetString(bytes);

            string decodedGuid = CryptionLogic.DecryptUserString(fileContent);

            return new Guid(decodedGuid);
            //}
            //catch (FirebaseStorageException firebaseException)
            //{
            //    return Guid.Empty;
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception.Message);
            //    return Guid.Empty;
            //}
        }

        public static async Task<bool> CheckIfUserExists(string userName, string password)
        {
            //try
            //{
            await CreateFireBaseSecureLogin();
            string encryptedWord = CryptionLogic.EncryptUserString("Exist");
            var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
            {
                ThrowOnCancel = true,
                AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken)
            })
            .Child(userName)
            .Child($"{password}.txt");

            var gsreference = await task.GetDownloadUrlAsync();

            return true;
            //}
            //catch (FirebaseStorageException firebaseException)
            //{
            //    return false;
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception.Message);
            //    return false;
            //}
        }


        public static async Task<Guid> CreateNewUserPassword(string userName, string password, Guid id)
        {
            try
            {
                await CreateFireBaseSecureLogin();
                string encryptedPasswordContent = CryptionLogic.EncryptUserString(id.ToString());
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF32.GetBytes(encryptedPasswordContent));

                var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                    AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken)
                })
                    .Child(userName)
                    .Child($"{password}.txt")
                    .PutAsync(memoryStream);

                task.Progress.ProgressChanged += (s, args) =>
                {
                };
                string name = await task;

                memoryStream.Close();

                return id;
            }
            catch (FirebaseStorageException firebaseException)
            {
                Console.WriteLine(firebaseException.Message);
                return Guid.Empty;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return Guid.Empty;
            }
        }


        public static async Task<bool> DeletePsaswordFile(string userName, string password)
        {
            try
            {
                await CreateFireBaseSecureLogin();
                var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                    AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken)
                })
                    .Child(userName)
                    .Child($"{password}.txt")
                    .DeleteAsync();


                await task;

                return true;
            }
            catch (FirebaseStorageException firebaseException)
            {
                Console.WriteLine(firebaseException.Message);
                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }

        public static async Task<bool> CreateExistFile(string userName, string password)
        {
            try
            {
                await CreateFireBaseSecureLogin();
                MemoryStream memoryStream = new MemoryStream(Encoding.UTF32.GetBytes(password));

                var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                    AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken),
                })
                    .Child(userName)
                    .Child($"Exist.txt")
                    .PutAsync(memoryStream);

                task.Progress.ProgressChanged += (s, args) =>
                {
                };
                string name = await task;

                memoryStream.Close();

                return true;
            }
            catch (FirebaseStorageException firebaseException)
            {
                Console.WriteLine(firebaseException.Message);
                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }


        public static async Task<string> GetExistsFile(string userName)
        {
            try
            {
                await CreateFireBaseSecureLogin();
                var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                    AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken),
                })
                .Child(userName)
                .Child($"Exist.txt");
                var gsreference = await task.GetDownloadUrlAsync();

                string fileContent = string.Empty;
                using (HttpClient httpClient = new HttpClient())
                {
                    fileContent = await httpClient.GetStringAsync(gsreference);
                }
                byte[] bytes = Encoding.Default.GetBytes(fileContent);
                fileContent = Encoding.UTF32.GetString(bytes);


                return fileContent;
            }
            catch (FirebaseStorageException firebaseException)
            {
                Console.WriteLine(firebaseException.Message);
                return String.Empty;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return String.Empty;
            }
        }

        public static async Task<bool> CheckIfUserConfigurationExists(string userName, Guid id)
        {
            try
            {
                await CreateFireBaseSecureLogin();
                var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                    AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken),
                })
                .Child(userName)
                .Child(id.ToString())
                .Child($"Configuration.txt");

                var gsreference = await task.GetDownloadUrlAsync();
                string fileContent = string.Empty;
                using (HttpClient httpClient = new HttpClient())
                {
                    fileContent = await httpClient.GetStringAsync(gsreference);
                }
                byte[] bytes = Encoding.Default.GetBytes(fileContent);
                fileContent = Encoding.UTF32.GetString(bytes);

                return true;
            }
            catch (FirebaseStorageException firebaseException)
            {
                Console.WriteLine(firebaseException.Message);
                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }



        public static async Task<bool> CreateUserConfiguration(UserConfiguration configuration)
        {
            if (configuration.id != Guid.Empty)
            {
                try
                {
                    await CreateFireBaseSecureLogin();
                    string configurationAsJson = JsonConvert.SerializeObject(configuration);
                    string encryptedConfigurationContent = CryptionLogic.EncryptUserString(configurationAsJson);

                    MemoryStream memoryStream = new MemoryStream(Encoding.UTF32.GetBytes(encryptedConfigurationContent));

                    var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
                    {
                        ThrowOnCancel = true,
                        AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken),
                    })
                        .Child(configuration.userName)
                        .Child(configuration.id.ToString())
                        .Child($"Configuration.txt")
                        .PutAsync(memoryStream);

                    task.Progress.ProgressChanged += (s, args) =>
                    {
                    };
                    string name = await task;

                    memoryStream.Close();

                    return true;
                }
                catch (FirebaseStorageException firebaseException)
                {
                    Console.WriteLine(firebaseException.Message);
                    return false;
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static async Task<UserConfiguration> GetUserConfiguration(string userName, Guid id)
        {
            //try
            //{
            await CreateFireBaseSecureLogin();
            var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
            {
                ThrowOnCancel = true,
                AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken),
            })
            .Child(userName)
            .Child(id.ToString())
            .Child($"Configuration.txt");
            var gsreference = await task.GetDownloadUrlAsync();

            string fileContent = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                fileContent = await httpClient.GetStringAsync(gsreference);
            }
            byte[] bytes = Encoding.Default.GetBytes(fileContent);
            fileContent = Encoding.UTF32.GetString(bytes);

            string decodedConfiguration = CryptionLogic.DecryptUserString(fileContent);
            UserConfiguration configuration = JsonConvert.DeserializeObject<UserConfiguration>(decodedConfiguration);
            return configuration;
            //}
            //catch (FirebaseStorageException firebaseException)
            //{
            //    Console.WriteLine(firebaseException.Message);
            //    return new UserConfiguration();
            //}
            //catch (Exception exception)
            //{
            //    Console.WriteLine(exception.Message);
            //    return new UserConfiguration();
            //}
        }


        public static async Task<string> UploadProfilePicture(UploadFile uploadFile)
        {
            try
            {
                await CreateFireBaseSecureLogin();
                MemoryStream memoryStream = new MemoryStream(uploadFile.fileStream);
                memoryStream.Position = 0;

                Stream stream = memoryStream;

                var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                    AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken),
                })
                    .Child(uploadFile.userName)
                    .Child(uploadFile.fileName.ToString())
                    .Child("logo.png")
                    .PutAsync(stream);

                task.Progress.ProgressChanged += (s, args) => { };
                var result = await task;
                return result;
            }
            catch (FirebaseStorageException firebaseException)
            {
                Console.WriteLine(firebaseException.Message);
                return String.Empty;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return String.Empty;
            }
        }

        public static async Task<string> GetLinkToProfilePic(string userName, string id)
        {
            try
            {
                await CreateFireBaseSecureLogin();
                var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                    AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken),
                })
                    .Child(userName)
                    .Child(id.ToString())
                    .Child("logo.png");
                var gsreference = await task.GetDownloadUrlAsync();

                string fileContent = string.Empty;
                using (HttpClient httpClient = new HttpClient())
                {
                    fileContent = await httpClient.GetStringAsync(gsreference);
                }

                return gsreference;
            }
            catch (FirebaseStorageException firebaseException)
            {
                Console.WriteLine(firebaseException.Message);
                return String.Empty;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return String.Empty;
            }
        }

        public static async Task<bool> DeleteProfilePicture(DeleteFileRequest deleteFileRequest)
        {
            try
            {
                await CreateFireBaseSecureLogin();
                var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                    AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken),
                })
                    .Child(deleteFileRequest.userName)
                    .Child(deleteFileRequest.fileName.ToString())
                    .Child("logo.png")
                    .DeleteAsync();

                await task;
                return true;
            }
            catch (FirebaseStorageException firebaseException)
            {
                Console.WriteLine(firebaseException.Message);
                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }

        public static async Task<bool> DeletePasswordFile(string userName, string password)
        {
            try
            {
                await CreateFireBaseSecureLogin();
                var task = new FirebaseStorage(storageBucketUser, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                    AuthTokenAsyncFactory = () => Task.FromResult(credentials.User.Credential.IdToken),
                })
                    .Child(userName)
                    .Child($"{password}.txt")
                    .DeleteAsync();

                await task;
                return true;
            }
            catch (FirebaseStorageException firebaseException)
            {
                Console.WriteLine(firebaseException.Message);
                return false;
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
        }
    }
}