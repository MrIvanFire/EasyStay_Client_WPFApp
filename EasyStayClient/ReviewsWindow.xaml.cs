using System;
using System.Net.Http;
using System.Windows;

namespace EasyStayClient
{
    public partial class ReviewsWindow : Window
    {
        public ReviewsWindow()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminMainWindow mainWindow = new AdminMainWindow();
            mainWindow.Show();
            this.Close();
        }

        private async void BtnDeleteReview_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtReviewId.Text))
            {
                MessageBox.Show("Введите ID отзыва", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtReviewId.Text, out int reviewId))
            {
                MessageBox.Show("ID отзыва должен быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var response = await ApiEasyStay.DeleteAsync($"reviews/{reviewId}");

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Отзыв {reviewId} удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                TxtReviewId.Clear();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"Ошибка: {response.StatusCode}\n{error}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}