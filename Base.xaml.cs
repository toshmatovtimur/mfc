using System.Windows;

namespace mfc_tomsk
{
    /// <summary>
    /// Логика взаимодействия для Base.xaml
    /// </summary>
    public partial class Base : Window
    {

        public Base()
        {
            InitializeComponent();
            MainFrame.Content = new PAGE.CardPage();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Список должностных лиц
            MainFrame.Content = new PAGE.Dolzhnost_list();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Список ответственных лиц
            MainFrame.Content = new PAGE.Admins();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Филиалы МФЦ
            MainFrame.Content = new PAGE.Departments();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //Сформировать карточку доступа
            AccessCard accessCard = new AccessCard();
            accessCard.Show();
         }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //Таблица карточек доступа
            MainFrame.Content = new PAGE.CardPage();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            //Запрос карточки
            MainFrame.Content = new PAGE.Request_User();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            //Запрос карточек определенного филиала
            MainFrame.Content = new PAGE.Reestr_Request_Page();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            PDF_Card_GET pDF = new PDF_Card_GET();
            pDF.ShowDialog();
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            //Сформировать реестр доступа
            GetMyReestrToPdf toPdf = new GetMyReestrToPdf();
            toPdf.ShowDialog();
        }
    }
}
