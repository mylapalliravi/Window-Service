using System;
using System.ServiceProcess;
using System.Configuration;
using System.Threading;
using System.IO;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;

namespace WindowsService
{
    [RunInstaller(true)]
    public partial class Service1 : ServiceBase
    {
        //int ScheduleTime = Convert.ToInt32(ConfigurationSettings.AppSettings["ThreadTime"]);
        // string constring = "server=192.168.1.7;database=LongHaul;uid=sa;password=!n1@Nd;";
       // int ScheduleTime = 2;
        public Thread worker = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ThreadStart start = new ThreadStart(Working);
                worker = new Thread(start);
                worker.Start();
            }
            catch (Exception)
            {

                throw;
            }
        }

        string longs;
        int iserror;
        public void Working()
        {
            while (true)
            {
                string path = "C:\\whatsapdata.text";
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    DataSet data = new DataSet();
                    data = get_Whatsapdata();


                    for(int _i=0;_i<data.Tables[0].Rows.Count;_i++)
                    { 

                    writer.WriteLine(data.Tables[0].Rows[_i][0].ToString()+' '+data.Tables[0].Rows[_i][1].ToString()+' '+data.Tables[0].Rows[_i][2].ToString()+' '+data.Tables[0].Rows[_i][3].ToString()+' '+data.Tables[0].Rows[_i][4]+' '+data.Tables[0].Rows[_i][5]+' '+data.Tables[0].Rows[_i][6]+' '+data.Tables[0].Rows[_i][7]+' '+data.Tables[0].Rows[_i][8]);
                        writer.WriteLine();
                    }
                    writer.Close();
                   // Whatsap_WidnowService(longs,iserror);

                }
                  //Thread.Sleep(ScheduleTime * 60 * 1000);
            }
        }

        protected override void OnStop()
        {
            try
            {
                if ((worker != null) & worker.IsAlive)
                {
                    worker.Abort();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        //#region LogFile Write

        public static string FileName = "";
        public static string FilePath = "";
        public static string FileSize = "";
        public static string FileLocation = "";
        public static string FileContent = "";
        public static string FileFullName = "";
        public static string FillZipFileName = "";
        public static StreamWriter sw = null;
        public static void Whatsap_WidnowService(string logString, int _IsError)
        {
            SqlConnection con;
            try
            {


                // Create the CSV file to which grid data will be exported.
                FilePath = @"D:\WebApps\AutoExeFiles\Teevra\ErrorLogs\";
                FileName = "Whatsappdata2" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                FileFullName = FilePath.ToString() + FileName.ToString().Trim();
                FileInfo file = new FileInfo(FileFullName);
                if (!Directory.Exists(FilePath.ToString())) Directory.CreateDirectory(FilePath.ToString());
                sw = (File.Exists(FileFullName)) ? File.AppendText(FileFullName) : File.CreateText(FileFullName);
                //  sw.WriteLine(logString.ToString() + " :: " + DateTime.Now);
                sw.WriteLine(string.Format("Window service called on " + DateTime.Now.ToString("dd/mm/yyyy hh:mm tt") + ""));


                sw.Close();
            }
            catch
            {

            }
            finally
            {

            }
        }
        //#endregion  LogFile

        public DataSet get_Whatsapdata()
        {
            //SqlConnection con = new SqlConnection("constring");
            //SqlCommand cmd = new SqlCommand("LongHaul..[usp_WhatsApppMessages]", con);
            //cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //con.Open();
            //cmd.Parameters.AddWithValue("@flag", "INCOMINGMESSAGE");
            //SqlDataAdapter sda = new SqlDataAdapter(cmd);
            //System.Data.DataSet ds = new System.Data.DataSet();
            //sda.Fill(ds);
            //return ds;
            // AllRequests AR = new AllRequests();
            //con.Open();
            SqlConnection con = new SqlConnection("server=192.168.1.7;database=LongHaul;uid=sa;password=!n1@Nd;");
            SqlCommand cmd = new SqlCommand("[usp_whatsapmessage]", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataSet dt = new DataSet();
            sd.Fill(dt);
            con.Close();
            return dt;


        }
    }
}
    

             
    


