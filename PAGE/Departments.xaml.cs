using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace mfc_tomsk.PAGE
{
    public partial class Departments : Page
    {
        public Departments()
        {
            InitializeComponent();
            Departments_List();
        }
        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText((sender as TextBlock).Text);
            Progress progress = new();
            progress.Show();
        }
        private async void Departments_List()
        {
             await StartDepartTable();
        }
        private async Task StartDepartTable()
        {
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var depart = await db.Departments.ToListAsync();
                    Dispatcher.Invoke(() =>
                    {
                        listviewDepartments.ItemsSource = depart.ToList();
                    });
                }
            });
        }
    }
}
