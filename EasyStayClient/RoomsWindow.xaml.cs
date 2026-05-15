using System;
using System.Windows;

namespace EasyStayClient
{
    public partial class RoomsWindow : Window
    {
        public RoomsWindow()
        {
            InitializeComponent();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminMainWindow mainWindow = new AdminMainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void BtnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtHotelId.Text))
            {
                MessageBox.Show("Введите ID отеля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtHotelId.Text, out int hotelId))
            {
                MessageBox.Show("ID отеля должен быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AddRoomWindow addRoomWindow = new AddRoomWindow();
            addRoomWindow.HotelId = hotelId;
            addRoomWindow.Show();
            this.Close();
        }

        private void BtnEditRoom_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtHotelId.Text))
            {
                MessageBox.Show("Введите ID отеля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtHotelId.Text, out int hotelId))
            {
                MessageBox.Show("ID отеля должен быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            EditRoomWindow editRoomWindow = new EditRoomWindow();
            editRoomWindow.HotelId = hotelId;
            editRoomWindow.Show();
            this.Close();
        }

        private void BtnDeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtHotelId.Text))
            {
                MessageBox.Show("Введите ID отеля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!int.TryParse(TxtHotelId.Text, out int hotelId))
            {
                MessageBox.Show("ID отеля должен быть числом", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DeleteRoomWindow deleteRoomWindow = new DeleteRoomWindow();
            deleteRoomWindow.HotelId = hotelId;
            deleteRoomWindow.Show();
            this.Close();
        }
    }
}