using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EasyStayClient
{
    /// <summary>
    /// Логика взаимодействия для AdminMainWindow.xaml
    /// </summary>
    public partial class AdminMainWindow : Window
    {
        public AdminMainWindow()
        {
            InitializeComponent();
        }
        private void BtnHotels_Click(object sender, RoutedEventArgs e)
        { 
            HotelsWindow hotelsWindow = new HotelsWindow();
            hotelsWindow.Show(); 
            this.Close();
        }

        private void BtnBookings_Click(object sender, RoutedEventArgs e)
        { 
            BookingsWindow bookingsWindow = new BookingsWindow();
            bookingsWindow.Show();
            this.Close();
        }

        private void BtnRooms_Click(object sender, RoutedEventArgs e)
        { 
            RoomsWindow roomsWindow = new RoomsWindow();
            roomsWindow.Show();
            this.Close();
        }

        private void BtnReviews_Click(object sender, RoutedEventArgs e)
        { 
            ReviewsWindow reviewsWindow = new ReviewsWindow();
            reviewsWindow.Show();
            this.Close();
        }
         
    }
}
