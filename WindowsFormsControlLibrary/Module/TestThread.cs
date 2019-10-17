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

namespace TestThread
{
    public partial class TestThread1
    {
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
                        TempVal.Append(Temp.ToString() + "℃," + Humidity.ToString() + "%RH" + Environment.NewLine);
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
                foreach(TemperatureControl t in tp2)
                {
                    t.ChartValueFill(value);
                }
            }
        }
        public bool IsStop = false;
        public bool IsTestEnd = false;
        public void MeasureMeterCurrent(object tu)
        {
            StringBuilder MeterVal = new StringBuilder();
            string Datesandhour1 = DateTime.Now.ToString("_hh_mm_ss_");
            MeterConfig.MeterInit(MeterSourceName);
            MeterConfig.SetVoltage(MeterRange, MeterResolution);
            MeterConfig.MeasurementInit();
            decimal data = 0;
            string danwei = "A";
            int count = 0;
            int countrecord = 0;
           // Random rd1 = new Random();        
               while (!IsStop||!IsTestEnd)
               {
                 //  MeterCurrent = rd1.Next(1, 100);
                   MeterCurrent = MeterConfig.MeasureSinglePoint();
                   Datesandhour1 = DateTime.Now.ToString("_hh_mm_ss_,");
                   data = ChangeDataToD(MeterCurrent);
                if(data>=(decimal)0.1)
                {
                    danwei = "A";
                }
                else if(data < (decimal)0.1 && data > (decimal)0.0001)
                {
                    data = data * 1000;
                    danwei = "mA";
                }
                else
                {
                    data = data * 1000000;
                    danwei = "uA";
                }

                if (MeasureMetertSwitch)
                   {
                       count++;
                    countrecord++;
                        if (countrecord == MeterFileRecordStep)
                        {
                            countrecord = 0;
                            MeterVal.Append(Datesandhour1+MeterCurrent.ToString() + danwei + PowerSourceVal.ToString() + "V" + Environment.NewLine);
                        }
                       if (count > 120)
                       {
                           count = 0;
                           FileOperation.createFile(MeterFilePath, MeterFileName, MeterVal.ToString());
                           MeterVal.Clear();
                       }                  
                   }
                   Thread.Sleep(500);                
                   TestUnit tu2 = new TestUnit();
                   tu2 = (TestUnit)tu;
                   tu2.ChartValueFill((float)MeterCurrent);
               }
        }
        public static object locker = new object();

        public void PCBcontrol()
        {
            while (true)
            {
                lock (locker)
                {
                    PCBControl123(Function);
                    Function = 100;
                }
                Thread.Sleep(100);
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
        public Int16 Fuel1 = 0;
        public Int16 Fuel2 = 0;
        public Int16 Fuel3 = 0;
        public Int16 Fuel4 = 0;
        public bool PowerONOFF=true; //true--on false--off
        public bool Level = true;//true--high ,false --low
        public bool LEDONOFF = true;
        public byte LED = 0;
        public int Function = 100;
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
                    PB.SetFule(Channal, Fuel1, Fuel2, Fuel3, Fuel4);
                }

            }
        }
        public static decimal ChangeDataToD(double strData1)
        {
            decimal dData = 0.0M;
            string strData = strData1.ToString();
            try
            {
                if (strData.Contains("E") || strData.Contains("e"))
                {
                    strData = strData.Substring(0, strData.Length - 1).Trim();
                    dData = Convert.ToDecimal(Decimal.Parse(strData.ToString(), System.Globalization.NumberStyles.Float));
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
