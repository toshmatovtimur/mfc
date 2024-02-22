using mfc_tomsk.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Kernel.Font;
using iText.Kernel.Colors;
using iText.Layout.Properties;
using Microsoft.Win32;
using iText.Layout.Borders;
using System.Threading.Tasks;
using System.Windows.Media;

namespace mfc_tomsk
{
    public partial class AccessCard : Window
    {
        public int counter = 1;
        public AccessCard()
        {
            InitializeComponent();
            StartComboBox();
        }
        private async void Button_Click_6(object sender, RoutedEventArgs e)
        {
            //Сохранить карточку пользователя
            //Проверка на корректность ввода
            
            using (mfcContext db = new mfcContext())
            {
                int temp = 0;
                var count = await db.Users.CountAsync();
                if (counter > count || count == 0)
                {
                    temp++;
                    //Сохранить карту
                    await SaveCard(count);
                }
                if (temp == 0)
                {
                    //Обновление карточки
                    await UpdateCard();
                }
            }
        }
        private async Task SaveCard(int count)
        {
            await Task.Run(() =>
            {
                using(mfcContext db = new mfcContext())
                {
                    Dispatcher.Invoke(async () =>
                    {
                        if (Snils1(Snils.Text)
                        && FIO($"{Family.Text}{Name.Text}{Lastname.Text}")
                        && (!(string.IsNullOrWhiteSpace($"{XAML_Login.Text}")))
                        && (!(string.IsNullOrWhiteSpace($"{XAML_Password.Text}")))
                        && (!(string.IsNullOrWhiteSpace($"{XAML_PassFhraze.Text}")))
                        && DateBool(XAML_Date.Text)
                        && (!(string.IsNullOrWhiteSpace($"{Family.Text}")))
                        && (!(string.IsNullOrWhiteSpace($"{Name.Text}")))
                        && (!(string.IsNullOrWhiteSpace($"{Lastname.Text}"))))
                        {
                            //Если все прошло проверку успешно
                            int NumbAdmin = 0;
                            int NumbPosition = 0;
                            int NumbDepartment = 0;
                            var GetMyAdmins = await db.Admins.FromSqlRaw("SELECT * FROM Admin").ToListAsync();
                            var GetMyPositions = await db.Positions.FromSqlRaw("SELECT * FROM Position").ToListAsync();
                            var GetMyDepartments = await db.Departments.FromSqlRaw("SELECT * FROM Department").ToListAsync();
                            string rezstring = RezAdminName(Admin_ComboBox.Text);
                            foreach (var item in GetMyAdmins)
                            {
                                if (rezstring == item.Number.ToString())
                                {
                                    NumbAdmin = (int)item.Id;
                                }
                            }
                            foreach (var item in GetMyPositions)
                            {
                                if (item.PositionName == Position_ComboBox.Text)
                                {
                                    NumbPosition = (int)item.Id;
                                }
                            }
                            foreach (var item in GetMyDepartments)
                            {
                                if (item.DepartmentName == filial.Text)
                                {
                                    NumbDepartment = (int)item.Id;
                                }
                            }
                            await db.Database.ExecuteSqlInterpolatedAsync($"INSERT INTO User(id, Firstname, Name, Lastname, Login, Password, PasswordFhraze, Snils, DateGets, IdAdmin, IdPosition, IdDepartment) VALUES({count + 1}, {Family.Text}, {Name.Text}, {Lastname.Text}, {XAML_Login.Text}, {XAML_Password.Text}, {XAML_PassFhraze.Text}, {Snils.Text}, {XAML_Date.Text}, {NumbAdmin}, {NumbPosition}, {NumbDepartment})");
                            counter = count + 1;
                            await StartForm();
                        }
                        else
                        {
                            MessageBox.Show("Проверьте правильность заполнения полей");
                            if (!(Snils1(Snils.Text)))
                            {
                                Snils.BorderBrush = Brushes.Red;
                            }
                            if (!FIO(Family.Text))
                            {
                                Family.BorderBrush = Brushes.Red;
                            }
                            if (!FIO(Name.Text))
                            {
                                Name.BorderBrush = Brushes.Red;
                            }
                            if (!FIO(Lastname.Text))
                            {
                                Lastname.BorderBrush = Brushes.Red;
                            }
                            if (!DateBool(XAML_Date.Text) || string.IsNullOrWhiteSpace(XAML_Date.Text))
                            {
                                XAML_Date.BorderBrush = Brushes.Red;
                            }
                            if ((string.IsNullOrWhiteSpace(XAML_Login.Text)))
                            {
                                XAML_Login.BorderBrush = Brushes.Red;
                            }
                            if ((string.IsNullOrWhiteSpace(XAML_Password.Text)))
                            {
                                XAML_Password.BorderBrush = Brushes.Red;
                            }
                            if ((string.IsNullOrWhiteSpace(XAML_PassFhraze.Text)))
                            {
                                XAML_PassFhraze.BorderBrush = Brushes.Red;
                            }
                            if ((string.IsNullOrWhiteSpace(Family.Text)))
                            {
                                Family.BorderBrush = Brushes.Red;
                            }
                            if ((string.IsNullOrWhiteSpace(Name.Text)))
                            {
                                Name.BorderBrush = Brushes.Red;
                            }
                            if ((string.IsNullOrWhiteSpace(Lastname.Text)))
                            {
                                Lastname.BorderBrush = Brushes.Red;
                            }
                        }
                    });
                }
            });
        }
        private async Task UpdateCard()
        {
            await Task.Run(async () =>
            {
                using(mfcContext db = new mfcContext())
                {
                    var user = await db.Users.Where(u => u.Id == counter).FirstOrDefaultAsync();
                    //Провести обновление карточки
                    int NumbAdmin = 0;
                    int NumbPosition = 0;
                    int NumbDepartment = 0;

                    var GetMyAdmins = await db.Admins.FromSqlRaw("SELECT * FROM Admin").ToListAsync();
                    var GetMyPositions = await db.Positions.FromSqlRaw("SELECT * FROM Position").ToListAsync();
                    var GetMyDepartments = await db.Departments.FromSqlRaw("SELECT * FROM Department").ToListAsync();
                    string rezstring = "";
                    Dispatcher.Invoke(() =>
                    {
                        rezstring = RezAdminName(Admin_ComboBox.Text);
                    });
                   
                    foreach (var item in GetMyAdmins)
                    {
                        if (rezstring == item.Number.ToString())
                        {
                            NumbAdmin = (int)item.Id;
                            break;
                        }
                    }
                    string pos = "";
                    Dispatcher.Invoke(() =>
                    {
                        pos = Position_ComboBox.Text;
                    });
                    foreach (var item in GetMyPositions)
                    {
                        if (item.PositionName == pos)
                        {
                            NumbPosition = (int)item.Id;
                            break;
                        }
                    }
                    string dep = "";
                    Dispatcher.Invoke(() =>
                    {
                        dep = filial.Text;
                    });
                    foreach (var item in GetMyDepartments)
                    {
                        if (item.DepartmentName == dep)
                        {
                            NumbDepartment = (int)item.Id;
                            break;
                        }
                    }
                    Dispatcher.Invoke(() =>
                    {
                        var updatecard = db.Database.ExecuteSqlRawAsync("UPDATE User SET Firstname={0}, name={1}, Lastname={2}, Login={3}, Password={4}, PasswordFhraze={5}, Snils={6}, DateGets={7}, IdAdmin={8}, IdPosition={9}, IdDepartment={10} WHERE id={11}", Family.Text, Name.Text, Lastname.Text, XAML_Login.Text, XAML_Password.Text, XAML_PassFhraze.Text, Snils.Text, XAML_Date.Text, NumbAdmin, NumbPosition, NumbDepartment, counter);
                    });
                    MessageBox.Show("Успешно обновлено");
                }
            });
        }
        private async Task StartForm()
        {
            try
            {
                using (mfcContext db = new mfcContext())
                {
            
                    var getcount = await db.Users.CountAsync();
                    if (getcount < counter)
                    {
                          await CardIF();                  
                    }
                    else
                    {
                          await CardRead();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            
        }
        private async Task CardIF()
        {
            await Task.Run(async () =>
            {
                using(mfcContext db = new mfcContext())
                {
                    var GetMyUsers = await db.Users.FromSqlRaw("SELECT * FROM User").ToListAsync();
                    
                    counter--;
                    foreach (var item in GetMyUsers)
                    {
                        if (item.Id == counter)
                        {
                            var GetMyAdmins = await db.Admins.FromSqlRaw("SELECT * FROM Admin").ToListAsync();
                            var GetMyPositions = await db.Positions.FromSqlRaw("SELECT * FROM Position").ToListAsync();
                            var GetMyDepartments = await db.Departments.FromSqlRaw("SELECT * FROM Department").ToListAsync();
                            foreach (var a in GetMyAdmins)
                            {
                                if (item.IdAdmin == a.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        Admin_ComboBox.Text = $"{a.Firstname} {a.Name.Substring(0, 1)}. {a.Lastname.Substring(0, 1)}. {a.Number}";
                                    });
                                    break;
                                }
                            }
                            foreach (var a in GetMyPositions)
                            {
                                if (item.IdPosition == a.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        Position_ComboBox.Text = a.PositionName;
                                    });
                                    break;
                                }
                            }
                            foreach (var a in GetMyDepartments)
                            {
                                if (item.IdDepartment == a.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        filial.Text = a.DepartmentName;
                                    });
                                   
                                    break;
                                }
                            }
                            var cou = await db.Users.CountAsync();
                            Dispatcher.Invoke(() =>
                            {
                               Family.Text = item.Firstname;
                               Name.Text = item.Name;
                               Lastname.Text = item.Lastname;
                               XAML_Login.Text = item.Login;
                               XAML_Password.Text = item.Password;
                               XAML_PassFhraze.Text = item.PasswordFhraze;
                               XAML_Date.Text = item.DateGets;
                               XAML_Numb.Text = $"{counter} из {Convert.ToInt32(cou)}";
                               Snils.Text = item.Snils;
                            });
                            break;
                        }
                    }
                }
            });
        }
        private async Task CardRead()
        {
            await Task.Run(async () =>
            {
                using(mfcContext db = new mfcContext())
                {
                    var GetMyUsers = await db.Users.FromSqlRaw("SELECT * FROM User").ToListAsync();
                    foreach (var item in GetMyUsers)
                    {
                        if (item.Id == counter)
                        {
                            var GetMyAdmins = await db.Admins.FromSqlRaw("SELECT * FROM Admin").ToListAsync();
                            var GetMyPositions = await db.Positions.FromSqlRaw("SELECT * FROM Position").ToListAsync();
                            var GetMyDepartments = await db.Departments.FromSqlRaw("SELECT * FROM Department").ToListAsync();
                            foreach (var a in GetMyAdmins)
                            {
                                if (item.IdAdmin == a.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        Admin_ComboBox.Text = $"{a.Firstname} {a.Name.Substring(0, 1)}. {a.Lastname.Substring(0, 1)}. {a.Number}";
                                    });
                                    break;
                                }
                            }
                            foreach (var a in GetMyPositions)
                            {
                                if (item.IdPosition == a.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        Position_ComboBox.Text = a.PositionName;
                                    });
                                    break;
                                }
                            }
                            foreach (var a in GetMyDepartments)
                            {
                                if (item.IdDepartment == a.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        filial.Text = a.DepartmentName;
                                    });
                                    break;
                                }
                            }
                            var cou = await db.Users.CountAsync();
                            Dispatcher.Invoke(() =>
                            {
                                 Family.Text = item.Firstname;
                                 Name.Text = item.Name;
                                 Lastname.Text = item.Lastname;
                                 XAML_Login.Text = item.Login;
                                 XAML_Password.Text = item.Password;
                                 XAML_PassFhraze.Text = item.PasswordFhraze;
                                 XAML_Date.Text = item.DateGets;
                                 XAML_Numb.Text = $"{counter} из {Convert.ToInt32(cou)}";
                                 Snils.Text = item.Snils;
                            });
                            break;
                        }
                    }
                }

            });
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        { 
            //Удалить карточку
            using (mfcContext db = new mfcContext())
            {
                var usercount = await db.Users.CountAsync();
                if(usercount == 0)
                {
                    MessageBox.Show("Таблица пользователей пуста! Удалять некого!)");
                    deletemyuser.IsEnabled = false;
                }
                else
                {
                    var deletemyuser = await db.Database.ExecuteSqlRawAsync("DELETE FROM User WHERE id = {0}", counter);
                    var r = await db.Database.ExecuteSqlRawAsync("UPDATE User SET id = id - 1 WHERE id > {0}", counter - 1);
                    await StartForm();
                }
            }
        }
        ///////////////////СТРЕЛКИ ПЕРЕКЛЮЧАТЕЛИ МЕЖДУ КАРТОЧКАМИ//////////////////////////
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Левое одиночное нажатие
            using (mfcContext db = new mfcContext())
            {
                UpdateTextBox();
                var count = await db.Users.CountAsync();
                counter--;
                if (counter == 0)
                {
                    counter = Convert.ToInt32(count);
                    await StartForm();
                }
                else
                {
                    await StartForm();
                }
            }
        }
        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UpdateTextBox();
            //Левое переключение до конца
                counter = 1;
                await StartForm();
        }
        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            UpdateTextBox();
            //Правое одиночное нажатие
            using (mfcContext db = new mfcContext())
            {
                var count = await db.Users.CountAsync();
                counter++;
                if (counter == count + 1)
                {
                    counter = 1;
                    await StartForm();
                }
                else
                {
                    await StartForm();
                }
            }
        }
        private async void Button_Click_4(object sender, RoutedEventArgs e)
        {
            UpdateTextBox();
            //Правое переключение до конца
            using (mfcContext db = new mfcContext())
            {
                var count = await db.Users.CountAsync();
                counter = Convert.ToInt32(count);
                await StartForm();
            }
        }
        private async void Button_Click_5(object sender, RoutedEventArgs e)
        {
            //*** Падаем на пустую карточку, новую! Переход
            UpdateTextBox();
            await CleanCard();
        }
        private async Task CleanCard()
        {
            await Task.Run(async () =>
            {
                using (mfcContext db = new())
                {
                    var count = await db.Users.CountAsync();
                    counter = Convert.ToInt32(count) + 1;
                    await BoxClear();
                }
            });
        }
        ///////////////////ПРОВЕРКИ РАЗЛИЧНЫЕ////////////////////////
        private string RezAdminName(string s)
        {
            string ns = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (char.IsDigit(s[i]))
                {
                    ns += s[i];
                }
            }
            return ns;
        }   
        private bool FIO(string d)
        {
            int temp = 0;
            for (int i = 0; i < d.Length; i++)
            {
                if (!char.IsDigit(d[i]))
                {
                    temp++;
                }
            }
            if (temp == d.Length)
                return true;
            return false;
        }    
        private bool DateBool(string empty)
        {
            DateTime dt;
            bool parse = DateTime.TryParse(empty, out dt);
            if (parse == true)
            {
                return true;
            }
            else return false;
        }   
        private bool Snils1(string snils)
        {
            int temp = 0;
            for (int i = 0; i < snils.Length; i++)
            {
                if (char.IsDigit(snils[i]))
                {
                    temp++;
                }
            }
            if (temp == 11 && snils.Length <= 14) return true;
            else return false;
        }
        private async void Test_Clear_Poisk(object sender, KeyEventArgs e)
        {
            await Down1();
        }
        private async void GoGoGo(object sender, KeyEventArgs e)
        {
            //При поднятии клавиши
            await UP();
        }
        private async Task UP()
        {
            await Task.Run(async () =>
            {
                using(mfcContext db = new mfcContext())
                {
                    var search = await db.Users.ToListAsync();
                    string small = "";
                    Dispatcher.Invoke(() =>
                    {
                        small = Search.Text.ToLower();
                    });
                    foreach (var user in search)
                    {
                         if (user.Lastname.ToLower().Contains(small)
                             || user.Name.ToLower().Contains(small)
                             || user.Login.ToLower().Contains(small)
                             || user.Firstname.ToLower().Contains(small))
                         {
                            var GetMyAdmins = await db.Admins.FromSqlRaw("SELECT * FROM Admin").ToListAsync();
                            var GetMyPositions = await db.Positions.FromSqlRaw("SELECT * FROM Position").ToListAsync();
                            var GetMyDepartments = await db.Departments.FromSqlRaw("SELECT * FROM Department").ToListAsync();
                            foreach (var a in GetMyAdmins)
                            {
                                if (user.IdAdmin == a.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        Admin_ComboBox.Text = $"{a.Firstname} {a.Name.Substring(0, 1)}. {a.Lastname.Substring(0, 1)}. {a.Number}";
                                    });
                                    break;
                                }
                            }
                            foreach (var a in GetMyPositions)
                            {
                                if (user.IdPosition == a.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        Position_ComboBox.Text = a.PositionName;
                                    });
                                    break;
                                }
                            }
                            foreach (var a in GetMyDepartments)
                            {
                                if (user.IdDepartment == a.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        filial.Text = a.DepartmentName;
                                    });
                                    break;
                                }
                            }
                            var cou = await db.Users.CountAsync();
                            Dispatcher.Invoke(() =>
                            {
                                Family.Text = user.Firstname;
                                Name.Text = user.Name;
                                Lastname.Text = user.Lastname;
                                XAML_Login.Text = user.Login;
                                XAML_Password.Text = user.Password;
                                XAML_PassFhraze.Text = user.PasswordFhraze;
                                XAML_Date.Text = user.DateGets;
                                XAML_Numb.Text = $"{user.Id} из {cou}";
                                Snils.Text = user.Snils;
                                counter = (int)user.Id;
                            });
                            break;
                         }
                    }
                }
            });
        }
        private async Task Down1()
        {
            //Динамический поиск
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var search = await db.Users.ToListAsync();

                    string small = "";
                    
                    Dispatcher.Invoke(() =>
                    {
                        small = Search.Text.ToLower();
                    });
                    foreach (var user in search)
                    {
                        if (user.Lastname.ToLower().Contains(small)
                            || user.Name.ToLower().Contains(small)
                            || user.Login.ToLower().Contains(small)
                            || user.Firstname.ToLower().Contains(small))
                        {
                            var GetMyAdmins = await db.Admins.FromSqlRaw("SELECT * FROM Admin").ToListAsync();
                            var GetMyPositions = await db.Positions.FromSqlRaw("SELECT * FROM Position").ToListAsync();
                            var GetMyDepartments = await db.Departments.FromSqlRaw("SELECT * FROM Department").ToListAsync();
                            foreach (var a in GetMyAdmins)
                            {
                                if (user.IdAdmin == a.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        Admin_ComboBox.Text = $"{a.Firstname} {a.Name.Substring(0, 1)}. {a.Lastname.Substring(0, 1)}. {a.Number}";
                                    });
                                    break;
                                }
                            }
                            foreach (var a in GetMyPositions)
                            {
                                if (user.IdPosition == a.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        Position_ComboBox.Text = a.PositionName;
                                    });
                                    break;
                                }
                            }
                            foreach (var a in GetMyDepartments)
                            {
                                if (user.IdDepartment == a.Id)
                                {
                                    Dispatcher.Invoke(() =>
                                    {
                                        filial.Text = a.DepartmentName;
                                    });
                                    break;
                                }
                            }
                            var cou = await db.Users.CountAsync();
                            Dispatcher.Invoke(() =>
                            {
                                 Family.Text = user.Firstname;
                                 Name.Text = user.Name;
                                 Lastname.Text = user.Lastname;
                                 XAML_Login.Text = user.Login;
                                 XAML_Password.Text = user.Password;
                                 XAML_PassFhraze.Text = user.PasswordFhraze;
                                 XAML_Date.Text = user.DateGets;
                                 XAML_Numb.Text = $"{user.Id} из {cou}";
                                 Snils.Text = user.Snils;
                                counter = (int)user.Id;
                            });
                            break;
                        }
                    }
                }
            });
        }

        /// <summary>
        /// ///ВСЕ Асинхронно!!!!
        /// </summary>
        ////////////////////ПОЛУЧЕНИЕ СПИСКА ИЗ БД В COMBOBOX/////////////////////////////
        private async void StartComboBox()
        {
            await Combobox_List();
            await Position_List();
            await Admin_List();
            await StartForm();
        }
        private async Task Combobox_List()
        {  //Получение списка филиалов
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var depart = await db.Departments.FromSqlRaw("SELECT * FROM Department").ToListAsync();
                    List<string> list = new List<string>();
                    foreach (var department in depart)
                    {
                        list.Add(department.DepartmentName);
                    }
                    Dispatcher.Invoke(() =>
                    {
                        filial.ItemsSource = list;
                        filial.SelectedIndex = 0;
                    });
                }
            });  
        }
        private async Task Position_List()
        {//Получение списка Должностей
            await Task.Run(async () =>
            {
                using (mfcContext db = new mfcContext())
                {
                    var depart = await db.Positions.FromSqlRaw("SELECT * FROM Position").ToListAsync();
                    List<string> list = new List<string>();
                    foreach (var department in depart)
                    {
                        list.Add(department.PositionName);
                    }
                    Dispatcher.Invoke(() =>
                    {
                        Position_ComboBox.ItemsSource = list;
                        Position_ComboBox.SelectedIndex = 0;
                    });
                }
            });
        }   
        private async Task Admin_List()
        {
            using (mfcContext db = new mfcContext())
            {
                await Task.Run(async () =>
                {
                    var depart = await db.Admins.FromSqlRaw("SELECT * FROM Admin").ToListAsync();
                    List<string> list = new List<string>();
                    foreach (var department in depart)
                    {
                        list.Add($"{department.Firstname} {department.Name.Substring(0, 1)}. {department.Lastname.Substring(0, 1)}. {department.Number}");
                    }
                    Dispatcher.Invoke(() =>
                    {
                        Admin_ComboBox.ItemsSource = list;
                        Admin_ComboBox.SelectedIndex = 0;
                    });
                    
                });
            }
        }
        private void Close_AccessCard_Click(object sender, RoutedEventArgs e)
        {
                Close();
        }
        private async void Button_Click_7(object sender, RoutedEventArgs e)
        {
            //Сохранить в PDF
            await SaveToPdf();
        }
        private async Task SaveToPdf()
        {
            //Сохранение в PDF
            if (string.IsNullOrWhiteSpace(Snils.Text)
               || (string.IsNullOrWhiteSpace($"{XAML_Login.Text}"))
               || (string.IsNullOrWhiteSpace($"{Family.Text}"))
               || (string.IsNullOrWhiteSpace($"{Name.Text}"))
               || (string.IsNullOrWhiteSpace($"{Lastname.Text}"))
               || (string.IsNullOrWhiteSpace($"{XAML_Password.Text}"))
               || (string.IsNullOrWhiteSpace($"{XAML_PassFhraze.Text}"))
               || string.IsNullOrWhiteSpace(XAML_Date.Text)
               || (string.IsNullOrWhiteSpace($"{Family.Text}"))
               || (string.IsNullOrWhiteSpace($"{Name.Text}"))
               || (string.IsNullOrWhiteSpace($"{Lastname.Text}")))
            {
                MessageBox.Show("Не все поля заполнены!");
            }
            else
            {
                await Task.Run(() =>
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.Filter = "PDF document (*.pdf)|*.pdf";

                    if (dialog.ShowDialog() == true)
                    {
                        string fileName = dialog.FileName;
                        PdfDocument pdfDoc = new PdfDocument(new PdfWriter(fileName));
                        Document doc = new Document(pdfDoc, iText.Kernel.Geom.PageSize.A4);
                        PdfFont f2 = PdfFontFactory.CreateFont("arial.ttf", "Identity-H");


                        //Наименование филиала или подразделения
                        Table table = new Table(new float[] { 700, 300 }).SetWidth(UnitValue.CreatePercentValue(100));
                        iText.Layout.Style styleone = new iText.Layout.Style().SetFontColor(ColorConstants.RED).SetFontSize(9).SetBorder(new SolidBorder(ColorConstants.BLACK, 0, 4));
                        Cell cell2 = new Cell().Add(new Paragraph(" Наименование филиала или подразделения")).SetFont(f2).AddStyle(styleone);
                        table.AddCell(cell2).SetFont(f2);
                        cell2 = new Cell().Add(new Paragraph(" Внутренний код")).SetFont(f2).AddStyle(styleone);
                        table.AddCell(cell2).SetFont(f2);
                        Table _table = new Table(new float[] { 500, 500 }).SetWidth(UnitValue.CreatePercentValue(100));
                        Table table1 = new Table(1).SetWidth(UnitValue.CreatePercentValue(100));
                        Table table2 = new Table(new float[] { 200, 800 }).SetWidth(UnitValue.CreatePercentValue(100));
                        Dispatcher.Invoke(() =>
                        {
                            /*=================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>*/
                            iText.Layout.Style _styleone = new iText.Layout.Style().SetFontColor(ColorConstants.BLUE).SetFontSize(11).SetBorder(new SolidBorder(ColorConstants.BLACK, 0, 5));
                            cell2 = new Cell().Add(new Paragraph($" {filial.Text}")).SetFont(f2).AddStyle(_styleone);
                            table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph($" {Snils.Text}")).SetFont(f2).AddStyle(_styleone);
                            table.AddCell(cell2).SetFont(f2);


                            //Фио Должность
                            cell2 = new Cell().Add(new Paragraph(" Фамилия")).SetFont(f2).AddStyle(styleone);
                            _table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph(" Отчество")).SetFont(f2).AddStyle(styleone);
                            _table.AddCell(cell2);
                            /*===================================>>>>>>>>>>>>>>>>>>>>>>>>>*/
                            cell2 = new Cell().Add(new Paragraph($" {Family.Text}")).SetFont(f2).AddStyle(_styleone);
                            _table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph($" {Lastname.Text}")).SetFont(f2).AddStyle(_styleone);
                            _table.AddCell(cell2);


                            //Имя Должность
                            cell2 = new Cell().Add(new Paragraph(" Имя")).SetFont(f2).AddStyle(styleone);
                            _table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph(" Должность")).SetFont(f2).AddStyle(styleone);
                            _table.AddCell(cell2);
                            /*===================================>>>>>>>>>>>>>>>>>>>>>>>>>*/
                            cell2 = new Cell().Add(new Paragraph($" {Name.Text}")).SetFont(f2).AddStyle(_styleone);
                            _table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph($" {Position_ComboBox.Text}")).SetFont(f2).AddStyle(_styleone);
                            _table.AddCell(cell2);


                            //Логин Пароль
                            cell2 = new Cell().Add(new Paragraph(" Логин (имя для доступа)")).SetFont(f2).AddStyle(styleone);
                            _table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph(" Пароль")).SetFont(f2).AddStyle(styleone);
                            _table.AddCell(cell2);
                            /*===================================>>>>>>>>>>>>>>>>>>>>>>>>>*/
                            cell2 = new Cell().Add(new Paragraph($" {XAML_Login.Text}")).SetFont(f2).AddStyle(_styleone);
                            _table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph($" {XAML_Password.Text}")).SetFont(f2).AddStyle(_styleone);
                            _table.AddCell(cell2);

                            //Парольная фраза

                            cell2 = new Cell().Add(new Paragraph(" Парольная фраза")).SetFont(f2).AddStyle(styleone);
                            table1.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph($" {XAML_PassFhraze.Text}")).SetFont(f2).AddStyle(_styleone);
                            table1.AddCell(cell2);


                            //Дата выдачи/Ответственное лицо

                            cell2 = new Cell().Add(new Paragraph(" Дата выдачи")).SetFont(f2).AddStyle(styleone);
                            table2.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph(" Ответственное лицо")).SetFont(f2).AddStyle(styleone);
                            table2.AddCell(cell2);

                            /*===============>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>*/

                            cell2 = new Cell().Add(new Paragraph($" {XAML_Date.Text}")).SetFont(f2).AddStyle(_styleone);
                            table2.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph($" {Admin_ComboBox.Text}")).SetFont(f2).AddStyle(_styleone);
                            table2.AddCell(cell2);

                        });
                        doc.Add(table.SetBorderTop(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 3)));
                        doc.Add(_table.SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)));
                        doc.Add(table1.SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)));
                        doc.Add(table2.SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)));
                        doc.Close();
                    }
                });
            }
           
        }
        private async Task BoxClear()
        {
            await Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    filial.SelectedIndex = 0;
                    Position_ComboBox.SelectedIndex = 0;
                    Admin_ComboBox.SelectedIndex = 0;
                    Family.Text = null;
                    Name.Text = null;
                    Lastname.Clear();
                    XAML_Login.Clear();
                    XAML_Password.Clear();
                    XAML_PassFhraze.Clear();
                    XAML_Date.Text = null;
                    XAML_Numb.Text = counter.ToString();
                    Snils.Clear();
                });
            });
        }
        private void Snilsblue(object sender, KeyEventArgs e)
        {
            Snils.BorderBrush = Brushes.Gray;
        }
        private void Fam(object sender, KeyEventArgs e)
        {
            Family.BorderBrush = Brushes.Gray;
        }
        private void Nam(object sender, KeyEventArgs e)
        {
            Name.BorderBrush = Brushes.Gray;
        }
        private void Las(object sender, KeyEventArgs e)
        {
            Lastname.BorderBrush = Brushes.Gray;
        }
        private void Log(object sender, KeyEventArgs e)
        {
            XAML_Login.BorderBrush = Brushes.Gray;
        }
        private void Pas(object sender, KeyEventArgs e)
        {
            XAML_Password.BorderBrush = Brushes.Gray;
        }
        private void PASF(object sender, KeyEventArgs e)
        {
            XAML_PassFhraze.BorderBrush = Brushes.Gray;
        }
        private void Dat(object sender, KeyEventArgs e)
        {
            XAML_Date.BorderBrush = Brushes.Gray;
        }
        private void UpdateTextBox()
        {
            Snils.BorderBrush = Brushes.Gray;
            Family.BorderBrush = Brushes.Gray;
            Name.BorderBrush = Brushes.Gray;
            Lastname.BorderBrush = Brushes.Gray;
            XAML_Login.BorderBrush = Brushes.Gray;
            XAML_Password.BorderBrush = Brushes.Gray;
            XAML_PassFhraze.BorderBrush = Brushes.Gray;
            XAML_Date.BorderBrush = Brushes.Gray;
        }
        private void OpenCa(object sender, RoutedEventArgs e)
        {
            XAML_Date.BorderBrush = Brushes.Gray;
        }
    }
}