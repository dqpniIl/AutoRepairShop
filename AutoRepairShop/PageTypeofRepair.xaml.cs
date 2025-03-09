using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace AutoRepairShop
{
    /// <summary>
    /// Логика взаимодействия для PageDep.xaml
    /// </summary>
    public partial class PageTypeofRepair : Page
    {
        Entities entities = new Entities();
        public PageTypeofRepair()
        {
            InitializeComponent();
            foreach (var type in entities.TypeOfRepair)
                lboxTypeofRepair.Items.Add(type);
        }
        private void lboxTypeofRepair_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_vid = lboxTypeofRepair.SelectedItem as TypeOfRepair;
            if (selected_vid != null)
            {
                tbTypeofRepair.Text = selected_vid.vid_TypeOfRepair;
            }
            else
            {
                tbTypeofRepair.Text = "";
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbTypeofRepair.Text))
                MessageBox.Show("Заполните поле вида!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                var existVid = entities.TypeOfRepair.FirstOrDefault(x => x.vid_TypeOfRepair == tbTypeofRepair.Text);
                if (existVid != null)
                    MessageBox.Show("Такой вид уже есть", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                {
                    var selectedVid = lboxTypeofRepair.SelectedItem as TypeOfRepair;
                    string tbIshodniy, tbIzmenenie;
                    if (selectedVid != null)
                    {
                        tbIshodniy = selectedVid.vid_TypeOfRepair;
                        selectedVid.vid_TypeOfRepair = tbTypeofRepair.Text;
                        tbIzmenenie = tbTypeofRepair.Text;
                        lboxTypeofRepair.Items.Refresh();
                        MessageBox.Show("Вид ремонта машины успешно измененён!", "Изменение", MessageBoxButton.OK, MessageBoxImage.Information);
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
                            Change = $"Изменена запись(Вид ремонта машины): {tbIshodniy} => {tbIzmenenie}",
                            Change_date = DateTime.Now.ToString(),
                        };
                        entities.ChangesDataBase.Add(change);
                        entities.SaveChanges();
                    }
                    else
                    {
                        var newVid = new TypeOfRepair();
                        newVid.vid_TypeOfRepair = tbTypeofRepair.Text;
                        string VidString;
                        VidString = tbTypeofRepair.Text;
                        entities.TypeOfRepair.Add(newVid);
                        entities.SaveChanges();
                        lboxTypeofRepair.Items.Add(newVid);
                        MessageBox.Show("Вид ремонта добавлен", "Добавление", MessageBoxButton.OK, MessageBoxImage.Information);
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
                            Change = $"Добавлена запись(Вид ремонта машины): {VidString}",
                            Change_date = DateTime.Now.ToString(),
                        };
                        entities.ChangesDataBase.Add(change);
                        entities.SaveChanges();
                    }
                }
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            var delete_vid = lboxTypeofRepair.SelectedItem as TypeOfRepair;
            if (delete_vid == null)
            {
                MessageBox.Show("Выберите вид для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var exist = entities.Repair.Any(repair => repair.id_TypeOfRepair == delete_vid.id_TypeOfRepair);
            if (exist)
            {
                MessageBox.Show("Запись удалить нельзя!\nСуществуют связующие записи с заявками на ремонт машин!\nУдалите их и повторите попытку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                lboxTypeofRepair.SelectedItem = null;
            }
            else
            {
                var rezult = MessageBox.Show("Вы действительно хотите удалить запись?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rezult == MessageBoxResult.Yes)
                {
                    var User = entities.Users.FirstOrDefault(x => x.Id_User == GlobalUser.globalIdUser);
                    string proverkaNaSurnameName;
                    string delete_vidDublyash = delete_vid.vid_TypeOfRepair;
                    proverkaNaSurnameName = User.surname + " " + User.name;
                    if (User.surname == null && User.name == null)
                    {
                        proverkaNaSurnameName = User.login;
                    }
                    ChangesDataBase change = new ChangesDataBase
                    {
                        Changer_surnameAndname = proverkaNaSurnameName,
                        Change = $"Удалена запись(Вид ремонта машины): {delete_vidDublyash}",
                        Change_date = DateTime.Now.ToString(),
                    };
                    entities.ChangesDataBase.Add(change);
                    MessageBox.Show("Запись удалёна!", "Удаление", MessageBoxButton.OK, MessageBoxImage.Question);
                    entities.TypeOfRepair.Remove(delete_vid);
                    entities.SaveChanges();
                    lboxTypeofRepair.Items.Remove(delete_vid);
                    lboxTypeofRepair.SelectedItem = null;
                    lboxTypeofRepair.Items.Refresh();
                }
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            lboxTypeofRepair.SelectedIndex = -1;
            tbTypeofRepair.Clear();
            tbTypeofRepair.Focus();
        }
    }
}

