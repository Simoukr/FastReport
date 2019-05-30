using FastReport;
using FastReport.Export.Pdf;
using ResultPdf.DateSetFill;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace ResultPdf
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataOffice FDataSet;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            List<Experience> list = new List<Experience>()
            {
                new Experience(){ Start="1994-03-01", End="1997-06-05", Location="北京"},
                new Experience(){ Start="1997-07-01", End="1999-06-21", Location="郑州"},
                new Experience(){ Start="2000-01-01", End="2001-03-10", Location="上海"},
                new Experience(){ Start="2002-01-01", End="2003-01-01", Location="洛阳"},
                new Experience(){ Start="2005-01-01", End="2007-01-01", Location="天津"},
                new Experience(){ Start="2008-01-01", End="2019-01-01", Location="大连"},
            };
            grid_saffer.ItemsSource = list;
            CreateDataSet();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CreateDataSet()
        {
            //创建一个数据集
            FDataSet = new DataOffice();

            DataTable table = new DataTable();
            table.TableName = "Experience";//表名 对应数据源名称

            FDataSet.Tables.Add(table);

            table.Columns.Add("start", typeof(string));//列名数据源集合的抬头
            table.Columns.Add("end", typeof(string));//列名数据源集合的抬头
            table.Columns.Add("Location", typeof(string));//列名数据源集合的抬头
            foreach (var item in grid_saffer.ItemsSource)//找到对应的数据集合
            {
                Experience experience = item as Experience;
                table.Rows.Add(experience.Start, experience.End, experience.Location);
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            Report report = new Report();
            try
            {
                string filename = AppDomain.CurrentDomain.BaseDirectory+ "ReportFile\\Office.frx";//去跟目录找到对应的报表文件

                report.Load(filename);//打开文件
                report.RegisterData(FDataSet);//将数据绑定在报表之中
                report.Prepare();
                report.GetDataSource(FDataSet.Tables[0].TableName).Enabled = true;//找对应的文件名称
                //  report.Show();
                report.ShowPrepared();
                report.Dispose();

            }
            catch (Exception ex)
            {
                report.Dispose();//如果出错也要清理下，不然下一次无法打开
            }
        }
    }
}
