using System;
using System.Collections.Generic;
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

        public List<shoppingUserControl> shoppingUserControls;

        public MainWindow()
        {
            bl = new BL();

            InitializeComponent();

            shoppingUserControls = new List<shoppingUserControl>();

            model = new mainWindowsViewModel(bl, storePieChart);

            DataContext = model;
        }


        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            approveWindows approveWindows = new approveWindows(bl);
            approveWindows.ShowDialog();
        }

        private void btnAllApprovedBuys_Click(object sender, RoutedEventArgs e)
        {
            AllApprovedWindows allApprovedWindows = new AllApprovedWindows(bl);
            allApprovedWindows.ShowDialog();
        }

        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            BuyWindow buyWindow = new BuyWindow(bl);
            buyWindow.Show();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            model.refreshAll();
        }

        private void TabItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            MessageBox.Show("היי");
        }
    }
}