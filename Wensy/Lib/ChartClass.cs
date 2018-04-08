using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using System.IO;
namespace ServicePoint.Lib
{
    public class ChartClass
    {
        private static string strDefaultPage1;
        private static string strDefaultPage2;
        private static string strDefaultAlert;

        public static string strdefaultalert
        {
            get {
                if (string.IsNullOrEmpty(strDefaultAlert))
                {
                    strDefaultAlert = GetDefaultAlert();
                }
                return strDefaultAlert;
            }

        }
        public static string strdefaultpage1
        {
            get
            {
                if (string.IsNullOrEmpty(strDefaultPage1))
                {
                    strDefaultPage1 = GetDefaultPageType1();
                }
                return strDefaultPage1;
            }

        }
        public static string strdefaultpage2
        {
            get
            {
                if (string.IsNullOrEmpty(strDefaultPage2))
                {
                    strDefaultPage2 = GetDefaultPageType2();
                }
                return strDefaultPage2;
            }

        }
        public static string GetDefaultPageType1()
        {
            string fileName = "DefaultPage_Type1.html";
            string path = HttpContext.Current.Request.MapPath("/COMMON/UC/");
            string text = System.IO.File.ReadAllText(path + fileName);

            return text;
        }
        public static string GetDefaultPageType2()
        {
            string fileName = "DefaultPage_Type2.html";
            string path = HttpContext.Current.Request.MapPath("/COMMON/UC/");
            string text = System.IO.File.ReadAllText(path + fileName);

            return text;
        }
        public static string GetDefaultAlert()
        {
            string path = HttpContext.Current.Request.MapPath("/COMMON/UC/");
            string text = System.IO.File.ReadAllText(path + @"/DefaultPage_Alert.html");
            return text;
        }
        public static string SetCheckColor(int numBitValue)
        {
            string strState = "";
            string returnTag = "";
            if (numBitValue == 0)
                strState = "success";
            else
                strState = "danger";
            string strChecked = "<span class=\"glyphicon glyphicon-record text-#STATE#\" aria-hidden=\"true\"></span>";
            returnTag = strChecked.Replace("#STATE#", strState);
            return returnTag;
        }
        public static string SetProgressBar_Value(double numNowValue, double numMaxValue, string strText)
        {
            return SetProgressBar_Value(numNowValue, numMaxValue).Replace("</div></div>", strText + "</div></div>");
        }
        public static string SetProgressBar_Value(double numNowValue, double numMaxValue)
        {
            string returnTag = "";
            string strState = "";

            double dblPer;
            strState = "success";
            dblPer = Math.Round((Lib.Util.TConverter<double>(numNowValue) / Lib.Util.TConverter<double>(numMaxValue)), 2) * 100;
            string strProcesstag = "<div class=\"progress \"><div class=\"progress-bar progress-bar-#STATE# progress-bar-striped active\" role=\"progressbar\" aria-valuenow=\"#NOWVALUE#\" aria-valuemin=\"0\" aria-valuemax=\"#MAXVALUE#\" style=\"width: #NOWVALUEPER#%;min-width: 2em;\">#NOWVALUE#</div></div>";
            returnTag = strProcesstag.Replace("#NOWVALUE#", numNowValue.ToString()).Replace("#STATE#", strState).Replace("#MAXVALUE#", numMaxValue.ToString()).Replace("#NOWVALUEPER#", dblPer.ToString());
            return returnTag;
        }
        public static string SetProgressBar_Per(double numNowValue, double numMaxValue, string strText)
        {
            return SetProgressBar_Per(numNowValue, numMaxValue).Replace("</div></div>", strText + "</div></div>");
        }
        public static string SetProgressBar_Per(double numNowValue, double numMaxValue)
        {
            string returnTag = "";
            string strState = "";

            double dblPer;
            strState = "success";
            dblPer = Math.Round((Lib.Util.TConverter<double>(numNowValue) / Lib.Util.TConverter<double>(numMaxValue)), 2) * 100;
            string strProcesstag = "<div class=\"progress \"><div class=\"progress-bar progress-bar-#STATE# progress-bar-striped active\" role=\"progressbar\" aria-valuenow=\"#NOWVALUE#\" aria-valuemin=\"0\" aria-valuemax=\"#MAXVALUE#\" style=\"width: #NOWVALUEPER#%;min-width: 2em;\">#NOWVALUEPER#</div></div>";
            returnTag = strProcesstag.Replace("#NOWVALUE#", numNowValue.ToString()).Replace("#STATE#", strState).Replace("#MAXVALUE#", numMaxValue.ToString()).Replace("#NOWVALUEPER#", dblPer.ToString());
            return returnTag;
        }
        public static string SetProcessPQL(int numNowValue, int numProcessMaxValue)
        {
            string returnTag = "";
            string strState = "";
            int numMaxValue = (numNowValue >= numProcessMaxValue ? numNowValue : numProcessMaxValue);

            double dblPer;

            if (numNowValue >= numProcessMaxValue)
            {
                dblPer = 100;
                strState = "danger";
            }
            else
            {
                strState = "success";
                dblPer = Math.Round((Lib.Util.TConverter<double>(numNowValue) / Lib.Util.TConverter<double>(numProcessMaxValue)), 2) * 100;
            }

            string strProcesstag = "<div class=\"progress\"><div class=\"progress-bar progress-bar-#STATE# progress-bar-striped active\" role=\"progressbar\" aria-valuenow=\"#NOWVALUE#\" aria-valuemin=\"0\" aria-valuemax=\"#MAXVALUE#\" style=\"width: #NOWVALUEPER#%;min-width: 2em;\">#NOWVALUE# </div></div>";
            returnTag = strProcesstag.Replace("#NOWVALUE#", numNowValue.ToString()).Replace("#STATE#", strState).Replace("#MAXVALUE#", numMaxValue.ToString()).Replace("#NOWVALUEPER#", dblPer.ToString());
            return returnTag;
        }
        public static Chart SetLineChart(Chart chartControl)
        {
            chartControl.Width = Unit.Pixel(100);
            chartControl.Height = Unit.Pixel(100);
            chartControl.BackColor = Color.WhiteSmoke;
            chartControl.BackGradientStyle = GradientStyle.None;
            chartControl.Series.Add("Series1");
            chartControl.Series[0].ChartType = SeriesChartType.Spline;
            chartControl.Series[0].Color = Color.MidnightBlue;

            chartControl.ChartAreas.Add("ChartArea");
            chartControl.ChartAreas[0].BackGradientStyle = GradientStyle.TopBottom;
            chartControl.ChartAreas[0].BackColor = Color.Gainsboro;
            chartControl.ChartAreas[0].BackSecondaryColor = Color.White;
            chartControl.ChartAreas[0].Position.X = 0;
            chartControl.ChartAreas[0].Position.Y = 0;

            chartControl.ChartAreas[0].Position.Height = 100;
            chartControl.ChartAreas[0].Position.Width = 100;
            // 차트 라인 색상 변경
            chartControl.Series[0].Color = Color.MidnightBlue;
            chartControl.ChartAreas[0].AxisX.LabelStyle.Enabled = false;

            chartControl.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            chartControl.ChartAreas[0].AxisX.LineColor = Color.CornflowerBlue;
            chartControl.ChartAreas[0].AxisY.LineColor = Color.CornflowerBlue;
            chartControl.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
            chartControl.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;
            chartControl.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartControl.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightGray;
            chartControl.ChartAreas[0].AxisX.Interval = 1;
            chartControl.ChartAreas[0].AxisX.IsLabelAutoFit = false;
            chartControl.ChartAreas[0].AxisY.IsLabelAutoFit = false;
            chartControl.ChartAreas[0].ShadowColor = Color.Transparent;
            chartControl.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            chartControl.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightGray;
            chartControl.ChartAreas[0].BorderWidth = 1;
            chartControl.ChartAreas[0].BorderColor = Color.White;
            chartControl.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
            //chartControl.ChartAreas[0].Position.Height = 100;
            chartControl.ChartAreas[0].AxisX.LineColor = Color.CornflowerBlue;
            chartControl.ChartAreas[0].AxisY.LineColor = Color.CornflowerBlue;
            chartControl.ChartAreas[0].AxisX.IsLabelAutoFit = false;

            //chartControl.ChartAreas[0].AxisY.Maximum = 100;
            chartControl.Palette = ChartColorPalette.BrightPastel;

            return chartControl;
        }
        public static Chart SetLineChart(Chart chartControl, DataTable dt, string xvalue, string yvalue, int maxAxisY, int width, int height)
        {
            if (width == 0)
                width = 100;
            if (height == 0)
                height = 100;

            chartControl = SetLineChart(chartControl);
            if (maxAxisY > 0)
                chartControl.ChartAreas[0].AxisY.Maximum = maxAxisY;
            chartControl.Width = Unit.Pixel(width);
            chartControl.Height = Unit.Pixel(height);

            if (dt != null || dt.Rows.Count != 0)
            {
                chartControl.DataSource = dt;
                chartControl.Series[0].XValueMember = xvalue;
                chartControl.Series[0].YValueMembers = yvalue;
            }
            return chartControl;
        }
        public static Chart SetLineChart(Chart chartControl, DataTable dt, string xvalue, string yvalue, string y2value, int maxAxisY, int width, int height)
        {
            chartControl = SetLineChart(chartControl, dt, xvalue, yvalue, maxAxisY, width, height);
            if (dt != null || dt.Rows.Count != 0)
            {
                chartControl.Series.Add("Series2");

                chartControl.Series[1].ChartType = SeriesChartType.Spline;
                chartControl.Series[1].Color = Color.Red;
                chartControl.Series[1].XValueMember = xvalue;
                chartControl.Series[1].YValueMembers = y2value;
                chartControl.ChartAreas[0].Position.Width = 100;
                chartControl.ChartAreas[0].Position.Height = 90;
                chartControl.ChartAreas[0].Position.Y = 5;

            }
            return chartControl;
        }


