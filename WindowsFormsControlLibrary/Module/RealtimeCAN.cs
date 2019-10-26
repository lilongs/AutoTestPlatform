using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsControlLibrary.Module
{
    public partial class KvaserCommunication
    {
        /*******************************************************
        * Function name : BZCount
        * Parameter     : u8CurrentBZ[in], the current BZ value
        * Return        : the BZ update value
        * Description   : Update BZ value
        *******************************************************/
        public byte BZCount(byte u8CurrentBZ)
        {
            byte u8NewBZ;
            if (u8CurrentBZ == 0x0F)
            {
                u8NewBZ = 0x00;
            }
            else
            {
                u8NewBZ = (byte)(u8CurrentBZ + 1);
            }

            // BZdata = (byte)((BZdata & 0xF0) | u8NewBZ);
            return u8NewBZ;
        }

        /*******************************************************
       * Function name : CalcCrc
       * Parameter     : aId[in], message id
       *                 aDlc[in], message dlc
       *                 data[in], message data
       * Return        : CRC byte
       * Description   : Calculate CRC byte value
       *******************************************************/
        public byte CalcCrc(long aId, int aDlc, byte[] data)
        {
            // disable warning "Variable will only be initialized once"
            //#pragma warning(disable: 2065)

            byte u8_crc;
            byte u8_byte_index;
            byte u8_bit_index;
            byte u8_crc_poly = 0x2F;
            // u8_BZ = BZCount((byte)(data[1] & 0x0F));
            if (aId == 795)
            {
                u8_crc = 0xFF;
            }

            u8_crc = 0xFF;

            for (u8_byte_index = 1; u8_byte_index < aDlc; u8_byte_index++)
            {
                u8_crc = (byte)(u8_crc ^ data[u8_byte_index]);

                for (u8_bit_index = 0; u8_bit_index < 8; u8_bit_index++)
                {
                    if ((u8_crc & 0x80) != 0)//最高位是0 else
                    {
                        u8_crc = (byte)((u8_crc << 1) ^ u8_crc_poly);
                    }
                    else
                    {
                        u8_crc = (byte)(u8_crc << 1);
                    }

                }
            }
            u8_crc ^= GetTransID(aId, (byte)(data[1] & 0x0F));
            for (u8_bit_index = 0; u8_bit_index < 8; u8_bit_index++)
            {
                if ((u8_crc & 0x80) != 0)
                {
                    u8_crc = (byte)((u8_crc << 1) ^ u8_crc_poly);
                }
                else
                {
                    u8_crc = (byte)(u8_crc << 1);
                }

            }

            u8_crc = (byte)~u8_crc;
            // Console.WriteLine("CRC CRC CRC CRC CRC CRC CRC CRC CRC CRC CRC CRC CRC CR========={00000}", u8_crc);
            return u8_crc;
        }
        public byte[] a_CRC_Table_ESP_21 = { 0xb4, 0xef, 0xf8, 0x49, 0x1e, 0xe5, 0xc2, 0xc0, 0x97, 0x19, 0x3c, 0xc9, 0xf1, 0x98, 0xd6, 0x61 };
        public byte[] a_CRC_Table_ESP_24 = { 0x67, 0x8A, 0xAE, 0x22, 0x4D, 0xD0, 0x51, 0x80, 0x5C, 0xB9, 0xCE, 0x1E, 0xDF, 0x02, 0x2D, 0xD4 };
        public byte[] a_CRC_Table_TSK_07 = { 0x78, 0x68, 0x3A, 0x31, 0x16, 0x08, 0x4F, 0xDE, 0xF7, 0x35, 0x19, 0xE6, 0x28, 0x2F, 0x59, 0x82 };
        public byte[] a_CRC_Table_ESP_20 = { 0xac, 0xb3, 0xab, 0xeb, 0x7a, 0xe1, 0x3b, 0xf7, 0x73, 0xba, 0x7c, 0x9e, 0x06, 0x5f, 0x02, 0xd9 };
        public byte[] a_CRC_Table = { 0x67, 0x8A, 0xAE, 0x22, 0x4D, 0xD0, 0x51, 0x80, 0x5C, 0xB9, 0xCE, 0x1E, 0xDF, 0x02, 0x2D, 0xD4 };
        public byte[] a_CRC_Table_TSG_FT_02 = { 0xc4, 0x6a, 0x69, 0x30, 0xcf, 0x61, 0x58, 0x51, 0x1b, 0x86, 0x99, 0xd3, 0xf6, 0x1d, 0x9a, 0x37 };
        public byte[] a_CRC_Table_SMLS_01 = { 0xc3, 0x79, 0xbf, 0xdb, 0xe9, 0x11, 0x46, 0x86, 0x69, 0xb6, 0x9b, 0x29, 0x15, 0x9c, 0x45, 0x0d };

        /*******************************************************
        * Function name : GetTransID
        * Parameter     : u16_ID[in], can id which need to transfer
        *                 u8NewBZ[in], the new BZ value
        * Return        : the id after transfer
        * Description   : Transfer CAN id in calculate CRC
        *******************************************************/
        byte GetTransID(long u16_ID, byte u8NewBZ)
        {
            int u8_BZ = System.Convert.ToInt16(u8NewBZ);
            switch (u16_ID)
            {
                case (0x30C):   //writelineex(-3,1, "0x30C");
                    return (0x0F);

                case (0x324):   //writelineex(-3,1, "0x324");
                    return (0x27);

                case (0x32A):   //writelineex(-3,1, "0x32A");
                    return (0x29);

                case (0x31E):   //writelineex(-3,1, "0x31E");
                    return (a_CRC_Table_TSK_07[u8_BZ]);

                case (0x65D):   //writelineex(-3,1, "0x65D");
                    return (a_CRC_Table_ESP_20[u8_BZ]);

                case (0x31B):   //writelineex(-3,1, "0x31B");
                    return (a_CRC_Table_ESP_24[u8_BZ]);

                case (0xFD):    //writelineex(-3,1, "0xFD");
                    return (a_CRC_Table_ESP_21[u8_BZ]);

                case (0x40):    //writelineex(-3,1, "0x40");
                    return (0x40);

                case (0x3D4):
                    return (a_CRC_Table_SMLS_01[u8_BZ]);

                case (0x30F):
                    return (a_CRC_Table[u8_BZ]);

                case (0x3E5):
                    return (a_CRC_Table_TSG_FT_02[u8_BZ]);

                case (0x3C0):
                    return (0xC3);

                case (0x520):
                    return (0x44);

                case (0x5CA):
                    return (0);

                case (0x387):
                    return (0);

                case (0x396):
                    return (0);

                case (0x12DD5470):
                    return (0);

                case (0x656):
                    return (0);

                case (0x101):
                    return (0);

                case (0x116):
                    return (0);

                case (0x12B):
                    return (0);

                case (0x584):
                    return (0);

                case (0x58C):
                    return (0);

                case (0x3D5):
                    return (0);

                case (0x493):
                    return (0);

                case (0x48D):
                    return (0);

                case (0x641):
                    return (0);

                case (0x187):
                    return (0);

                case (0x494):
                    return (0);

                case (0x394):
                    return (0);

                default:
                    //write("ID: %x default", u16_ID);
                    break;
            }
            return (0);
        }
        /*******************************************************
* Function name : ExtractDTC
* Parameter     : baData[in], raw data
*                 saDTC[out], DTC list after extract
* Return        : the id after transfer
* Description   : Extract the DTCs
*******************************************************/
        public void ExtractDTC(byte[] baData, ref List<string> saDTC)
        {
            string sDTC = "";
            saDTC.Clear();
            for (int i = 3; i < baData.Length; i = i + 4)
            {
                if (baData[i] == 0xA0)
                {
                    if (baData[i + 1] == 0x00)
                    {
                        if (baData[i + 2] < 0x40)
                        {
                            sDTC = "0x" + baData[i].ToString("X2") + baData[i + 1].ToString("X2") + baData[i + 2].ToString("X2");
                        }
                        else
                        {
                            sDTC = "0x" + baData[i].ToString("X2") + baData[i + 1].ToString("X2") + baData[i + 3].ToString("X2");
                        }
                    }
                    else
                    {
                        if (baData[i + 2] == 0x00)
                        {
                            sDTC = "0x" + baData[i].ToString("X2") + baData[i + 2].ToString("X2") + baData[i + 3].ToString("X2");
                        }
                    }
                    saDTC.Add(sDTC);
                }
            }
        }


        /*******************************************************
        * Function name : ReadDTC
        * Parameter     : ichan[in], which channel need to Read DTC
        * Return        : NULL
        * Description   : Read DTC
        *******************************************************/
    

        public void ReadDTC(out string msg)
        {
            byte[] baRequest = { 0x19, 0x02, 0x09 };
            string readdtc = "19 02 09";
            byte[] baResponse = null;
            List<string> saDTC = new List<string>();
            saDTC.Clear();

            CANmsgSend(readdtc);
            baResponse = CANmsgReceived();
            //ExecuteService(ichan, baRequest, ref baResponse);
            int ilength = baResponse.Length;
            string date = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss: ");

            if (ilength > 0)
            {
                ExtractDTC(baResponse, ref saDTC);
                if (saDTC.Count == 0)
                {
                    msg = "";
                }
                else
                {
                    string sDTC = "DTC ";
                    string sDTC1 = "";
                    for (int i = 0; i < saDTC.Count; i++)
                    {
                        sDTC += saDTC[i];
                        sDTC1 += saDTC[i];
                        if (i < saDTC.Count - 1)
                        {
                            sDTC += ";";
                            sDTC1 += ";";
                        }
                    }
                    sDTC += " detect";
                    msg = date+ "Read DTC failed," +sDTC+"."+Environment.NewLine;
                }
            }
            else
            {
                msg = "";
            }
        }
        public static int[] chan1_Resetcounter = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        int m_iaResetCounter = 0;

        /*******************************************************
        * Function name : CheckResetCounter
        * Parameter     : ichan[in], which channel need to read reset counter
        * Return        : NULL
        * Description   : Read reset counter code
        *******************************************************/

        public void CheckResetCounter(out string msg)
        {
            byte[] baRequest = { 0x22, 0x11, 0x09 };
            string resetcounter = "22 11 09";
            byte[] baResponse = null;
            int[] baReset = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            CANmsgSend(resetcounter);
            baResponse = CANmsgReceived();
            baReset = chan1_Resetcounter;
            string date = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss: ");

            bool flagb = false;
            string sTemp = "";
            m_iaResetCounter = 1;
            if (baResponse.Length > 0)
            {
                if (baResponse[0] == 0x62)
                {
                    byte counter = baResponse[4];

                   if(counter!=0)
                    {

                    }
               
                }
                else if (baResponse[0] == 0x7F)
                {
                    sTemp = date+string.Format("Deteced reset counter  Negative response")+Environment.NewLine;
                    m_iaResetCounter = 0;
                }
            }
            else
            {
                sTemp = "";
                 m_iaResetCounter = 0;
            }

            msg = sTemp;
            chan1_Resetcounter = baReset;
        }
        /*******************************************************
* Function name : ResetCounterContent
* Parameter     : db[in], the counter raw data
* Return        : the meaning of the reset counter code 
* Description   : Extract the reset counter information
*******************************************************/
        string ResetCounterContent(int db)
        {
            switch (db)
            {
                case 0: return "Byte 2 (ResetCounter_EcuReset)";
                case 1: return "Byte 3 (ResetCounter_ClockMon)";
                case 2: return "Byte 4 (ResetCounter_POR)";
                case 3: return "Byte 5 (ResetCounter_Watchdog)";
                case 4: return "Byte 6 (ResetCounter_Traps)";
                case 5: return "Byte 7 (Counter_RamError)";
                case 6: return "Byte 8 (ResetCounter_WatchdogGraphicSystem)";
                case 7: return "Byte 9 (ResetCounter_TrapsGraphicSystem)";
                case 8: return "Byte 10 (ResetCounter_StackMonitor)";
                case 9: return "Byte 11 (Reset Counter Timeout Module)";
                case 10: return "Byte 12 ( Reset Counter Jump-To-Zero)";
                default: return "0";
            }
        }

        /*******************************************************
        * Function name : IC_Clock_Set
        * Parameter     : ichan[in], which channel need to set clock time
        * Return        : NULL
        * Description   : Set DUT clock time
        *******************************************************/
        public void IC_Clock_Set(out string msg)
        {
            byte hour, min;
            DateTime currentTime = DateTime.Now;
            min = (byte)currentTime.Minute;
            hour = (byte)currentTime.Hour;
            string baRequest = "10 03";
            byte[] baResponse = null;
            msg = "";
            CANmsgSend(baRequest);
            baResponse = CANmsgReceived();
          
            Thread.Sleep(200);

            CANmsgSend("2E F1 98 00 00 00 00 00 00");
            baResponse = CANmsgReceived();
         
            Thread.Sleep(200);
            CANmsgSend("2E 22 39 "+min.ToString().PadLeft(2,'0'));
            baResponse = CANmsgReceived();
            if(baResponse.Length==0||baResponse[0]!=0x6E)
            {
                msg = "Set clock error."+Environment.NewLine;
            }
            Thread.Sleep(100);
            CANmsgSend("2E 22 38 "+hour.ToString().PadLeft(2,'0'));
            if (baResponse.Length == 0 || baResponse[0] != 0x6E)
            {
                msg = "Set clock error." + Environment.NewLine;
            }
            baResponse = CANmsgReceived();
        }

        /*******************************************************
        * Function name : IC_Clock_Readtime
        * Parameter     : ichan[in], which channel need to read clock time
        * Return        : NULL
        * Description   : Read DUT clock time
        *******************************************************/
        public void IC_Clock_Readtime(out byte hour,out byte min,out byte second)
        {
            byte[] baRequest = { 0x22, 0x22, 0x38 };
            byte[] baResponse = null;
          
            string sTemp = "";
            hour = 0;min = 0;second = 0;
            for (int i = 0; i < 6; i++)
            {
                CANmsgSend("22 22 16");
                baResponse = CANmsgReceived();
             
                    if (baResponse.Length == 6 )
                    {
                        if (baResponse[3] == 0 && baResponse[4] == 0&& baResponse[5] == 0)
                        {
                        }
                        else
                        {
                            hour = baResponse[3];
                            min = baResponse[4];
                            second = baResponse[5];
                            i = 6;
                        }
                    }
            }
        }
        /*******************************************************
        * Function name : ClearDTC
        * Parameter     : ichan[in], which channel need to clear DTC
        * Return        : NULL
        * Description   : Clear DTC
        *******************************************************/
        public void ClearDTC(out string msg)
        {
            byte[] baRequest = { 0x14, 0xff, 0xff, 0xff };
            byte[] baResponse = null;
            CANmsgSend("14 ff ff ff");
            baResponse = CANmsgReceived();
            int ilength = baResponse.Length;
            msg = "";
            string date = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss: ");

            if (ilength > 0)
            {
                if (baResponse[0] == 0x54)
                {
                }
                else if (baResponse[0] == 0x7f)
                {
                    msg = date+"Clear DTC error."+Environment.NewLine;
                }
                else
                {
                    msg = date+"Clear DTC error respose." + Environment.NewLine;
                }
            }
            else
            {
                msg = date+"Clear DTC no response"+Environment.NewLine;
            }
        }
        /*******************************************************
      * Function name : ReadSWVersion
      * Parameter     : ichan[in], which channel need to read clock time
      * Return        : NULL
      * Description   : Read product software version 
      *******************************************************/
        public string ReadSWVersion(out string msg)
        {
            byte[] baResponse = null;
            string sInfo = "";
            string sDescription = "Read Software version";
            Customer_Session_Check();
            byte[] baRequest = { 0x22, 0xF1, 0X89 };
          
            CANmsgSend("22 F1 89");
            baResponse = CANmsgReceived();
            msg = "";
            string date = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss: ");

            if (baResponse.Length > 0)
            {
                if (baResponse.Length == 7)
                {
                    if (baResponse[0] == 0x62)
                    {
                        sInfo = ((char)baResponse[3]).ToString() + ((char)baResponse[4]).ToString() + ((char)baResponse[5]).ToString() + ((char)baResponse[6]).ToString();
                    }
                    else
                    {
                        msg = date+"Read SW version error :"+ByteToHex(baResponse);
                    }
                }
                else if (baResponse.Length == 3)
                {
                    if (baResponse[0] == 0x7F)
                    {
                        msg =date+ "Read SW version error :" + ByteToHex(baResponse);
                    }
                }
            }
            else
            {
                msg = date+"Read SW version error :" + ByteToHex(baResponse);
            }
            return sInfo;
        }

        /*******************************************************
        * Function name : ReadHWVersion
        * Parameter     : ichan[in], which channel need to read clock time
        * Return        : NULL
        * Description   : Read product hardware version 
        *******************************************************/
        public string ReadHWVersion(out string msg)
        {
            byte[] baResponse = null;
            string sInfo = "";
            string sDescription = "Read hardware version";
            string date = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss: ");

            Customer_Session_Check();
            byte[] baRequest = { 0x22, 0xF1, 0xA3 };
            CANmsgSend("22 F1 A3");
            baResponse = CANmsgReceived();
            msg = "";
            if (baResponse.Length > 0)
            {
                if (baResponse[0] == 0x7F)
                {
                    msg = date+"Read HW version error :" + ByteToHex(baResponse);
                }
                if (baResponse.Length > 5)
                {
                    if (baResponse[0] == 0x62)
                    {
                        sInfo = ((char)baResponse[3]).ToString() + ((char)baResponse[4]).ToString() + ((char)baResponse[5]).ToString();
                    }
                    else
                    {
                        msg =date+ "Read HW version error :" + ByteToHex(baResponse);
                    }
                }
            }
            else
            {
                msg = date+"Read HW version error :" + ByteToHex(baResponse);
            }
            return sInfo;
        }
        /*******************************************************
        * Function name : ReadFazitnum
        * Parameter     : ichan[in], which channel need to read clock time
        * Return        : NULL
        * Description   : Read product hardware version 
        *******************************************************/
        public string ReadFazitnum(out string msg)
        {

            byte[] baResponse = null;

            string sInfo = "";
            string sDescription = "Read Fazit number";

            Customer_Session_Check();
            string date = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss: ");

            byte[] baRequest = { 0x22, 0xF1, 0x7C };
            // byte[] baRequest = { 0x22, 0xFD, 0X7C };
            CANmsgSend("22 F1 7C");
            baResponse = CANmsgReceived();
            msg = "";
            if (baResponse.Length > 0)
            {
                if (baResponse[0] == 0x7F)
                {
                    msg = date+"Read Fazit version error :" + ByteToHex(baResponse);
                }
                if (baResponse.Length > 25)
                {
                    if (baResponse[0] == 0x62)
                    {
                        for (int i = 3; i < 26; i++)
                        {
                            sInfo += ((char)baResponse[i]).ToString();
                        }
                        // sSoftwareVersion = ((char)baResponse[3]).ToString() + ((char)baResponse[4]).ToString() + ((char)baResponse[5]).ToString();
                    }
                    else
                    {
                        msg =date+ "Read fazit version error :" + ByteToHex(baResponse);
                    }
                }
            }
            else
            {
                msg =date+ "Read fazit version error :" + ByteToHex(baResponse);
            }
            return sInfo;
        }
        /*******************************************************
        * Function name : ReadPartNumber
        * Parameter     : ichan[in], which channel need to read clock time
        * Return        : NULL
        * Description   : Read product hardware version 
        *******************************************************/
        public string ReadPartNumber(out string msg)
        {
            byte[] baResponse = null;
            string sInfo = "";
            string sDescription = "Read Part number";
            Customer_Session_Check();
            byte[] baRequest = { 0x22, 0xF1, 0X87 };
            // byte[] baRequest = { 0x22, 0xF1, 0X87 };
            CANmsgSend("22 F1 87");
            baResponse = CANmsgReceived();
            string date = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss: ");

            msg = "";
            if (baResponse.Length > 0)
            {
                if (baResponse[0] == 0x7F)
                {
                    msg =date+ "Read part number error :" + ByteToHex(baResponse);
                }
                if (baResponse.Length > 10)
                {
                    if (baResponse[0] == 0x62)
                    {
                        for (int i = 3; i < 14; i++)
                        {
                            sInfo += ((char)baResponse[i]).ToString();
                        }
                        //sSoftwareVersion = ((char)baResponse[3]).ToString() + ((char)baResponse[4]).ToString() + ((char)baResponse[5]).ToString();
                        int ilength = sInfo.Length;
                        string s1 = sInfo.Substring(0, 3);
                        string s2 = sInfo.Substring(3, 3);
                        string s3 = sInfo.Substring(6, ilength - 6);
                        sInfo = s1 + "." + s2 + "." + s3;
                    }
                    else
                    {
                        msg = date+"Read part number error :" + ByteToHex(baResponse);
                    }
                }
            }
            else
            {
                msg = date+"Read part number error :" + ByteToHex(baResponse);
            }
            return sInfo;
        }
     /*******************************************************
     * Function name : ReadVisteonPartNumber
     * Parameter     : ichan[in], which channel need to read clock time
     * Return        : NULL
     * Description   : Read product hardware version 
     *******************************************************/
        public string ReadVisteonPartNumber(out string msg)
        {
            byte[] baResponse = null;
            string sInfo = "";
            string sDescription = "Read Visteon part number";
            string date = DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss: ");

            Customer_Session_Check();
            byte[] baRequest = { 0x22, 0xFD, 0X0D };
            CANmsgSend("22 FD 0D");
            baResponse = CANmsgReceived();
            msg = "";
            if (baResponse.Length > 0)
            {
                if (baResponse[0] == 0x7F)
                {
                    msg = date+"Read visteon part number error :" + ByteToHex(baResponse);
                }
                if (baResponse.Length > 20)
                {
                    if (baResponse[0] == 0x62)
                    {
                        for (int i = 3; i < 21; i++)
                        {
                            sInfo += ((char)baResponse[i]).ToString();
                        }
                        //sSoftwareVersion = ((char)baResponse[3]).ToString() + ((char)baResponse[4]).ToString() + ((char)baResponse[5]).ToString();
                    }
                    else
                    {
                        msg = date+"Read visteon part number error :" + ByteToHex(baResponse);
                    }
                }
                else
                {
                    msg = date+"Read visteon part number error :" + ByteToHex(baResponse);
                }
            }
            else
            {
                msg =date+ "Read visteon part number error :" + ByteToHex(baResponse);
            }
            return sInfo;
        }

        public void Customer_Session_Check()
        {
            byte[] baResponse = null;
            string sTemp = "";
            CANmsgSend("10 61");
            baResponse = CANmsgReceived();
            Thread.Sleep(100);
        }

        public void ClearResetCounter()
        {
            byte[] baResponse = null;

            string sInfo = "";
            string sDescription = "Clear Reset counter";

            Customer_Session_Check();

            byte[] baRequest = { 0x2E, 0xFD, 0XEA, 0X01 };
            CANmsgSend("2E FD EA 01");
            baResponse = CANmsgReceived();


            /*
            if (baResponse.Length > 0)
            {
                if (baResponse[0] == 0x7F)
                {
                    ModBasic.OutputReport(sDescription, "Failed", "Negative response");
                }
                else if (baResponse.Length > 0)
                {
                    if (baResponse[0] == 0x62)
                    {
                       // for (int i = 3; i < 21; i++)
                       // {
                       //     sInfo += ((char)baResponse[i]).ToString();
                       // }
                        //sSoftwareVersion = ((char)baResponse[3]).ToString() + ((char)baResponse[4]).ToString() + ((char)baResponse[5]).ToString();
                        ModBasic.OutputReport(sDescription, "Done", sInfo);
                    }
                    else
                    {
                        ModBasic.OutputReport(sDescription, "Failed", "The response is " + baResponse[0].ToString("X2"));
                    }
                }
                else
                {
                    ModBasic.OutputReport(sDescription, "Failed", "The response length is wrong ");
                }

            }
            else
            {
                ModBasic.OutputReport(sDescription, "Failed", "No response");
            }
            */
        }

    }
}
