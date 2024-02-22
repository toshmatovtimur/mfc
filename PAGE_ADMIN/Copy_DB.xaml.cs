using Microsoft.Win32;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace mfc_tomsk.PAGE_ADMIN
{
    public partial class Copy_DB : Page
    {
        public Copy_DB()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            await CopyDb();

        }

        private async Task CopyDb()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "(*.db)|*.db";
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    await Task.Run(() =>
                    {
                        File.Copy("mfc.db", dialog.FileName);
                    });
                    MessageBox.Show("Резервное копирование прошло успешно!");
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
               
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Инструкция по восстановлению БД из копии

        }
    }
}
