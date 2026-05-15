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
    /// Логика взаимодействия для HotelsWindow.xaml
    /// </summary>
    public partial class HotelsWindow : Window
    {
        public HotelsWindow()
        {
            InitializeComponent();
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            AdminMainWindow mainWindow = new AdminMainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void BtnAddHotel_Click(object sender, RoutedEventArgs e)
        {
            AddHotelWindow addHotelWindow = new AddHotelWindow();
            addHotelWindow.Show();
            this.Close();
        }

        private void BtnEditHotel_Click(object sender, RoutedEventArgs e)
        {
            EditHotelWindow editHotelWindow = new EditHotelWindow();
            editHotelWindow.Show();
            this.Close();
        }

        private void BtnDeleteHotel_Click(object sender, RoutedEventArgs e)
        {
            DeleteHotelWindow deleteHotelWindow = new DeleteHotelWindow();
            deleteHotelWindow.Show();
            this.Close();
        }
    }
}
