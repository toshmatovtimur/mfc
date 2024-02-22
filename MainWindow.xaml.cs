using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace mfc_tomsk
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            AD();
        }
        private async void AD()
        {
            using (mfcContext db = new mfcContext())
            {
                var res = await db.PassworDs.ToListAsync();
                foreach (var item in res) { }
            }
        }
        private  async void GoEnter(object sender, TouchEventArgs e)
        {
            //При нажатии на Enter
            await WorkToPasswordBox();
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            //При клике на кнопку Войти
            await WorkToPasswordBox();
        }
        private void Gou(object sender, KeyEventArgs e)
        {
            //Событие при нажатии на PassWordBox
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }
        private async Task WorkToPasswordBox()
        {
            int temp = 0;
            using (mfcContext db = new mfcContext())
            {
                var pass = await db.PassworDs.ToListAsync();
                foreach (var item in pass)
                {
                    if (password.Password == item.passText && item.Id == 1)
                    {
                        //Привет! Открывает окно пользователя
                        temp++;
                        Base @base = new();
                        @base.Show();
                        Close();
                    }
                    else if(password.Password == item.passText && item.Id == 2)
                    {
                        temp++;
                        SuperAdmin superAdmin = new();
                        superAdmin.Show();
                        Close();
                    }
                }
                if (temp == 0)
                {
                    MessageBox.Show("Неверный пароль");
                }
            }
        }
    }
}
