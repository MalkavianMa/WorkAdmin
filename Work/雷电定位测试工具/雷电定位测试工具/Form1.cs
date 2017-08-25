using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

                for (int i = 0; i < FRfaultRecordRtn.faultRecords.Length; i++)
                {
                    writeLog("返回rtn:" + FRrtn + "返回error:" + FRerror + "返回faultRecordRtn" +"编号"+ FRfaultRecordRtn.faultRecords[i].id.ToString()+"一次设备名"+ FRfaultRecordRtn.faultRecords[i].deviceOneName.ToString()+ "故障时间毫秒值" + FRfaultRecordRtn.faultRecords[i].faultTime.ToString()+ "主站名" + FRfaultRecordRtn.faultRecords[i].stationName.ToString()+ "录波器名" + FRfaultRecordRtn.faultRecords[i].recorderName+ "故障相别"+ FRfaultRecordRtn.faultRecords[i].phase+"故障测距"+ FRfaultRecordRtn.faultRecords[i].location.ToString(), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                }
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
            PositionModel po = new PositionModel();
            Double poWave = Convert.ToDouble(tbxleftLocation.Text.Split('_')[0]);
            Double pwWave = Convert.ToDouble(tbxleftLocation.Text.Split('_')[1]);
            po = DistanceHelper.FindNeighPosition(poWave, pwWave, Convert.ToDouble(tbxRange.Text));//(Convert.ToDouble(tbxleftLocation.Text),Convert.ToDouble(tbxRightLocation.Text), rangWave);
            richTextBox1.Text = "返回" + po.MaxLat + "|" + po.MaxLng + "|" + po.MinLat + "|" + po.MinLng;
            writeLog("返回" + po.MaxLat + "|" + po.MaxLng + "|" + po.MinLat + "|" + po.MinLng, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));
        }

        private void btnKMcal_Click(object sender, EventArgs e)
        {
            double result = DistanceHelper.GetDistance(Convert.ToDouble(tbxlat.Text), Convert.ToDouble(tbxlng.Text), Convert.ToDouble(tbxlat2.Text), Convert.ToDouble(tbxlng2.Text));
            writeLog("返回计算距离" + result, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

        }


        //参数（起点坐标，角度，斜边长（距离）） 这是一个基本的三角函数应用
        private double[] getNewPoint(double[] point, double angle, double bevel)
        {
            //在Flash中顺时针角度为正，逆时针角度为负
            //换算过程中先将角度转为弧度
            var radian = angle * Math.PI / 180;
            var xMargin = Math.Cos(radian) * bevel;
            var yMargin = Math.Sin(radian) * bevel;
            return new double[] { point[0] + xMargin, point[1] + yMargin };

        }


        private Stopwatch stw = new Stopwatch();
 
        private void button1_Click(object sender, EventArgs e)
        { //
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                stw.Start();

                float poWave = Convert.ToSingle(textBox1.Text.Split('_')[0]);
                float pwWave = Convert.ToSingle(textBox1.Text.Split('_')[1]);
                float prWave = Convert.ToSingle(textBox2.Text);

                PositionModel bto = new 雷电定位测试工具.PositionModel();
                bto = DistanceHelper.FindNeighPosition(poWave, pwWave, prWave);

                richTextBox2.Text = "左下:经度\r" + bto.MinLng + "\r纬度\r" + bto.MinLat + "\r" + "右上:经度\r" + bto.MaxLng + "\r纬度:\r" + bto.MaxLat;
                writeLog("采集数据开始leftLocation =" + " 经度\n" + bto.MinLng + "纬度\n" + bto.MinLat + "rightLocation=" + "右上:经度\n" + bto.MaxLng + "纬度:\n" + bto.MaxLat + "startTime =" + tbxstarttime.Text + "endTime =" + tbxEndtime.Text, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                //   private Stopwatch stw = new Stopwatch();
                stw.Stop();
                writeLog("采集数据结束，耗时 " + stw.Elapsed.Minutes + "分 " + stw.Elapsed.Seconds + "秒" + stw.Elapsed.Milliseconds + "毫秒。", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffffff"));

                // stw.Start();
                //writeLog("采集数据开始(leftLocation =rightLocation =startTime =endTime =)...",DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss.ffffff"));




                //stw.Stop(); 
                /// writeLog("采集数据结束，耗时 " + stw.Elapsed.Minutes + "分 " +stw.Elapsed.Seconds+"秒"+stw.Elapsed.Milliseconds+"毫秒。")
                //      Double poWave = Convert.ToDouble(tbxleftLocation.Text.Split('_')[0]);
                //     Double pwWave = Convert.ToDouble(tbxleftLocation.Text.Split('_')[1]);
                //  double[] point = { poWave, pwWave };
                //  double bevel = 5 * Math.Sqrt(2);
                ////double [] result=   getNewPoint(point, 45, bevel);
                //PointF fo = new PointF();
                //fo.X =poWave;
                //fo.Y = pwWave;
                //richTextBox2.Text = getNewPoint(fo, 45, bevel).X.ToString()+"\n" + getNewPoint(fo, 45, bevel).Y.ToString();
                // richTextBox2.Text = result[0].ToString() +"\n"+ result[1].ToString (); 
            }
        }

        //参数（起点坐标，角度，斜边长（距离）） 这是一个基本的三角函数应用
        private PointF getNewPoint(PointF point, double angle, double bevel)
        {
            //在Flash中顺时针角度为正，逆时针角度为负
            //换算过程中先将角度转为弧度
            var radian = angle * Math.PI / 180;
            var xMargin = Math.Cos(radian) * bevel;
            var yMargin = Math.Sin(radian) * bevel;
            return new PointF(point.X + (float)xMargin, point.Y + (float)yMargin);

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
