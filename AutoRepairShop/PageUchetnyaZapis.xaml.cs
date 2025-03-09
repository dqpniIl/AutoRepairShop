using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AutoRepairShop
{
    /// <summary>
    /// Логика взаимодействия для PageUchetnyaZapis.xaml
    /// </summary>
    public partial class PageUchetnyaZapis : Page
    {
        Entities entities = new Entities();
        public PageUchetnyaZapis()
        {
            InitializeComponent();
            tbSurname.PreviewTextInput += tb_Text;
            tbName.PreviewTextInput += tb_Text;
            var User = entities.Users.FirstOrDefault(x => x.Id_User == GlobalUser.globalIdUser);
            if (User.surname != null)
                tbSurname.Text = User.surname;
            if (User.name != null)
                tbName.Text = User.name;
        }
        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var User = entities.Users.FirstOrDefault(x => x.Id_User == GlobalUser.globalIdUser);
            if (tbSurname.Text == "" && tbName.Text == "")
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                User.surname = tbSurname.Text;
                User.name = tbName.Text;
            }
            entities.SaveChanges();
            MessageBox.Show("Сохранено успешно!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void tb_Text(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^а-яА-Я]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
