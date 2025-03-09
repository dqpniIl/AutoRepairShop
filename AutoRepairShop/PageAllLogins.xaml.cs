using System.Linq;
using System.Windows.Controls;

namespace AutoRepairShop
{
    /// <summary>
    /// Логика взаимодействия для PageAllLogins.xaml
    /// </summary>
    public partial class PageAllLogins : Page
    {
        public PageAllLogins()
        {
            InitializeComponent();
            dGridlogin.ItemsSource = Entities.GetContext().Users.ToList();
        }
    }
}
