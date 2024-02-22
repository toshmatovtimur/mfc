using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace mfc_tomsk.PAGE
{
    public partial class Admins : Page
    {
        public Admins()
        {
            InitializeComponent();
            Admin_List();
        }

        private async void Admin_List()
        {
           await StartAdminTable();
        }
        private async Task StartAdminTable()
        {
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var admins = await db.Admins.ToListAsync();
                    Dispatcher.Invoke(() =>
                    {
                        listviewAdmins.ItemsSource = admins.ToList();
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
