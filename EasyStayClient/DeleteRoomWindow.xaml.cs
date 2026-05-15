using System;
using System.Net.Http;
using System.Windows;

namespace EasyStayClient
{
    public partial class DeleteRoomWindow : Window
    {
        public int HotelId { get; set; }

        public DeleteRoomWindow()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            RoomsWindow roomsWindow = new RoomsWindow();
            roomsWindow.Show();
            this.Close();
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtRoomNumber.Text))
            {
                MessageBox.Show("Введите номер комнаты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (HotelId == 0)
            {
                MessageBox.Show("ID отеля не передан. Вернитесь в окно управления номерами и укажите ID.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string roomNumber = TxtRoomNumber.Text.Trim();

            var response = await ApiEasyStay.DeleteAsync($"rooms/hotel/{HotelId}/room/{roomNumber}");

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Номер {roomNumber} удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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