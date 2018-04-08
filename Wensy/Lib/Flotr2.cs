using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Data;

namespace ServicePoint.Lib
{
    public class chartProperty
    {
        public string XvalueColumn;
        public string YvalueColumn;
        public string tmpValueColum;//DualLine, Disk 차트에서만 쓰는값
        public List<string> list_Ycolumn;
        public string strAccessibleHeaderText;
        public StringBuilder sb_Value1;
        public StringBuilder sb_Value2;
        public StringBuilder sb_tmp;
        public int numWidth;
        public int numHeight;
        public string strChartType;
        public int maxYvalue;
        public string verticalLines;
        public string horizontalLines;
        public bool bolUseY2;
        public string bolShowXlabel;
        public string bolShowYlabel;
        public string bolShowY2label;
        public string bolShowLegent;
        public string strPObject,strPCounter,strInstance;
        public bool bolSet;
        public chartProperty()
        {
            list_Ycolumn = new List<string>();
            sb_Value1 = new StringBuilder();
            sb_Value2 = new StringBuilder();
            sb_tmp = new StringBuilder();
            maxYvalue = 100;
            numHeight = 100;
            numWidth = 100;
            horizontalLines = "true";
            verticalLines = "false";
            bolUseY2 = false;
            bolShowLegent = "false";
            bolShowXlabel = "false";
            bolShowY2label = "false";
            bolShowYlabel = "false";
            bolSet = false;
        }
        public void SetYColumn()
        {
            var yaaray = System.Text.RegularExpressions.Regex.Split(YvalueColumn, "#c#");
            foreach (string y in yaaray)
            {
                list_Ycolumn.Add(y);
            }
        }

    }
    class Flotr2
    {
        public static void SetCell(int numServerNum, TableCell cell)
        {
            var strHeaderText = ((DataControlFieldCell)cell).ContainingField.ToString();
            string strServerNum = numServerNum.ToString();
            var strAccessibleHeaderText = (((DataControlFieldCell)(cell)).ContainingField).AccessibleHeaderText;
            if (strAccessibleHeaderText.IndexOf("_CHART") > -1)
                cell.Text = "<div  id=\"grp_" + strServerNum + "_" + strAccessibleHeaderText + "\"></div>";
            //    cell.Text = cell.Text.Replace("#CHART#","<div  id=\"grp_" + strServerNum + "_" + strAccessibleHeaderText + "\"></div>");
            //if (strAccessibleHeaderText.IndexOf("COL_CHART") > -1)
            //    cell.Text = "<div id=\"grp_" + strServerNum + "_" + strAccessibleHeaderText + "\"></div>";
            //if (strAccessibleHeaderText.IndexOf("BAR_CHART") > -1)
            //    cell.Text = "<div id=\"grp_" + strServerNum + "_" + strAccessibleHeaderText + "\"></div>";
        }
        public static List<chartProperty> SetArrayString_Lines(DataTable dt, List<chartProperty> cplst, string strChartName)
        {
            if (dt.Rows.Count == 0)
            {
                return cplst;
            }
            foreach (chartProperty cp in cplst)
            {
                if (cp.strAccessibleHeaderText == strChartName)
                {
                    if (cp.strChartType == "LINE")
                    {
                        cp.list_Ycolumn.Clear();
                        cp.SetYColumn();
                        int cnt = 0;
                        foreach (string strColumName in cp.list_Ycolumn)
                        {
                            if (dt.Columns.Contains(strColumName))
                            {
                                cnt++;
                                cp.sb_Value1.Append("{label:\"" + strColumName + "\",data:");

                                cp.sb_Value1.Append("[");
                                foreach (DataRow dr in dt.Rows)
                                {
                                    cp.sb_Value1.Append("[" + Util.getDateTime(Convert.ToDateTime(dr[cp.XvalueColumn])) + "," + dr[strColumName].ToString() + "],");
                                }
                                cp.sb_Value1 = cp.sb_Value1.Remove(cp.sb_Value1.Length - 1, 1);

                                cp.sb_Value1.Append("]},");

                                if (dt.Columns.Count == cnt)
                                    cp.sb_Value1 = cp.sb_Value1.Remove(cp.sb_Value1.Length - 1, 1);
                                cp.bolSet = true;
                            }
                        }
                        cp.sb_Value1 = cp.sb_Value1.Remove(cp.sb_Value1.Length - 1, 1);

                    }
                }
            }
            return cplst;
        }
        public static List<chartProperty> SetArrayString_Lines_Report(DataTable dt, List<chartProperty> cplst)
        {
            if (dt.Rows.Count == 0)
            {
                return cplst;
            }
            foreach (chartProperty cp in cplst)
            {
                //dt 에서 실제로 사용할 데이터만 추린다
                DataTable dt_NowChart = new DataTable();
                dt_NowChart = dt.Clone();

                string Pobject = cp.strPObject;
                string PCounter = cp.strPCounter;
                string PInstance = cp.strInstance;
                string strWhere = "PObjectName = '" + Pobject + "' And PCounterName = '" + PCounter + "'";
                if (PInstance != "")
                {
                    strWhere = strWhere + " AND InstanceName='" + PInstance + "'";
                }
                DataRow[] drs = dt.Select(strWhere);
                foreach (DataRow dr in drs)
                {
                    dt_NowChart.ImportRow(dr);
                }

                if (cp.bolSet == false)
                {
                    if (cp.strChartType == "LINE")
                    {
                        cp.list_Ycolumn.Clear();
                        cp.SetYColumn();
                        int cnt = 0;
                        foreach (string strColumName in cp.list_Ycolumn)
                        {
                            if (dt_NowChart.Columns.Contains(strColumName))
                            {
                                cnt++;
                                cp.sb_Value1.Append("{label:\"" + strColumName + "\",data:");

                                cp.sb_Value1.Append("[");
                                foreach (DataRow dr in dt_NowChart.Rows)
                                {
                                    cp.sb_Value1.Append("[" + Util.getDateTime(Convert.ToDateTime(dr[cp.XvalueColumn])) + "," + dr[strColumName].ToString() + "],");
                                }
                                cp.sb_Value1 = cp.sb_Value1.Remove(cp.sb_Value1.Length - 1, 1);

                                cp.sb_Value1.Append("]},");

                                if (dt_NowChart.Columns.Count == cnt)
                                    cp.sb_Value1 = cp.sb_Value1.Remove(cp.sb_Value1.Length - 1, 1);
                                cp.bolSet = true;
                            }
                        }

                    }
                }
            }
            return cplst;
        }
        public static List<chartProperty> SetArrayString_Lines(DataTable dt, List<chartProperty> cplst)
        {
            if (dt.Rows.Count == 0)
            {
                return cplst;
            }
            foreach (chartProperty cp in cplst)
            {
                //dt 에서 실제로 사용할 데이터만 추린다
                DataTable dt_NowChart = dt;
                

                if (cp.bolSet == false)
                {
                    if (cp.strChartType == "LINE")
                    {
                        cp.list_Ycolumn.Clear();
                        cp.SetYColumn();
                        int cnt = 0;
                        foreach (string strColumName in cp.list_Ycolumn)
                        {
                            if (dt_NowChart.Columns.Contains(strColumName))
                            {
                                cnt++;
                                cp.sb_Value1.Append("{label:\"" + strColumName + "\",data:");

                                cp.sb_Value1.Append("[");
                                foreach (DataRow dr in dt_NowChart.Rows)
                                {
                                    cp.sb_Value1.Append("[" + Util.getDateTime(Convert.ToDateTime(dr[cp.XvalueColumn])) + "," + dr[strColumName].ToString() + "],");
                                }
                                cp.sb_Value1 = cp.sb_Value1.Remove(cp.sb_Value1.Length - 1, 1);

                                cp.sb_Value1.Append("]},");

                                if (dt_NowChart.Columns.Count == cnt)
                                    cp.sb_Value1 = cp.sb_Value1.Remove(cp.sb_Value1.Length - 1, 1);
                                cp.bolSet = true;
                            }
                        }

                    }
                }
            }
            return cplst;
        }
        //SetArrayString_Lines으로 변경  사용안함 151202
        public static List<chartProperty> SetArrayString_Line_DualYAxis(DataTable dt, List<chartProperty> cplst)
        {

            if (dt.Rows.Count == 0)
            {
                return cplst;
            }
            foreach (chartProperty cp in cplst)
            {
                if (dt.Columns.Contains(cp.YvalueColumn) && dt.Columns.Contains(cp.tmpValueColum))
                {
                    if (cp.strChartType == "DUALLINE")
                    {
                        if (cp.tmpValueColum.Length > 0)
                            cp.sb_Value2.Append("{yaxis:2,label:\"" + cp.tmpValueColum + "\",data:[");
                        cp.sb_Value1.Append("{label:\"" + cp.YvalueColumn + "\",data:[");
                    }
                }
            }
            foreach (DataRow dr in dt.Rows)
            {
                foreach (chartProperty cp in cplst)
                {
                    if (dt.Columns.Contains(cp.YvalueColumn) && dt.Columns.Contains(cp.tmpValueColum))
                    {
                        if (cp.strChartType == "DUALLINE")
                        {

                            cp.sb_Value1.Append("[" + Util.getDateTime(Convert.ToDateTime(dr[cp.XvalueColumn])) + "," + dr[cp.YvalueColumn].ToString() + "],");
                            cp.sb_Value2.Append("[" + Util.getDateTime(Convert.ToDateTime(dr[cp.XvalueColumn])) + "," + dr[cp.tmpValueColum].ToString() + "],");

                        }
                    }
                }
            }
            foreach (chartProperty cp in cplst)
            {
                if (cp.strChartType == "DUALLINE")
                {
                    if (dt.Columns.Contains(cp.YvalueColumn) && dt.Columns.Contains(cp.tmpValueColum))
                    {
                        if (cp.tmpValueColum.Length > 0)
                            cp.sb_Value2 = cp.sb_Value2.Remove(cp.sb_Value2.Length - 1, 1);

                        cp.sb_Value1 = cp.sb_Value1.Remove(cp.sb_Value1.Length - 1, 1);
                        if (cp.tmpValueColum.Length > 0)
                            cp.sb_Value2.Append("]};");
                        cp.sb_Value1.Append("]};");
                    }
                }
            }
            return cplst;
        }
        public static List<chartProperty> SetArrayString_Bar(DataTable dt, List<chartProperty> cplst)
        {
            if (dt.Rows.Count == 0)
            {
                return cplst;
            }
            foreach (chartProperty cp in cplst)
            {

                if (cp.strChartType == "BAR")
                {
                    cp.sb_Value1.Append("[");
                    cp.sb_Value2.Append("[");
                }
            }
            foreach (chartProperty cp in cplst)
            {
                if (cp.strChartType == "BAR")
                {
                    int idx = 0;

                    foreach (DataRow dr in dt.Rows)
                    {

                        cp.sb_Value1.Append("[[0," + idx.ToString() + "," + Math.Round(Convert.ToDouble(dr[cp.XvalueColumn]), 0).ToString() + "]],");
                        cp.sb_Value2.Append("'" + dr[cp.YvalueColumn].ToString() + Math.Round(Convert.ToDouble(dr[cp.XvalueColumn]), 0).ToString() + "%(" + Math.Round(Convert.ToDouble(dr[cp.tmpValueColum]), 0).ToString() + "GB)',");
                        idx++;
                    }

                }
            }
            foreach (chartProperty cp in cplst)
            {
                if (cp.strChartType == "BAR")
                {
                    cp.sb_Value2 = cp.sb_Value2.Remove(cp.sb_Value2.Length - 1, 1);
                    cp.sb_Value1 = cp.sb_Value1.Remove(cp.sb_Value1.Length - 1, 1);
                    cp.sb_Value2.Append("];");
                    cp.sb_Value1.Append("];");
                }
            }
            return cplst;
        }
        public static List<chartProperty> SetArrayString_Col(DataTable dt, List<chartProperty> cplst)
        {
            if (dt.Rows.Count == 0)
            {
                return cplst;
            }
            foreach (chartProperty cp in cplst)
            {

                if (cp.strChartType == "COL")
                {
                    cp.sb_Value1.Append("[0," + dt.Rows[0][cp.YvalueColumn].ToString() + "];");
                }
            }
            return cplst;
        }
        public static List<chartProperty> SetArrayString_Cols(DataTable dt, List<chartProperty> cplst)
        {
            if (dt.Rows.Count == 0)
            {
                return cplst;
            }
            foreach (chartProperty cp in cplst)
            {
                if (cp.strChartType == "COLS")
                {
                    cp.sb_Value1.Append("[");
                }
            }
            int i = 1;
            foreach (DataRow dr in dt.Rows)
            {

                foreach (chartProperty cp in cplst)
                {
                    if (cp.strChartType == "COLS")
                    {
                        //cp.sb_Value1.Append("[" + Util.getDateTime(Convert.ToDateTime(dr[cp.XvalueColumn])) + "," + dr[cp.YvalueColumn].ToString() + "],");
                        cp.sb_Value1.Append("[" + i.ToString() + "," + dr[cp.YvalueColumn].ToString() + "],");

                    }

                }
                i++;
            }
            foreach (chartProperty cp in cplst)
            {
                if (cp.strChartType == "COLS")
                {

                    cp.sb_Value1 = cp.sb_Value1.Remove(cp.sb_Value1.Length - 1, 1);
                    cp.sb_Value1.Append("];");
                }
            }
            return cplst;
        }
        public static List<chartProperty> SetArrayString_Cols_Horizon(DataTable dt, List<chartProperty> cplst)
        {
            if (dt.Rows.Count == 0)
            {
                return cplst;
            }
            foreach (chartProperty cp in cplst)
            {
                if (dt.Columns.Contains(cp.XvalueColumn) == false || dt.Columns.Contains(cp.YvalueColumn) == false)
                    continue;
                if (cp.strChartType == "COLSHORIZON")
                {
                    if (cp.sb_Value1.Length > 0)
                        continue;
                    int cnt = 0;

                    cp.sb_Value1.Append("[");
                    foreach (DataRow dr in dt.Rows)
                    {
                        cnt++;

                        cp.sb_Value1.Append("{label:\"" + dr[cp.XvalueColumn].ToString() + "\",data:[[" + dr[cp.YvalueColumn].ToString() + "," + cnt.ToString() + "]]},");
                    }
                    cp.sb_Value1 = cp.sb_Value1.Remove(cp.sb_Value1.Length - 1, 1);
                    cp.sb_Value1.Append("];");

                }
            }
            return cplst;
        }
        public static List<chartProperty> SetArrayString_Pie(DataTable dt, List<chartProperty> cplst)
        {
            if (dt.Rows.Count == 0)
            {
                return cplst;
            }
            foreach (chartProperty cp in cplst)
            {

                if (cp.strChartType == "PIE")
                {
                    cp.SetYColumn();
                    cp.sb_Value1.Append("[");
                    foreach (string c in cp.list_Ycolumn)
                    {
                        cp.sb_Value1.Append("{data:[[0," + dt.Rows[0][c].ToString() + "]],label:'" + c.ToString() + "'},");
                    }

                    cp.sb_Value1 = cp.sb_Value1.Remove(cp.sb_Value1.Length - 1, 1);
                    cp.sb_Value1.Append("];");
                }
            }
            return cplst;
        }
        public static chartProperty chartProperty(string xvalue, string yvalue, string y2value, string strHeaderText, string strChartType, int numMaxYvalue, int numWidth, int numHeight, string horizon, string vertical, string showlegend, string showxlabel, string showylabel, string showy2label,string PObject,string PCounter,string Instance)
        {
            chartProperty cp = new chartProperty();
            cp = chartProperty(xvalue, yvalue, y2value, strHeaderText, strChartType, numMaxYvalue, numWidth, numHeight, horizon, vertical, showlegend,  showxlabel,  showylabel,  showy2label);
            cp.strPObject = PObject;
            cp.strPCounter = PCounter;
            cp.strInstance = Instance;
            return cp;
        }
        public static chartProperty chartProperty(string xvalue, string yvalue, string y2value, string strHeaderText, string strChartType, int numMaxYvalue, int numWidth, int numHeight, string horizon, string vertical, string showlegend, string showxlabel, string showylabel, string showy2label)
        {
            chartProperty cp = new chartProperty();
            cp = chartProperty(xvalue, yvalue, y2value, strHeaderText, strChartType, numMaxYvalue, numWidth, numHeight, horizon, vertical);
            cp.bolShowLegent = showlegend;
            cp.bolShowXlabel = showxlabel;
            cp.bolShowYlabel = showylabel;
            cp.bolShowY2label = showy2label;
            return cp;
        }
        public static chartProperty chartProperty(string xvalue, string yvalue, string y2value, string strHeaderText, string strChartType, int numMaxYvalue, int numWidth, int numHeight, string horizon, string vertical)
        {
            chartProperty cp = new chartProperty();
            cp = chartProperty(xvalue, yvalue, y2value, strHeaderText, strChartType, numMaxYvalue, numWidth, numHeight);
            cp.verticalLines = vertical;
            cp.horizontalLines = horizon;
            return cp;
        }
        public static chartProperty chartProperty(string xvalue, string yvalue, string y2value, string strHeaderText, string strChartType, int numMaxYvalue, int numWidth, int numHeight)
        {
            chartProperty cp = new chartProperty();
            cp = chartProperty(xvalue, yvalue, y2value, strHeaderText, strChartType, numMaxYvalue);
            cp.numWidth = numWidth;
            cp.numHeight = numHeight;
            return cp;
        }
        public static chartProperty chartProperty(string xvalue, string yvalue, string y2value, string strHeaderText, string strChartType, int numMaxYvalue)
        {
            chartProperty cp = new chartProperty();
            cp = chartProperty(xvalue, yvalue, y2value, strHeaderText, strChartType);
            cp.maxYvalue = numMaxYvalue;
            return cp;
        }
        public static chartProperty chartProperty(string xvalue, string yvalue, string y2value, string strHeaderText, string strChartType)
        {
            chartProperty cp = new chartProperty();
            cp.XvalueColumn = xvalue;
            cp.YvalueColumn = yvalue;
            cp.tmpValueColum = y2value;
            cp.strAccessibleHeaderText = strHeaderText;
            if (string.IsNullOrEmpty(strChartType))
                strChartType = "LINE";
            cp.strChartType = strChartType;
            return cp;
        }
        public static StringBuilder SetStringValue(List<chartProperty> cplst, StringBuilder sb, string strServerNum)
        {
            foreach (chartProperty cp in cplst)
            {
                if (string.IsNullOrEmpty(cp.sb_Value1.ToString()))
                    continue;
                if (cp.strChartType == "DUALLINE")
                {
                    sb.Append("var " + cp.strAccessibleHeaderText + "Value =" + cp.sb_Value1.ToString());
                    sb.Append("var " + cp.strAccessibleHeaderText + "Value2 =" + cp.sb_Value2.ToString());
                    sb.Append("func_DualYLine(document.getElementById(\"grp_" + strServerNum + "_" + cp.strAccessibleHeaderText + "\")," + cp.strAccessibleHeaderText + "Value," + cp.strAccessibleHeaderText + "Value2," + cp.maxYvalue.ToString() + "," + cp.numWidth + "," + cp.numHeight + "," + cp.horizontalLines + "," + cp.verticalLines + "," + cp.bolShowLegent + "," + cp.bolShowXlabel + "," + cp.bolShowYlabel + "," + cp.bolShowY2label + ",'" + cp.YvalueColumn + "','" + cp.tmpValueColum + "');");

                }
                if (cp.strChartType == "LINE")
                {
                    sb.Append("var " + cp.strAccessibleHeaderText + "Value =[" + cp.sb_Value1.ToString() + "];");
                    sb.Append("func_Line(document.getElementById(\"grp_" + strServerNum + "_" + cp.strAccessibleHeaderText + "\")," + cp.strAccessibleHeaderText + "Value," + cp.maxYvalue.ToString() + "," + cp.numWidth + "," + cp.numHeight + "," + cp.horizontalLines + "," + cp.verticalLines + "," + cp.bolShowLegent + "," + cp.bolShowXlabel + "," + cp.bolShowYlabel + "," + cp.bolShowY2label + ");");
                }
                if (cp.strChartType == "BAR")
                {
                    sb.Append("var " + cp.strAccessibleHeaderText + "Value =" + cp.sb_Value1.ToString());
                    sb.Append("var " + cp.strAccessibleHeaderText + "Value2 =" + cp.sb_Value2.ToString());
                    sb.Append("func_Bar(document.getElementById(\"grp_" + strServerNum + "_" + cp.strAccessibleHeaderText + "\")," + cp.strAccessibleHeaderText + "Value," + cp.strAccessibleHeaderText + "Value2," + cp.maxYvalue.ToString() + "," + cp.numWidth + "," + cp.numHeight + "," + cp.horizontalLines + "," + cp.verticalLines + "," + cp.bolShowLegent + "," + cp.bolShowXlabel + "," + cp.bolShowYlabel + "," + cp.bolShowY2label + ");");

                }
                if (cp.strChartType == "COL")
                {
                    sb.Append("var " + cp.strAccessibleHeaderText + "Value =" + cp.sb_Value1.ToString());
                    sb.Append("func_Col(document.getElementById(\"grp_" + strServerNum + "_" + cp.strAccessibleHeaderText + "\")," + cp.strAccessibleHeaderText + "Value," + cp.maxYvalue.ToString() + "," + cp.numWidth + "," + cp.numHeight + "," + cp.horizontalLines + "," + cp.verticalLines + "," + cp.bolShowLegent + "," + cp.bolShowXlabel + "," + cp.bolShowYlabel + "," + cp.bolShowY2label + ");");
                }
                if (cp.strChartType == "COLS")
                {
                    sb.Append("var " + cp.strAccessibleHeaderText + "Value =" + cp.sb_Value1.ToString());
                    sb.Append("func_Cols(document.getElementById(\"grp_" + strServerNum + "_" + cp.strAccessibleHeaderText + "\")," + cp.strAccessibleHeaderText + "Value," + cp.maxYvalue.ToString() + "," + cp.numWidth + "," + cp.numHeight + "," + cp.horizontalLines + "," + cp.verticalLines + "," + cp.bolShowLegent + "," + cp.bolShowXlabel + "," + cp.bolShowYlabel + "," + cp.bolShowY2label + ");");
                }
                if (cp.strChartType == "PIE")
                {
                    sb.Append("var " + cp.strAccessibleHeaderText + "Value =" + cp.sb_Value1.ToString());
                    sb.Append("func_Pie(document.getElementById(\"grp_" + strServerNum + "_" + cp.strAccessibleHeaderText + "\")," + cp.strAccessibleHeaderText + "Value," + cp.maxYvalue.ToString() + "," + cp.numWidth + "," + cp.numHeight + "," + cp.horizontalLines + "," + cp.verticalLines + "," + cp.bolShowLegent + "," + cp.bolShowXlabel + "," + cp.bolShowYlabel + "," + cp.bolShowY2label + ");");
                }
                if (cp.strChartType == "COLSHORIZON")
                {
                    sb.Append("var " + cp.strAccessibleHeaderText + "Value =" + cp.sb_Value1.ToString());
                    sb.Append("func_Cols_Horizon(document.getElementById(\"grp_" + strServerNum + "_" + cp.strAccessibleHeaderText + "\")," + cp.strAccessibleHeaderText + "Value," + cp.maxYvalue.ToString() + "," + cp.numWidth + "," + cp.numHeight + "," + cp.horizontalLines + "," + cp.verticalLines + "," + cp.bolShowLegent + "," + cp.bolShowXlabel + "," + cp.bolShowYlabel + "," + cp.bolShowY2label + ");");
                }
            }
            cplst.Clear();
            return sb;
        }
        //ColumnName = 0:Pobject 1:PCounter 2:Instance
        public static string DecodingChartName_Pobject(string strChartName, int ColumName)
        {
            string strReturnValue = "";
            string[] separating = { "_c_" };
            string strReturn = System.Web.HttpUtility.UrlDecode(strChartName.Replace("_LINE_CHART",""));
            string[] strReturnArray = strReturn.Split(separating, System.StringSplitOptions.RemoveEmptyEntries);
            //인스턴스가 존재하지않을경우 
            if (strReturnArray.Count() == 3 && ColumName != 2)
            {
                strReturnValue = strReturnArray[ColumName];
            }
            else
            {
                if (strReturnArray.Count()==3 && ColumName==2)
                {
                    strReturnValue = strReturnArray[ColumName];
                }
            }
            
            return strReturnValue;
        }

    }
}