        public static Chart SetColChart(Chart chartControl)
        {

            chartControl.ChartAreas.Add("ChartArea");
            chartControl.Series.Add("Series1");
            chartControl.Series[0].ShadowOffset = 1;
            chartControl.Series[0].ChartType = SeriesChartType.Column;
            chartControl.Series[0].MarkerSize = 5;
            chartControl.Series[0].MarkerStyle = MarkerStyle.Square;
            chartControl.Series[0]["PixelPointWidth"] = "20"; //막대그래프의 넓이설정

            chartControl.Series[0].IsValueShownAsLabel = true;

            chartControl.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            chartControl.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chartControl.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chartControl.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

            chartControl.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
            chartControl.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;


            chartControl.ChartAreas[0].BackGradientStyle = GradientStyle.TopBottom;
            chartControl.ChartAreas[0].BackColor = Color.Gainsboro;
            chartControl.ChartAreas[0].BackSecondaryColor = Color.White;

            chartControl.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.LightGray;
            chartControl.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.LightGray;

            chartControl.BackColor = Color.WhiteSmoke;
            chartControl.BackGradientStyle = GradientStyle.None;
            chartControl.Palette = ChartColorPalette.BrightPastel;
            return chartControl;
        }
        public static Chart SetColChart(Chart chartControl, DataTable dt, string xvalue, string yvalue, int maxAxisY, int width, int height)
        {
            chartControl = SetColChart(chartControl);
            if (width == 0)
                width = 100;
            if (height == 0)
                height = 100;
            if (maxAxisY == 0)
                maxAxisY = 32;
            chartControl.Width = Unit.Pixel(width + 10);
            chartControl.Height = Unit.Pixel(height + 10);
            chartControl.ChartAreas[0].AxisY.Maximum = maxAxisY;
            chartControl.ChartAreas[0].AxisY.Minimum = 0;

            if (dt != null || dt.Rows.Count != 0)
            {
                chartControl.DataSource = dt;
                chartControl.Series[0].XValueMember = xvalue;
                chartControl.Series[0].YValueMembers = yvalue;
            }
            return chartControl;
        }
        public static bool SaveChart(Chart chartControl, string strServerType, string strSuffix)
        {

            try
            {
                chartControl.ImageType = ChartImageType.Jpeg;
                chartControl.ImageStorageMode = ImageStorageMode.UseImageLocation;
                string filePath = HttpContext.Current.Server.MapPath(@"\") + "ChartImages/" + strServerType + "/";
                DirectoryInfo dir = new DirectoryInfo(filePath);//문자열 변수로 디렉토리를 생성
                if (dir.Exists == false)
                {
                    dir.Create();
                }
                string strFileName = "";
                if (chartControl.Series.Count > 1)
                    strFileName = strSuffix + "_" + chartControl.Series[0].XValueMember.ToString() + "_" + chartControl.Series[0].YValueMembers.ToString() + ".Jpeg";
                else
                    strFileName = strSuffix + "_" + chartControl.Series[0].XValueMember.ToString() + "_" + chartControl.Series[0].YValueMembers.ToString() + "_" + chartControl.Series[1].YValueMembers.ToString() + ".Jpeg";
                chartControl.SaveImage(filePath + strFileName);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static Chart gridDiskBarChart(Chart Chart1, DataTable dtDiskData, int width, int height)
        {
            HttpContext context = HttpContext.Current;

            if (width == 0)
                width = 150;
            if (height == 0)
                height = 130;

            Chart1.Width = Unit.Pixel(width);
            Chart1.Height = Unit.Pixel(height);
            Chart1.BackColor = Color.WhiteSmoke;
            Chart1.BackGradientStyle = GradientStyle.None;
            Chart1.Palette = ChartColorPalette.BrightPastel;

            Chart1.Series.Add("S1");
            Chart1.Series.Add("S2");

            DataTable dtChart = new DataTable();
            dtChart.Columns.Clear();
            dtChart.Columns.Add("Drive", typeof(string));
            dtChart.Columns.Add("usage", typeof(float));
            dtChart.Columns.Add("space", typeof(string));
            dtChart.Columns.Add("remained", typeof(float));

            float flPValue = 0;
            float flFreeSpace = 0;
            string strLabel = "";

            if (dtDiskData.Rows.Count > 0)
            {
                foreach (DataRow dr in dtDiskData.Rows)
                {
                    flPValue = Convert.ToSingle(dr["FreePercent"]);
                    flFreeSpace = Convert.ToSingle(dr["FreeGB"]);
                    strLabel = Math.Round(flPValue).ToString() + "%" + "(" + flFreeSpace.ToString("#,##0.##GB") + ")";
                    dtChart.Rows.Add(dr["Drive"].ToString(), flPValue, strLabel, 100 - flPValue);
                }

                //Color.LightGray, Color.Transparent, Color.YellowGreen, Color.DarkGray, Color.Transparent, Color.MidnightBlue
                //Chart1.Series["S1"].Points.DataBindXY(astr, x1);
                //Chart1.Series["S2"].Points.DataBindXY(astr, x2);

                Chart1.Series["S1"].Points.DataBind(dtChart.AsEnumerable(), "Drive", "usage", "");
                Chart1.Series["S2"].Points.DataBind(dtChart.AsEnumerable(), "Drive", "remained", "Label=space");
            }

            Chart1.Series["S1"].ChartType = SeriesChartType.StackedBar100;
            Chart1.Series["S1"].Color = Color.YellowGreen;
            Chart1.Series["S1"].IsValueShownAsLabel = false;
            Chart1.Series["S1"].SmartLabelStyle.Enabled = true;

            Chart1.Series["S2"].ChartType = SeriesChartType.StackedBar100;
            Chart1.Series["S2"].Color = Color.DarkGray;
            Chart1.Series["S2"].IsValueShownAsLabel = true;
            Chart1.Series["S2"].CustomProperties = "BarLabelStyle=Left";
            Chart1.Series["S2"].LabelForeColor = Color.MidnightBlue;
            //Chart1.Series["S2"].Label = "#VALY{#,##MB}";
            Chart1.Series["S2"].Font = new Font("Verdana", 8, FontStyle.Regular, GraphicsUnit.Point);

            Chart1.ChartAreas.Add("ChartArea1");
            Chart1.ChartAreas[0].Area3DStyle.Enable3D = false;
            //Chart1.ChartAreas[0].AxisY.LabelStyle.Format = "#,###";
            Chart1.ChartAreas[0].AxisY.LabelStyle.Enabled = false;
            //Chart1.ChartAreas[0].AxisX.LabelStyle.Format = "#,###";
            //Chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = false;
            Chart1.ChartAreas[0].AxisY.LabelStyle.Font = new Font("Verdana", 8, FontStyle.Regular, GraphicsUnit.Point);
            Chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            Chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            Chart1.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
            Chart1.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;

            Chart1.ChartAreas[0].AxisX.LineColor = Color.LightGray;
            Chart1.ChartAreas[0].AxisY.LineColor = Color.Transparent;

            Chart1.ChartAreas[0].BackColor = Color.Transparent;
            Chart1.BackColor = Color.LightGray;

            return Chart1;
        }
    }
}
