using BusinessEntities;
using BusinessLayer;
using LiveCharts;
using LiveCharts.Helpers;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsSystemsEngineeringProject
{
    class mainWindowsViewModel
    {
        BL bl;

        public SeriesCollection MonthChart { get; set; }
        public SeriesCollection WeekChart { get; set; }
        public SeriesCollection productTimeChart { get; set; }
        public SeriesCollection storeTimeChart { get; set; }

        public string[] MonthChartLabels { get; set; }
        public string[] WeekChartLabels { get; set; }
        public string[] productTimeChartLabels { get; set; }
        public string[] storeTimeChartLabels { get; set; }

        public Func<double, string> Formatter { get; set; }

        //public Func<ChartPoint, string> PointLabel { get; set; }

        PieChart storePieChart;

        public mainWindowsViewModel(BL bl, PieChart storePieChart)
        {
            this.bl = bl;
            this.storePieChart = storePieChart;

            MonthChart = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "כמות מוצרים",
                    Values = new ChartValues<double> { }
                },
                new ColumnSeries
                {
                    Title = "מחיר",
                    Values = new ChartValues<double> { }
                }
            };

            MonthChartLabels = new[] { "ינואר", "פברואר", "מרץ", "אפריל" , "מאי" , "יוני", "יולי", "אוגוסט", "ספטמבר", "אוקטובר", "נובמבר", "דצמבר"};

            WeekChart = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "כמות מוצרים",
                    Values = new ChartValues<double> { }
                },
                new ColumnSeries
                {
                    Title = "מחיר",
                    Values = new ChartValues<double> { }
                }
            };

            productTimeChart = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "כמות רכישות",
                    Values = new ChartValues<double> { }
                }
            };

            storeTimeChart = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "כמות רכישות",
                    Values = new ChartValues<double> { }
                }
            };

            WeekChartLabels = new[] {"ראשון" , "שני", "שלישי", "רביעי", "חמישי", "שישי", "שבת"};
            productTimeChartLabels = WeekChartLabels;
            storeTimeChartLabels = WeekChartLabels;

            Formatter = value => value.ToString("N");

            refreshAll();
        }

        internal void refreshAll()
        {
            refreshMonthChart();
            refreshWeekChart();
            refreshstorePieChart();
        }

        public string PointLabel(ChartPoint chartPoint)
        {
            var ret = string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);
            return ret;
        }


        public void refreshProductTimeChart(product product)
        {
            productTimeChart[0].Values = bl.getProductWeek(product).AsChartValues();
        }


        public void refreshMonthChart()
        {
            MonthChart[0].Values = bl.getMonthProductsCount().AsChartValues();
            MonthChart[1].Values = bl.getMonthPrice().AsChartValues();
        }


        public void refreshWeekChart()
        {
            WeekChart[0].Values = bl.getWeekProductsCount().AsChartValues();
            WeekChart[1].Values = bl.getWeekPrice().AsChartValues();
        }

        public void refreshstorePieChart()
        {
            storePieChart.Series = new SeriesCollection();

            foreach (var storeCount in bl.GetStoreCounts())
            {
                storePieChart.Series.Add(new PieSeries
                {
                    Title = storeCount.storeName,
                    Values = new ChartValues<double> { storeCount.count },
                    DataLabels = true,
                    LabelPoint = PointLabel
                }) ;
            }
        }

        internal void refreshStoreTimeChart(string storeName)
        {
            storeTimeChart[0].Values = bl.getStoreWeek(storeName).AsChartValues();
        }
    }
}
