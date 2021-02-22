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

namespace WindowsSystemsEngineeringProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BL bl;
        public List<shoppingUserControl> shoppingUserControls;
        public MainWindow()
        {
            bl = new BL();

            InitializeComponent();

            shoppingUserControls = new List<shoppingUserControl>();
        }


        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            approveWindows approveWindows = new approveWindows();
            approveWindows.ShowDialog();
        }

        private void btnAllApprovedBuys_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}