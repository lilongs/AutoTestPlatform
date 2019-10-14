/***************************************************
 *      Copyright Keysight Technologies 2003-2019
 **************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Ivi.Driver.Interop;
using Agilent.AgilentE36xx.Interop;

namespace AgilentE36xxPower
{
    class PowerConfig
    {
        AgilentE36xx driver = new AgilentE36xx();

        public bool PowerInit(string resourceDesc)
        {
            string initOptions = "QueryInstrStatus=true, Simulate=true, DriverSetup= Model=E36311A, Trace=false, TraceName=c:\\temp\\traceOut";
            bool idquery = true;
            bool reset = true;
            // Initialize the driver.  See driver help topic "Initializing the IVI-COM Driver" for additional information
            if (!driver.Initialized)
            {
                driver.Initialize(resourceDesc, idquery, reset, initOptions);
            }
            return driver.Initialized;
        }

        public void PowerOutput(double voltage,bool Enable)
        {
            IAgilentE36xxOutput pOutput1 = driver.Outputs.get_Item(driver.Outputs.get_Name(1));
            // Set output voltage.
            pOutput1.VoltageLevel = voltage;       
            // Enable all outputs
            driver.Outputs.Enabled = Enable;         
        }

        public void MeasurePowerOutput(out double voltage,out double current)
        {
            IAgilentE36xxOutput pOutput1 = driver.Outputs.get_Item(driver.Outputs.get_Name(1));
            voltage=pOutput1.Measure(AgilentE36xxMeasurementTypeEnum.AgilentE36xxMeasurementVoltage);
            current = pOutput1.Measure(AgilentE36xxMeasurementTypeEnum.AgilentE36xxMeasurementCurrent);
        }
       
    }
}
