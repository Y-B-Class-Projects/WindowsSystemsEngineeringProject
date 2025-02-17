﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using BusinessEntities;
using System.Windows;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Data = Google.Apis.Sheets.v4.Data;

namespace DataLayer
{
    public class DAL
    {
        string spreadsheetId;
        SheetsService sheetsService;
        string googleSheetsRange;

        public DAL()
        {
            this.spreadsheetId = "1JyI7zpten-jeh4ZJRZyiRNhRR27LYuYgF0Yqa3zrGOY";
            
            sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = GetCredential(),
                ApplicationName = "Google - SheetsSample / 0.1",
            });
            googleSheetsRange = "A:E";
        }


        private UserCredential GetCredential()
        {
            UserCredential credential1;
            string[] newScopes = new string[] { "https://www.googleapis.com/auth/drive", "https://www.googleapis.com/auth/drive.file", "https://www.googleapis.com/auth/spreadsheets" };
            using (var stream = new FileStream(@"..\..\..\DataLayer\credentials.json", FileMode.Open, FileAccess.Read))
            {
                const string credPath = "token.json";
                credential1 = GoogleWebAuthorizationBroker
                    .AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        newScopes,
                        "user",
                        CancellationToken.None,
                        new FileDataStore(credPath, true))
                    .Result;
            }
            return credential1;
        }

        public System.Collections.IEnumerable getProducts()
        {
            List<product> products = new List<product>();
            using (var ctx = new ProductsContext())
            {
                products = ctx.Products.ToList();
            }
            return products;
        }

        public void addProduct(product p)
        {
            using (var ctx = new ProductsContext())
            {
                ctx.Products.Add(p);
                ctx.SaveChanges();
            }
        }


        public product getProduct(long ID)
        {
            product Product;
            using (var ctx = new ProductsContext())
            {
                Product = ctx.Products.FirstOrDefault(p => p.productID == ID);
            }
            return Product;
        }


        public void updateProduct(product newProduct)
        {
            product Product;
            using (var ctx = new ProductsContext())
            {
                Product = ctx.Products.FirstOrDefault(p => p.productID == newProduct.productID);
                Product.DeepCopy(newProduct);
                ctx.SaveChanges();
            }
        }


        public List<buy> getUnapprovedBuys()
        {
            List<buy> Buys = new List<buy>();
            using (var ctx = new unapprovedBuysContext())
            {
                Buys = ctx.Buys.ToList();
            }
            return Buys;
        }


        public List<buy> getApprovedBuys()
        {
            List<buy> Buys = new List<buy>();
            using (var ctx = new approvedBuysContext())
            {
                Buys = ctx.Buys.ToList();
            }
            return Buys;
        }


        public buy getUnapprovedBuy(long productID, DateTime date, string storeName)
        {
            buy buy;
            using (var ctx = new unapprovedBuysContext())
            {
                buy = ctx.Buys.FirstOrDefault(b => b.productID == productID && b.date == date && b.storeName == storeName);
            }
            return buy;
        }


        public buy getApprovedBuy(long productID, DateTime date, string storeName)
        {
            buy buy;
            using (var ctx = new approvedBuysContext())
            {
                buy = ctx.Buys.FirstOrDefault(b => b.productID == productID && b.date == date && b.storeName == storeName);
            }
            return buy;
        }


        public void deleteUnapprovedBuy(buy buy)
        {
            buy buyToDelete;
            using (var ctx = new unapprovedBuysContext())
            {
                buyToDelete = ctx.Buys.FirstOrDefault(b => b.productID == buy.productID && b.date == buy.date && b.storeName == buy.storeName);
                ctx.Buys.Remove(buyToDelete);
                ctx.SaveChanges();
            }
        }


        public void addUnapprovedBuy(buy newBuy)
        {
            try
            {
                using (var ctx = new unapprovedBuysContext())
                {
                    ctx.Buys.Add(newBuy);
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                buy buy = getUnapprovedBuy(newBuy.productID, newBuy.date, newBuy.storeName);
                float pricePerOne = buy.price / buy.amount;
                buy.amount += 1;
                buy.price += pricePerOne;
                updateUnappruvedBuy(buy);
            }
        }


        public void addApprovedBuy(buy newBuy)
        {
            try
            {
                using (var ctx = new approvedBuysContext())
                {
                    ctx.Buys.Add(newBuy);
                    ctx.SaveChanges();
                }
            }
            catch (Exception e)
            {
                buy buy = getApprovedBuy(newBuy.productID, newBuy.date, newBuy.storeName);
                float pricePerOne = buy.price / buy.amount;
                buy.amount += newBuy.amount;
                buy.price += pricePerOne;
                updateAppruvedBuy(buy);
            }
        }


        public void updateUnappruvedBuy(buy newBuy)
        {
            buy buy;
            using (var ctx = new unapprovedBuysContext())
            {
                buy = ctx.Buys.FirstOrDefault(b => b.productID == newBuy.productID && b.date == newBuy.date && b.storeName == newBuy.storeName);
                buy.DeepCopy(newBuy);
                ctx.SaveChanges();
            }
        }


        public void updateAppruvedBuy(buy newBuy)
        {
            buy buy;
            using (var ctx = new approvedBuysContext())
            {
                buy = ctx.Buys.FirstOrDefault(b => b.productID == newBuy.productID && b.date == newBuy.date && b.storeName == newBuy.storeName);
                buy.DeepCopy(newBuy);
                ctx.SaveChanges();
            }
        }


        public List<string> getStoresNames()
        {
            string[] dirs = Directory.GetDirectories(@"..\..\..\DataLayer\QR_Codes");

            List<string> ret = new List<string>();

            foreach (var item in dirs)
            {
                ret.Add(Path.GetFileName(item));
            }
            return ret;
        }


        public List<string> getProductsToBuyNames(string storeName)
        {
            DirectoryInfo d = new DirectoryInfo(@"..\..\..\DataLayer\QR_Codes\" + storeName);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*.png"); //Getting Text files
            List<string> ret = new List<string>();
            foreach (FileInfo file in Files)
            {
                ret.Add(file.Name.Split('.')[0]);
            }
            
            return ret;
        }


        private IList<IList<Object>> getGoogleData()
        {
            SpreadsheetsResource.ValuesResource.GetRequest request1 =
                    sheetsService.Spreadsheets.Values.Get(spreadsheetId, googleSheetsRange);

            try
            {
                ValueRange response = request1.Execute();

                return response.Values;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        private void cleanGoogleSheets()
        {
            Data.ClearValuesRequest requestBody = new Data.ClearValuesRequest();

            SpreadsheetsResource.ValuesResource.ClearRequest request = sheetsService.Spreadsheets.Values.Clear(requestBody, spreadsheetId, googleSheetsRange);

            try
            {
                request.Execute();
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        public void updateFromGoogle()
        {
            var GoogleData = getGoogleData();
            List<buy> buyList = new List<buy>();
            if(GoogleData != null)
            {
                foreach (var row in GoogleData)
                {
                    if (row.Count != 0)
                    {
                        int productID = (int)(long.Parse(row[2].ToString()) % 1000000000);
                        product Product = getProduct(productID);

                        if (Product == null) // the product dont exist in DB
                        {
                            Product = new product() { Photo = null, productID = productID, productName = row[3].ToString() };

                            string photoURL = "https://m.pricez.co.il/ProductPictures/200x/" + row[2] + ".jpg";
                            string photoPath;

                            try
                            {
                                using (WebClient client = new WebClient())
                                {
                                    photoPath = @"../../Pictures/" + Product.productName + ".jpg";
                                    client.DownloadFile(new Uri(photoURL), photoPath);
                                }
                            }
                            catch (Exception)
                            {
                                photoPath = @"../../Pictures/noPhoto.png";
                            }
                            Product.Photo = photoPath;
                            addProduct(Product);
                        }
                        buy b = new buy() { productID = productID, date = DateTime.Parse(row[0].ToString().Split(new string[] { "at" }, StringSplitOptions.None)[0]), amount = 1, price = float.Parse(row[4].ToString().Split('@')[1]), storeName = row[4].ToString().Split('@')[0], isApproved = false };
                        buyList.Add(b);
                        addUnapprovedBuy(b);
                    }
                }
                cleanGoogleSheets();
            }
        }
    }
}