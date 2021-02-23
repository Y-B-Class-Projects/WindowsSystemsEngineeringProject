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
    }
}