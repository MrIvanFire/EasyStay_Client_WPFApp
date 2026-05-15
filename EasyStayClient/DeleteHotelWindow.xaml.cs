using System;
using System.Net.Http;
using System.Windows;

namespace EasyStayClient
{
    public partial class DeleteHotelWindow : Window
    {
        public DeleteHotelWindow()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            HotelsWindow hotelsWindow = new HotelsWindow();
            hotelsWindow.Show();
            this.Close();
        }

        private async void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtId.Text))
            {
                MessageBox.Show("Введите ID отеля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtId.Text, out int hotelId))
            {
                MessageBox.Show("ID должен быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var response = await ApiEasyStay.DeleteAsync($"hotels/{hotelId}");

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Отель с ID {hotelId} удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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