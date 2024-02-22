using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;


namespace mfc_tomsk.PAGE
{
    public partial class Reestr_Request_Page : Page
    {
        public Reestr_Request_Page()
        {
            InitializeComponent();
            Starting();
        }
        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText((sender as TextBlock).Text);
            Progress progress = new();
            progress.Show();
        }
        private async void Starting()
        {
            await GetMYDep();
            await Start();
        }
        private async Task GetMYDep()
        {
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var getmylikedepartmens = await db.Departments.ToListAsync();
                    List<string> list = new List<string>();
                    foreach (var item in getmylikedepartmens)
                    {
                        list.Add(item.DepartmentName);
                    }
                    Dispatcher.Invoke(() =>
                    {
                        MyDepartments.ItemsSource = list;
                        MyDepartments.SelectedIndex = 0;
                    });
                }
            });
        }
        private async void GoHard(object sender, EventArgs e)
        {
            await Start();
        }
        private async Task Start()
        {
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var getmymind = from use in await db.Users.ToListAsync()
                                    join admin in await db.Admins.ToListAsync() on use.IdAdmin equals admin.Id
                                    join position in await db.Positions.ToListAsync() on use.IdPosition equals position.Id
                                    join department in await db.Departments.ToListAsync() on use.IdDepartment equals department.Id
                                    where department.DepartmentName == MyDepartments.Text
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
                        listview.ItemsSource = getmymind.ToList();
                    });
                }
            });
        }
    }
}
