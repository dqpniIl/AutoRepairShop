using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace AutoRepairShop
{
    /// <summary>
    /// Логика взаимодействия для WindowLogin.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {
        public WindowLogin()
        {
            InitializeComponent();
            Navigation.MainFrame = FrameLogin;
            FrameLogin.Navigate(new PageLog());
        }
        public class HashFunction
        {
            public static string EncryptPassword(string password)
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(password);
                    byte[] hash = sha256.ComputeHash(bytes);
                    string base64Hash = Convert.ToBase64String(hash);
                    return base64Hash;
                }
            }
        }
    }
}
