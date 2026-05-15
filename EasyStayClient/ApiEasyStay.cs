using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using System.IO;
using System.Reflection;

namespace EasyStayClient
{
    public static class ApiEasyStay
    {
        private static readonly HttpClient _httpsClient;
        private const string BaseApiUrl = "https://easystay.tunyl.com/api/";
        private static bool _firebaseInitialized = false;

        static ApiEasyStay()
        {
            _httpsClient = new HttpClient();
            _httpsClient.BaseAddress = new Uri(BaseApiUrl);
            _httpsClient.DefaultRequestHeaders.Accept.Clear();
            _httpsClient.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            InitializeFirebase();
        }

        private static void InitializeFirebase()
        {
            if (_firebaseInitialized) return;

            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                // ВЫВОДИМ ВСЕ РЕСУРСЫ ДЛЯ ОТЛАДКИ
                Console.WriteLine("=== Доступные ресурсы ===");
                foreach (var name in assembly.GetManifestResourceNames())
                {
                    Console.WriteLine($"- {name}");
                }

                // Ищем JSON файл в любом месте (включая папку Resources)
                var resourceName = assembly.GetManifestResourceNames()
                    .FirstOrDefault(name => name.EndsWith(".json") &&
                                           (name.Contains("firebase") || name.Contains("adminsdk")));

                if (resourceName == null)
                {
                    Console.WriteLine("Firebase JSON файл не найден в ресурсах");
                    Console.WriteLine("Проверьте, что файл имеет Build Action = Embedded resource");
                    return;
                }

                Console.WriteLine($"Найден файл: {resourceName}");

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string jsonContent = reader.ReadToEnd();
                    Console.WriteLine($"JSON загружен, длина: {jsonContent.Length} символов");

                    if (FirebaseApp.DefaultInstance == null)
                    {
                        FirebaseApp.Create(new AppOptions()
                        {
                            Credential = GoogleCredential.FromJson(jsonContent)
                        });
                        _firebaseInitialized = true;
                        Console.WriteLine("Firebase инициализирован УСПЕШНО!");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка инициализации Firebase: {ex.Message}");
                Console.WriteLine($"Детали: {ex.StackTrace}");
            }
        }

        // GET - получение данных
        public static async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await _httpsClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }

        // POST - создание новой записи
        public static async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpsClient.PostAsync(endpoint, content);
        }

        // PUT - обновление существующей записи
        public static async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T data)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpsClient.PutAsync(endpoint, content);
        }

        // DELETE - удаление записи
        public static async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            return await _httpsClient.DeleteAsync(endpoint);
        }

        // Отправка push-уведомления через Firebase
        public static async Task<bool> SendPushNotification(string deviceToken, string title, string body)
        {
            if (!_firebaseInitialized)
            {
                Console.WriteLine("Firebase не инициализирован");
                return false;
            }

            if (string.IsNullOrEmpty(deviceToken))
            {
                Console.WriteLine("Device token пустой");
                return false;
            }

            try
            {
                var message = new Message()
                {
                    Token = deviceToken,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body,
                    }
                };

                string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Console.WriteLine($"Уведомление отправлено: {response}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки уведомления: {ex.Message}");
                return false;
            }
        }
    }
}