using BusinessLayer;
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
using System.Windows.Shapes;

namespace WindowsSystemsEngineeringProject
{
    /// <summary>
    /// Interaction logic for AllApprovedWindows.xaml
    /// </summary>
    public partial class AllApprovedWindows : Window
    {
        private BL bl;
        public List<shoppingUserControl> shoppingUserControls;

        public AllApprovedWindows(BL bl)
        {
            InitializeComponent();

            this.bl = bl;

            shoppingUserControls = new List<shoppingUserControl>();

            refresh();
        }


        public void refresh()
        {
            //var buys = bl.getBuys();
            //if (buys != null)
            //{
            //    foreach (var buy in buys)
            //    {
            //        if (buy.isApproved == true)
            //        {
            //            shoppingUserControl shoppingUserControl = new shoppingUserControl(buy, bl,false);
            //            stk.Children.Add(shoppingUserControl);
            //            shoppingUserControls.Add(shoppingUserControl);
            //        }
            //    }
            //}
        }
    }
}
