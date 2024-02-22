using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.EntityFrameworkCore;


namespace mfc_tomsk.PAGE_ADMIN
{
    public partial class Password : Page
    {
        public Password()
        {
            InitializeComponent();
            admin_superadmin.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Отменить
            admin_superadmin.Text = null;
            tekushiypassword.Clear();
            newpassword.Clear();
            povtorpassword.Clear();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Изменить пароль
           if(string.IsNullOrWhiteSpace(admin_superadmin.Text)
            || string.IsNullOrWhiteSpace(tekushiypassword.Password)
            || string.IsNullOrWhiteSpace(newpassword.Password)
            || string.IsNullOrWhiteSpace(povtorpassword.Password))
            {
                if (string.IsNullOrWhiteSpace(tekushiypassword.Password))
                {
                    tekushiypassword.BorderBrush = Brushes.Red;
                }
                if (string.IsNullOrWhiteSpace(newpassword.Password))
                {
                    newpassword.BorderBrush = Brushes.Red;
                }
                if (string.IsNullOrWhiteSpace(povtorpassword.Password))
                {
                    povtorpassword.BorderBrush = Brushes.Red;
                }
                MessageBox.Show("Пропущено поле");
            }
            else
            {
                if(admin_superadmin.Text == "Админ")
                {
                    using(mfcContext db = new mfcContext())
                    {
                        var getmypassword = await db.PassworDs.Where(u => u.Id == 1).ToListAsync();
                        foreach (var item in getmypassword)
                        {
                            if(tekushiypassword.Password == item.passText)
                            {
                                if(newpassword.Password == povtorpassword.Password)
                                {
                                    var getfirst = await db.PassworDs.Where(u => u.Id == 1).FirstOrDefaultAsync();
                                    getfirst.passText = newpassword.Password;
                                    await db.SaveChangesAsync();
                                    MessageBox.Show("Пароль изменен");
                                    tekushiypassword.Clear();
                                    newpassword.Clear();
                                    povtorpassword.Clear();
                                }
                                else
                                {
                                    MessageBox.Show("Пароли не совпадают");
                                    newpassword.BorderBrush = Brushes.Red;
                                    povtorpassword.BorderBrush = Brushes.Red;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Неправильный текущий пароль");
                                tekushiypassword.BorderBrush = Brushes.Red;
                            }
                        }
                    }
                }
                else if(admin_superadmin.Text == "СуперАдмин")
                {
                 
                    using (mfcContext db = new mfcContext())
                    {
                        var getmypassword = await db.PassworDs.Where(u => u.Id == 2).ToListAsync();
                        foreach (var item in getmypassword)
                        {
                            if (tekushiypassword.Password == item.passText)
                            {
                                if (newpassword.Password == povtorpassword.Password)
                                {
                                    var getfirst = await db.PassworDs.Where(u => u.Id == 2).FirstOrDefaultAsync();
                                    getfirst.passText = newpassword.Password;
                                    await db.SaveChangesAsync();
                                    MessageBox.Show("Пароль изменен");
                                    tekushiypassword.Clear();
                                    newpassword.Clear();
                                    povtorpassword.Clear();
                                }
                                else
                                {
                                    MessageBox.Show("Пароли не совпадают");
                                    newpassword.BorderBrush = Brushes.Red;
                                    povtorpassword.BorderBrush = Brushes.Red;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Неправильный текущий пароль");
                                tekushiypassword.BorderBrush= Brushes.Red;
                            }
                        }
                    }
                }
            }
        }

        private void Tekushiy(object sender, System.Windows.Input.MouseEventArgs e)
        {
            tekushiypassword.BorderBrush = Brushes.LightGray;
        }

        private void NewPass(object sender, System.Windows.Input.MouseEventArgs e)
        {
            newpassword.BorderBrush = Brushes.LightGray;
        }

        private void Povtor(object sender, System.Windows.Input.MouseEventArgs e)
        {
            povtorpassword.BorderBrush = Brushes.LightGray;
        }
    }
}
