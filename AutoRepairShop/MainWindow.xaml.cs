using System.Windows;

namespace AutoRepairShop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new PageCars());
            Navigation.MainFrame = MainFrame;
        }
        private void btnCars_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageCars());
        }
        private void btnTypeofRepair_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageTypeofRepair());
        }
        private void btnRepair_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageRepair());
        }
        private void btnAddCar_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageAddCar(null));
        }
        private void btnUchetnyaZapis_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageUchetnyaZapis());
        }
        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageChanges());
        }
        private void btnAllLogins_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageAllLogins());
        }
    }
    public class GlobalUser
    {
        public static int globalIdUser;
        public static bool admin = false;
    }
}
