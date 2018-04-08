using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace ServicePoint.Lib
{
    class ConvertingProc
    {

        public static DataTable SetDashboardDtServer(DataTable dt)
        {

            foreach (DataRow dr in dt.Rows)
            {
                //tbDashboard 변환
                if (dt.Columns.Contains("ASPRequests"))
                {
                    dr["ASPRequests"] = Math.Round(Convert.ToDouble(dr["ASPRequests"]), 2);
                }

                if (dt.Columns.Contains("CPU-Total"))//소수2자리까지
                    dr["CPU-Total"] = Math.Round(Convert.ToDouble(dr["CPU-Total"]), 2);

                if (dt.Columns.Contains("CPU-Kernel"))//소수2자리까지
                    dr["CPU-Kernel"] = Math.Round(Convert.ToDouble(dr["CPU-Kernel"]), 2);

                if (dt.Columns.Contains("CPU-SQL"))//소수2자리까지
                    dr["CPU-SQL"] = Math.Round(Convert.ToDouble(dr["CPU-SQL"]), 2);

                if (dt.Columns.Contains("CPU-W3WP"))
                    dr["CPU-W3WP"] = Math.Round(Convert.ToDouble(dr["CPU-W3WP"]), 2);

                if (dt.Columns.Contains("CommittedMemory"))//byte -> GB
                    dr["CommittedMemory"] = Math.Round((Convert.ToDouble(dr["CommittedMemory"])) / (1024 * 1024 * 1024), 2);

                if (dt.Columns.Contains("NetworkBytes-Total"))//byte->MB
                    dr["NetworkBytes-Total"] = Math.Round((Convert.ToDouble(dr["NetworkBytes-Total"])) / (1024 * 1024), 2);

                if (dt.Columns.Contains("NetworkBytes-Sent"))//byte->MB
                    dr["NetworkBytes-Sent"] = Math.Round((Convert.ToDouble(dr["NetworkBytes-Sent"])) / (1024 * 1024), 2);

                if (dt.Columns.Contains("NetworkBytes-Received"))//byte->MB
                    dr["NetworkBytes-Received"] = Math.Round((Convert.ToDouble(dr["NetworkBytes-Received"])) / (1024 * 1024), 2);


                if (dt.Columns.Contains("RamSize"))//byte->GB
                    dr["RamSize"] = Math.Round(Convert.ToDouble(dr["RamSize"]) / Convert.ToDouble(1024 * 1024 * 1024), 2);
                
                if (dt.Columns.Contains("SQLTotalServerMemory"))//KB->GB
                    dr["SQLTotalServerMemory"] = Math.Round((Convert.ToDouble(dr["SQLTotalServerMemory"])) / (1024 * 1024), 2);

                if (dt.Columns.Contains("SQLTargetServerMemory"))//KB->GB
                    dr["SQLTargetServerMemory"] = Math.Round((Convert.ToDouble(dr["SQLTargetServerMemory"])) / (1024 * 1024), 2);
                
                if (dt.Columns.Contains("w3wpTotalWorkProcessMemory"))//byte->GB
                    dr["w3wpTotalWorkProcessMemory"] = Math.Round((Convert.ToDouble(dr["w3wpTotalWorkProcessMemory"])) / (1024 * 1024 * 1024), 2);

                if (dt.Columns.Contains("w3wpMaxWorkProcessMemory"))//byte->GB
                    dr["w3wpMaxWorkProcessMemory"] = Math.Round((Convert.ToDouble(dr["w3wpMaxWorkProcessMemory"])) / (1024 * 1024 * 1024), 2);

                if (dt.Columns.Contains("AvailableMemory"))//MB->GB
                    dr["AvailableMemory"] = Math.Round((Convert.ToDouble(dr["AvailableMemory"])) / (1024), 2);

                if (dt.Columns.Contains("PhysicalDiskRead"))
                    dr["PhysicalDiskRead"] = Math.Round(Convert.ToDouble(dr["PhysicalDiskRead"]), 2);

                if (dt.Columns.Contains("PhysicalDiskWrite"))
                    dr["PhysicalDiskWrite"] = Math.Round(Convert.ToDouble(dr["PhysicalDiskWrite"]), 2);

                if (dt.Columns.Contains("NetworkBytes-IISTotal"))//byte->MB
                    dr["NetworkBytes-IISTotal"] = Math.Round((Convert.ToDouble(dr["NetworkBytes-IISTotal"])) / (1024 * 1024), 2);

                if (dt.Columns.Contains("SQLBufferCacheSize"))//page->MB
                    dr["SQLBufferCacheSize"] = Math.Round(((Convert.ToDouble(dr["SQLBufferCacheSize"])) * 8) / 1024, 2);

                if (dt.Columns.Contains("SQLBufferCacheHit"))//소수2자리까지
                    dr["SQLBufferCacheHit"] = Math.Round(Convert.ToDouble(dr["SQLBufferCacheHit"]), 2);

                if (dt.Columns.Contains("SQLPlanCacheSize"))//page->MB
                    dr["SQLPlanCacheSize"] = Math.Round(((Convert.ToDouble(dr["SQLPlanCacheSize"])) * 8) / 1024, 2);

                if (dt.Columns.Contains("SQLPlanCacheHit"))//소수2자리까지
                    dr["SQLPlanCacheHit"] = Math.Round(Convert.ToDouble(dr["SQLPlanCacheHit"]), 2);

                if (dt.Columns.Contains("SQLCompilations"))//소수2자리까지
                    dr["SQLCompilations"] = Math.Round(Convert.ToDouble(dr["SQLCompilations"]), 2);

                if (dt.Columns.Contains("SQLReCompilations"))//소수2자리까지
                    dr["SQLReCompilations"] = Math.Round(Convert.ToDouble(dr["SQLReCompilations"]), 2);

                if (dt.Columns.Contains("SQLBatchRequests"))//소수2자리까지
                    dr["SQLBatchRequests"] = Math.Round(Convert.ToDouble(dr["SQLBatchRequests"]), 2);

                if (dt.Columns.Contains("SQLCheckpointPage"))//소수2자리까지
                    dr["SQLCheckpointPage"] = Math.Round(Convert.ToDouble(dr["SQLCheckpointPage"]), 2);

                if (dt.Columns.Contains("SQLLogFlushes"))//소수2자리까지
                    dr["SQLLogFlushes"] = Math.Round(Convert.ToDouble(dr["SQLLogFlushes"]), 2);

                if (dt.Columns.Contains("SQLLazyWrites"))//소수2자리까지
                    dr["SQLLazyWrites"] = Math.Round(Convert.ToDouble(dr["SQLLazyWrites"]), 2);

                if (dt.Columns.Contains("SQLReadahead"))//소수2자리까지
                    dr["SQLReadahead"] = Math.Round(Convert.ToDouble(dr["SQLReadahead"]), 2);



                //tbDatabses 변환
                if (dt.Columns.Contains("datafilesize_use"))//MB->GB
                    dr["datafilesize_use"] = Math.Round((Convert.ToDouble(dr["datafilesize_use"])) / 1024, 2);
                if (dt.Columns.Contains("logfilesize_use"))//MB->GB
                    dr["logfilesize_use"] = Math.Round((Convert.ToDouble(dr["logfilesize_use"])) / 1024, 2);
                if (dt.Columns.Contains("DataFileSize_Total"))//MB->GB
                    dr["DataFileSize_Total"] = Math.Round((Convert.ToDouble(dr["DataFileSize_Total"])) / 1024, 2);
                if (dt.Columns.Contains("LogFileSize_Total"))//MB->GB
                    dr["LogFileSize_Total"] = Math.Round((Convert.ToDouble(dr["LogFileSize_Total"])) / 1024, 2);


                //Disk 변환
                if (dt.Columns.Contains("MaxDiskTime"))
                {
                    if (dr["MaxDiskTime"] != DBNull.Value)
                        dr["MaxDiskTime"] = Math.Round(Convert.ToDouble(dr["MaxDiskTime"]), 2);
                    else
                        dr["MaxDiskTime"] = 0;
                }

                if (dt.Columns.Contains("MinDiskIdleTime"))
                {
                    if (dr["MinDiskIdleTime"] != DBNull.Value)
                        dr["MinDiskIdleTime"] = Math.Round(Convert.ToDouble(dr["MinDiskIdleTime"]), 2);
                    else
                        dr["MinDiskIdleTime"] = 0;
                   
                }

                if (dt.Columns.Contains("IISRequests"))
                {
                    if (dr["IISRequests"] != DBNull.Value)
                        dr["IISRequests"] = Math.Round(Convert.ToDouble(dr["IISRequests"]), 2);
                    else
                        dr["IISRequests"] = 0;
                }
                


               
                
                
               
            }

            return dt;
        }
        public static DataTable dataTableNull(DataTable dt)
        {
            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dr[dc] == DBNull.Value)
                    {
                        dr[dc] = SetDefault(dc);
                    }
                }
            }
            return dt;
        }

        public static DataTable w_Dashboard(DataTable dt_Org)
        {
            dt_Org.Columns.Add("AgentStatus");
            DataTable dt = new DataTable();
            dt = dt_Org.Clone();
            foreach (DataRow dr in dt_Org.Rows)
            {
                DataRow dr_new = dt.NewRow();

                if (dr["TimeIn_UTC"].ToString() !="")
                {
                    var i = Datediff(dr.Field<DateTime>("TimeIn_UTC"), dr.Field<DateTime>("DB_UTC"));

                    if (i > 120)
                        dr["AgentStatus"] = 0;
                    else
                        dr["AgentStatus"] = 1;
                }
                else
                {
                    dr["AgentStatus"] = 0;
                }

                foreach (DataColumn dc in dt_Org.Columns)
                {
                    if (dr[dc] == DBNull.Value)
                    {
                        dr[dc] = SetDefault(dc);
                    }
                }
                
                dt.ImportRow(dr);
            }

            dt = ChangeDashboardColumnName(dt);

            return dt;
        }

        public static DataTable SetDiskProc(DataTable dtDisk)
        {
            /*
            DataTable dt = new DataTable();
            dt.Columns.Add("Drive");
            dt.Columns.Add("FreeGB");
            dt.Columns.Add("FreePercent");
            
            foreach (DataRow dr in dtDisk.Rows)
            {
                string Drive = dr["InstanceName"].ToString();
                string FreeSpace_Percent = dr["P164"].ToString();
                string FreeSpace_Mega = dr["P018"].ToString();

                if (Drive == "_Total" || (Drive.Length >= 14 && Drive.Substring(0, 14) == "HarddiskVolume"))
                {
                    
                }
                else
                {
                    dt.Rows.Add(Drive, Math.Round((Convert.ToDouble(FreeSpace_Mega) / (1024)), 2), FreeSpace_Percent);
                }
            }

            return dt;
            */
            string[] strValue = new string[3];
            Dictionary<string, string[]> dicRow = new Dictionary<string, string[]>();
            DataTable dt = new DataTable();
            dt.Columns.Add("Drive");
            dt.Columns.Add("FreeGB");
            dt.Columns.Add("FreePercent");
            foreach (DataRow dr in dtDisk.Rows)
            {
                if (dicRow.ContainsKey(dr["InstanceName"].ToString()))
                {
                    strValue = dicRow[dr["Instancename"].ToString()];
                    if (dr["PCID"].ToString().ToUpper() == "P164") //Free Percent
                    {
                        strValue[0] = dr["InstanceName"].ToString();
                        strValue[1] = dr["Pvalue"].ToString();
                    }
                    else if (dr["PCID"].ToString().ToUpper() == "P018") //Free Megabytes
                    {
                        strValue[0] = dr["InsTanceName"].ToString();
                        strValue[2] = dr["Pvalue"].ToString();
                    }

                    //else
                    //{
                    //    strValue[0] = dr["InstanceName"].ToString();
                    //    strValue[3] = dr["Pvalue"].ToString();
                    //}
                    dicRow[dr["InstanceName"].ToString()] = strValue;
                }
                else
                {
                    strValue = new string[3];
                    if (dr["PCID"].ToString().ToUpper() == "P164") //잔여사용 용량
                    {
                        strValue[0] = dr["InstanceName"].ToString();
                        strValue[1] = dr["Pvalue"].ToString();
                    }
                    else if (dr["PCID"].ToString().ToUpper() == "P018") //Free Megabytes
                    {
                        strValue[0] = dr["InsTanceName"].ToString();
                        strValue[2] = dr["Pvalue"].ToString();
                    }
                    //else
                    //{
                    //    strValue[0] = dr["InstanceName"].ToString();
                    //    strValue[3] = dr["Pvalue"].ToString();
                    //}
                    dicRow.Add(dr["InstanceName"].ToString(), strValue);
                }

            }
            foreach (KeyValuePair<string, string[]> ky in dicRow)
            {
                DataRow dr = dt.NewRow();
                dr["Drive"] = ky.Value[0];
                dr["FreePercent"] = ky.Value[1];
                dr["FreeGB"] = Math.Round((Convert.ToDouble(ky.Value[2]) / (1024)), 2);
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable SetDiskMaxProc(DataTable dtDisk)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("MaxDiskTime");
            dt.Columns.Add("MaxDiskTimeInstance");
            dt.Columns.Add("MinDiskIdleTime");
            dt.Columns.Add("MinDiskIdleTimeInstance");
            dt.Columns.Add("MaxDiskQueueLength");
            dt.Columns.Add("MaxDiskQueueLengthInstance");

            double MaxDiskTime = 0;
            string MaxDiskTimeInstance = "";
            double MinDiskIdleTime = 110;
            string MinDiskIdleTimeInstance = "";
            double MaxDiskQueueLength = 0;
            string MaxDiskQueueLengthInstance = "";

            foreach (DataRow dr in dtDisk.Rows)
            {
                
                string Drive = dr["InstanceName"].ToString();


                if (Drive == "_Total" || (Drive.Length >= 14 && Drive.Substring(0, 14) == "HarddiskVolume"))
                {

                }
                else
                {
                    if (dr["PCID"].ToString().ToUpper() == "P015")
                    { 
                        if (Convert.ToDouble(dr["PValue"].ToString()) > MaxDiskTime)
                        {
                            MaxDiskTime = Convert.ToDouble(dr["PValue"].ToString());
                            MaxDiskTimeInstance = dr["InstanceName"].ToString();
                        }
                    }

                    if (dr["PCID"].ToString().ToUpper() == "P190")
                    {
                        if (Convert.ToDouble(dr["PValue"].ToString()) < MinDiskIdleTime)
                        {
                            MinDiskIdleTime = Convert.ToDouble(dr["PValue"].ToString());
                            MinDiskIdleTimeInstance = dr["InstanceName"].ToString();
                        }
                    }

                    if (dr["PCID"].ToString().ToUpper() == "P194")
                    {
                        if (Convert.ToDouble(dr["PValue"].ToString()) > MaxDiskQueueLength)
                        {
                            MaxDiskQueueLength = Convert.ToDouble(dr["PValue"].ToString());
                            MaxDiskQueueLengthInstance = dr["InstanceName"].ToString();
                        }
                    }
                }
            }

            dt.Rows.Add(MaxDiskTime, MaxDiskTimeInstance, MinDiskIdleTime, MinDiskIdleTimeInstance, MaxDiskQueueLength, MaxDiskQueueLengthInstance);

            return dt;
        }

        public static DataTable SetDiskProc_All(DataTable dt)
        {
            string[] strValue = new string[4];

            SortedDictionary<string, SortedDictionary<string, string[]>> dicKey = new SortedDictionary<string, SortedDictionary<string, string[]>>();
            DataTable dt_tmp = new DataTable();
            dt_tmp.Columns.Add("Drive");
            dt_tmp.Columns.Add("FreeGB");
            dt_tmp.Columns.Add("FreePercent");
            dt_tmp.Columns.Add("ServerNum", typeof(int));
            foreach (DataRow dr in dt.Rows)
            {
                SortedDictionary<string, string[]> dicRow = new SortedDictionary<string, string[]>();

                //servernum이 잇다면
                if (dicKey.ContainsKey(dr["ServerNum"].ToString()))
                {
                    dicRow = dicKey[dr["ServerNum"].ToString()];
                    if (dicRow.ContainsKey(dr["InstanceName"].ToString()))
                    {
                        strValue = dicRow[dr["Instancename"].ToString()];
                        ////서버넘버가 같은게 잇으면 여기 
                        //if (strValue[3].ToString() == dr["ServerNum"].ToString())
                        //{ }
                        //else
                        //{ }
                        if (dr["PCID"].ToString().ToUpper() == "P164") //잔여사용 용량
                        {
                            strValue[0] = dr["InstanceName"].ToString();
                            strValue[1] = dr["Pvalue"].ToString();
                            strValue[3] = dr["ServerNum"].ToString();
                        }
                        else if (dr["PCID"].ToString().ToUpper() == "P018") //Free Megabytes
                        {
                            strValue[0] = dr["InstanceName"].ToString();
                            strValue[2] = dr["Pvalue"].ToString();
                            strValue[3] = dr["ServerNum"].ToString();
                        }
                       
                        dicRow[dr["InstanceName"].ToString()] = strValue;
                    }
                    else
                    {
                        strValue = new string[4];
                        if (dr["PCID"].ToString().ToUpper() == "P164") //잔여사용 용량
                        {
                            strValue[0] = dr["InstanceName"].ToString();
                            strValue[1] = dr["Pvalue"].ToString();
                            strValue[3] = dr["ServerNum"].ToString();
                        }
                        else if (dr["PCID"].ToString().ToUpper() == "P018") //Free Megabytes
                        {
                            strValue[0] = dr["InstanceName"].ToString();
                            strValue[2] = dr["Pvalue"].ToString();
                            strValue[3] = dr["ServerNum"].ToString();
                        }

                        dicRow.Add(dr["InstanceName"].ToString(), strValue);
                    }
                    dicKey[dr["ServerNum"].ToString()] = dicRow;
                }
                else
                {
                    dicKey.Add(dr["ServerNum"].ToString(), dicRow);
                    if (dicRow.ContainsKey(dr["InstanceName"].ToString()))
                    {
                        strValue = dicRow[dr["Instancename"].ToString()];
                        ////서버넘버가 같은게 잇으면 여기 
                        //if (strValue[3].ToString() == dr["ServerNum"].ToString())
                        //{ }
                        //else
                        //{ }
                        if (dr["PCID"].ToString().ToUpper() == "P164") //잔여사용 용량
                        {
                            strValue[0] = dr["InstanceName"].ToString();
                            strValue[1] = dr["Pvalue"].ToString();
                            strValue[3] = dr["ServerNum"].ToString();
                        }
                        else if (dr["PCID"].ToString().ToUpper() == "P018") //Free Megabytes
                        {
                            strValue[0] = dr["InstanceName"].ToString();
                            strValue[2] = dr["Pvalue"].ToString();
                            strValue[3] = dr["ServerNum"].ToString();
                        }

                        dicRow[dr["InstanceName"].ToString()] = strValue;
                    }
                    else
                    {
                        strValue = new string[4];
                        if (dr["PCID"].ToString().ToUpper() == "P164") //잔여사용 용량
                        {
                            strValue[0] = dr["InstanceName"].ToString();
                            strValue[1] = dr["Pvalue"].ToString();
                            strValue[3] = dr["ServerNum"].ToString();
                        }
                        else if (dr["PCID"].ToString().ToUpper() == "P018") //Free Megabytes
                        {
                            strValue[0] = dr["InstanceName"].ToString();
                            strValue[2] = dr["Pvalue"].ToString();
                            strValue[3] = dr["ServerNum"].ToString();
                        }

                        dicRow.Add(dr["InstanceName"].ToString(), strValue);
                    }
                    dicKey[dr["ServerNum"].ToString()] = dicRow;
                }


            }
            foreach (KeyValuePair<string, SortedDictionary<string, string[]>> ky in dicKey)
            {
                foreach (var k in ky.Value)
                {
                    DataRow dr = dt_tmp.NewRow();
                    dr["Drive"] = k.Value[0];
                    dr["FreePercent"] = k.Value[1];
                    dr["FreeGB"] = Math.Round((Convert.ToDouble(k.Value[2]) / (1024)), 2);
                    dr["ServerNum"] = Convert.ToInt32(ky.Key);
                    dt_tmp.Rows.Add(dr);


                }
                //    foreach(KeyValuePair<string,string[]> k in ky)
                //    {
                //    DataRow dr = dt_tmp.NewRow();
                //    dr["Drive"] = ky.Value[0];
                //    dr["FreePercent"] = ky.Value[1];
                //    dr["FreeGB"] = ky.Value[2];
                //    dr["ServerNum"] = ky.Value[3];
                //    dt_tmp.Rows.Add(dr);
                //        }
            }
            return dt_tmp;
        }




        public static DataTable ChangeDashboardColumnName(DataTable dt)
        {
            if (dt.Columns.Contains("P0"))
                dt.Columns["P0"].ColumnName = "CPU-Total";
            if (dt.Columns.Contains("P1"))
                dt.Columns["P1"].ColumnName = "CPU-Kernel";
            if (dt.Columns.Contains("P2"))
                dt.Columns["P2"].ColumnName = "ProcessorQueueLength";
            if (dt.Columns.Contains("P3"))
                dt.Columns["P3"].ColumnName = "AvailableMemory";
            if (dt.Columns.Contains("P4"))
                dt.Columns["P4"].ColumnName = "CommittedMemory";
            if (dt.Columns.Contains("P5"))
                dt.Columns["P5"].ColumnName = "LogicalDiskTIme";
            if (dt.Columns.Contains("P6"))
                dt.Columns["P6"].ColumnName = "LogicalDiskAvgRead";
            if (dt.Columns.Contains("P7"))
                dt.Columns["P7"].ColumnName = "LogicalDiskFreeMByte-C";
            if (dt.Columns.Contains("P8"))
                dt.Columns["P8"].ColumnName = "NetworkBytes-Total";
            if (dt.Columns.Contains("P9"))
                dt.Columns["P9"].ColumnName = "NetworkBytes-Received";
            if (dt.Columns.Contains("P10"))
                dt.Columns["P10"].ColumnName = "NetworkBytes-Sent";
            if (dt.Columns.Contains("P11"))
                dt.Columns["P11"].ColumnName = "NetworkBytes-IISTotal";
            if (dt.Columns.Contains("P12"))
                dt.Columns["P12"].ColumnName = "IISCurrentConnection";
            if (dt.Columns.Contains("P13"))
                dt.Columns["P13"].ColumnName = "CPU-W3WP";
            if (dt.Columns.Contains("P14"))
                dt.Columns["P14"].ColumnName = "w3wpMaxWorkProcessMemory";
            if (dt.Columns.Contains("P15"))
                dt.Columns["P15"].ColumnName = "w3wpTotalWorkProcessMemory";
            if (dt.Columns.Contains("P16"))
                dt.Columns["P16"].ColumnName = "ASPRequestExecutionTime";
            if (dt.Columns.Contains("P17"))
                dt.Columns["P17"].ColumnName = "ASPRequestsExecuting";
            if (dt.Columns.Contains("P18"))
                dt.Columns["P18"].ColumnName = "ASPRequestsQueue";
            if (dt.Columns.Contains("P19"))
                dt.Columns["P19"].ColumnName = "ASPRequestsPerSec";
            if (dt.Columns.Contains("P20"))
                dt.Columns["P20"].ColumnName = "CPU-SQL";
            if (dt.Columns.Contains("P21"))
                dt.Columns["P21"].ColumnName = "SQLTargetServerMemory";
            if (dt.Columns.Contains("P22"))
                dt.Columns["P22"].ColumnName = "SQLTotalServerMemory";
            if (dt.Columns.Contains("P23"))
                dt.Columns["P23"].ColumnName = "PhysicalDiskIdleTime";
            if (dt.Columns.Contains("P24"))
                dt.Columns["P24"].ColumnName = "PhysicalDiskTime";
            if (dt.Columns.Contains("P25"))
                dt.Columns["P25"].ColumnName = "PhysicalDIskQueueLength";
            if (dt.Columns.Contains("P26"))
                dt.Columns["P26"].ColumnName = "SQLBufferCacheHit";
            if (dt.Columns.Contains("P27"))
                dt.Columns["P27"].ColumnName = "SQLPlanCacheHit";
            if (dt.Columns.Contains("P28"))
                dt.Columns["P28"].ColumnName = "SQLBatchRequests";
            if (dt.Columns.Contains("P29"))
                dt.Columns["P29"].ColumnName = "SQLCompilations";
            if (dt.Columns.Contains("P30"))
                dt.Columns["P30"].ColumnName = "SQLReCompilations";
            if (dt.Columns.Contains("P31"))
                dt.Columns["P31"].ColumnName = "SQLPageLifeExpectancy";
            if (dt.Columns.Contains("P32"))
                dt.Columns["P32"].ColumnName = "SQLBufferCacheSize";
            if (dt.Columns.Contains("P33"))
                dt.Columns["P33"].ColumnName = "SQLPlanCacheSize";
            if (dt.Columns.Contains("P34"))
                dt.Columns["P34"].ColumnName = "SQLCheckpointPage";
            if (dt.Columns.Contains("P35"))
                dt.Columns["P35"].ColumnName = "PhysicalDiskRead";
            if (dt.Columns.Contains("P36"))
                dt.Columns["P36"].ColumnName = "PhysicalDiskWrite";
            if (dt.Columns.Contains("P37"))
                dt.Columns["P37"].ColumnName = "SQLReadahead";
            if (dt.Columns.Contains("P38"))
                dt.Columns["P38"].ColumnName = "SQLLazyWrites";
            if (dt.Columns.Contains("P39"))
                dt.Columns["P39"].ColumnName = "SQLLogFlushes";
            if (dt.Columns.Contains("P40"))
                dt.Columns["P40"].ColumnName = "SQLConnection";
            if (dt.Columns.Contains("P41"))
                dt.Columns["P41"].ColumnName = "SQLDataFileSize";
            if (dt.Columns.Contains("P42"))
                dt.Columns["P42"].ColumnName = "SQLLogFIleSize";
            if (dt.Columns.Contains("P43"))
                dt.Columns["P43"].ColumnName = "SQLLogFileUsedSize";
            if (dt.Columns.Contains("P44"))
                dt.Columns["P44"].ColumnName = "SQLReponstime";
            if (dt.Columns.Contains("P45"))
                dt.Columns["P45"].ColumnName = "NetworkOutputQueueLength";
            if (dt.Columns.Contains("P46"))
                dt.Columns["P46"].ColumnName = "SQLActiveTempTable";
            if (dt.Columns.Contains("P47"))
                dt.Columns["P47"].ColumnName = "SQLActiveTransactions";
            if (dt.Columns.Contains("P48"))
                dt.Columns["P48"].ColumnName = "IISActiveRequests";
            if (dt.Columns.Contains("P49"))
                dt.Columns["P49"].ColumnName = "IISRequests";
            if (dt.Columns.Contains("P50"))
                dt.Columns["P50"].ColumnName = "IISCurrentUser";
            if (dt.Columns.Contains("P51"))
                dt.Columns["P51"].ColumnName = "";
            if (dt.Columns.Contains("P52"))
                dt.Columns["P52"].ColumnName = "IIStoDBConnection";
               


            return dt;
        }
             

        public static object SetDefault(DataColumn dc)
        {
            object objectValue;
            if (dc.DataType.Name.ToLower().IndexOf("string") > -1)
                objectValue = string.Empty;
            if (dc.DataType.Name.ToLower().IndexOf("datetime") > -1)
                objectValue = DateTime.Now;
            else
                objectValue = 0;

            return objectValue;

        }

        public static Int64 Datediff(DateTime dtm, DateTime dtmcurrent)
        {
            Int64 numValue = dtmcurrent.Ticks - dtm.Ticks;
            TimeSpan ts = TimeSpan.FromTicks(numValue);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        public static DataTable SetByteToAny_RowValue(DataTable dt, Dictionary<string, string> dicChangeValue, string strChangeColumnname)
        {

            foreach (DataRow dr in dt.Rows)
            {
                foreach (DataColumn dc in dt.Columns)
                {
                    foreach (var i in dicChangeValue)
                    {
                        Int64 intValue = Convert.ToInt64(i.Value);
                        if (dr[dc].ToString() == i.Key)
                            dr[strChangeColumnname] = Convert.ToDouble((Convert.ToInt64(dr[strChangeColumnname])) / (intValue));
                    }
                }
            }
            return dt;
        }

        public static DataTable SetByteToAny_Column(DataTable dt, Dictionary<string, string> dicChangeValue)
        {
            foreach (DataRow dr in dt.Rows)
            {
                foreach (var i in dicChangeValue)
                {
                    if (dt.Columns.Contains(i.Key))
                        dr[i.Key] = (Convert.ToDouble(dr[i.Key]) / (Convert.ToInt64(dicChangeValue.Values)));
                }
            }
            return dt;
        }
        public static DataTable Pivot(DataTable dt_Origin, string strPivotColumn, string strGroupPingCol, string strValueCol)
        {
            DataTable dt = new DataTable();
            SortedDictionary<string, string> dicColumnName = new SortedDictionary<string, string>();
            dicColumnName = GetColumn(dt_Origin, strPivotColumn);
            dt = Pivot(dt_Origin, dicColumnName, strPivotColumn, strGroupPingCol, strValueCol);
            return dt;
        }

        public static string GetColumname(DataColumnCollection dccol)
        {
            List<string> listColumn = new List<string>();
            foreach (DataColumn dc in dccol)
            {
                if (dc.ColumnName != "TimeIn")
                    listColumn.Add(dc.ColumnName);
            }
            return string.Join("#c#", listColumn);
        }
        /// <summary>
        /// strPivotColumn = 각 라인이될 그래프의 instance,strGroupPingCol = x축 값,strValuecol =y축값
        /// </summary>
        /// <param name="dt_Origin"></param>
        /// <param name="dicColumnName"></param>
        /// <param name="strPivotColumn"></param>
        /// <param name="strGroupPingCol"></param>
        /// <param name="strValueCol"></param>
        /// <returns></returns>
        public static DataTable Pivot(DataTable dt_Origin, SortedDictionary<string, string> dicColumnName, string strPivotColumn, string strGroupPingCol, string strValueCol)
        {
            // dictionary 로 컬럼데이터:변경할 컬럼명을 받아서 데이터 테이블생성후 새컬럼을 생성

            SortedDictionary<string, string[]> dicRow = new SortedDictionary<string, string[]>();
            string[] strValue = new string[dicColumnName.Count];
            DataTable dt = new DataTable();
            dt.Columns.Add(strGroupPingCol,typeof(DateTime));
            foreach (KeyValuePair<string, string> col in dicColumnName)
            {
                dt.Columns.Add(col.Value);
            }
            foreach (DataRow dr in dt_Origin.Rows)
            {
                //Timeln 이 잇다면
                if (dicRow.ContainsKey(dr[strGroupPingCol].ToString()))
                {
                    int i = 0;
                    //배열을 가져온다
                    strValue = dicRow[dr[strGroupPingCol].ToString()];
                    //각각의 컬럼키값에 맞으면 배열에 값을넣어준다
                    foreach (KeyValuePair<string, string> kv in dicColumnName)
                    {
                        if (dr[strPivotColumn].ToString() == kv.Key.ToString())
                        {
                            strValue[i] = dr[strValueCol].ToString();
                        }
                        i++;
                    }

                    dicRow[dr[strGroupPingCol].ToString()] = strValue;
                }
                //TimeIn 이 없다면
                else
                {
                    //배열을 가져온다
                    int i = 0;
                    strValue = new string[dicColumnName.Count];
                    foreach (KeyValuePair<string, string> kv in dicColumnName)
                    {
                        if (dr[strPivotColumn].ToString() == kv.Key.ToString())
                        {
                            strValue[i] = dr[strValueCol].ToString();
                        }
                        i++;
                    }
                    dicRow.Add(dr[strGroupPingCol].ToString(), strValue);
                }

            }
            foreach (KeyValuePair<string, string[]> ky in dicRow)
            {
                DataRow dr = dt.NewRow();
                int i = 1;
                dr[0] = ky.Key;
                foreach (string s in ky.Value)
                {
                    dr[i] = Math.Round(Convert.ToDouble(s), 2);
                    i++;
                }

                dt.Rows.Add(dr);
            }
            var dv = dt.DefaultView;
            dv.Sort = strGroupPingCol.ToString() + " asc";
            dt = dv.ToTable();
            return dt;
        }
        public static SortedDictionary<string, string> GetColumn(DataTable dt, string PivotTargetColumnName)
        {
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            var groupDatatable = from tbl in dt.AsEnumerable()
                                 group tbl by new { DatabaseName = tbl[PivotTargetColumnName] } into groupby
                                 select new
                                 {
                                     value = groupby.Key
                                 };

            foreach (var s in groupDatatable)
            {
                dic.Add(s.value.DatabaseName.ToString(), s.value.DatabaseName.ToString());

            }
            return dic;
        }
    }
}
