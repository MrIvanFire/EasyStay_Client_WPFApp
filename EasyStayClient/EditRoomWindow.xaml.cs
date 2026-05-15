using System;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace EasyStayClient
{
    public partial class EditRoomWindow : Window
    {
        public int HotelId { get; set; }

        public EditRoomWindow()
        {
            InitializeComponent();
            Loaded += EditRoomWindow_Loaded;
        }

        private void EditRoomWindow_Loaded(object sender, RoutedEventArgs e)
        {
            RoomType.Items.Add("Стандарт");
            RoomType.Items.Add("Люкс");
            RoomType.Items.Add("Семейный");
            RoomType.Items.Add("Эконом");
            RoomType.SelectedIndex = 0;
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            RoomsWindow roomsWindow = new RoomsWindow();
            roomsWindow.Show();
            this.Close();
        }

        private async void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtRoomNumber.Text) || RoomType.SelectedItem == null)
            {
                MessageBox.Show("Заполните номер комнаты и выберите тип", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (HotelId == 0)
            {
                MessageBox.Show("ID отеля не передан.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var room = new
            {
                roomNumber = TxtRoomNumber.Text.Trim(),
                status = "свободен",
                pricePerNight = 0m
            };

            var response = await ApiEasyStay.PutAsync($"rooms/hotel/{HotelId}/room/{TxtRoomNumber.Text.Trim()}", room);

            if (response.IsSuccessStatusCode)
            {
                string photoUrl = TxtPhotoUrl.Text.Trim();
                if (!string.IsNullOrEmpty(photoUrl))
                {
                    var photo = new { roomId = HotelId, photoUrl = photoUrl, isMain = true };
                    await ApiEasyStay.PostAsync("roomphotos", photo);
                }

                MessageBox.Show("Данные номера обновлены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                BtnBack_Click(sender, e);
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка: {response.StatusCode}\n{error}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}