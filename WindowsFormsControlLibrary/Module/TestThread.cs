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
       public bool MeasureMetertSwitch = true;
       public bool MeasurTempSwitch = false;
      // public Dictionary<string, bool> DicMeasurTempSwitch = new Dictionary<string, bool>();

      
       public bool   DisplayEnable = true;

       public string PowerFilePath = "E:\\";
       public string PowerFileName = "123.txt";

       public string TempFilePath = "E:\\";
       public string TempFileName = "1234.txt";

       public string MeterFilePath = "E:\\";
       public string MeterFileName = "";

        Meter MeterConfig = new Meter();
        PowerConfig PowerControl = new PowerConfig();
        FileOperate FileOperation = new FileOperate();


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
            while (true)
            {               
                Temp = rd1.Next(1, 100);
                Humidity = rd1.Next(1, 100);
                if (MeasurTempSwitch)
                {
                    count++;
                    TempVal.Append(Temp.ToString() + "," + Humidity.ToString() + Environment.NewLine);                 
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
        public void MeasureMeterCurrent(object tu)
        {
            StringBuilder MeterVal = new StringBuilder();
            MeterConfig.MeterInit(MeterSourceName);
            MeterConfig.SetVoltage(MeterRange, MeterResolution);
            int count = 0;
           // Random rd1 = new Random();        
               while (true)
               {
                 //  MeterCurrent = rd1.Next(1, 100);
                   MeterCurrent = MeterConfig.MeasureSinglePoint();
                   if (MeasureMetertSwitch)
                   {
                       count++;
                       MeterVal.Append(MeterCurrent.ToString() + Environment.NewLine);
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
    }
}
