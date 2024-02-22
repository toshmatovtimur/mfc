using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using mfc_tomsk.Models;
using System.Windows.Media;
using System.Windows.Input;
using System.Threading.Tasks;

namespace mfc_tomsk.PAGE_ADMIN
{
    public partial class Adminpage : Page
    {

        public Adminpage()
        {
            InitializeComponent();
            AdminsInCombobox();
        }

        private async void AdminsInCombobox()
        {
            await StartAdmin();
        }

        private async Task StartAdmin()
        {
            List<string> list = new List<string>();
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var getmylikeadmins = await db.Admins.FromSqlRaw("SELECT * FROM Admin").ToListAsync();
                    foreach (var item in getmylikeadmins)
                    {
                        list.Add($"{item.Firstname} {item.Name} {item.Lastname}");
                    }
                }
            });
            adminspage.ItemsSource = list.ToList();
            adminspage.SelectedIndex = 0;
            await Start();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Добавить администратора
            Firstname.Text = null;
            Name.Text = null;
            Lastname.Text = null ;
            Number.Text = null;
            adminspage.Text = null;
            Addd.IsEnabled = false;
            Firstname.Focus();
        }

        private async void GoGoGo_Click(object sender, RoutedEventArgs e)
        {
            //Принимаю изменения, обновление, удаление, добавление
            if(string.IsNullOrWhiteSpace(Firstname.Text) 
              || !CheckStringNumber(Firstname.Text)
              || string.IsNullOrWhiteSpace(Name.Text)
              || !CheckStringNumber(Name.Text)
              || string.IsNullOrWhiteSpace(Lastname.Text)
              || !CheckStringNumber(Lastname.Text)
              || string.IsNullOrWhiteSpace(Number.Text)
              || CheckIsNumber(Number.Text))
            {
                if (string.IsNullOrWhiteSpace(Firstname.Text) || !CheckStringNumber(Firstname.Text))
                {
                    Firstname.BorderBrush = Brushes.Red;
                }
                if (string.IsNullOrWhiteSpace(Name.Text) || !CheckStringNumber(Name.Text))
                {
                    Name.BorderBrush = Brushes.Red;
                }
                if (string.IsNullOrWhiteSpace(Lastname.Text) || !CheckStringNumber(Lastname.Text))
                {
                    Lastname.BorderBrush = Brushes.Red;
                }
                if (string.IsNullOrWhiteSpace(Number.Text) || CheckIsNumber(Number.Text))
                {
                    Number.BorderBrush = Brushes.Red;
                }
                MessageBox.Show("Проверьте правильность заполнения полей");
            }
            else
            {
                if (adminspage.Text != null )
                {
                    //Обновление
                    using (mfcContext db = new mfcContext())
                    {
                        var getAdmin = await db.Admins.Where(u => u.Number.ToString() == Number.Text).FirstOrDefaultAsync();
                        getAdmin.Firstname = Firstname.Text;
                        getAdmin.Name = Name.Text;
                        getAdmin.Lastname = Lastname.Text;
                        getAdmin.Number = Convert.ToInt64(Number.Text);
                        await db.SaveChangesAsync();
                        MessageBox.Show("Обновлено");
                        AdminsInCombobox();
                        Addd.IsEnabled = true;
                    }
                }
                else
                {
                    //Добавление нового админа
                    Admin admin1 = new Admin();
                    admin1.Firstname = Firstname.Text;
                    admin1.Name = Name.Text;
                    admin1.Lastname = Lastname.Text;
                    admin1.Number = Convert.ToInt64(Number.Text);
                     using (mfcContext db = new mfcContext())
                     {
                         await db.Admins.AddAsync(admin1);
                         await db.SaveChangesAsync();
                     }
                     MessageBox.Show("Новый Админ добавлен");
                     AdminsInCombobox();
                     await Start();
                    Addd.IsEnabled = true;
                }
            }
        }

        private async void test(object sender, EventArgs e) //Событие заполнения полей при выборе
        {
            await Start();
        }

        private async Task Start()
        {
            string str = adminspage.Text;
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var getmyadmins = await db.Admins.FromSqlRaw("SELECT * FROM Admin").ToListAsync();
                    foreach (var admin in getmyadmins)
                    {
                        if ($"{admin.Firstname} {admin.Name} {admin.Lastname}" == str)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                Firstname.Text = admin.Firstname;
                                Name.Text = admin.Name;
                                Lastname.Text = admin.Lastname;
                                Number.Text = admin.Number.ToString();
                            });
                            break;
                        }
                    }
                }
            });
        }
        private bool CheckIsNumber(string s)
        {
            int temp = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (!char.IsDigit(s[i]))
                {
                    temp++;
                }
            }
            if(temp > 0)
            {
                return true;
            }
            else return false;
        }
        private bool CheckStringNumber(string s)
        {
            int temp = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]))
                {
                    temp++;
                }
            }
            if (temp > 0)
            {
                return false;
            }
            else return true;
        }
        private void MouseNumb(object sender, MouseEventArgs e)
        {
            Number.BorderBrush = Brushes.LightGray;
        }
        private void MouseLas(object sender, MouseEventArgs e)
        {
            Lastname.BorderBrush = Brushes.LightGray;
        }
        private void MouseNam(object sender, MouseEventArgs e)
        {
            Name.BorderBrush = Brushes.LightGray;
        }
        private void MouseFam(object sender, MouseEventArgs e)
        {
            Firstname.BorderBrush = Brushes.LightGray;
        }
    }
}