using System.Linq;
using System.Windows.Controls;

namespace AutoRepairShop
{
    /// <summary>
    /// Логика взаимодействия для PageChanges.xaml
    /// </summary>
    public partial class PageChanges : Page
    {
        public PageChanges()
        {
            InitializeComponent();
            dGridChanges.ItemsSource = Entities.GetContext().ChangesDataBase.ToList();
        }
    }
}
