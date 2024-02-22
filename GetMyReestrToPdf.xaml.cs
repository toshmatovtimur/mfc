using System;
using System.Collections.Generic;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using System.Linq;
using System.Windows;
using iText.Layout.Borders;
using System.Threading.Tasks;

namespace mfc_tomsk
{
    public partial class GetMyReestrToPdf : Window
    {
        public GetMyReestrToPdf()
        {
            InitializeComponent();
            LoadReestr();
        }
        private void SaveReestrToPdf_Copy_Click(object sender, RoutedEventArgs e)
        {
            //Выйти
            Close();
        }
        private async void SaveReestrToPdf_Click(object sender, RoutedEventArgs e)
        {
            //Сохранение реестра в PDF
            int _depID = 0;
            using(mfcContext db = new mfcContext())
            {
                var plist = await db.Departments.FromSqlRaw("SELECT * FROM Department").ToListAsync();
                foreach (var item in plist)
                {
                    if(item.DepartmentName == DepartPDF.Text)
                    {
                        _depID = (int)item.Id;
                        break;
                    }
                }
                var countreestr = await db.Users.FromSqlRaw("SELECT * FROM User WHERE IdDepartment = {0}", _depID).CountAsync();
                if(countreestr == 0)
                {
                    MessageBox.Show("Список сотрудников выбранного филиала пуст");
                    return;
                }
                else
                {
                    await PDF(_depID);
                }
            }
        }
        private async void LoadReestr()
        {
            using(mfcContext db = new mfcContext())
            {
                var plist = await db.Departments.FromSqlRaw("SELECT * FROM Department").ToListAsync();
                List<string> list = new List<string>();
                foreach (var item in plist)
                {
                    list.Add(item.DepartmentName);
                }
                DepartPDF.ItemsSource = list;
                DepartPDF.SelectedIndex = 0;
            }
        }
        private static async Task PDF(int departID)
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

                    using (mfcContext db = new mfcContext())
                    {
                        var getreestrs = from user in await db.Users.ToListAsync()
                                         join admin in await db.Admins.ToListAsync() on user.IdAdmin equals admin.Id
                                         join depart in await db.Departments.ToListAsync() on user.IdDepartment equals depart.Id
                                         join position in await db.Positions.ToListAsync() on user.IdPosition equals position.Id
                                         where depart.Id == departID
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

                        foreach (var item in getreestrs)
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
                            cell2 = new Cell().Add(new Paragraph($" {item.Department}")).SetFont(f2).AddStyle(_styleone);
                            table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph($" {item.Snils}")).SetFont(f2).AddStyle(_styleone);
                            table.AddCell(cell2).SetFont(f2);


                            //Фио Должность
                            Table _table = new Table(new float[] { 500, 500 }).SetWidth(UnitValue.CreatePercentValue(100));
                            cell2 = new Cell().Add(new Paragraph(" Фамилия")).SetFont(f2).AddStyle(styleone);
                            _table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph(" Отчество")).SetFont(f2).AddStyle(styleone);
                            _table.AddCell(cell2);
                            /*===================================>>>>>>>>>>>>>>>>>>>>>>>>>*/
                            cell2 = new Cell().Add(new Paragraph($" {item.Family}")).SetFont(f2).AddStyle(_styleone);
                            _table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph($" {item.Lastname}")).SetFont(f2).AddStyle(_styleone);
                            _table.AddCell(cell2);


                            //Имя Должность
                            cell2 = new Cell().Add(new Paragraph(" Имя")).SetFont(f2).AddStyle(styleone);
                            _table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph(" Должность")).SetFont(f2).AddStyle(styleone);
                            _table.AddCell(cell2);
                            /*===================================>>>>>>>>>>>>>>>>>>>>>>>>>*/
                            cell2 = new Cell().Add(new Paragraph($" {item.Name}")).SetFont(f2).AddStyle(_styleone);
                            _table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph($" {item.Position}")).SetFont(f2).AddStyle(_styleone);
                            _table.AddCell(cell2);


                            //Логин Пароль
                            cell2 = new Cell().Add(new Paragraph(" Логин (имя для доступа)")).SetFont(f2).AddStyle(styleone);
                            _table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph(" Пароль")).SetFont(f2).AddStyle(styleone);
                            _table.AddCell(cell2);
                            /*===================================>>>>>>>>>>>>>>>>>>>>>>>>>*/
                            cell2 = new Cell().Add(new Paragraph($" {item.Login}")).SetFont(f2).AddStyle(_styleone);
                            _table.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph($" {item.Password}")).SetFont(f2).AddStyle(_styleone);
                            _table.AddCell(cell2);

                            //Парольная фраза
                            Table table1 = new iText.Layout.Element.Table(1).SetWidth(UnitValue.CreatePercentValue(100));
                            cell2 = new Cell().Add(new Paragraph(" Парольная фраза")).SetFont(f2).AddStyle(styleone);
                            table1.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph($" {item.PassFzraze}")).SetFont(f2).AddStyle(_styleone);
                            table1.AddCell(cell2);


                            //Дата выдачи/Ответственное лицо


                            Table table2 = new Table(new float[] { 200, 800 }).SetWidth(UnitValue.CreatePercentValue(100));
                            cell2 = new Cell().Add(new Paragraph(" Дата выдачи")).SetFont(f2).AddStyle(styleone);
                            table2.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph(" Ответственное лицо")).SetFont(f2).AddStyle(styleone);
                            table2.AddCell(cell2);

                            /*===============>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>*/

                            cell2 = new Cell().Add(new Paragraph($" {item.Data}")).SetFont(f2).AddStyle(_styleone);
                            table2.AddCell(cell2);
                            cell2 = new Cell().Add(new Paragraph($" {item.Adm}")).SetFont(f2).AddStyle(_styleone);
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
                        doc.Close();
                    }
                }
            });
        }
    }
}