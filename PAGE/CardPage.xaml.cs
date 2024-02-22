using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace mfc_tomsk.PAGE
{
    public partial class CardPage : Page
    {
        public CardPage()
        {
            InitializeComponent();
            GetMyUsersNow();
        }
        private async void GetMyUsersNow()
        {
            await StartUserTable();
        }
        private async Task StartUserTable()
        {
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var getmyusers = from user in await db.Users.ToListAsync()
                                     join admin in await db.Admins.ToListAsync() on user.IdAdmin equals admin.Id
                                     join position in await db.Positions.ToListAsync() on user.IdPosition equals position.Id
                                     join department in await db.Departments.ToListAsync() on user.IdDepartment equals department.Id
                                     select new
                                     {
                                         Code = user.Id,
                                         FirstName = user.Firstname,
                                         Name = user.Name,
                                         LastName = user.Lastname,
                                         Login = user.Login,
                                         Password = user.Password,
                                         PasswordFhraze = user.PasswordFhraze,
                                         Snils = user.Snils,
                                         MyDate = user.DateGets,
                                         Admin = $"{admin.Firstname} {admin.Name.Substring(0, 1)}.{admin.Lastname.Substring(0, 1)}.",
                                         Depart = department.DepartmentName,
                                         Position = position.PositionName
                                     };
                    Dispatcher.Invoke(() =>
                    {
                        listviewCards.ItemsSource = getmyusers.ToList();
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
