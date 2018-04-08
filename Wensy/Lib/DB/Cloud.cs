using System;
using System.Data;
using System.Configuration;
using ServicePoint.Lib;
using System.Data.SqlClient;

namespace DB
{
    class Cloud : DB_Base_NoTran
    {
        //string QueryString;

        public void SetCmd(string dbName)
        {
            base.SetCmdCon(dbName);
        }
        public void CloseCon()
        {
            base.Close();
        }

        public int get_MyServerList(int MemberNum)
        {
            string QueryString = "";
            SetCmd("Cloud");

            QueryString = @"select ServerNum from tbservers_member where MemberNum = " + MemberNum;

            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int get_ServerStatus(string ServerNum)
        {
            string QueryString = "";
            SetCmd("Cloud");

            QueryString = @"select ServerNum,HostName,DisplayName,DisplayGroup,RAMSIZE,IPAddress,ServerType,CurrentStatus,WinVer,Processors,TimeIn,TimeIn_UTC,GETUTCDATE() as DB_UTC from tbHostStatus where ServerNum in (" + ServerNum + ")";

            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;
        }
        public int get_ServerList(int MemberNum)
        {
            string QueryString = "";
            SetCmd("Cloud");

            QueryString = @"select ServerNum,HostName,DisplayName,DisplayGroup,RAMSIZE,IPAddress,ServerType,CurrentStatus,WinVer,Processors,TimeIn,TimeIn_UTC,GETUTCDATE() as DB_UTC from tbHostStatus where ServerNum in (select ServerNum from tbservers_member where MemberNum="+MemberNum+")";

            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;

        }
        public int get_AlertMessage(string ServerNum, int numDuration)
        {
            string QueryString;
            SetCmd("Cloud");

            QueryString = @"select TimeIn_UTC, Data_JSON from tbAlerts_JSON where TimeIn_UTC >= DATEADD(MINUTE, -" + numDuration + ", GETUTCDATE()) and ServerNum in (" + ServerNum + ")";
            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;

        }

        public int get_PerfvaluesData_Company(int CompanyNum)
        {
            string QueryString;
            SetCmd("Cloud");

            QueryString = @"select a.TimeIn_UTC, Data_JSON from tbPerfmonValues_JSON as a with(nolock) inner join tbHostStatus as b on a.TimeIn_UTC = b.TimeIn_UTC and a.ServerNum = b.ServerNum where b.CompanyNum = " + CompanyNum;
            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;

        }

        public int get_Dashboard(string ServerNum)
        {
            string QueryString;
            SetCmd("Cloud");

            QueryString = @"select a.ServerNum,a.DisplayName,a.DisplayGroup,a.RAMSIZE,a.IPAddress,a.ServerType,a.CurrentStatus,a.TimeIn,a.TimeIn_UTC,GETUTCDATE() as DB_UTC,
b.P0,b.P1,b.P2,b.P3,b.P4,b.P5,b.P6,b.P7,b.P8,b.P9,b.P10,
b.P11,b.P12,b.P13,b.P14,b.P15,b.P16,b.P17,b.P18,b.P19,
b.P20,b.P21,b.P22,b.P23,b.P24,b.P25,b.P26,b.P27,b.P28,b.P29,
b.P30,b.P31
from tbHostStatus as a
	left outer join tbDashboard as b on a.ServerNum = b.ServerNum and a.TimeIn_UTC = b.TimeIn_UTC
where a.ServerNum in (" + ServerNum + ")"; ;
            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;

        }

        public int get_Dashboard_Chart(string ServerNum)
        {
            string QueryString;
            SetCmd("Cloud");

            QueryString = @"select TimeIn,ServerNum,
P0,P1,P2,P3,P4,P6,P7,P8,P9,
P10,P11,P12,P13,P14,P15,P16,P17,P18,P19,
P20,P21,P22,P26,P27,P28,P29,
P30,P31,P32,P33,P34,P35,P36,P37,P38,P39,
P40,P44,P45,P46,P47,P48,P49,P50,P52
from tbDashboard 
where ServerNum in (" + ServerNum + ") and TimeIn_UTC >= DATEADD(MINUTE, -15, GETUTCDATE())"; ;
            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;

        }
        

        //SQL Procedure Call
        public int W_dashboard(int MemberNum, int CompanyNum, string ServerType)
        {
          
            string QueryString="";
            SetCmd("Cloud");

            if (ServerType == "ALL")
            {
                QueryString = @"select a.ServerNum,a.DisplayName,a.DisplayGroup,a.RAMSIZE,a.IPAddress,a.ServerType,a.CurrentStatus,b.P0,b.P1,b.P2,b.P3,b.P4,b.P7,b.P6,b.P9,b.P10,b.P5,b.P8,a.TimeIn, a.TimeIn_UTC, GETUTCDATE() as DB_UTC
from tbHostStatus as a
	inner join tbServers_Member as c on a.ServerNum = c.ServerNum
	left outer join tbDashboard as b on a.ServerNum = b.ServerNum and a.TimeIn_UTC = b.TimeIn_UTC
where c.MemberNum = " + MemberNum + " and c.CompanyNum = " + CompanyNum + " Order by a.DisplayGroup, a.DisplayName";
            }

            if (ServerType == "Web")
            {
                QueryString = @"select a.ServerNum, a.DisplayName,a.DisplayGroup,a.RAMSIZE,a.IPAddress,a.ServerType,a.CurrentStatus,b.P0,b.P1,b.P2,b.P3,b.P4,b.P7,b.P6,b.P9,b.P10,b.P5,b.P8,b.P11,b.P12,b.P13,b.P14,b.P15,b.P16,b.P17,b.P18,b.P19,a.TimeIn,a.TimeIn_UTC, GETUTCDATE() as DB_UTC
from tbHostStatus as a
inner join tbServers_Member as c on a.ServerNum = c.ServerNum and a.ServerType = '" + ServerType + "'" + 
@" left outer join tbDashboard as b on a.ServerNum = b.ServerNum and a.TimeIn_UTC = b.TimeIn_UTC where c.MemberNum = " + MemberNum  + " and c.CompanyNum = " + CompanyNum + " Order by a.DisplayGroup, a.DisplayName";
            }

            if (ServerType == "SQL")
            {
                QueryString = @"select a.ServerNum, a.DisplayName,a.DisplayGroup,a.RAMSIZE,a.IPAddress,a.ServerType,a.CurrentStatus,b.P0,b.P1,b.P2,b.P3,b.P4,b.P7,b.P6,b.P9,b.P10,b.P5,b.P8,b.P20,b.P21,b.P22,b.P23,b.P24,b.P25,b.P26,b.P27,b.P28,b.P29,b.P30,b.P31,a.TimeIn,a.TimeIn_UTC, GETUTCDATE() as DB_UTC
from tbHostStatus as a
inner join tbServers_Member as c on a.ServerNum = c.ServerNum and a.ServerType = '" + ServerType + "'" +
@" left outer join tbDashboard as b on a.ServerNum = b.ServerNum and a.TimeIn_UTC = b.TimeIn_UTC where c.MemberNum = " + MemberNum + " and c.CompanyNum = " + CompanyNum + " Order by a.DisplayGroup, a.DisplayName";
            }

            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;

        }

        public int w_Dashboard_Chart(int MemberNum, int CompanyNum, int numDuration_Sec, string ServerType)
        {
            string QueryString = "";

            SetCmd("Cloud");

            if (ServerType == "Windows")
            {
                QueryString += "select b.TimeIn,b.ServerNum,b.P0, b.P1, b.P2, b.P3,b.P4,b.P8,b.P6, b.P7,c.DisplayName";

            }

            if (ServerType == "Web")
            {
                QueryString += "select b.TimeIn,b.ServerNum,b.P0, b.P1, b.P2, b.P3,b.P4, b.P8,b.P6, b.P7, P11,P12,P13,P14,P15,P16,P17,P18,P19, c.DisplayName";

            }

            if (ServerType == "SQL")
            {
                QueryString += "select b.TimeIn, b.ServerNum,P0,P1,P2,P3,P8,P9,P10,P45,P4,P22,P21,P32,P26,P33,P27,P31,P29,P30,P28,P35,P36,P34,P39,P38,P37,P46,P47,P44,P40,P20, c.DisplayName";
            }

            QueryString += " from tbDashboard as b inner join tbServers_Member as a on a.ServerNum = b.ServerNum inner join tbHostStatus as c on a.ServerNum = c.ServerNum" +
              " where a.CompanyNum = " + CompanyNum + " and a.MemberNum = " + MemberNum + " and b.TimeIn_UTC >= DATEADD(MINUTE, -" + numDuration_Sec + ", GETUTCDATE()) order by b.TimeIn, b.ServerNum";

            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int w_Dashboard_DiskFreeSpace_Company(int MemberNum, int CompanyNum)
        {
            string QueryString = "";

            SetCmd("Cloud");
            QueryString = "select a.ServerNum, a.TimeIn, PCID, PValue, InstanceName from tbPerfmonValues as a with (nolock) inner join tbHostStatus as b on a.TimeIn_UTC = b.TimeIn_UTC and a.ServerNum = b.ServerNum where b.CompanyNum = " + +CompanyNum + " and PCID in('P164', 'P018', 'P015', 'P190', 'P194')";

            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;
        }
        
        public int get_SQLDatabaseFileSize(int ServerNum)
        {
            SetCmd("Cloud");
            SetQuery("select top 1 TimeIn_UTC, ServerNum, Data_JSON from tbSQLDataBaseFileSize_JSON where ServerNum = " + ServerNum + " order by TimeIn_UTC desc");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int get_SQLSession(int ServerNum)
        {
            SetCmd("Cloud");
            SetQuery("select top 1 TimeIn_UTC, ServerNum, Data_JSON from tbSQLSession_JSON where ServerNum = " + ServerNum + " order by TimeIn_UTC desc");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int get_SQLServiceStatus(int ServerNum)
        {
            SetCmd("Cloud");
            SetQuery("select top 1 TimeIn_UTC, ServerNum, Data_JSON from tbSQLServiceStatus_JSON where ServerNum = " + ServerNum + " order by TimeIn_UTC desc");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int get_SQLAgentFail(int ServerNum)
        {
            SetCmd("Cloud");
            SetQuery("select top 1 TimeIn_UTC, ServerNum, Data_JSON from tbSQLAgentFail_JSON where ServerNum = " + ServerNum + " order by TimeIn_UTC desc");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int get_SQLLinked(int ServerNum)
        {
            SetCmd("Cloud");
            SetQuery("select top 1 TimeIn_UTC, ServerNum, Data_JSON from tbSQLLinked_JSON where ServerNum = " + ServerNum + " order by TimeIn_UTC desc");
            
            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int get_SQLErrorlog(int ServerNum)
        {
            SetCmd("Cloud");
            SetQuery("select top 1 TimeIn_UTC, ServerNum, Data_JSON from tbSQLErrorlog_JSON where ServerNum = " + ServerNum + " order by TimeIn_UTC desc");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int get_SQLIndexflag(int ServerNum)
        {
            SetCmd("Cloud");
            SetQuery("select top 1 TimeIn_UTC, ServerNum, Data_JSON from tbSQLindexflagment_JSON where ServerNum = " + ServerNum + " order by TimeIn_UTC desc");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }
        
        public int w_Dashboard_SQL_Databases(int MemberNum, int CompanyNum, int ServerNum, int numDuration)
        {
            string QueryString = "";

            SetCmd("Cloud");
            QueryString = @"declare @MemberNum int = " + MemberNum + " declare @CompanyNum int = " + CompanyNum + " declare @ServerNum int = " + ServerNum + " declare @Duration int = " + numDuration +
@" declare @CurrentDate datetime 
set @CurrentDate = GETUTCDATE() 
declare @Databases int
declare @DataFileSize_Total float
declare @DataFileSize_Use float
declare @LogFileSize_Total float
declare @LogFileSize_Use float
declare @MaxVLF int
declare @MaxVLF_DB nvarchar(128)
declare @SQLService int
declare @SQLAgent int
declare @SQLLinked int
declare @Session int
declare @ActiveSession int
declare @Errorlog int
declare @IndexFlag int
declare @IndexFlag_Table nvarchar(128)

select
	TimeIn,ServerNum,DatabaseName,Total_Databases_Size_MB,Datafile_Size_MB,Reserved_MB,Reserved_Percent,Unallocated_Space_MB,Unallocated_Percent,Data_MB,
	Data_Percent,Index_MB,Index_Percent,Unused_MB,Unused_Percent,Transaction_Log_Size,Log_Size_MB,Log_Used_Size_MB,Log_Used_Size_Percent,Log_Unused_Size_MB,
	Log_UnUsed_Size_Percent,Avg_vlf_Size,Total_vlf_cnt,Active_vlf_cnt into #Databases
from tbSQLDataBaseFileSize
where ServerNum = @ServerNum and TimeIn_UTC = (select top 1 TimeIn_UTC from tbSQLDataBaseFileSize where ServerNum = @ServerNum order by TimeIn_UTC desc)

set @Databases = (select count(*) from #Databases)

select @DataFileSize_Total = sum(Datafile_Size_MB),@DataFileSize_Use = sum(Data_MB+Index_MB),@LogFileSize_Total = sum(Transaction_Log_Size),@LogFileSize_Use = sum(Log_Used_Size_MB) from #Databases
																																													 z
select top 1 @MaxVLF = Total_vlf_cnt, @MaxVLF_DB = DatabaseName from #Databases order by Total_vlf_cnt desc

select @Session =sum(TotalSession), @ActiveSession = sum(ActiveSession) from tbSQLSession where ServerNum = @ServerNum and TimeIn_UTC = (select top 1 TimeIn_UTC from tbSQLSession where ServerNum = @ServerNum order by TimeIn_UTC desc)

select @SQLService = count(*) from tbSQLServiceStatus_JSON where ServerNum = @ServerNum and process_id is null and TimeIn_UTC = (select top 1 TimeIn_UTC from tbSQLServiceStatus where ServerNum = @ServerNum order by TimeIn_UTC desc)
select @SQLAgent = count(*) from tbSQLAgentFail_JSON where TimeIn_UTC >= DATEADD(minute, -1, @CurrentDate) and ServerNum = @ServerNum
select @SQLLinked = count(*) from tbSQLLinkedCheck where TimeIn_UTC >= DATEADD(minute, -1, @CurrentDate) and ServerNum = @ServerNum
select @Errorlog = count(*) from tbSQLErrorlog_JSON where TimeIn_UTC >= DATEADD(minute, -1, @CurrentDate) and ServerNum = @ServerNum
																																													  
select top 1 @IndexFlag = avg_frag_percent, @IndexFlag_Table = [db_name]+' : '+[object_name]  from tbSQLindexflagment where ServerNum = @ServerNum and TimeIn_UTC = (select top 1 TimeIn_UTC from tbSQLIndexflagment where ServerNum = @ServerNum order by TimeIn_UTC desc) order by avg_frag_percent desc 

select
	@Databases as DatabasesCNT,
	@DataFileSize_Total as DataFileSize_Total,
	@DataFileSize_Use as DataFileSize_Use, 
	@LogFileSize_Total as LogFileSize_Total,
	@LogFileSize_Use as LogFileSize_Use,
	@MaxVLF as MaxVLF,
	@MaxVLF_DB as MaxVLF_DB,
	@SQLService as SQLService,
	@SQLAgent as SQLAgent,
	@SQLLinked as SQLLinked,
	@Session as SessionCNT,
	@ActiveSession as ActiveSession,
	@Errorlog as Errorlog,
	@IndexFlag as IndexFlag,
	@IndexFlag_Table as IndexFlag_Table

drop table #Databases";

            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbCompany_Member_List(int CompanyNum)
        {
            SetCmd("Cloud");
            SetQuery("select MemberNum,Email,Name as MemberName,Grade from tbMember where CompanyNum = " + CompanyNum);

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int ServicePointLogin(string strEmail, string strPwd)
        {
            SetCmd("Cloud");
            SetQuery("select MemberNum, Name as MemberName, CompanyNum, Grade, CompanyName from tbMember where Email = '" + strEmail + "' and UserPWD = '" + strPwd + "'");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbCompany_Member_Add(int CompanyNum, string strEmail, string strPass, string strMemberName, string strCompanyName, int numGrade)
        {
           
            SetCmd("Cloud");
            SetQuery(@"declare @Result int
declare @MemberNum int
set @MemberNum = (select isnull(max(MemberNum), 0) from tbMember) + 1

if exists (select * from dbo.tbMember where Email = '" + strEmail + "')" +
@"begin
	select -2 as Result
	return 
end
insert into dbo.tbMember (MemberNum, Email, UserPWD, Name, RegDate, CompanyName,CompanyNum, Grade) values (@MemberNum, '" + strEmail + "', '" + strPass + "', '" + strMemberName + "', getdate(), '" + strCompanyName + "', " + CompanyNum + ", " + numGrade + ") select 1 as Result");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_CheckUserEmail(string strEmail)
        {
            SetCmd("Cloud");
            SetQuery("select Email from tbMember where Email = '" + strEmail + "'");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbCompany_Member_Update(int MemberNum, int CompanyNum, int MemberNum_Target, int numGrade)
        {
            SetCmd("Cloud");
            SetQuery("update tbMember set Grade = " + numGrade + " where MemberNum = " + MemberNum_Target + " select 1 as result");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbCompany_Member_Del(int MemberNum, int CompanyNum, int MemberNum_Target)
        {
            SetCmd("Cloud");
            SetQuery(@"declare @MemberNum int = " + MemberNum + " declare @CompanyNum int = " + CompanyNum + " declare @MemberNum_Target int = " + MemberNum_Target + 
@" declare @Result int declare @Grade int declare @Grade_Target int 
 if @MemberNum = @MemberNum_Target begin select - 4 as result return end
 select @Grade = Grade from tbMember where MemberNum = @MemberNum
 select @Grade_Target = Grade from tbMember where MemberNum = @MemberNum_Target
 if @Grade_Target = 9 begin select - 3 as result return end
 if @Grade < 8 begin select - 3 as result return end
 update tbMember set Email = '####' + Email, CompanyNum = 0 where CompanyNum = @CompanyNum and MemberNum = @MemberNum_Target select 1 result");

            dsReturn = ExecuteDataSet();
            return nReturn;


        }

        public int m_tbServers_List(int CompanyNum)
        {
          
            SetCmd("Cloud");
            SetQuery("select ServerNum,DisplayName,DisplayGroup,ServerType,RegDate,ProductKey,RegionCode,AgentVer from tbHostStatus where CompanyNum = " + CompanyNum );

            dsReturn = ExecuteDataSet();
            return nReturn;

        }

        public int m_tbServers_Update(int MemberNum, int CompanyNum, string strDisplayName, string strDisplayGroup, int numServer, string strlanguage)
        {
           
            string QueryString = "";
            SetCmd("Cloud");
            QueryString = @"declare @MemberNum int = " + MemberNum + " declare @CompanyNum int = " + CompanyNum + " declare @DisplayName nvarchar(64) = '" + strDisplayName + "' declare @DisplayGroup nvarchar(64) = '" + strDisplayGroup + "' declare @ServerNum int = " + numServer + " declare @RegionCode nvarchar(10) = '" + strlanguage + "'" +
@" declare @ServerType nvarchar(50) declare @ProductKey nvarchar(64) declare @DisplayNameCheck nvarchar(50) declare @DisplayGroupCheck nvarchar(50) declare @RegionCode_Old nvarchar(10)
select @DisplayNameCheck = DisplayName, @DisplayGroupCheck = DisplayGroup, @ServerType = ServerType, @ProductKey = ProductKey , @RegionCode_Old = RegionCode from tbHostStatus where ServerNum = @ServerNum
if @DisplayName = @DisplayNameCheck and @DisplayGroup = @DisplayGroupCheck begin if exists (select * from tbHostStatus where CompanyNum = @CompanyNum and DisplayName = @DisplayName and ServerNum <> @ServerNum) begin select -9 as Result return end end
update tbHostStatus set DisplayName = @DisplayName, DisplayGroup = @DisplayGroup, RegionCode = @RegionCode where ServerNum = @ServerNum 
if @RegionCode_Old <> @RegionCode begin update tbAlertRules_Server set ReasonCodeDesc = b.ReasonCodeDesc from tbAlertRules_Server as a inner join tbAlertRules as b on a.ReasonCode = b.ReasonCode where a.ServerNum = @ServerNum and b.RegionCode = @RegionCode end  select 1 as Result";

            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbServers_Del(int MemberNum, int CompanyNum, int numServer)
        {
          
            SetCmd("Cloud");
            SetQuery("delete tbHostStatus where ServerNum = " + numServer + 
" delete tbAlertRules_Server where ServerNum = " + numServer +
" delete tbPCID_Server where ServerNum = " + numServer +
" delete tbSQLQueryDefinition_Server where ServerNum = " + numServer +
" delete tbServers_Member where ServerNum = " + numServer +
" delete tbSQLConfiguration_Server where ServerNum = " + numServer +
" delete tbSQLServerInfo where ServerNum = " + numServer);

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbServers_Add(int MemberNum, int CompanyNum, string strDisplayName, string strDisplayGroup, string strServerType, string strlanguage)
        {
           
            string @QueryString = "";
            string strProductKey = Util.EncryptText(Util.getDateTime(DateTime.Now).ToString() + "_#_" + MemberNum);
            SetCmd("Cloud");
            QueryString = @"declare @MemberNum int = " + MemberNum + " declare @CompanyNum int = " + CompanyNum + " declare @DisplayName nvarchar(64) ='" + strDisplayName + "' declare @DisplayGroup nvarchar(64) = '" + strDisplayGroup + "' declare @ServerType nvarchar(30) = '" + strServerType + "' declare @RegionCode nvarchar(10) = '" + strlanguage + "' declare @ProductKey nvarchar(128) = '" + strProductKey + "'" +
@" declare @ServerNum int 
 set @ServerNum =  (select isnull(Max(ServerNum), 0) from tbHostStatus) + 1
 declare @S_type table(ServerType nvarchar(50))
 insert into @S_type values ('Windows')
 if @ServerType <> 'Windows' begin insert into @S_type values (@ServerType) end
 if exists (select * from tbHostStatus where CompanyNum = @CompanyNum and DisplayName = @DisplayName) begin select -9 as Result return end
 if len(@DisplayName) = 0 or len(@DisplayGroup) = 0 begin select -8 as Result return end
 insert into tbHostStatus (ServerNum, RegDate, ServerType, DisplayName, DisplayGroup, CompanyNum, ProductKey, RegionCode) values (@ServerNum, getdate(), @ServerType, @DisplayName, @DisplayGroup, @CompanyNum, @ProductKey, @RegionCode)
 insert into tbServers_Member(CompanyNum, MemberNum, ServerNum) values (@CompanyNum, @MemberNum, @ServerNum)
 insert into tbPCID_Server (ServerNum ,PCID, ServerType, PObjectName, PCounterName, HasInstance, ValueDescription, RValueDescription, used) 
 select @ServerNum, PCID, a.ServerType, a.PObjectName, a.PCounterName, a.HasInstance, a.ValueDescription, a.RValueDescription, 1 from tbPCID as a inner join @S_type as b on a.ServerType = b.ServerType order by PCID
 insert into tbPInstance_Server (ServerNum, PCID, InstanceName, IfContains) select @ServerNum, b.PCID, b.InstanceName, b.IfContains from dbo.tbPCID as a inner join tbPInstance as b on a.PCID = b.PCID inner join @S_type as c on a.ServerType = c.ServerType order by PCID
 insert into tbAlertRules_Server (ServerNum, ServerType, ReasonCode, PCID, Threshold, TOperator, ReasonCodeDesc, InstanceName, Duration, HasReference, RecordApps, IsEnabled, AlertLevel ,RefDescription, ReqActionCode, MobileAlert) select @ServerNum, a.ServerType, a.ReasonCode, a.PCID, a.Threshold, a.TOperator, a.ReasonCodeDesc, a.InstanceName, a.Duration, a.HasReference, a.RecordApps, a.IsEnabled, a.AlertLevel, a.RefDescription, a.ReqActionCode, a.MobileAlert from tbAlertRules as a inner join @S_type as b on a.ServerType =b.ServerType where a.RegionCode = @RegionCode order by ReasonCode
 if @ServerType = 'SQL'
 begin
	insert into tbSQLQueryDefinition_Server (ServerNum, QueryID, Interval, DestinationTable, Query, Enabled, QueryDescription) select @ServerNum, QueryID, Interval, DestinationTable, Query, Enabled, QueryDescription from tbSQLQueryDefinition
	insert into tbSQLServerInfo (ServerNum) values (@ServerNum)
end
select 1 as Result";

            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;

        }

        public int m_tbServers_Member_List(int CompanyNum, int numServer)
        {
          

            SetCmd("Cloud");
            SetQuery("select a.ServerNum,a.MemberNum,b.Name as MemberName,b.Email from tbServers_Member as a inner join tbMember as b on a.CompanyNum = b.CompanyNum and a.MemberNum = b.MemberNum where a.CompanyNum = " + CompanyNum + " and a.ServerNum = " + numServer);

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbServers_Member_List_NotExist(int CompanyNum, int numServer)
        {
           

            SetCmd("Cloud");
            SetQuery(@"select
	y.ServerNum,
	y.MemberNum,
	y.MemberName
from
(
select
	CompanyNum,
	MemberNum
from tbServers_Member
where CompanyNum = " + CompanyNum + " and ServerNum = " + numServer + 
@")as x
	right join 
(
select
	a.ServerNum as ServerNum,
	b.MemberNum,
	b.name as MemberName
from tbHostStatus as a
	inner join tbMember as b on a.CompanyNum = b.CompanyNum
where a.CompanyNum = " + CompanyNum + " and a.ServerNum = " + numServer + 
@") as y on x.MemberNum = y.MemberNum
where x.MemberNum is null");

            dsReturn = ExecuteDataSet();
            return nReturn;


        }

        public int m_tbServers_Member_Add(int CompanyNum, int numServer, int MemberNum)
        {
            
            SetCmd("Cloud");
            SetQuery("insert into tbServers_Member(CompanyNum, MemberNum, ServerNum) values (" + CompanyNum + ", " + MemberNum + ", " + numServer + ")");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }


        public int m_tbServers_Member_Del(int CompanyNum, int numServer, int MemberNum)
        {
           
            SetCmd("Cloud");
            SetQuery("delete tbServers_Member where CompanyNum = " + CompanyNum + " and MemberNum = " + MemberNum + " and ServerNum = " + numServer);

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbAlertRules_Server_List_AlertLevel(int numServer)
        {
          

            SetCmd("Cloud");
            SetQuery(@"select a.ReasonCode,a.ReasonCodeDesc,a.Threshold,a.Duration,a.InstanceName,a.RecordApps,a.IsEnabled,a.AlertLevel,b.PObjectName,b.PCounterName,a.MobileAlert from tbAlertRules_Server as a left outer JOIN tbPCID_Server as b ON a.ServerNum = b.ServerNum and a.PCID = b.PCID where a.ServerNum = " + numServer);

            dsReturn = ExecuteDataSet();
            return nReturn;


        }

        public DataSet m_tbAlertRules_Server_List_AlertLevel_ods(int numServer)
        {

            m_tbAlertRules_Server_List_AlertLevel(numServer);
            return dsReturn;
        }

        public int m_tbAlertRules_Server_Update(int numServer, string strReasonCoed, double dblThreshod, string strInstanceName, int numDuration, bool bolIsEnabled, string strAlertLevel, bool bolRecordApps, string strAlertDescription, string strMobileAlert)
        {
           

            SetCmd("Cloud");
            SetQuery("UPDATE tbAlertRules_Server SET Threshold = " + dblThreshod + ", InstanceName = '" + strInstanceName + "', Duration = " + numDuration + ", IsEnabled = '" + bolIsEnabled + "', AlertLevel = '" + strAlertLevel + "', RecordApps = '" + bolRecordApps + "', ReasonCodeDesc = '" + strAlertDescription + "', MobileAlert = '" + strMobileAlert + "' WHERE ServerNum = " + numServer + " and ReasonCode = '" + strReasonCoed + "'");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbAlertOptions_Update(int CompanyNum, int numPushInterval, int numPushMaxOccurs, int numPushResetInterval, bool bolUsePushAlert)
        {
          

            SetCmd("Cloud");
            SetQuery("update tbAlertOptions  set PushInterval = " + numPushInterval + ", PushMaxOccurs = " + numPushMaxOccurs + ", PushResetInterval = " + numPushResetInterval + ", UsePushAlert = " + bolUsePushAlert + " where CompanyNum = " + CompanyNum);

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbAlertOptions_List(int CompanyNum)
        {
            

            SetCmd("Cloud");
            //SetQuery("select CompanyNum, PushInterval, PushMaxOccurs, PushResetInterval, UsePushAlert from tbAlertOptions where CompanyNum = " + CompanyNum);
            SetQuery("select 1 as CompanyNum, 2 as PushInterval, 3 as PushMaxOccurs, 4 as PushResetInterval, 5 as UsePushAlert");
            dsReturn = ExecuteDataSet();
            return nReturn;

        }

        public int m_tbPCID_Server_PCounterName_List(int ServerNum, string strPObjectname)
        {
          
            SetCmd("Cloud");
            SetQuery("select Servernum,PCID,PCounterName,HasInstance,ValueDescription,RValueDescription,used from tbPCID_Server where ServerNum = " + ServerNum + " and PObjectName = '" + strPObjectname + "' order by PCounterName");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbPInstance_Server_PInstance_List(int ServerNum, string strPCID)
        {
           

            SetCmd("Cloud");
            SetQuery("select Servernum,PCID,InstanceName,IfContains from tbPInstance_Server where ServerNum = " + ServerNum + " and PCID = '" + strPCID + "' order by InstanceName");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbPInstance_Server_Add(int ServerNum, string strPCID, string strInstanceName, bool bolIfContains)
        {
          
            string QueryString = "";
            SetCmd("Cloud");
            QueryString = @"update tbPInstance_Server set IfContains = '" + bolIfContains + "' where ServerNum = " + ServerNum + " and PCID = '" + strPCID + "' and InstanceName = '" + strInstanceName + "'" +
@"if @@ROWCOUNT = 0 begin insert into tbPInstance_Server  (ServerNum, PCID, InstanceName, IfContains) values (" + ServerNum + ", '" + strPCID + "', '" + strInstanceName + "', '" + bolIfContains + "') end select 1 as result";

            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbPInstance_Server_Del(int ServerNum, string strPCID, string strInstanceName)
        {
           

            SetCmd("Cloud");
            SetQuery("delete tbPInstance_Server where ServerNum = " + ServerNum + " and PCID = '" + strPCID + "' and InstanceName = '" + strInstanceName + "'");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbPCID_Server_PObject_List(int ServerNum)
        {
           

            SetCmd("Cloud");
            SetQuery("select ServerNum,ServerType,PObjectName from tbPCID_Server where ServerNum = " + ServerNum + " group by ServerNum, ServerType, PObjectName order by PObjectName");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbPCID_Server_PObject_List(int ServerNum, string strPObjectName)
        {
           

            SetCmd("Cloud");
            SetQuery("select Servernum,PCID,PCounterName,HasInstance,ValueDescription,RValueDescription,used from tbPCID_Server where ServerNum = " + ServerNum + " and PObjectName = '" + strPObjectName + "' order by PCounterName");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbAlertRules_Server_List(int numServer, string strPCID)
        {
           

            SetCmd("Cloud");
            SetQuery("select ServerNum,ServerType,ReasonCode,PCID,Threshold,TOperator,ReasonCodeDesc,InstanceName,Duration,HasReference,RecordApps,IsEnabled,AlertLevel,RefDescription,ReqActionCode,MobileAlert from tbAlertRules_Server where ServerNum = " + numServer + " and PCID = '" + strPCID + "' order by PCID");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbMember_Add(string strEmail, string strPass, string strName, string strCompanyName, int numGrade)
        {
           

            string QueryString = "";

            SetCmd("Cloud");

            QueryString = @"declare @Email nvarchar(100) = '" + strEmail + "' declare @UserPWD nvarchar(100) = '" + strPass + "' declare @Name nvarchar(50) = '" + strName + "' declare @CompanyName nvarchar(50) = '" + strCompanyName + "' declare @Grade int = " + numGrade +
@" declare @MemberNum int declare @CompanyNum int set @MemberNum = (select isnull(max(membernum), 0) from tbMember) + 1
if exists(select * from dbo.tbMember where Email = @Email) begin select - 2 as Result return end
insert into dbo.tbMember(MemberNum, Email, UserPWD, Name, RegDate, CompanyName, CompanyNum, Grade) values(@MemberNum, @Email, @UserPWD, @Name, getdate(), @CompanyName, @MemberNum, @Grade)
insert into tbAlertOptions(CompanyNum, PushInterval, PushMaxOccurs, PushResetInterval, UsePushAlert) values(@MemberNum, 2, 3, 60, 1) select 1 as Result";

            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbMember_Update(int MemberNum, string strName, string strPass, string strPass_New, string strHp, string strCompanyName)
        {
           

            SetCmd("Cloud");
            SetQuery("update tbMember set  Name = '" + strName + "', UserPWD = '" + strPass_New + "', HP = '" + strHp + "', CompanyName = '" + strCompanyName + "' where MemberNum = " + MemberNum + " and UserPWD = '" + strPass + "'");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_Profile_Info(int MemberNum)
        {
            

            SetCmd("Cloud");
            SetQuery("select Email, Name, RegDate, HP, CompanyName, CompanyNum from tbMember where MemberNum = " + MemberNum);

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_ServerList(int MemberNum, string strServerType)
        {
            

            SetCmd("Cloud");

            if(strServerType != "ALL")
            {
                SetQuery("select b.DisplayName,b.ServerNum from tbServers_Member as a inner join tbHostStatus as b on a.ServerNum = b.ServerNum where a.MemberNum = " + MemberNum + " and b.ServerType = '" + strServerType + "'");
            }

            if (strServerType == "ALL")
            {
                SetQuery("select b.DisplayName,b.ServerNum from tbServers_Member as a inner join tbHostStatus as b on a.ServerNum = b.ServerNum where a.MemberNum = " + MemberNum + "");
            }

            

            dsReturn = ExecuteDataSet();
            return nReturn;

        }

        public int R_HostInfo(int ServerNum)
        {
            
            SetCmd("Cloud");
            SetQuery("select DisplayName,Winver,IPAddress,RAMSize,HostName,ServerType from tbHostStatus where ServerNum = " + ServerNum + "");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_WEB_Report(int ServerNum, DateTime dtmStart, DateTime dtmEnd)
        {
            

            SetCmd("Cloud");
            SetQuery("select TimeIn,P0,P1,P2,P3,P4,P5,P6,P7,P8,P9,P10,P11,P12,P13,P14,P15,P16,P17,P18,P19 from tbDashboard where Timein_UTC >= '" + dtmStart + "' and Timein_UTC < '" + dtmEnd + "' and ServerNum = " + ServerNum);

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_PCID_Instance(int ServerNum, DateTime dtmStart, DateTime dtmEnd, string strPCID)
        {
          

            SetCmd("Cloud");
            SetQuery("select TImeIn,PCID,PValue,InstanceName from tbPerfmonValues where TimeIn_UTC >= '" + dtmStart + "' and TimeIn_UTC < '" + dtmEnd + "' and ServerNum = " + ServerNum + " and PCID = '" + strPCID + "'");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_WEB_TimeTaken(int ServerNum, DateTime dtmStart, DateTime dtmEnd)
        {
            
            SetCmd("Cloud");
            SetQuery("SELECT TOP(20) URI, AVG(AvgTimeTaken) AS [Average Time Taken], MAX(MaxTimeTaken) AS [Max Time Taken] FROM tbIISLog WHERE TimeIn >= '" + dtmStart + "' and TimeIn <= '" + dtmEnd + "' and ServerNum = " + ServerNum + " GROUP BY URI ORDER BY [Average Time Taken] DESC");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_WEB_Byte(int ServerNum, DateTime dtmStart, DateTime dtmEnd)
        {
            

            SetCmd("Cloud");
            SetQuery("SELECT TOP(20) URI,SUM(CAST(SCBytes AS float)) AS [Total Bytes from Server],SUM(Hits) AS [Total Hits] FROM tbIISLog WHERE TimeIn >= '" + dtmStart + "' and TimeIn <= '" + dtmEnd + "' and ServerNum = " + ServerNum + " GROUP BY URI ORDER BY [Total Bytes from Server] DESC");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_WEB_RequestStatus(int ServerNum, DateTime dtmStart, DateTime dtmEnd)
        {
          

            SetCmd("Cloud");
            SetQuery("SELECT ValueDescription, LogValue, SUM(TotalNumber) AS [Total] FROM tbIISRequestStatus WHERE TimeIn >= '" + dtmStart + "' AND TimeIn <= '" + dtmEnd + "' AND ServerNum = " + ServerNum + " GROUP BY ValueDescription, LogValue ORDER BY ValueDescription, Total DESC");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_WEB_Errors(int ServerNum, DateTime dtmStart, DateTime dtmEnd)
        {
           

            SetCmd("Cloud");
            SetQuery("SELECT TOP(20) URI,SUM(Hits) AS [Total Hits],StatusCode as [Status Code],Win32StatusCode as [Win32 Status Code] FROM tbIISLog WHERE TimeIn >= '" + dtmStart + "' AND TimeIn <= '" + dtmEnd + "' AND ServerNum = " + ServerNum + " and StatusCode >= 400 GROUP BY URI, StatusCode, Win32StatusCode ORDER BY [Total Hits]DESC");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_WEB_ServiceStatus(int ServerNum, DateTime dtmStart, DateTime dtmEnd)
        {
           
            SetCmd("Cloud");
            SetQuery("SELECT SUM(TotalHits) AS [Total Hits], SUM(TotalSCBytes) AS [Total Bytes from Server], SUM(TotalCSBytes) AS [Total Bytes from Clients],SUM(TotalCIP) AS [Total Client IP], SUM(TotalErrors) AS [Total Errors] FROM tbIISServiceStatus WHERE TimeIn >= '" + dtmStart + "' AND TimeIn <= '" + dtmEnd + "' AND ServerNum = " + ServerNum);

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_SQL_Performance(int ServerNum, DateTime dtmStart, DateTime dtmEnd)
        {
           
            string QueryString = "";
            SetCmd("Cloud");
            QueryString = @"select TimeIn,PCID,PValue from tbPerfmonValues where Timein_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein_UTC <= '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum + " and PCID = 'P001' and InstanceName = '_Total' union all" +
                @" select TimeIn,PCID,RValue as PValue from tbPerfmonValues where Timein_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein_UTC <= '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum + " and PCID = 'P006' and InstanceName = 'sqlserver' union all" +
                @" select TimeIn,PCID,PValue from tbPerfmonValues where Timein_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein_UTC <= '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum + " and PCID in (" +
    @" 'P139', 'P004', 'P007', 'P008',
    'P170', 'P171', 'P100', 'P101', 'P081',
    'P138', 'P106', 'P107', 'P108', 'P084',
    'P099', 'P098', 'P168',
    'P178', 'P180', 'P006', 'P075', 'P077',
    'P078', 'P079', 'P082', 'P083', 'P085',
    'P086', 'P087', 'P088', 'P096', 'P097',
    'P173', 'P104', 'P105', 'P172', 'P175')";

            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_SQL_CPU(int ServerNum, DateTime dtmStart, DateTime dtmEnd)
        {
           

            SetCmd("Cloud");
            SetQuery(@"select TImeIn,PCID,Pvalue from tbPerfmonValues where Timein_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum + " and PCID = 'P001' and InstanceName = '_Total' union all" + 
@" select TImeIn,PCID,Pvalue from tbPerfmonValues where Timein_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum + " and PCID = 'P139' union all" + 
@" select TImeIn,PCID,Pvalue from tbPerfmonValues where Timein_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum + " and PCID in ('P004', 'P107', 'P108', 'P106', 'P184')");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_SQL_CPU_Query(int ServerNum, DateTime dtmStart, DateTime dtmEnd)
        {
          
            SetCmd("Cloud");
            SetQuery("select a.login_name,a.db_name,b.cpu_time,a.full_query_text from tbSQLCurrentExecution as a with (nolock) inner join  (select top 20 TimeIn_UTC,servernum, cpu_time from tbSQLCurrentExecution where Timein_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and Timein_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and ServerNum = " + ServerNum + " order by cpu_time desc) as b on a.TimeIn_UTC = b.TimeIn_UTC and a.ServerNum = b.ServerNum order by b.cpu_time desc");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_PCID(int ServerNum, DateTime dtmStart, DateTime dtmEnd, string strPCID)
        {
          

            SetCmd("Cloud");
            SetQuery(@"select a.TimeIn,a.PCID,a.PValue,b.PCounterName 
from tbPerfmonValues as a 
	inner join tbPCID_Server as b on a.PCID = b.PCID and a.ServerNum = b.ServerNum
where a.TimeIn_UTC >= '" + dtmStart.ToString("yyyy-MM-dd HH:mm:ss") + "' and a.TimeIn_UTC < '" + dtmEnd.ToString("yyyy-MM-dd HH:mm:ss") + "' and b.ServerNum = " + ServerNum + " and b.PCID in(" + strPCID + ") order by a.TimeIn_UTC, b.PCounterName");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int W_SQLConfiguration(int ServerNum)
        {
          
            SetCmd("Cloud");
            SetQuery(@"set nocount on
set transaction isolation level read uncommitted  

select
	Name
	,convert(nvarchar(50),Value) as Value
	,convert(nvarchar(50),Minimum) as Minimum
	,convert(nvarchar(50),Maximum) as Maximum
	,convert(nvarchar(50),Value_in_use) as Value_in_use
	,description
	,convert(nvarchar(50),is_dynamic) as is_dynamic
	,convert(nvarchar(50),is_advanced) as is_advanced
from tbSQLConfiguration_Server
where ServerNum = " + ServerNum + " order by Name");

            dsReturn = ExecuteDataSet();
            return nReturn;


        }

        public int w_PCID_Instance(int ServerNum, int numDuration, string strPCID)
        {
          

            SetCmd("Cloud");
            SetQuery("select TImeIn,PCID,PValue,InstanceName from tbPerfmonValues where TimeIn_UTC >= dateadd(MINUTE, -" + numDuration + ", GETUTCDATE()) and ServerNum = " + ServerNum + " and PCID = '" + strPCID + "' order by TimeIn_UTC, InstanceName");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int w_SQLDatabasesDetail(int ServerNum)
        {
          
            SetCmd("Cloud");
            SetQuery(@"select top 1 TimeIn,database_name,create_date,compatibility_level,collation_name,user_access_desc,is_read_only,is_auto_shrink_on,state_desc,is_in_standby,snapshot_isolation_state_desc
	,is_read_committed_snapshot_on,recovery_model_desc,page_verify_option_desc,is_auto_create_stats_on,is_auto_update_stats_on,is_auto_update_stats_async_on,is_fulltext_enabled,is_trustworthy_on,is_parameterization_forced
	,is_db_chaining_on,is_broker_enabled,is_published,is_subscribed,is_merge_published,is_distributor from tbSQLDatabase with (nolock) where ServerNum = " + ServerNum + "	order by TimeIn_UTC desc");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int R_Windows_Perfmon(int ServerNum, DateTime dtmStart,DateTime dtmEnd, string strPCID,string strInstanceName)
        {
          

            SetCmd("Cloud");

            if (strPCID == "")
            {
                SetQuery(@"select TImeIn,InstanceName,PValue from tbPerfmonValues where Timein_UTC >= '" + dtmStart + "' and Timein_UTC < '" + dtmEnd + "' and ServerNum = " + ServerNum + " and PCID  = '" + strPCID + "'");
            }

            if (strPCID != "")
            {
                SetQuery(@"select TImeIn,InstanceName,PValue from tbPerfmonValues where Timein_UTC >= '" + dtmStart + "' and Timein_UTC < '" + dtmEnd + "' and ServerNum = " + ServerNum + " and PCID  = '" + strPCID + "' and InstanceName = '" + strInstanceName + "'");
            }
            

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int m_tbPCID_Server_Counter_List(int ServerNum)
        {
          
            SetCmd("Cloud");
            SetQuery("select a.ServerNum,a.PCID,a.PObjectName,a.PCounterName,b.InstanceName,a.HasInstance from tbPCID_Server as a left outer join tbPInstance_Server as b on a.ServerNum = b.ServerNum and a.PCID = b.PCID where a.ServerNum = " + ServerNum + " order by a.PObjectName, a.PCounterName, b.InstanceName");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }
        public int w_CPUUsage_Detail(int ServerNum)
        {
         

            SetCmd("Cloud");
            SetQuery("select top 10 InstanceName ,RValue as PValue from tbPerfmonValues as a inner join tbHoststatus as b on a.servernum = b.servernum and a.TimeIn_UTC = b.TimeIn_UTC where b.ServerNum = " + ServerNum + " and PCID = 'P006' order by PValue desc");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int w_CPUUsage(int ServerNum,int numDuration)
        {
          
            SetCmd("Cloud");
            SetQuery(@"select TimeIn,P0 as TotalCPU,P1 as KernelCPU,case when  P0 > P1 then P0-P1 else 0 end as UserCPU,P2 as PQL from tbDashboard where TimeIn_UTC >= dateadd(minute, " + -numDuration + ", GETUTCDATE()) and ServerNum = " + ServerNum + " order by TimeIn_UTC");

            dsReturn = ExecuteDataSet();
            return nReturn;

        }

        public int w_SQLMemory(int ServerNum, int numDuration)
        {
           

            SetCmd("Cloud");
            //SetQuery(@"select TimeIn, P100, P101, P081, P090, P182, P183, P180, P178, P168, P177 from tbSQLMemory with (nolock) where ServerNum = " + ServerNum + " and TimeIn_UTC >= DATEADD(Minute, "+ -numDuration + ", GETUTCDATE()) order by TimeIn_UTC");
            SetQuery(@"select TimeIn, PCID, InstanceName, PValue from tbPerfmonValues with (nolock) where ServerNum = " + ServerNum + " and TimeIn_UTC >= DATEADD(Minute, " + -numDuration + ", GETUTCDATE()) and PCID in ('P100', 'P101', 'P081', 'P090', 'P182', 'P183', 'P180', 'P178', 'P168', 'P177') order by TimeIn_UTC");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int w_MemoryUsage_Detail(int ServerNum)
        {
            

            SetCmd("Cloud");
            SetQuery(@"select top 10 InstanceName, PValue from tbPerfmonValues as a with (nolock) inner join tbHoststatus as b with (nolock) on a.TimeIn_UTC = b.TimeIn_UTC and a.ServerNum = b.ServerNum where a.ServerNum = " + ServerNum + " and PCID = 'P013' order by PValue desc");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int w_MemoryUsage(int ServerNum, int numDuration)
        {
            

            SetCmd("Cloud");
            SetQuery("select TimeIn, P4, P3 from tbDashboard where TimeIn_UTC >= dateadd(minute, -" + numDuration + ", GETUTCDATE()) and ServerNum = " + ServerNum);

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int w_SQLActiveSession(int ServerNum)
        {
            

            SetCmd("Cloud");
            SetQuery(@"select TimeIn,Login_Name,Host_Name,Client_Net_Address,TotalSession,ActiveSession from tbSQLSession where ServerNum = " + ServerNum + "	and TimeIn_UTC = (select top 1 TimeIn_UTC from tbSQLSession where ServerNum = " + ServerNum + " order by TimeIn_UTC desc)");

            dsReturn = ExecuteDataSet();
            return nReturn;

        }

        public int w_CurrentExecution_List(int ServerNum)
        {
           

            SetCmd("Cloud");
            SetQuery(@"SELECT TimeIn,db_name,command,cpu_time,total_elapsed_time,logical_reads,reads,writes,blocking_session_id,wait_type,wait_time,wait_resource,full_query_text FROM tbSQLCurrentExecution
where ServerNum = " + ServerNum + " and TimeIn_UTC = (select top 1 TimeIn_UTC from tbSQLSession where ServerNum = " + ServerNum + " order by TimeIn_UTC desc)");

            dsReturn = ExecuteDataSet();
            return nReturn;

        }

        public int w_SQLDatabasesFileSize(int ServerNum, int numDuration)
        {
           

            SetCmd("Cloud");
            SetQuery(@"set nocount on
set transaction isolation level read uncommitted  

select 
	TimeIn,
	DatabaseName,
	Total_Databases_Size_MB,
	Datafile_Size_MB,
	Reserved_MB,
	Reserved_Percent,
	Unallocated_Space_MB,
	Unallocated_Percent,
	Data_MB,
	Data_Percent,
	Index_MB,
	Index_Percent,
	Unused_MB,
	Unused_Percent,
	Transaction_Log_Size,
	Log_Size_MB,
	Log_Used_Size_MB,
	Log_Used_Size_Percent,
	Log_Unused_Size_MB,
	Log_UnUsed_Size_Percent,
	Avg_vlf_Size,
	Total_vlf_cnt,
	Active_vlf_cnt
from tbSQLDataBaseFileSize
where TimeIn_UTC >= dateadd(MINUTE, " + -numDuration + ", GETUTCDATE())	and ServerNum = " + ServerNum + " order by TimeIn_UTC, DatabaseName");

            dsReturn = ExecuteDataSet();
            return nReturn;

        }
        public int w_SQLServiceStatus(int ServerNum)
        {
          

            SetCmd("Cloud");
            SetQuery(@"select TimeIn,servicename,process_id,startup_type_desc,status_desc,last_startup_time,service_account,is_clustered,cluster_nodename,filename from tbSQLServiceStatus where ServerNum = " + ServerNum + "	and TimeIn_UTC = (select top 1 TimeIn_UTC from tbSQLServiceStatus where ServerNum = " + ServerNum + " order by TimeIn_UTC desc)");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int w_SQLIndexFlagment(int ServerNum)
        {
            
            SetCmd("Cloud");
            SetQuery(@"
select * 
from
(
select 
	TimeIn
	,db_name
	,object_name
	,index_name
	,index_type
	,alloc_unit_type
	,index_depth
	,index_level
	,avg_frag_percent
	,fragment_count
	,avg_frag_size_in_page
	,page_count
	,ROW_NUMBER() over (partition by db_name order by avg_frag_percent desc ) as Num
from tbSQLindexflagment 
where ServerNum = " + ServerNum + " and TimeIn_UTC = (select top 1 TimeIn_UTC from tbSQLindexflagment where ServerNum = " + ServerNum + " order by TimeIn_UTC desc)) as x where Num < 10 order by [db_name], Num desc");

            dsReturn = ExecuteDataSet();
            return nReturn;

        }
        public int w_SQLLinkedCheck(int ServerNum)
        {
           
            SetCmd("Cloud");
            SetQuery(@"select TimeIn, LinkedName from tbSQLLinkedCheck where ServerNum = " + ServerNum + " and TimeIn_UTC > DATEADD(minute, -1, GETUTCDATE())");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }
        public int w_SQLAgentFail(int ServerNum)
        {
          

            SetCmd("Cloud");
            SetQuery(@"select TimeIn,JOB_NAME,RUN_REQUESTED_DATE,LAST_EXECUTED_STEP_ID,JOB_HISTORY_ID,MESSAGE,STEP_NAME,COMMAND from tbSQLAgentFail where ServerNum = " + ServerNum + " and TimeIn_UTC > DATEADD(minute, -1, GETUTCDATE())");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }
        public int w_SQLErrorlog(int ServerNum)
        {
           

            SetCmd("Cloud");
            SetQuery(@"select LogDate, ProcessInfo, ErrorText from tbSQLErrorlog where ServerNum = " + ServerNum + " and TimeIn_UTC >= dateadd (MINUTE, -1, GETUTCDATE()) order by LogDate");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }
        public int w_tbAppTrace(int ServerNum, DateTime dtm_UTC)
        {
            
            string QueryString;

            SetCmd("Cloud");
            string convertDt = dtm_UTC.ToString("yyyy-MM-dd HH:mm:ss");
            QueryString = "SELECT URI, ClientLocation, RunningTime, TimeIn, ReasonCode FROM tbIISAppTrace WHERE TimeIn_UTC = '" + convertDt + "' and ServerNum = " + ServerNum;
                
            SetQuery(QueryString);
            dsReturn = ExecuteDataSet();
            return nReturn;
        }
        public int w_ChartSubject(int CompanyNum, int MemberNum, string strReasonCode)
        {
           

            SetCmd("Cloud");
            SetQuery(@"SELECT b.ValueDescription
FROM tbAlertRules_Server as a
	INNER JOIN tbPCID_Server as b on a.ServerNum = b.ServerNum and a.PCID = b.PCID
	inner join tbServers_Member as c on b.ServerNum = c.ServerNum
WHERE c.CompanyNum = " + CompanyNum + 
	" and c.MemberNum = " + MemberNum + 
	" and a.ReasonCode = '" + strReasonCode + "'");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }
        public int w_AlertDetail_Table(int CompanyNum, int MemberNum, int ServerNum,string strInstanceName, string strReasonCode)
        {
            

            SetCmd("Cloud");
            SetQuery(@"SELECT TOP (30) a.TimeIn,a.ReasonCode,b.DisplayName,a.InstanceName,a.PValue,a.AlertDescription FROM tbAlerts as a inner join tbHostStatus as b on a.ServerNum = b.ServerNum WHERE a.TimeIn_UTC >= DATEADD(MINUTE, -60, GETUTCDATE())  and b.ServerNum = " + ServerNum + " AND a.ReasonCode = '" + strReasonCode + "' AND a.InstanceName = '" + strInstanceName + "' ORDER BY a.TimeIn_UTC DESC");

            dsReturn = ExecuteDataSet();
            return nReturn;

        }
        public int w_AlertDetail_Chart(int CompanyNum, int MemberNum, int ServerNum, string strInstanceName, string strReasonCode)
        {
            

            SetCmd("Cloud");
            SetQuery(@"SELECT a.TimeIn,a.PValue,b.ReasonCode FROM tbPerfmonValues as a INNER JOIN tbAlertRules_Server as b on a.ServerNum = b.ServerNum and a.PCID = b.PCID
WHERE b.ReasonCode = '" + strReasonCode + "' AND a.InstanceName = '" + strInstanceName + "' AND a.ServerNum = " + ServerNum + " AND a.TimeIn_UTC >= DATEADD(MINUTE, -60, GETUTCDATE()) ORDER BY TimeIn_UTC DESC");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public int W_WEB_TimeTaken(int ServerNum, DateTime dtmStart, DateTime dtmEnd)
        {
            

            SetCmd("Cloud");
            SetQuery("SELECT TOP(5) URI, AVG(AvgTimeTaken) AS[Average Time Taken], MAX(MaxTimeTaken) AS[Max Time Taken] FROM tbIISLog WHERE TimeIn >= '" + dtmStart + "' and TimeIn <= '" + dtmEnd + "' and ServerNum = "  + ServerNum + " GROUP BY URI ORDER BY[Average Time Taken] DESC");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }
        public int W_WEB_Byte(int ServerNum, DateTime dtmStart, DateTime dtmEnd)
        {
          

            SetCmd("Cloud");
            SetQuery("SELECT TOP(5) URI, SUM(CAST(SCBytes AS float)) AS [Total Bytes from Server], SUM(Hits) AS [Total Hits] FROM tbIISLog WHERE TimeIn >= '" + dtmStart + "' and TimeIn <= '" + dtmEnd + "' and ServerNum = " + ServerNum + " GROUP BY URI ORDER BY [Total Bytes from Server] DESC");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }
        public int m_FindPWD(string strEmail)
        {
            SetCmd("Cloud");
            base.ClareParameter();
            AddParameter("@Email", SqlDbType.NVarChar, 200, strEmail);
            SetProcedure("m_FindPWD");

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        //SQL Ad-Hoc Call
        public int R_Adhoc(string QueryString)
        {
            SetCmd("Cloud");
            SetQuery(QueryString);

            dsReturn = ExecuteDataSet();
            return nReturn;
        }

        public void R_Adhoc2(string QueryString)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString =  "Data Source=127.0.0.1;Initial Catalog=ServicePoint;Persist Security Info=True;User ID=sa;Password=kang0726!@#$";
            con.Open();
            cmd.Connection = con;
            SetQuery(QueryString);
            SqlDataReader sdr = ExecuteDataReader();
          
            DataSet ds_temp = new DataSet();
            DataTable dt_temp = new DataTable();
                        
            ds_temp.Tables.Add(dt_temp);
            //ds.tables[0].add(dt);
            ds_temp.Load(sdr, LoadOption.PreserveChanges, dt_temp);
            dsReturn = ds_temp;
            con.Close();
            //return nReturn;
            

        }










    }
}
