using System;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace EasyStayClient
{
    public partial class AddRoomWindow : Window
    {
        public int HotelId { get; set; }

        public AddRoomWindow()
        {
            InitializeComponent();
            Loaded += AddRoomWindow_Loaded;
        }

        private void AddRoomWindow_Loaded(object sender, RoutedEventArgs e)
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

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
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
                pricePerNight = 0m,
                status = "свободен",
                roomTypeId = HotelId
            };

            var response = await ApiEasyStay.PostAsync("rooms", room);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var json = JsonDocument.Parse(responseContent);
                int roomId = json.RootElement.GetProperty("id").GetInt32();

                string photoUrl = TxtPhotoUrl.Text.Trim();
                if (!string.IsNullOrEmpty(photoUrl))
                {
                    var photo = new { roomId = roomId, photoUrl = photoUrl, isMain = true };
                    await ApiEasyStay.PostAsync("roomphotos", photo);
                }

                MessageBox.Show("Номер добавлен", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                BtnBack_Click(sender, e);
            }
            else
            {
                MessageBox.Show($"Ошибка: {response.StatusCode}\n{responseContent}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}