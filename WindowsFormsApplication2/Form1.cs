using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// button的点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
                CrystalReport1 cr1 = new CrystalReport1();
                cr1.SetDataSource(GetDataTable()); cr1.PrintOptions.PaperSize = PaperSize.PaperA4;
                cr1.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
                DiskFileDestinationOptions objFile = new DiskFileDestinationOptions();
                objFile.DiskFileName = (@"..\\" + "test.pdf");
                cr1.ExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                cr1.ExportOptions.DestinationOptions = objFile;
                cr1.ExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                cr1.Export();
        }
        /// <summary>
        /// 创建一个DataTable
        /// </summary>
        /// <returns></returns>
        private DataTable GetDataTable()
        {
            DataTable tblDatas = new DataTable("Datas");
            tblDatas.Columns.Add("ID", Type.GetType("System.Int32"));
            tblDatas.Columns[0].AutoIncrement = true;
            tblDatas.Columns[0].AutoIncrementSeed = 1;
            tblDatas.Columns[0].AutoIncrementStep = 1;

            tblDatas.Columns.Add("Product", Type.GetType("System.String"));
            tblDatas.Columns.Add("Version", Type.GetType("System.String"));
            tblDatas.Columns.Add("Description", Type.GetType("System.String"));

            tblDatas.Rows.Add(new object[] { null, "a", "b", "c" });
            tblDatas.Rows.Add(new object[] { null, "d", "e", "f" });
            tblDatas.Rows.Add(new object[] { null, "g", "h", "i" });
            tblDatas.Rows.Add(new object[] { null, "j", "k", "l" });
            tblDatas.Rows.Add(new object[] { null, "m", "n", "o" });
            return tblDatas;
        }
    }
}
