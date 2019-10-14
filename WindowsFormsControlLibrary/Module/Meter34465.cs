/***************************************************
 *     Copyright Keysight Technologies 2013-2016
 **************************************************/
using System;
using System.Collections;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Agilent.Ag3446x.Interop;

namespace Ag3446x_CS
{
   
    class Meter
    {

        Ag3446x driver = new Ag3446x();
        public  bool MeterInit(string resourceDesc)
        {
           
            string initOptions = "QueryInstrStatus=true, Simulate=false, DriverSetup= Model=34460A, Trace=false, TraceName=c:\\temp\\traceOut";
            bool idquery = true;
            bool reset = true;
            if (driver.Initialized == false)
            {
                driver.Initialize(resourceDesc, idquery, reset, initOptions);
            }
            Thread.Sleep(10);
            return !driver.Initialized;
        }

        public void SetVoltage(double range,double resolution)
        {
            
            driver.DCVoltage.Configure(range, resolution);
            driver.DCVoltage.AutoZero = Ag3446xAutoZeroEnum.Ag3446xAutoZeroOnce;
            // Set reading rate to 0.02 NPLC's
            driver.DCVoltage.NPLC = 10;
            // Set up triggering for 1000 samples from a single trigger event
           // driver.Trigger.Source = Ag3446xTriggerSourceEnum.Ag3446xTriggerSourceImmediate;
           
        }
        public void SetCurrent(double range,double resolution)
        {
          
            driver.DCCurrent.Configure(range, resolution);
            driver.DCCurrent.AutoZero = Ag3446xAutoZeroEnum.Ag3446xAutoZeroOnce;

            // Set reading rate to 0.02 NPLC's
            driver.DCCurrent.NPLC = 10;

            // Set up triggering for 1000 samples from a single trigger event
            driver.Trigger.Source = Ag3446xTriggerSourceEnum.Ag3446xTriggerSourceImmediate;
           
        }
        public ArrayList MesureMutiPoint()
        {
            
            driver.Measurement.Initiate();
            driver.Trigger.Count = 1;
            driver.Trigger.Delay = 0;
            driver.Trigger.SampleCount = 1000;
            driver.System.WaitForOperationComplete(1000);
            int dataPts = 0;
            ArrayList data = new ArrayList();

            for (int i = 0; i < 10; i++)
            {
              
                while (dataPts < 100)
                {
                    dataPts = driver.Measurement.ReadingCount;
                }
                // We have 100 data points, lets read them out
                double[] tempData = driver.Measurement.RemoveReadings(100);
                // Add them to the "collection" array
                // At this point you could also "process" the data in some way while waiting for the next
                // "block" of measurements to be acquired by the instrument.
                data.AddRange(tempData);
                dataPts = 0;
            }
            return data;
        }
        public double MeasureSinglePoint()
        {
          
            double data2 = 0;
              driver.Measurement.Initiate();
                  driver.System.WaitForOperationComplete(1000);
                   data2 = driver.Measurement.Fetch(1000);
              return data2;
        }    
    }
}
