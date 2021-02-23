using BusinessLayer;
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
using System.Windows.Shapes;

namespace WindowsSystemsEngineeringProject
{
    /// <summary>
    /// Interaction logic for BuyWindow.xaml
    /// </summary>
    public partial class BuyWindow : Window
    {
        BL bl;
        List<string> productsNames;
        public BuyWindow(BL bl)
        {
            InitializeComponent();

            this.bl = bl;

            cmxProduct.IsEnabled = false;

            cmxProduct.ItemsSource = productsNames;

            cmxStores.ItemsSource = bl.getStoresNames();
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
    }
}