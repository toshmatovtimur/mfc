using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace mfc_tomsk.PAGE
{
    public partial class Request_User : Page
    {
        public Request_User()
        {
            InitializeComponent();
        }

        private async void GoDown(object sender, KeyEventArgs e)
        {
            await DownKlava();
        }

        private async void GoUp(object sender, KeyEventArgs e)
        {
            await Upklava();
        }

        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText((sender as TextBlock).Text);
            Progress progress = new();
            progress.Show();
        }
        private async Task Upklava()
        {
            await Task.Run(async () =>
            {
                //При удалении или поднятии клавиши
                string dynemic = "";
                Dispatcher.Invoke(() =>
                {
                    dynemic = Request.Text.ToLower();
                });
                using (mfcContext db = new mfcContext())
                {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
                    var getmymind = from use in await db.Users.ToListAsync()
                                    join admin in await db.Admins.ToListAsync() on use.IdAdmin equals admin.Id
                                    join position in await db.Positions.ToListAsync() on use.IdPosition equals position.Id
                                    join department in await db.Departments.ToListAsync() on use.IdDepartment equals department.Id
                                    where use.Firstname.ToLower().Contains(dynemic)
                                       || use.Lastname.ToLower().Contains(dynemic)
                                       || use.Name.ToLower().Contains(dynemic)
                                       || use.Login.ToLower().Contains(dynemic)
                                       || use.Snils.Contains(dynemic)
                                    select new
                                    {
                                        Code = use.Id,
                                        Depart = department.DepartmentName,
                                        Snils = use.Snils,
                                        FirstName = use.Firstname,
                                        Name = use.Name,
                                        LastName = use.Lastname,
                                        Position = position.PositionName,
                                        Login = use.Login,
                                        Password = use.Password,
                                        PasswordFhraze = use.PasswordFhraze,
                                        MyDate = use.DateGets,
                                        Admin = admin.Firstname + " " + admin.Name.Substring(0, 1) + "." + admin.Lastname.Substring(0, 1)
                                    };
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
                    Dispatcher.Invoke(() =>
                    {
                        listviewCards.ItemsSource = getmymind.ToList();
                    });
                }
            });
        }
        private async Task DownKlava()
        {
            await Task.Run(async () =>
            {
                //При вводе чего либо
                string dynemic = "";
                Dispatcher.Invoke(() =>
                {
                    dynemic = Request.Text.ToLower();
                });
                using (mfcContext db = new mfcContext())
                {
                    var getmymind = from use in await db.Users.ToListAsync()
                                    join admin in await db.Admins.ToListAsync() on use.IdAdmin equals admin.Id
                                    join position in await db.Positions.ToListAsync() on use.IdPosition equals position.Id
                                    join department in await db.Departments.ToListAsync() on use.IdDepartment equals department.Id
                                    where use.Firstname.ToLower().Contains(dynemic)
                                       || use.Lastname.ToLower().Contains(dynemic)
                                       || use.Name.ToLower().Contains(dynemic)
                                       || use.Login.ToLower().Contains(dynemic)
                                       || use.Snils.Contains(dynemic)
                                    select new
                                    {
                                        Code = use.Id,
                                        Depart = department.DepartmentName,
                                        Snils = use.Snils,
                                        FirstName = use.Firstname,
                                        Name = use.Name,
                                        LastName = use.Lastname,
                                        Position = position.PositionName,
                                        Login = use.Login,
                                        Password = use.Password,
                                        PasswordFhraze = use.PasswordFhraze,
                                        MyDate = use.DateGets,
                                        Admin = admin.Firstname + " " + admin.Name.Substring(0, 1) + "." + admin.Lastname.Substring(0, 1)
                                    };
                    Dispatcher.Invoke(() =>
                    {
                        listviewCards.ItemsSource = getmymind.ToList();
                    }); 
                }
            });
        }
    }
}
