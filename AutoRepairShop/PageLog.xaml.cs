using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static AutoRepairShop.WindowLogin;

namespace AutoRepairShop
{
    /// <summary>
    /// Логика взаимодействия для PageLog.xaml
    /// </summary>
    public partial class PageLog : Page
    {
        Entities entities = new Entities();
        public PageLog()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tblogin.Text) || string.IsNullOrEmpty(tbpassword.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool pass = false;
            foreach (var user in entities.Users)
            {
                if (tblogin.Text == user.login || tblogin.Text == user.Id_User.ToString())
                {
                    if (HashFunction.EncryptPassword(tbpassword.Text) == user.password)
                    {
                        if (user.role == "Admin")
                        {
                            GlobalUser.admin = true;
                        }
                        var mainwin = new MainWindow();
                        mainwin.Show();
                        Window currentWindow = Window.GetWindow(this);
                        currentWindow.Close();
                        pass = true;
                    }
                }
            }
            if (!pass)
            {
                MessageBox.Show("Неверный Логин или Пароль. Пожалуйста проверьте правильность введённых данных и повторите попытку.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                var wincaptcha = new WinCaptha();
                wincaptcha.ShowDialog();
            }
            else
            {
                string datauser = tblogin.Text;
                if (int.TryParse(datauser, out int id))
                {
                    GlobalUser.globalIdUser = id;
                    var User = entities.Users.FirstOrDefault(x => x.Id_User == GlobalUser.globalIdUser);
                    User.loginDate = (DateTime.Now).ToString();
                    entities.SaveChanges();
                }
                else
                {
                    var User = entities.Users.FirstOrDefault(u => u.login == datauser);
                    GlobalUser.globalIdUser = User.Id_User;
                    User.loginDate = (DateTime.Now).ToString();
                    entities.SaveChanges();
                }
                GlobalUser.admin = false;
            }
        }
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new PageReg());
        }
    }
}
