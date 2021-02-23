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
using System.ComponentModel;
using BusinessLayer;
using System.Net;

namespace WindowsSystemsEngineeringProject
{
    /// <summary>
    /// Interaction logic for shoppingUserControl.xaml
    /// </summary>
    public partial class shoppingUserControl : UserControl
    {
        buyViewModel model;
        approveWindows approveWindows;
        BL bl;

        public shoppingUserControl(buy buy, BL bl, bool isApproveWindows, approveWindows approveWindows = null)
        {
            InitializeComponent();

            model = new buyViewModel(buy, bl.getProduct(buy.productID), bl);

            this.approveWindows = approveWindows;

            UserContrilGrid.DataContext = model;

            this.bl = bl;

            if(isApproveWindows == false)
            {
                btnOK.Visibility = Visibility.Hidden;
                btnDelete.Visibility = Visibility.Hidden;
                this.Width = 365;
                txbAmount.IsEnabled = false;
                txbCost.IsEnabled = false;
            }
        }  


        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            model.browsePhoto();
            model.refreshPhotoData(approveWindows.shoppingUserControls);     
        }


        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            model.approveBuy();

            ((StackPanel)Parent).Children.Remove(this);
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            model.deleteBuy();
            ((StackPanel)Parent).Children.Remove(this);
        }

    }
}