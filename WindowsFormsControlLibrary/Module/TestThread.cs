using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ag3446x_CS;
using AgilentE36xxPower;
using WindowsFormsControlLibrary;
using AutoTestDLL.Model;
using System.Threading;
using AutoTestDLL.Util;
using WindowsFormsControlLibrary.Module;
using System.Runtime.InteropServices;
using System.Drawing;

namespace TestThread
{
    public partial class TestThread1
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public string MeterSourceName = "";
       public string PowerSourceName = "";
       public string TempSourceName = "";

       public byte TempSensor1Add = 1;
       public byte TempSensor2Add = 2;
       public byte TempSensor3Add = 3;

       public double MeterRange = 1, MeterResolution = 0.0001;
       public double PowerVoltage = 0, PowerCurrent = 0, Temp = 0, Humidity = 0, MeterCurrent = 0;

       public bool MeasurePowerSwitch = false;
       public bool MeasureMetertSwitch = false;
       public bool MeasurTempSwitch = false;
      // public Dictionary<string, bool> DicMeasurTempSwitch = new Dictionary<string, bool>();

      
       public bool   DisplayEnable = true;

       public string PowerFilePath = "F:\\Power";
       public string PowerFileName = "123.txt";

       public string TempFilePath = "F:\\Temp";
       public string TempFileName = "1234.txt";

       public string MeterFilePath = "F:\\Meter";
       public string MeterFileName = "";
        public double PowerSourceVal = 0.0;

        public int TempFileRecordStep = 10;
        public int MeterFileRecordStep = 10;
        Meter MeterConfig = new Meter();
        PowerConfig PowerControl = new PowerConfig();
        FileOperate FileOperation = new FileOperate();
        PCBcontrol PB = new PCBcontrol();

