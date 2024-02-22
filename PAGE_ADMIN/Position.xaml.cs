using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace mfc_tomsk.PAGE_ADMIN
{
    public partial class Position : Page
    {
        public Position()
        {
            InitializeComponent();
            PositionInCombobox();
        }

        private async void PositionInCombobox()
        {
            List<string> list = new List<string>();
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var getmylikepositions = await db.Positions.FromSqlRaw("SELECT * FROM Position").ToListAsync();
                    foreach (var item in getmylikepositions)
                    {
                        list.Add($"{item.PositionName}");
                    }
                }
            });
            positionAdmin.ItemsSource = list.ToList();
            positionAdmin.SelectedIndex = 0;
            positionTextbox.Text = positionAdmin.Text;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Добавить новую должность
            ButDol.IsEnabled = false;
            positionAdmin.Text = null;
            positionTextbox.Text = null;
            positionTextbox.Focus();
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            //При нажатии Сохранить
            if (string.IsNullOrWhiteSpace(positionTextbox.Text))
            {
                positionTextbox.BorderBrush = System.Windows.Media.Brushes.Red;
                MessageBox.Show("Пустое поле");
                positionTextbox.Focus();
            }
            else
            {
                using (mfcContext db = new mfcContext())
                {
                    if (positionAdmin.Text != null)
                    {
                        var pos = await db.Database.ExecuteSqlRawAsync("UPDATE Position SET PositionName={0} WHERE PositionName={1}", positionTextbox.Text, positionAdmin.Text);
                        PositionInCombobox();
                        MessageBox.Show("Запись обновлена");
                        positionAdmin.SelectedIndex = 0;
                        positionTextbox.Text = positionAdmin.Text;
                        ButDol.IsEnabled = true;
                    }
                    else
                    {
                        var addpos = await db.Database.ExecuteSqlRawAsync("INSERT INTO Position(PositionName) VALUES({0})", positionTextbox.Text);
                        PositionInCombobox();
                        MessageBox.Show("Запись добавлена");
                        positionAdmin.SelectedIndex = 0;
                        positionTextbox.Text = positionAdmin.Text;
                        ButDol.IsEnabled = true;
                    }
                }
            }
        }

        private void test(object sender, EventArgs e)
        {
            positionTextbox.Text = positionAdmin.Text;
            if (positionAdmin.Text != null)
            {
                ButDol.IsEnabled = true;
            }
            if (positionAdmin.Text == null)
            {
                positionTextbox.Focus();
            }
        }

        private void GoGray(object sender, System.Windows.Input.MouseEventArgs e)
        {
            positionTextbox.BorderBrush = System.Windows.Media.Brushes.LightGray;
        }
    }
}
