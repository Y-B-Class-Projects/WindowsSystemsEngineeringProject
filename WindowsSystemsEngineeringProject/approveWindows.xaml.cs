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
    /// Interaction logic for approveWindows.xaml
    /// </summary>
    public partial class approveWindows : Window
    {
        private BL bl;
        public List<shoppingUserControl> shoppingUserControls;

        public approveWindows()
        {
            InitializeComponent();

            bl = new BL();

            shoppingUserControls = new List<shoppingUserControl>();

            refresh();
        }


        public void refresh()
        {
            stk.Children.Clear();

            var buys = bl.getAndUpdateFromGoogleBuys();
            if (buys != null)
            {
                foreach (var buy in buys)
                {
                    if (buy.isApproved == false)
                    {
                        shoppingUserControl shoppingUserControl = new shoppingUserControl(buy, bl, this);
                        stk.Children.Add(shoppingUserControl);
                        shoppingUserControls.Add(shoppingUserControl);
                    }
                }
            }
        }
    }
}
