using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;

namespace mfc_tomsk.PAGE_ADMIN
{
    public partial class Department : Page
    {
        public Department()
        {
            InitializeComponent();
            Start();
        }

        private async void Start()
        {
            var slovar = new Dictionary<string, string>();
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var getmydepartments = await db.Departments.FromSqlRaw("SELECT * FROM Department").ToListAsync();
                    foreach (var item in getmydepartments)
                    {
                        slovar.Add(item.DepartmentName, item.Address);
                    }
                }
            });
            departmentAdmin.ItemsSource = slovar.Keys.ToList();
            foreach (var item in slovar)
            {
                DepTextbox.Text = item.Key;
                ContactTextbox.Text = item.Value;
                break;
            }
            departmentAdmin.SelectedIndex = 0;
        }

        private async void Update(object sender, EventArgs e)
        {   
            if(departmentAdmin.Text == null)
            {
                DepTextbox.Focus();
            }
            else
            {
                ButtonDep.IsEnabled = true;
                var slovar = new Dictionary<string, string>();
                using (mfcContext db = new mfcContext())
                {
                    var getmydepartments = await db.Departments.FromSqlRaw("SELECT * FROM Department").ToListAsync();
                    foreach (var item in getmydepartments)
                    {
                        slovar.Add(item.DepartmentName, item.Address);
                    }
                }
                departmentAdmin.ItemsSource = slovar.Keys.ToList();
                foreach (var item in slovar)
                {
                    if (item.Key == departmentAdmin.Text)
                    {
                        DepTextbox.Text = item.Key;
                        ContactTextbox.Text = item.Value;
                        break;
                    }
                }
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //Сохранить
            if(string.IsNullOrWhiteSpace(DepTextbox.Text) || string.IsNullOrWhiteSpace(ContactTextbox.Text) || string.IsNullOrWhiteSpace(DepTextbox.Text) && string.IsNullOrWhiteSpace(ContactTextbox.Text))
            {
                if (string.IsNullOrWhiteSpace(DepTextbox.Text))
                {
                    DepTextbox.BorderBrush = Brushes.Red;
                }
                if (string.IsNullOrWhiteSpace(ContactTextbox.Text))
                {
                    ContactTextbox.BorderBrush = Brushes.Red;
                }
                MessageBox.Show("Пропущено поле");
                return;
            }

            else
            {
                 if(departmentAdmin.Text != null)
                 {
                      using (mfcContext db = new mfcContext())
                      {
                          var getmydepartments = await db.Departments.Where(u => u.DepartmentName == departmentAdmin.Text).FirstOrDefaultAsync();
                          getmydepartments.DepartmentName = DepTextbox.Text;
                          getmydepartments.Address = ContactTextbox.Text;
                          await db.SaveChangesAsync();
                          MessageBox.Show("Обновлено");
                          ButtonDep.IsEnabled = true;
                          Start();
                      }
                 }
                 else
                 {
                      using (mfcContext db = new mfcContext())
                      {
                          var gomydepartment = await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO Department(DepartmentName, Address) VALUES({DepTextbox.Text}, {ContactTextbox.Text})");
                          MessageBox.Show("Добавлено");
                        ButtonDep.IsEnabled = true;
                        Start();
                      }
                 }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Добавить филиал
            departmentAdmin.Text = null;
            DepTextbox.Text=null;
            ContactTextbox.Text = null;
            ButtonDep.IsEnabled = false;
            DepTextbox.Focus();
        }

        private void FilialBrush(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DepTextbox.BorderBrush = Brushes.LightGray;
        }

        private void ContactBrush(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ContactTextbox.BorderBrush = Brushes.LightGray;
        }
    }
}