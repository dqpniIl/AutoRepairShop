using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace AutoRepairShop
{
    /// <summary>
    /// Логика взаимодействия для PageAddCar.xaml
    /// </summary>
    public partial class PageAddCar : Page
    {
        Entities entities = new Entities();
        private Cars car = new Cars();
        private Cars carIshodnya = new Cars();
        BitmapImage bitmap;
        public PageAddCar(Cars selectCar)
        {
            InitializeComponent();
            if (selectCar != null)
            {
                car = selectCar;
            }
            DataContext = car;
            tbPriceCar.PreviewTextInput += tbPrice_Text;
            carIshodnya.mark_car = car.mark_car;
            carIshodnya.model_car = car.model_car;
            carIshodnya.price_car = car.price_car;
            carIshodnya.image_car = car.image_car;
        }
        private void tbPrice_Text(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
        private void btnSaveImage_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            if (dlg.ShowDialog() == true && !string.IsNullOrWhiteSpace(dlg.FileName))
                tbSaveImage.Text = dlg.FileName.ToString();
            tbSaveImage.Focus();
            car.image_car = dlg.FileName;
            string selectedImage = dlg.FileName;
            bitmap = new BitmapImage(new Uri(selectedImage));
            imageCar.Source = bitmap;
        }
        private void btnSaveCar_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(car.mark_car))
                errors.AppendLine("Не введена марка машины");
            if (string.IsNullOrWhiteSpace(car.model_car))
                errors.AppendLine("Не введена модель машины");
            if (string.IsNullOrWhiteSpace(car.price_car.ToString()))
                errors.AppendLine("Не введена цена машины");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            var User = entities.Users.FirstOrDefault(x => x.Id_User == GlobalUser.globalIdUser);
            string proverkaNaSurnameName;
            proverkaNaSurnameName = User.surname + " " + User.name;
            if (User.surname == null && User.name == null)//Проверка на пустоту данных
                proverkaNaSurnameName = User.login;
            if (car.Id_Car == 0)
            {
                Entities.GetContext().Cars.Add(car);
                ChangesDataBase new_change = new ChangesDataBase
                {
                    Changer_surnameAndname = proverkaNaSurnameName,
                    Change = $"Добавлена запись(Машины): Марка: {car.mark_car}, Модель: {car.model_car}, Цена: {car.price_car}",//В чем заключается изменение
                    Change_date = DateTime.Now.ToString(),
                };
                entities.ChangesDataBase.Add(new_change);//Занесение изменений
                entities.SaveChanges();//Сохранение записей в базе данных
                Entities.GetContext().SaveChanges();//Сохранение контекста записи
                MessageBox.Show("Данные сохранены!");
                Navigation.MainFrame.Navigate(new PageCars());
            }
            else
            {
                try
                {
                    var existingCar = entities.Cars.FirstOrDefault(c => c.Id_Car == car.Id_Car);
                    if (existingCar != null)
                    {
                        existingCar.mark_car = car.mark_car;
                        existingCar.model_car = car.model_car;
                        existingCar.price_car = car.price_car;
                        existingCar.image_car = car.image_car;
                        ChangesDataBase change = new ChangesDataBase
                        {
                            Changer_surnameAndname = proverkaNaSurnameName,
                            Change = $"Изменена запись(Машины): Марка: {carIshodnya.mark_car}, Модель: {carIshodnya.model_car}, Цена: {carIshodnya.price_car} => Марка: {car.mark_car}, Модель: {car.model_car}, Цена: {car.price_car}",
                            Change_date = DateTime.Now.ToString(),
                        };
                        entities.ChangesDataBase.Add(change);
                        entities.SaveChanges();
                        Entities.GetContext().SaveChanges();
                        MessageBox.Show("Изменения успешно сохранены!");
                        Navigation.MainFrame.Navigate(new PageCars());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
