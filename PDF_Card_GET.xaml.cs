using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using mfc_tomsk.Models;

namespace mfc_tomsk
{
    public partial class PDF_Card_GET : Window
    {
        
        public PDF_Card_GET()
        {
            InitializeComponent();
            Start();
        }
       
        private List<string> list = new();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Выход
            Close();
        }
        private new async void KeyDown(object sender, KeyEventArgs e)
        {
            //Событие при нажатии на клавишу Zapros
            await UpAndDown();
        }
        private new async void KeyUp(object sender, KeyEventArgs e)
        {
            //Событие при отпускании клавиши Zapros
            await UpAndDown();
        }
        private async Task UpAndDown()
        {
            await Task.Run(async () =>
            {
                string dynemic = "";
                Dispatcher.Invoke(() =>
                {
                    dynemic = Zapros.Text.ToLower();
                });
                using (mfcContext db = new mfcContext())
                {
                  var getmymind = from use in await db.Users.ToListAsync()
                                  where use.Firstname.ToLower().Contains(dynemic)
                                     || use.Lastname.ToLower().Contains(dynemic)
                                     || use.Name.ToLower().Contains(dynemic)
                                     || use.Login.ToLower().Contains(dynemic)
                                  select new
                                  {
                                      Code = use.Id,
                                      FirstName = $"{use.Firstname} {use.Name.Substring(0, 1)}.{use.Lastname.Substring(0, 1)}.",
                                      Login = use.Login
                                  };
                   Dispatcher.Invoke(() =>
                   {
                       listviewUSERSGETPDF.ItemsSource = getmymind.ToList();
                       
                   });
                }
            });
        }
        private static async Task PDF(string _admin, string _position, string _filial, string _family, string _name, string _lastname, string _login, string _password, string _passfhraze, string _date, string _snils)
        {
            //Создание документа
            await Task.Run(() =>
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "PDF document (*.pdf)|*.pdf";
                try
                {
                    if (dialog.ShowDialog() == true)
                    {
                        string fileName = dialog.FileName;
                        PdfDocument pdfDoc = new(new PdfWriter(fileName));
                        Document doc = new Document(pdfDoc, iText.Kernel.Geom.PageSize.A4);
                        PdfFont f2 = PdfFontFactory.CreateFont("arial.ttf", "Identity-H");


                        //Наименование филиала или подразделения
                        Table table = new Table(new float[] { 700, 300 }).SetWidth(UnitValue.CreatePercentValue(100));
                        iText.Layout.Style styleone = new iText.Layout.Style().SetFontColor(ColorConstants.RED).SetFontSize(9).SetBorder(new SolidBorder(ColorConstants.BLACK, 0, 4));
                        Cell cell2 = new Cell().Add(new Paragraph(" Наименование филиала или подразделения")).SetFont(f2).AddStyle(styleone);
                        table.AddCell(cell2).SetFont(f2);
                        cell2 = new Cell().Add(new Paragraph(" Внутренний код")).SetFont(f2).AddStyle(styleone);
                        table.AddCell(cell2).SetFont(f2);


                        /*=================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>*/
                        iText.Layout.Style _styleone = new iText.Layout.Style().SetFontColor(ColorConstants.BLUE).SetFontSize(11).SetBorder(new SolidBorder(ColorConstants.BLACK, 0, 5));
                        cell2 = new Cell().Add(new Paragraph($" {_filial}")).SetFont(f2).AddStyle(_styleone);
                        table.AddCell(cell2);
                        cell2 = new Cell().Add(new Paragraph($" {_snils}")).SetFont(f2).AddStyle(_styleone);
                        table.AddCell(cell2).SetFont(f2);
                        table.SetBorderTop(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 3));

                        //Фио Должность
                        Table _table = new Table(new float[] { 500, 500 }).SetWidth(UnitValue.CreatePercentValue(100));
                        cell2 = new Cell().Add(new Paragraph(" Фамилия")).SetFont(f2).AddStyle(styleone);
                        _table.AddCell(cell2);
                        cell2 = new Cell().Add(new Paragraph(" Отчество")).SetFont(f2).AddStyle(styleone);
                        _table.AddCell(cell2);
                        /*===================================>>>>>>>>>>>>>>>>>>>>>>>>>*/
                        cell2 = new Cell().Add(new Paragraph($" {_family}")).SetFont(f2).AddStyle(_styleone);
                        _table.AddCell(cell2);
                        cell2 = new Cell().Add(new Paragraph($" {_lastname}")).SetFont(f2).AddStyle(_styleone);
                        _table.AddCell(cell2);


                        //Имя Должность
                        cell2 = new Cell().Add(new Paragraph(" Имя")).SetFont(f2).AddStyle(styleone);
                        _table.AddCell(cell2);
                        cell2 = new Cell().Add(new Paragraph(" Должность")).SetFont(f2).AddStyle(styleone);
                        _table.AddCell(cell2);
                        /*===================================>>>>>>>>>>>>>>>>>>>>>>>>>*/
                        cell2 = new Cell().Add(new Paragraph($" {_name}")).SetFont(f2).AddStyle(_styleone);
                        _table.AddCell(cell2);
                        cell2 = new Cell().Add(new Paragraph($" {_position}")).SetFont(f2).AddStyle(_styleone);
                        _table.AddCell(cell2);


                        //Логин Пароль
                        cell2 = new Cell().Add(new Paragraph(" Логин (имя для доступа)")).SetFont(f2).AddStyle(styleone);
                        _table.AddCell(cell2);
                        cell2 = new Cell().Add(new Paragraph(" Пароль")).SetFont(f2).AddStyle(styleone);
                        _table.AddCell(cell2);
                        /*===================================>>>>>>>>>>>>>>>>>>>>>>>>>*/
                        cell2 = new Cell().Add(new Paragraph($" {_login}")).SetFont(f2).AddStyle(_styleone);
                        _table.AddCell(cell2);
                        cell2 = new Cell().Add(new Paragraph($" {_password}")).SetFont(f2).AddStyle(_styleone);
                        _table.AddCell(cell2);

                        //Парольная фраза
                        Table table1 = new Table(1).SetWidth(UnitValue.CreatePercentValue(100));
                        cell2 = new Cell().Add(new Paragraph(" Парольная фраза")).SetFont(f2).AddStyle(styleone);
                        table1.AddCell(cell2);
                        cell2 = new Cell().Add(new Paragraph($" {_passfhraze}")).SetFont(f2).AddStyle(_styleone);
                        table1.AddCell(cell2);


                        //Дата выдачи/Ответственное лицо


                        Table table2 = new Table(new float[] { 200, 800 }).SetWidth(UnitValue.CreatePercentValue(100));
                        cell2 = new Cell().Add(new Paragraph(" Дата выдачи")).SetFont(f2).AddStyle(styleone);
                        table2.AddCell(cell2);
                        cell2 = new Cell().Add(new Paragraph(" Ответственное лицо")).SetFont(f2).AddStyle(styleone);
                        table2.AddCell(cell2);

                        /*===============>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>*/

                        cell2 = new Cell().Add(new Paragraph($" {_date}")).SetFont(f2).AddStyle(_styleone);
                        table2.AddCell(cell2);
                        cell2 = new Cell().Add(new Paragraph($" {_admin}")).SetFont(f2).AddStyle(_styleone);
                        table2.AddCell(cell2);

                        /*************************************************/

                        doc.Add(table);
                        doc.Add(_table.SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)));
                        doc.Add(table1.SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)));
                        doc.Add(table2.SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)));
                        doc.Close();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            });
        }
        private async void Start()
        {
            using (mfcContext db = new mfcContext())
            {
                var getmymind = from use in await db.Users.ToListAsync()
                                select new
                                {
                                    Code = use.Id,
                                    FirstName = $"{use.Firstname} {use.Name.Substring(0, 1)}.{use.Lastname.Substring(0, 1)}.",
                                    Login = use.Login
                                };
                listviewUSERSGETPDF.ItemsSource = getmymind.ToList();
            }
        }
        private void OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            Clipboard.SetText((sender as System.Windows.Controls.TextBlock).Text);
            MessageBox.Show("Скопировано в буфер обмена");
        }
        private async void Test(object sender, MouseButtonEventArgs e)
        {
            listviewUSERSGETPDF.BorderBrush = System.Windows.Media.Brushes.LightGray;
            string g = RezzStr(listviewUSERSGETPDF.SelectedValue.ToString().Substring(7, 5));//Получаю код выбранного элемента
            using(mfcContext db = new mfcContext())
            {
                var getmymind = from use in await db.Users.ToListAsync()
                                where use.Id == Convert.ToInt32(g)
                                select new
                                {
                                    Code = use.Id,
                                    Login = use.Login,
                                    FirstName = $"{use.Firstname}   {use.Name}   {use.Lastname}"
                                };
              
                foreach (var item in getmymind)
                {
                    list.Add($"{item.Code}    {item.FirstName} ");
                }
            }
            MyList.ItemsSource = null;
            MyList.ItemsSource = list;
            MyList.BorderBrush = System.Windows.Media.Brushes.LightGray;
        }
        private string RezzStr(string d)//Получение цифр из строки
        {
            string g1 = "";
            for (int i = 0; i < d.Length; i++)
            {
                if (char.IsDigit(d[i]))
                {
                    g1 += d[i];
                }
            }
            return g1;
        }
        private void go(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if(MyList.SelectedItem.ToString() == list[i])
                {
                    list.Remove(list[i]);
                    i--;
                    break;
                }
            }
            MyList.ItemsSource = null;
            MyList.ItemsSource = list;
        }
        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Сохранить выбранные элементы в PDF
            if(MyList.Items.Count == 0)
            {
                MessageBox.Show("                    Список выбранных элементов пуст!\n\nВыберите нужную карточку в левой таблице двойным кликом");
                MyList.BorderBrush = System.Windows.Media.Brushes.Red;
                listviewUSERSGETPDF.BorderBrush = System.Windows.Media.Brushes.AliceBlue;
            }
            else
            {
                await SavePDF();
            }
        }
        private async Task SavePDF()
        {
            //Создание документа
            await Task.Run(async () =>
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "PDF document (*.pdf)|*.pdf";

                if (dialog.ShowDialog() == true)
                {
                    string fileName = dialog.FileName;
                    PdfDocument pdfDoc = new PdfDocument(new PdfWriter(fileName));
                    Document doc = new Document(pdfDoc, iText.Kernel.Geom.PageSize.A4);
                    PdfFont f2 = PdfFontFactory.CreateFont("arial.ttf", "Identity-H");

                    //Запрос
                    int temp = 0;
                    foreach (var item in MyList.Items)
                    {
                        temp = ReturnId(item.ToString());
                        using(mfcContext db = new mfcContext())
                        {
                            var getreestrs = from user in await db.Users.ToListAsync()
                                             join admin in await db.Admins.ToListAsync() on user.IdAdmin equals admin.Id
                                             join depart in await db.Departments.ToListAsync() on user.IdDepartment equals depart.Id
                                             join position in await db.Positions.ToListAsync() on user.IdPosition equals position.Id
                                             where user.Id == temp
                                             select new
                                             {
                                                 Department = depart.DepartmentName,
                                                 Snils = user.Snils,
                                                 Family = user.Firstname,
                                                 Name = user.Name,
                                                 Lastname = user.Lastname,
                                                 Position = position.PositionName,
                                                 Login = user.Login,
                                                 Password = user.Password,
                                                 PassFzraze = user.PasswordFhraze,
                                                 Data = user.DateGets,
                                                 Adm = $"{admin.Firstname} {admin.Name.Substring(0, 1)}.{admin.Lastname.Substring(0, 1)}. ({admin.Number})"
                                             };
                            foreach (var i in getreestrs)
                            {
                                //Наименование филиала или подразделения
                                Table table = new Table(new float[] { 700, 300 }).SetWidth(UnitValue.CreatePercentValue(100));
                                iText.Layout.Style styleone = new iText.Layout.Style().SetFontColor(ColorConstants.RED).SetFontSize(9).SetBorder(new SolidBorder(ColorConstants.BLACK, 0, 4));
                                Cell cell2 = new Cell().Add(new Paragraph(" Наименование филиала или подразделения")).SetFont(f2).AddStyle(styleone);
                                table.AddCell(cell2).SetFont(f2);
                                cell2 = new Cell().Add(new Paragraph(" Внутренний код")).SetFont(f2).AddStyle(styleone);
                                table.AddCell(cell2).SetFont(f2);


                                /*=================>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>*/
                                iText.Layout.Style _styleone = new iText.Layout.Style().SetFontColor(ColorConstants.BLUE).SetFontSize(11).SetBorder(new SolidBorder(ColorConstants.BLACK, 0, 5));
                                cell2 = new Cell().Add(new Paragraph($" {i.Department}")).SetFont(f2).AddStyle(_styleone);
                                table.AddCell(cell2);
                                cell2 = new Cell().Add(new Paragraph($" {i.Snils}")).SetFont(f2).AddStyle(_styleone);
                                table.AddCell(cell2).SetFont(f2);


                                //Фио Должность
                                Table _table = new Table(new float[] { 500, 500 }).SetWidth(UnitValue.CreatePercentValue(100));
                                cell2 = new Cell().Add(new Paragraph(" Фамилия")).SetFont(f2).AddStyle(styleone);
                                _table.AddCell(cell2);
                                cell2 = new Cell().Add(new Paragraph(" Отчество")).SetFont(f2).AddStyle(styleone);
                                _table.AddCell(cell2);
                                /*===================================>>>>>>>>>>>>>>>>>>>>>>>>>*/
                                cell2 = new Cell().Add(new Paragraph($" {i.Family}")).SetFont(f2).AddStyle(_styleone);
                                _table.AddCell(cell2);
                                cell2 = new Cell().Add(new Paragraph($" {i.Lastname}")).SetFont(f2).AddStyle(_styleone);
                                _table.AddCell(cell2);


                                //Имя Должность
                                cell2 = new Cell().Add(new Paragraph(" Имя")).SetFont(f2).AddStyle(styleone);
                                _table.AddCell(cell2);
                                cell2 = new Cell().Add(new Paragraph(" Должность")).SetFont(f2).AddStyle(styleone);
                                _table.AddCell(cell2);
                                /*===================================>>>>>>>>>>>>>>>>>>>>>>>>>*/
                                cell2 = new Cell().Add(new Paragraph($" {i.Name}")).SetFont(f2).AddStyle(_styleone);
                                _table.AddCell(cell2);
                                cell2 = new Cell().Add(new Paragraph($" {i.Position}")).SetFont(f2).AddStyle(_styleone);
                                _table.AddCell(cell2);


                                //Логин Пароль
                                cell2 = new Cell().Add(new Paragraph(" Логин (имя для доступа)")).SetFont(f2).AddStyle(styleone);
                                _table.AddCell(cell2);
                                cell2 = new Cell().Add(new Paragraph(" Пароль")).SetFont(f2).AddStyle(styleone);
                                _table.AddCell(cell2);
                                /*===================================>>>>>>>>>>>>>>>>>>>>>>>>>*/
                                cell2 = new Cell().Add(new Paragraph($" {i.Login}")).SetFont(f2).AddStyle(_styleone);
                                _table.AddCell(cell2);
                                cell2 = new Cell().Add(new Paragraph($" {i.Password}")).SetFont(f2).AddStyle(_styleone);
                                _table.AddCell(cell2);

                                //Парольная фраза
                                Table table1 = new iText.Layout.Element.Table(1).SetWidth(UnitValue.CreatePercentValue(100));
                                cell2 = new Cell().Add(new Paragraph(" Парольная фраза")).SetFont(f2).AddStyle(styleone);
                                table1.AddCell(cell2);
                                cell2 = new Cell().Add(new Paragraph($" {i.PassFzraze}")).SetFont(f2).AddStyle(_styleone);
                                table1.AddCell(cell2);


                                //Дата выдачи/Ответственное лицо


                                Table table2 = new Table(new float[] { 200, 800 }).SetWidth(UnitValue.CreatePercentValue(100));
                                cell2 = new Cell().Add(new Paragraph(" Дата выдачи")).SetFont(f2).AddStyle(styleone);
                                table2.AddCell(cell2);
                                cell2 = new Cell().Add(new Paragraph(" Ответственное лицо")).SetFont(f2).AddStyle(styleone);
                                table2.AddCell(cell2);

                                /*===============>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>*/

                                cell2 = new Cell().Add(new Paragraph($" {i.Data}")).SetFont(f2).AddStyle(_styleone);
                                table2.AddCell(cell2);
                                cell2 = new Cell().Add(new Paragraph($" {i.Adm}")).SetFont(f2).AddStyle(_styleone);
                                table2.AddCell(cell2);

                                Paragraph paragraph = new Paragraph("                                                     ");
                                Paragraph paragraph1 = new Paragraph("                                                     ");

                                /*************************************************/
                                doc.Add(table.SetBorderTop(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 3)));
                                doc.Add(_table.SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)));
                                doc.Add(table1.SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)));
                                doc.Add(table2.SetBorderLeft(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderBottom(new SolidBorder(ColorConstants.BLACK, 3)).SetBorderRight(new SolidBorder(ColorConstants.BLACK, 3)));
                                doc.Add(paragraph);
                                doc.Add(paragraph1);
                            }
                        }
                    }
                    doc.Close();
                }
            });
        }
        private int ReturnId(string str)
        {
            string faf = "";
            for (int i = 0; i < str.Length; i++)
            {
                if (char.IsDigit(str[i]))
                {
                    faf += str[i];
                }
            }
            return Convert.ToInt32(faf);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //Закрыть
            Close();
        }
    }
  
} 