using System.Windows;

namespace mfc_tomsk
{
    public partial class SuperAdmin : Window
    {
        public SuperAdmin()
        {
            InitializeComponent();
            MainFrame.Content = new PAGE_ADMIN.Pingvin();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Резервное копирование и восстановление Базы данных
            MainFrame.Content = new PAGE_ADMIN.Copy_DB();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Редактирование, добавление, удаление ответственных лиц
            MainFrame.Content = new PAGE_ADMIN.Adminpage();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Редактирование, добавление, удаление должностей
            MainFrame.Content = new PAGE_ADMIN.Position();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //Редактирование, добавление, удаление филиалов и подразделений
            MainFrame.Content = new PAGE_ADMIN.Department();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //Смена паролей для входа в БД
            MainFrame.Content = new PAGE_ADMIN.Password();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = new PAGE_ADMIN.Pingvin();
        }
    }
}
