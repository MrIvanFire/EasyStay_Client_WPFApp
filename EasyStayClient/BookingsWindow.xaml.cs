using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace EasyStayClient
{
    public partial class BookingsWindow : Window
    {
        public BookingsWindow()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminMainWindow mainWindow = new AdminMainWindow();
            mainWindow.Show();
            this.Close();
        }

        private async void BtnConfirmBooking_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBookingId.Text))
            {
                MessageBox.Show("Введите ID бронирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtBookingId.Text, out int bookingId))
            {
                MessageBox.Show("ID бронирования должен быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string updateData = "подтверждено";

            var response = await ApiEasyStay.PutAsync($"bookings/{bookingId}/status", updateData);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Бронирование {bookingId} подтверждено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                try
                {
                    var booking = await ApiEasyStay.GetAsync<JsonElement>($"bookings/{bookingId}");
                    int userId = booking.GetProperty("userId").GetInt32();
                    string deviceToken = await ApiEasyStay.GetAsync<string>($"users/{userId}/fcm-token");

                    if (string.IsNullOrEmpty(deviceToken))
                    {
                        Console.WriteLine("DeviceToken пустой, уведомление не отправлено");
                        return;
                    }
                    var room = booking.GetProperty("room");
                    var roomType = room.GetProperty("roomType");
                    var hotel = roomType.GetProperty("hotel");

                    string hotelName = hotel.GetProperty("name").GetString();
                    string roomCategory = roomType.GetProperty("category").GetString();
                    string checkIn = booking.GetProperty("checkInDate").GetString();
                    string checkOut = booking.GetProperty("checkOutDate").GetString();
                    DateTime checkInDate = DateTime.Parse(checkIn);
                    DateTime checkOutDate = DateTime.Parse(checkOut);

                    string title = "Бронирование подтверждено";
                    string body = $"Отель: {hotelName}\n" +
                                 $"Номер: {roomCategory}\n" +
                                 $"Заезд: {checkInDate:dd.MM.yyyy}\n" +
                                 $"Выезд: {checkOutDate:dd.MM.yyyy}";

                    await ApiEasyStay.SendPushNotification(deviceToken, title, body);
                    Console.WriteLine("Уведомление отправлено успешно!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка отправки уведомления: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }

                TxtBookingId.Clear();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка: {response.StatusCode}\n{error}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void BtnCancelBooking_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBookingId.Text))
            {
                MessageBox.Show("Введите ID бронирования", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtBookingId.Text, out int bookingId))
            {
                MessageBox.Show("ID бронирования должен быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string updateData = "отменено";
            var response = await ApiEasyStay.PutAsync($"bookings/{bookingId}/status", updateData);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Бронирование {bookingId} отменено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                try
                {
                    var booking = await ApiEasyStay.GetAsync<JsonElement>($"bookings/{bookingId}");
                    int userId = booking.GetProperty("userId").GetInt32();
                    string deviceToken = await ApiEasyStay.GetAsync<string>($"users/{userId}/fcm-token");

                    if (string.IsNullOrEmpty(deviceToken))
                    {
                        Console.WriteLine("DeviceToken пустой, уведомление не отправлено");
                        return;
                    }
                    var room = booking.GetProperty("room");
                    var roomType = room.GetProperty("roomType");
                    var hotel = roomType.GetProperty("hotel");

                    string hotelName = hotel.GetProperty("name").GetString();
                    string roomCategory = roomType.GetProperty("category").GetString();
                    string checkIn = booking.GetProperty("checkInDate").GetString();
                    string checkOut = booking.GetProperty("checkOutDate").GetString();
                    DateTime checkInDate = DateTime.Parse(checkIn);
                    DateTime checkOutDate = DateTime.Parse(checkOut);

                    string title = "Бронирование отменено";
                    string body = $"Отель: {hotelName}\n" +
                                 $"Номер: {roomCategory}\n" +
                                 $"Даты: {checkInDate:dd.MM.yyyy} - {checkOutDate:dd.MM.yyyy}";

                    await ApiEasyStay.SendPushNotification(deviceToken, title, body);
                    Console.WriteLine("Уведомление отправлено успешно!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка отправки уведомления: {ex.Message}");
                    Console.WriteLine($"Stack trace: {ex.StackTrace}");
                }

                TxtBookingId.Clear();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка: {response.StatusCode}\n{error}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}