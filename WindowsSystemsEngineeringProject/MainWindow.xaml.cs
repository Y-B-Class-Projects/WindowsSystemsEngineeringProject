using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessEntities;
using BusinessLayer;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using LiveCharts;
using LiveCharts.Wpf;

namespace WindowsSystemsEngineeringProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BL bl;
        mainWindowsViewModel model;
        List<string> productsNames;

        public List<shoppingUserControl> shoppingUserControls;

        public MainWindow()
        {
            bl = new BL();

            InitializeComponent();

            shoppingUserControls = new List<shoppingUserControl>();

            model = new mainWindowsViewModel(bl, storePieChart);

            cmbProductTime.ItemsSource = bl.getProducts();

            cmbStoreTime.ItemsSource = bl.getStoresNames();

            DataContext = model;
        }


        

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            model.refreshAll();

            string ret = "";

            foreach (var item in AI)
            {
                ret += item.Y.First() + "->" + bl.getProduct(item.X.First()) + "\n";
            }
            MessageBox.Show(ret);
        }

        private void TabItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MessageBox.Show("היי");
        }

        private void HistoryStk_Loaded(object sender, RoutedEventArgs e)
        {
            refreshHistoryStk();
        }

        private void refreshHistoryStk()
        {
            shoppingUserControls = new List<shoppingUserControl>();
            var buys = bl.getBuys();
            HistoryStk.Children.Clear();

            if (buys != null)
            {
                foreach (var buy in buys)
                {
                    if (buy.isApproved == true)
                    {
                        shoppingUserControl shoppingUserControl = new shoppingUserControl(buy, bl, false, this);
                        HistoryStk.Children.Add(shoppingUserControl);
                        shoppingUserControls.Add(shoppingUserControl);
                    }
                }
            }
        }

        private void checklistStk_Loaded(object sender, RoutedEventArgs e)
        {
            refreshchecklistStk();
        }

        private void refreshchecklistStk()
        {
            shoppingUserControls = new List<shoppingUserControl>();
            var buys = bl.getAndUpdateFromGoogleBuys();
            checklistStk.Children.Clear();

            if (buys != null)
            {
                foreach (var buy in buys)
                {
                    if (buy.isApproved == false)
                    {
                        shoppingUserControl shoppingUserControl = new shoppingUserControl(buy, bl, true, this);
                        checklistStk.Children.Add(shoppingUserControl);
                        shoppingUserControls.Add(shoppingUserControl);
                    }
                }
            }
        }

        private void cmbProductTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.refreshProductTimeChart((product)cmbProductTime.SelectedItem);
        }

        private void cmxProduct_KeyUp(object sender, KeyEventArgs e)
        {
            string input = ((TextBox)e.OriginalSource).Text;

            if (input != "")
                cmxProduct.ItemsSource = from item in productsNames
                                         where item.ToString().Contains(input)
                                         select item;
            else
                cmxProduct.ItemsSource = null;
        }

        private void cmxStores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            productsNames = bl.getProductsToBuyNames(cmxStores.SelectedItem.ToString());
            cmxProduct.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string imagePath = @"..\..\..\DataLayer\QR_Codes\" + cmxStores.SelectedItem.ToString() + @"\" + cmxProduct.SelectedItem.ToString() + ".png";

                var source = new BitmapImage();
                source.BeginInit();
                source.StreamSource = new MemoryStream(File.ReadAllBytes(imagePath));
                source.EndInit();

                imgQR.Source = source;
            }
            catch (Exception e1)
            {
                MessageBox.Show("תקלה, נסה שוב");
            }
        }

        private void cmbStoreTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            model.refreshStoreTimeChart(cmbStoreTime.SelectedItem.ToString());
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DateTime date = (DateTime)dtp.SelectedDate;
                int day = (int)date.DayOfWeek;

                var ruls = bl.AI();

                string buyList = "";

                foreach (var rule in ruls)
                {
                    if (rule.X.First() == day)
                    {
                        buyList += bl.getProduct(rule.Y.First()).productName += "\n";
                    }
                    else
                    {
                        if (rule.Y.First() == day)
                        {
                            buyList += bl.getProduct(rule.X.First()).productName += "\n";
                        }
                    }
                }

                // Must have write permissions to the path folder
                PdfWriter writer = new PdfWriter(@"..\..\..\buy lists\list for " + date.ToString("dd_MM_yyyy") + ".pdf");
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf);
                iText.Layout.Element.Paragraph header = new iText.Layout.Element.Paragraph("רשימת קניות ליום " + date.ToString("dd/MM/yyyy") + ":")
                   .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER)
                   .SetFontSize(20);

                document.Add(header);

                LineSeparator ls = new LineSeparator(new iText.Kernel.Pdf.Canvas.Draw.SolidLine());
                document.Add(ls);

                iText.Layout.Element.Paragraph paragraph1 = new iText.Layout.Element.Paragraph(buyList);
                document.Add(paragraph1);

                document.Close();
            }
            catch (Exception e1)
            {

                MessageBox.Show("שגיאה, נסה שוב");
            }
            

            
        }
    }
}