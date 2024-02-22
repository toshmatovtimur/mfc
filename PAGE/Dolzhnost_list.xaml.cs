using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace mfc_tomsk.PAGE
{
    public partial class Dolzhnost_list : Page
    {
        //Получение списка Должностей МФЦ асинхронным методом
        public Dolzhnost_list()
        {
            InitializeComponent();
            StartPosition();
        }
        private async void StartPosition()
        {
            await PositioN();
        }
        private async Task PositioN()
        {
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var dolzhnost = await db.Positions.ToListAsync();
                    Dispatcher.Invoke(() =>
                    {
                        listviewUsers.ItemsSource = dolzhnost.ToList();
                    });
                }
            });
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText((sender as TextBlock).Text);
            Progress progress = new();
            progress.Show();
        }
    }
}
