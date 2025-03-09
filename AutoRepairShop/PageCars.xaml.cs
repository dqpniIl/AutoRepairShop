using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace AutoRepairShop
{
    /// <summary>
    /// Логика взаимодействия для PageCars.xaml
    /// </summary>
    public partial class PageCars : Page
    {
        Entities entities = new Entities();
        private ObservableCollection<Cars> Zapisi;
        private Cars car = new Cars();
        public PageCars()
        {
            InitializeComponent();
            Zapisi = new ObservableCollection<Cars>();
            ListViewCars.Items.Clear();
            ListViewCars.ItemsSource = Zapisi;
            LoadingZapisi();
            DataContext = car;
            comboPriceCar.SelectionChanged += Filter_comboPriceCar;
        }
        private void LoadingZapisi()
        {
            using (var newContext = new Entities())
            {
                var newZapisi = newContext.Cars.ToList();
                Zapisi.Clear();
                foreach (var car in newZapisi)
                    Zapisi.Add(car);
            }
        }
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new PageAddCar((sender as Button).DataContext as Cars));
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите удалить выбранные элементы? {ListViewCars.SelectedItems.Count}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    using (var newContext = new Entities())
                    {
                        foreach (var zapis in ListViewCars.SelectedItems.Cast<Cars>().ToList())
                        {
                            var remove_zapis = newContext.Cars.Find(zapis.Id_Car);
                            var exist = entities.Repair.Any(repair => repair.id_Car == remove_zapis.Id_Car);
                            if (exist)
                            {
                                MessageBox.Show("Запись удалить нельзя!\nСуществуют связующие записи с чеками на ремонт машин!\nУдалите их и повторите попытку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                ListViewCars.SelectedIndex = -1;
                                return;
                            }
                            else
                            {
                                if (remove_zapis != null)
                                {
                                    newContext.Cars.Remove(remove_zapis);
                                    var User = entities.Users.FirstOrDefault(x => x.Id_User == GlobalUser.globalIdUser);
                                    string proverkaNaSurnameName;
                                    proverkaNaSurnameName = User.surname + " " + User.name;
                                    if (User.surname == null && User.name == null)
                                        proverkaNaSurnameName = User.login;
                                    ChangesDataBase change = new ChangesDataBase
                                    {
                                        Changer_surnameAndname = proverkaNaSurnameName,
                                        Change = $"Удалена запись(Машины): Марка: {remove_zapis.mark_car}, Модель: {remove_zapis.model_car}, Цена: {remove_zapis.price_car}",
                                        Change_date = DateTime.Now.ToString(),
                                    };
                                    newContext.ChangesDataBase.Add(change);
                                }
                            }
                        }
                        newContext.SaveChanges();
                        MessageBox.Show("Данные удалены!");
                        LoadingZapisi();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
        private void Filter_comboPriceCar(object sender, EventArgs e)
        {
            if (Zapisi == null)
                return;
            var selectedPriceCar = ((ComboBoxItem)comboPriceCar.SelectedItem).Content.ToString();
            if (selectedPriceCar == "-")
            {
                ListViewCars.ItemsSource = Zapisi;
            }
            else
            {
                double price = 0;
                switch (selectedPriceCar)
                {
                    case "до 500 тыс. рублей":
                        price = 500000;
                        break;
                    case "до 1 млн. рублей":
                        price = 1000000;
                        break;
                    case "до 3 млн. рублей":
                        price = 3000000;
                        break;
                    case "до 5 млн. рублей":
                        price = 5000000;
                        break;
                    case "до 10 млн. рублей":
                        price = 10000000;
                        break;
                    case "до 20 млн. рублей":
                        price = 20000000;
                        break;
                    default:
                        return;
                }
                var filterZapisi = Zapisi.Where(zapis => zapis.price_car <= price).ToList();
                ListViewCars.ItemsSource = filterZapisi;
            }
        }
        private void tbsearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbsearch.Text.ToLower();
            var collectionView = CollectionViewSource.GetDefaultView(ListViewCars.ItemsSource);
            if (collectionView != null)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    collectionView.Filter = null;
                }
                else
                {
                    collectionView.Filter = item =>
                    {
                        Cars zapis_Car = item as Cars;

                        if (zapis_Car != null)
                        {
                            return zapis_Car.mark_car.ToLower().Contains(searchText) || zapis_Car.model_car.ToLower().Contains(searchText) || zapis_Car.price_car.ToString().ToLower().Contains(searchText);
                        }
                        return false;
                    };
                }
            }
        }
    }
}
