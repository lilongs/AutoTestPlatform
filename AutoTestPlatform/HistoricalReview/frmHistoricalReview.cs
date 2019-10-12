using AutoTestDLL.Model;
using AutoTestDLL.Util;
using DevExpress.XtraCharts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoTestPlatform.HistoricalReview
{
    public partial class frmHistoricalReview : Form
    {
        public frmHistoricalReview()
        {
            InitializeComponent();
        }
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog filedialog = new OpenFileDialog();
                filedialog.DefaultExt = ".txt";

                if (filedialog.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = filedialog.FileName;
                    this.txtFile.Text = FilePath;
                    string title=string.Empty;
                    List<HistoryData> list = LoadHistoryInfo(FilePath, out title);
                    InitChart(title,list);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }

        /// <summary>
        /// 从指定路径的文件读取历史数据
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<HistoryData> LoadHistoryInfo(string path,out string title)
        {
            List<HistoryData> list = new List<HistoryData>();
            int i = 0;
            title = "";

            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.UTF8);
            while (!sr.EndOfStream)
            {
                if (i==0)
                {
                    title = sr.ReadLine();//首行为标题
                }
                else
                {
                    string rowInfo = sr.ReadLine();//数据行
                    string[] hisData=rowInfo.Split(';');
                    if (hisData.Length>0)
                    {
                        string[] HistoryData=hisData[0].Split(',');
                        /*历史数据：
                         (1)温湿度 2019-10-12 09:00:00.111,23.45,12.1;
                         (2)电流值 2019-10-12 09:00:00.111,23.2;
                        */
                        if (HistoryData.Length > 0)
                        {
                            for(int j = 1; j <= HistoryData.Length-1; j++)
                            {
                                HistoryData his = new HistoryData();
                                his.id = j;
                                his.time = Convert.ToDateTime(HistoryData[0]);
                                his.value = Convert.ToDouble(HistoryData[j]);
                                list.Add(his);
                            }
                        }

                        //if (HistoryData.Length == 3)
                        //{
                        //    //温度、湿度
                        //    for(int j = 1; j < HistoryData.Length-1; j++)
                        //    {
                        //        HistoryData his = new HistoryData();
                        //        his.id = i;
                        //        his.time = Convert.ToDateTime(HistoryData[0]);
                        //        his.value = Convert.ToDouble(HistoryData[i]);
                        //        list.Add(his);
                        //    }
                        //}
                        //else if(HistoryData.Length == 2)
                        //{
                        //    //电流
                        //    HistoryData his = new HistoryData();
                        //    his.id = 1;
                        //    his.time = Convert.ToDateTime(HistoryData[0]);
                        //    his.value = Convert.ToDouble(HistoryData[1]);
                        //    list.Add(his);
                        //}
                    }
                }
                i++;
            }
           
            return list;
        }

        private void InitChart(string title,List<HistoryData> data)
        {
            chartControl1.Series.Clear();
            chartControl1.Titles.Clear();

            int n = data.Max(x => x.id);
            for(int i = 1; i <= n; i++)
            {
                Series series = new Series("Series" + i, ViewType.Line);

               // series.DataSource = data.Where(x=>x.id==i);
                //series.ArgumentScaleType = ScaleType.Qualitative;
                //series.ArgumentDataMember = "time";
                //series.ArgumentScaleType = ScaleType.DateTime;
                //series.ValueScaleType = ScaleType.Numerical;
                //series.ValueDataMembers.AddRange(new string[] { "value" });
                PointSeriesView myView1 = (PointSeriesView)series.View;
                myView1.PointMarkerOptions.Size = 4;
                foreach (HistoryData historyData in data)
                {
                    if (i == historyData.id)
                        series.Points.Add(new SeriesPoint(historyData.time, historyData.value));
                }
                chartControl1.Series.Add(series);
            }

            XYDiagram diagram = (XYDiagram)chartControl1.Diagram;
            diagram.AxisX.QualitativeScaleOptions.AutoGrid = false;
            diagram.AxisX.DateTimeScaleOptions.ScaleMode = ScaleMode.Manual;//x轴是扫描轴，时间类型
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Second;//测量单位是秒这样才能显示到秒
            diagram.AxisX.DateTimeScaleOptions.GridAlignment = DateTimeGridAlignment.Minute;
            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 1;
            diagram.AxisX.Label.Angle = 30;
            diagram.AxisX.WholeRange.AutoSideMargins = false;
            diagram.AxisX.WholeRange.SideMarginsValue = 0;

            chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

            // Add a title to the chart (if necessary). 
            ChartTitle chartTitle1 = new ChartTitle();
            chartTitle1.Text = title;
            chartControl1.Titles.Add(chartTitle1);
        }

    }
}
