using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace 雷电定位测试工具
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //雷电定位测试工具.LDService.GetFlashReadService LDService = new LDService.GetFlashReadService();

            //雷电定位测试工具.FaultRecordService.FaultRecordServiceService FaultRecordServiceService = new FaultRecordService.FaultRecordServiceService();
            //FaultRecordServiceService.getFaultRecordByDeviceOneName()
        }
        private void writeLog(string log, string logtime)
        {
            try
            {
                this.listBox1.Items.Add(log + "  " + logtime);
                string des = Assembly.GetExecutingAssembly().Location;
                des = des.Substring(0, des.LastIndexOf(@"\")) + "\\Log";
                if (!Directory.Exists(des))
                {
                    Directory.CreateDirectory(des);
                }
                string filename = des + "\\leidian_log " + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                FileStream myFileStream = new FileStream(filename, FileMode.Append);
                StreamWriter sw = new StreamWriter(myFileStream, System.Text.Encoding.Default);
                sw.WriteLine(log + "  " + logtime);
                myFileStream.Flush();
                sw.Close();
                myFileStream.Close();
            }
            catch (System.Exception ex)
            {
                this.listBox1.Items.Add(ex.Message.ToString() + "  " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
            }
        }
        private void btntest_Click(object sender, EventArgs e)
        {
            雷电定位测试工具.LDService.GetFlashReadService LDService = new LDService.GetFlashReadService();
            XmlDocument doc = new XmlDocument();
            string result = "";
            // string getFlash = "";

            if (true)
            {
                //result = LDService.getFlashByRect(tbxleftLocation.Text, tbxRightLocation.Text, tbxstarttime.Text, tbxEndtime.Text, tbxStartline.Text, tbxLineCount.Text);

                // getFlash =  LDService.getFlashByRect(tbxleftLocation.Text, tbxRightLocation.Text, tbxstarttime.Text, tbxEndtime.Text, tbxStartline.Text, tbxLineCount.Text);
                // XmlNode root = doc.DocumentElement;
                // doc.LoadXml(getFlash);
                try
                {
                    // 雷电定位测试工具.QueryService queryservice = new 雷电定位测试工具.QueryService.QueryService();

                    doc.LoadXml(LDService.getFlashByRect(tbxleftLocation.Text, tbxRightLocation.Text, tbxstarttime.Text, tbxEndtime.Text, tbxStartline.Text, tbxLineCount.Text));
                    result = doc.InnerXml.ToString();

                    writeLog("返回" + result, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
                }
                catch (Exception ex)
                {
                    writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
                }
            }




        }

        private void btnLineQuery_Click(object sender, EventArgs e)
        {
            int FRrtn = -1;
            string FRerror = "";

            try
            {
                雷电定位测试工具.FaultRecordService.FaultRecordServiceService FRService = new FaultRecordService.FaultRecordServiceService();
                雷电定位测试工具.FaultRecordService.time startTime = new FaultRecordService.time();// tbxWaveStartTime.Text;
                雷电定位测试工具.FaultRecordService.time endTime = new FaultRecordService.time();// tbxWaveStartTime.Text;
                雷电定位测试工具.FaultRecordService.faultRecordRtn FRfaultRecordRtn = new FaultRecordService.faultRecordRtn();

                DateTime startTime1 = Convert.ToDateTime(tbxWaveStartTime.Text);
                DateTime endTime1 = Convert.ToDateTime(tbxWaveEndTime.Text);
                ConvertTime(startTime, startTime1);
                ConvertTime(endTime, endTime1);

                //FRService.getFaultRecordByDeviceOneName(tbxdeviceOneName.Text, tbxWaveStartTime.Text, tbxWaveEndTime.Text);

                FRrtn = FRService.getFaultRecordByDeviceOneName(tbxdeviceOneName.Text, startTime, endTime).rtn;
                FRerror = FRService.getFaultRecordByDeviceOneName(tbxdeviceOneName.Text, startTime, endTime).error;
                FRfaultRecordRtn.faultRecords = FRService.getFaultRecordByDeviceOneName(tbxdeviceOneName.Text, startTime, endTime).faultRecords;

                writeLog("返回rtn:" + FRrtn + "返回error:" + FRerror + "返回faultRecordRtn" + FRfaultRecordRtn.faultRecords.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

            }
            catch (Exception ex)
            {

                writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
            }
            // FRerror = FRService.getFaultRecordByDeviceOneName(tbxdeviceOneName.Text,,).error;
            //FaultRecordServiceService.getFaultRecordByDeviceOneName()
        }

        private static void ConvertTime(FaultRecordService.time startTime, DateTime startTime1)
        {
            startTime.day = startTime1.Day;
            startTime.hour = startTime1.Hour;
            startTime.minute = startTime1.Minute;
            startTime.month = startTime1.Month;
            startTime.msecond = startTime1.Millisecond;
            startTime.second = startTime1.Second;
            startTime.year = startTime1.Year;
        }

        private void btnWave_Click(object sender, EventArgs e)
        {
            long waveId = Convert.ToInt64(tbxWaveId.Text);// Convert.ToUInt32(tbxWaveId);
            int rtn = -1;
            byte[] bytes = new byte[] { };
            string error = "";
            try
            {
                雷电定位测试工具.FaultRecordService.FaultRecordServiceService FRService = new FaultRecordService.FaultRecordServiceService();

                rtn = FRService.getFile(waveId, tbxWaveExtension.Text).rtn;
                bytes = FRService.getFile(waveId, tbxWaveExtension.Text).bytes;
                // FileStream opBytes
             waveDownload(bytes);
                error = FRService.getFile(waveId, tbxWaveExtension.Text).error;
                //  writeLog("返回rtn:" + rtn + "返回ByteS:" + bytes + "返回error：" + error, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                writeLog("返回rtn:" + rtn + "返回error：" + error, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
            }
            catch (Exception ex)
            {
                writeLog("异常" + ex.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                // throw;
            }


        }

        private void waveDownload(byte[] bytes)
        {
            string des = Assembly.GetExecutingAssembly().Location;
            des = des.Substring(0, des.LastIndexOf(@"\")) + "\\Log";
            if (!Directory.Exists(des))
            {
                Directory.CreateDirectory(des);
            }
            string filename = des + "\\download " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff") + "." + tbxWaveExtension.Text;
            System.IO.File.WriteAllBytes(filename, bytes);
           
        }

        private void btnCalculator_Click(object sender, EventArgs e)
        {

        }

        ///// <remarks/>
        //[System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestNamespace = "http://www.sgcc.com.cn/sggis/service/gisservice", ResponseNamespace = "http://www.sgcc.com.cn/sggis/service/gisservice", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        //[return: System.Xml.Serialization.XmlElementAttribute("out")]
        //public string queryPSR(string inputXML)
        //{
        //    object[] results = this.Invoke("queryPSR", new object[] {
        //                inputXML});
        //    return ((string)(results[0]));
        //}
    }
}
