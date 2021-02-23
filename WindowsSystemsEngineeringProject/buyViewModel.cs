using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using BusinessEntities;
using BusinessLayer;

namespace WindowsSystemsEngineeringProject
{
    class buyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private BL bl;

        public buy buy { get; set; }
        ImageSource _photo;
        public product product { get; set; }
        private float pricePerOne;
        public DateTime date { get; set; }

        public float price
        {
            get { return buy.price; }
            set
            {
                buy.price = value;
                OnPropertyChanged("price");
            }
        }

        public int amount
        {
            get { return buy.amount; }
            set
            {
                buy.amount = value;
                price = pricePerOne * buy.amount;
                OnPropertyChanged("amount");
            }
        }


        public ImageSource photo
        {
            get { return _photo; }
            set
            {
                _photo = value;
                OnPropertyChanged("photo");
            }
        }

        public buyViewModel(buy buy, product product, BL bl)
        {
            this.bl = bl;
            this.buy = buy;
            this.product = product;
            pricePerOne = buy.price / buy.amount;
            amount = buy.amount;

            setImage(product.Photo);
        }

        public void refreshPhotoData(List<shoppingUserControl> shoppingUserControls)
        {
            foreach (var item in shoppingUserControls)
            {
                if (item.UserContrilGrid.DataContext != null)
                {
                    product p = ((buyViewModel)(item.UserContrilGrid.DataContext)).product;
                    string photo = bl.getProduct(p.productID).Photo;
                    ((buyViewModel)(item.UserContrilGrid.DataContext)).setImage(photo);
                }
            }
        }

        internal void deleteBuy()
        {
            bl.deleteBuy(buy);
        }

        public void setImage(byte[] data)
        {
            var source = new BitmapImage();
            source.BeginInit();
            source.StreamSource = new MemoryStream(data);
            source.EndInit();

            photo = source;
        }

        internal void browsePhoto()
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            //dlg.DefaultExt = ".txt";
            dlg.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox
            if (result == true)
            {

                string sourceFile = dlg.FileName;
                string destinationFile = System.IO.Path.Combine(@"..\..\Pictures", product.productName + "." + System.IO.Path.GetFileName(sourceFile).Split('.')[1]);

                System.IO.File.Copy(sourceFile, destinationFile, true);

                setImage(destinationFile);

                product.Photo = destinationFile;
                OnPropertyChanged("photo");

                bl.updateProduct(product);
            }
        }

        public void setImage(string path)
        {
            try
            {
                setImage(File.ReadAllBytes(path));
            }
            catch (Exception e)
            {
                setImage(File.ReadAllBytes(@"../../Pictures/noPhoto.png"));
            }
        }


        public void approveBuy()
        {
            buy.isApproved = true;
            bl.updateBuy(buy);
        }

        public void OnPropertyChanged([CallerMemberName] string name = null)
        {
            var handler = PropertyChanged;

            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
