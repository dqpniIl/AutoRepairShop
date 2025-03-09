using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoRepairShop
{
    /// <summary>
    /// Логика взаимодействия для PageRepair.xaml
    /// </summary>
    public partial class PageRepair : Page
    {
        Entities entities = new Entities();
        public PageRepair()
        {
            InitializeComponent();
            tbCost.PreviewTextInput += tbCost_Text;
            foreach (var repair in entities.Repair)
                lboxRepair.Items.Add(repair);
            foreach (var combo in entities.Cars)
                comboCar.Items.Add(combo);
            foreach (var combo in entities.TypeOfRepair)
                comboVid.Items.Add(combo);
        }
        private void lboxRepair_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_zapis = lboxRepair.SelectedItem as Repair;
            if (selected_zapis != null)
            {
                tbCost.Text = selected_zapis.cost_repair.ToString();
                comboCar.SelectedItem = selected_zapis.Cars;
                comboVid.SelectedItem = selected_zapis.TypeOfRepair;
            }
            else
            {
                tbCost.Text = "";
                comboCar.SelectedIndex = -1;
                comboVid.SelectedIndex = -1;
            }
        }
        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var zapis = lboxRepair.SelectedItem as Repair;
            if (tbCost.Text == "" || comboCar.SelectedIndex == -1 || comboVid.SelectedIndex == -1)
                MessageBox.Show("Заполни все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                if (zapis == null)
                {
                    zapis = new Repair();
                    entities.Repair.Add(zapis);
                    lboxRepair.Items.Add(zapis);
                    zapis.cost_repair = tbCost.Text;
                    zapis.id_Car = (comboCar.SelectedItem as Cars).Id_Car;
                    zapis.id_TypeOfRepair = (comboVid.SelectedItem as TypeOfRepair).id_TypeOfRepair;
                    string AddString;
                    AddString = $"Стоимость: {tbCost.Text}, Машина: {(comboCar.SelectedItem as Cars).model_car}, Вид ремонта: {(comboVid.SelectedItem as TypeOfRepair).vid_TypeOfRepair}";
                    entities.SaveChanges();
                    lboxRepair.Items.Refresh();
                    MessageBox.Show("Чек на ремонт добавлен", "Добавление", MessageBoxButton.OK, MessageBoxImage.Information);
                    var User = entities.Users.FirstOrDefault(x => x.Id_User == GlobalUser.globalIdUser);
                    string proverkaNaSurnameName;
                    proverkaNaSurnameName = User.surname + " " + User.name;
                    if (User.surname == null && User.name == null)
                    {
                        proverkaNaSurnameName = User.login;
                    }
                    ChangesDataBase change = new ChangesDataBase
                    {
                        Changer_surnameAndname = proverkaNaSurnameName,
                        Change = $"Добавлена запись(Чек на ремонт машины): {AddString}",
                        Change_date = DateTime.Now.ToString(),
                    };
                    entities.ChangesDataBase.Add(change);
                    entities.SaveChanges();
                }
                else
                {
                    string tbIshodniy, tbIzmenenie;
                    tbIshodniy = $"Стоимость: {zapis.cost_repair}, Машина: {zapis.Cars.model_car}, Вид ремонта: {zapis.TypeOfRepair.vid_TypeOfRepair}";
                    zapis.cost_repair = tbCost.Text;
                    zapis.id_Car = (comboCar.SelectedItem as Cars).Id_Car;
                    zapis.id_TypeOfRepair = (comboVid.SelectedItem as TypeOfRepair).id_TypeOfRepair;
                    tbIzmenenie = $"Стоимость: {tbCost.Text}, Машина: {(comboCar.SelectedItem as Cars).model_car}, Вид ремонта: {(comboVid.SelectedItem as TypeOfRepair).vid_TypeOfRepair}";
                    entities.SaveChanges();
                    lboxRepair.Items.Clear();
                    foreach (var repair in entities.Repair)
                        lboxRepair.Items.Add(repair);
                    MessageBox.Show("Чек на ремонт успешно измененён!", "Изменение", MessageBoxButton.OK, MessageBoxImage.Information);
                    var User = entities.Users.FirstOrDefault(x => x.Id_User == GlobalUser.globalIdUser);
                    string proverkaNaSurnameName;
                    proverkaNaSurnameName = User.surname + " " + User.name;
                    if (User.surname == null && User.name == null)
                    {
                        proverkaNaSurnameName = User.login;
                    }
                    ChangesDataBase change = new ChangesDataBase
                    {
                        Changer_surnameAndname = proverkaNaSurnameName,
                        Change = $"Изменена запись(Чек на ремонт машины): {tbIshodniy} => {tbIzmenenie}",
                        Change_date = DateTime.Now.ToString(),
                    };
                    entities.ChangesDataBase.Add(change);
                    entities.SaveChanges();
                }
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var rezult = MessageBox.Show("Вы действительно хотите удалить этот чек?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (rezult == MessageBoxResult.No)
                return;

            var delete_zapis = lboxRepair.SelectedItem as Repair;
            if (delete_zapis != null)
            {
                var User = entities.Users.FirstOrDefault(x => x.Id_User == GlobalUser.globalIdUser);
                string proverkaNaSurnameName;
                string delete_zapisDublyash = $"Стоимость: {delete_zapis.cost_repair}, Машина: {delete_zapis.Cars.model_car}, Вид ремонта: {delete_zapis.TypeOfRepair.vid_TypeOfRepair}";
                proverkaNaSurnameName = User.surname + " " + User.name;
                if (User.surname == null && User.name == null)
                {
                    proverkaNaSurnameName = User.login;
                }
                ChangesDataBase change = new ChangesDataBase
                {
                    Changer_surnameAndname = proverkaNaSurnameName,
                    Change = $"Удалена запись(Вид ремонта машины): {delete_zapisDublyash}",
                    Change_date = DateTime.Now.ToString(),
                };
                entities.ChangesDataBase.Add(change);
                entities.Repair.Remove(delete_zapis);
                entities.SaveChanges();
                tbCost.Clear();
                comboCar.SelectedIndex = -1;
                comboVid.SelectedIndex = -1;
                lboxRepair.Items.Remove(delete_zapis);
                MessageBox.Show("Чек удалён!", "Удаление", MessageBoxButton.OK, MessageBoxImage.Question);
            }
            else
                MessageBox.Show("Нет удаляемых чеков!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lboxRepair.SelectedIndex = -1;
            tbCost.Clear();
            comboCar.SelectedIndex = -1;
            comboVid.SelectedIndex = -1;
            tbCost.Focus();
        }
        private void tbCost_Text(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
