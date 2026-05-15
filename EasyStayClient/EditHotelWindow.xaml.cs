using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace EasyStayClient
{
    public partial class EditHotelWindow : Window
    {
        public EditHotelWindow()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            HotelsWindow hotelsWindow = new HotelsWindow();
            hotelsWindow.Show();
            this.Close();
        }

        private async void BtnConfirm_Click(object sender, RoutedEventArgs e)
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

            if (string.IsNullOrWhiteSpace(TxtName.Text) ||
                string.IsNullOrWhiteSpace(TxtCity.Text) ||
                string.IsNullOrWhiteSpace(TxtAddress.Text))
            {
                MessageBox.Show("Заполните название, город и адрес", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            decimal rating = 0;
            if (!string.IsNullOrWhiteSpace(TxtRating.Text))
            {
                if (!decimal.TryParse(TxtRating.Text, out rating) || rating < 1 || rating > 5)
                {
                    MessageBox.Show("Рейтинг должен быть числом от 1 до 5", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            string photoUrlsText = TxtPhotoUrls.Text.Trim();
            string[] photoUrls = string.IsNullOrEmpty(photoUrlsText)
                ? new string[] { }
                : photoUrlsText.Split(new char[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(u => u.Trim())
                               .Where(u => !string.IsNullOrEmpty(u))
                               .ToArray();

            var hotelPhotos = new List<object>();
            for (int i = 0; i < photoUrls.Length; i++)
            {
                hotelPhotos.Add(new
                {
                    photoUrl = photoUrls[i],
                    isMain = i == 0
                });
            }

            var hotel = new
            {
                name = TxtName.Text.Trim(),
                city = TxtCity.Text.Trim(),
                address = TxtAddress.Text.Trim(),
                rating = rating,
                description = TxtDescription.Text.Trim(),
                hotelPhotos = hotelPhotos
            };

            var response = await ApiEasyStay.PutAsync($"hotels/{hotelId}", hotel);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Данные отеля обновлены", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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