        public void MeasurePowerVoltageandCurrent(Object pc)
        {
            PowerControl.PowerInit(PowerSourceName);
            PowerControl.PowerOutput(12,true);
            StringBuilder PowerVal=new  StringBuilder();
            CurrentElectricValue value = new CurrentElectricValue();
           
          //  Random rd1 = new Random();
            int count = 0;
            while (true)
            {    
                PowerControl.MeasurePowerOutput(out PowerVoltage, out PowerCurrent);
               // PowerVoltage=rd1.Next(1,100);
               // PowerCurrent = rd1.Next(1, 100);
               // foreach (var item in DicMeasurTempSwitch)
                if (MeasurePowerSwitch)
                {
               //     if (item.Value)
                    {
                        count++;
                        PowerVal.Append(PowerVoltage.ToString() + "," + PowerCurrent.ToString() + Environment.NewLine);
                        if (count > 120)
                        {
                            count = 0;
                            FileOperation.createFile(PowerFilePath, PowerFileName, PowerVal.ToString());
                            PowerVal.Clear();
                        }
                    }
                }
                
                Thread.Sleep(1000);
                value.now = DateTime.Now;
                value.currentValue = (float)PowerCurrent;
                value.voltageValue = (float)PowerVoltage;

                PublicElectricControl pc2 = new PublicElectricControl();
                pc2 = (PublicElectricControl)pc;
                pc2.ChartValueFill(value);
            }
        }
        public void MeasureTempandHumidity(Object tp)
        {
            try
            {
                StringBuilder TempVal = new StringBuilder();
                Temperature_humidity value = new Temperature_humidity();
                Random rd1 = new Random();
                int count = 0;
                int countrecord = 0;
                while (true)
                {
                    Temp = rd1.Next(1, 100);
                    Humidity = rd1.Next(1, 100);
                    if (MeasurTempSwitch)
                    {
                        count++;
                        countrecord++;
                        if (countrecord == TempFileRecordStep)
                        {
                            countrecord = 0;
                            TempVal.Append(Temp.ToString().PadLeft(10,' ') + "℃,  " + Humidity.ToString().PadLeft(10,' ') + "%RH  " + Environment.NewLine);
                        }
                        if (count > 120)
                        {
                            count = 0;
                            FileOperation.createFile(TempFilePath, TempFileName, TempVal.ToString());
                            TempVal.Clear();
                        }
                    }
                    Thread.Sleep(1000);
                    value.now = DateTime.Now;
                    value.temperatureValue = (float)Temp;
                    value.humidtyValue = (float)Humidity;

                    List<TemperatureControl> tp2 = new List<TemperatureControl>();

                    tp2 = (List<TemperatureControl>)tp;
                    foreach (TemperatureControl t in tp2)
                    {
                        t.ChartValueFill(value);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }
        public bool IsStop = false;
        public bool IsTestEnd = false;
        public void MeasureMeterCurrent(object tu)
        {
            TestUnit tu2=new TestUnit();
          
            tu2 = (TestUnit)tu;
            try
            {
                StringBuilder MeterVal = new StringBuilder();
                string Datesandhour1 = DateTime.Now.ToString("_hh_mm_ss_");
                MeterConfig.MeterInit(MeterSourceName);
                MeterConfig.SetCurrent(MeterRange, MeterResolution);
              //  MeterConfig.MeasurementInit();
                decimal data = 0;
                string danwei = "A ";
                int count = 0;
                int countrecord = 0;
                float max = 1;
                // Random rd1 = new Random();        
                while (!IsStop || !IsTestEnd)
                {
                    //  MeterCurrent = rd1.Next(1, 100);
                    MeterCurrent = MeterConfig.MeasureSinglePoint();
                    Datesandhour1 = DateTime.Now.ToString("hh:mm:ss , ");
                    data = ChangeDataToD(MeterCurrent);
                    if (data >= (decimal)0.1)
                    {
                        danwei = "A ";
                        max = 1.5f;
                    }
                    else if (data < (decimal)0.1 && data > (decimal)0.0001)
                    {
                        data = data * 1000;
                        danwei = "mA ";
                        max = 0.1f;
                    }
                    else
                    {
                        data = data * 1000000;
                        danwei = "uA ";
                        max = 0.0001f;
                    }
                    data = Math.Round(data, 2);

                    tu2.SetScale(max, -0.2f, data.ToString()+danwei);
                    if (MeasureMetertSwitch)
                    {
                        count++;
                        countrecord++;
                        if (countrecord == MeterFileRecordStep)
                        {
                            countrecord = 0;
                            MeterVal.Append(Datesandhour1 + data.ToString().PadLeft(10,' ') + danwei + ",  " + PowerSourceVal.ToString() + "V  " + Environment.NewLine);
                        }
                        if (count > 120)
                        {
                            count = 0;
                            FileOperation.createFile(MeterFilePath, MeterFileName, MeterVal.ToString());
                            MeterVal.Clear();
                        }
                    }
                    Thread.Sleep(500);
                    tu2.ChartValueFill((float)MeterCurrent);
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex, ex.Message);
                tu2.ShowInfo(tu2.richTextBox1, ex.Message,Color.Red);
                tu2.Setnoticecolor(Color.Red);
            }
        }
        public static object locker = new object();

        public void PCBcontrol()
        {
            try
            {
                while (!IsStop || !IsTestEnd)
                {
                    lock (locker)
                    {
                        PCBControl123(Function);
                        Function = 5;
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
            }
        }
        //Function 0:SetPowerONOFF
        //Function 1:SetCurrentMeterChannal
        //Function 2:PCBboardInit
        //Function 3:SetLevel
        //Function 4:LEDcontrol
        //Function 5:SetFule
        //Fucntion 100:NON
        public byte Channal = 0;
        public Int16[] Fuel1 = new Int16[] { 270,70,270,70,70,70 };
        public Int16[] Fuel2 = new Int16[] { 270,70,270,70,70,70 };
        public Int16[] Fuel3 = new Int16[] { 270,70,270,70,70,70 };
        public Int16[] Fuel4 = new Int16[] { 270,70,270,70,70,70 };
        public bool PowerONOFF=true; //true--on false--off
        public bool Level = true;//true--high ,false --low
        public bool LEDONOFF = true;
        public byte LED = 0;
        public int Function = 100;
        public bool FuleEnbale = false;
        int cnt = 0;
        public void PCBControl123(int Function)
        {
            if(Function!=100)
            {
                if(Function==0)
                {
                    PB.SetPowerONOFF(Channal, PowerONOFF);
                }
                if (Function == 1)
                {
                    PB.SetCurrentMeterChannal(Channal);
                }
                else if(Function==2)
                {
                    PB.PCBboardInit();
                }
                else if(Function==3)
                {
                    PB.SetLevel(Channal, Level);
                }
                else if(Function==4)
                {
                    PB.LEDcontrol(LED,LEDONOFF);
                }
                else if(Function==5)
                {
                    if (FuleEnbale)
                    {
                        cnt++;
                        if(cnt>=6)
                        {
                            cnt = 0;
                        }
                        PB.SetFule(Channal, Fuel1[cnt], Fuel2[cnt], Fuel3[cnt], Fuel4[cnt]);
                        Thread.Sleep(2000);
                    }
                }

            }
        }
        public static decimal ChangeDataToD(double strData1)
        {
            decimal dData = 0.0M;
            string strData = Convert.ToString(strData1).ToUpper();
            try
            {
                if (strData.Contains("E") || strData.Contains("e"))
                {
                    double b = double.Parse(strData.ToUpper().Split('E')[0].ToString());//整数部分
                    double c = double.Parse(strData.ToUpper().Split('E')[1].ToString());//指数部分
                    dData =(decimal) (b * Math.Pow(10, c));
                }
                else
                {
               
                    dData = decimal.Parse(strData);
                }
            }
            catch (Exception)
            {
                dData = 0;
            }
            //	return double.Parse(dData.ToString());
            return dData;
        }
    }
}
