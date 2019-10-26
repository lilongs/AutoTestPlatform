using System;
using System.IO.Ports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace RS485
{
   public  class RS485Control
    {
  

   SerialPort sp = new SerialPort();

 
   public  int OPenPortsub(String Port)
   {
       if (!sp.IsOpen)
       {
           sp.PortName = Port;
           sp.BaudRate = 9600;
           sp.DataBits = 8;
           sp.StopBits = System.IO.Ports.StopBits.One;
           sp.Parity = System.IO.Ports.Parity.None;
           sp.ReadTimeout = 2000;
           sp.WriteTimeout = 2000;
           try
           {
              sp.Open();
                    return 1;
                }
           catch (System.Exception ex)
           {             
               sp.Close();
               sp.Dispose();
               return 2;
           }
       }
        else
        {
                sp.Close();
                sp.Dispose();
            }
       return 2;
   }

   public int OPenPort(string Port)
        {
            int result = OPenPortsub(Port);
            while(result==2)
            {
                OPenPortsub(Port);
                Thread.Sleep(500);
            }
            return 0;
         }
   public  int ClosePort(String Port)
   {

       if (sp.IsOpen)
       {
           try
           {
              sp.Close();
              sp.Dispose();
           }
           catch (System.Exception ex)
           {
             // MessageBox.Show("Error:" + ex.Message, "Error");
             sp.Dispose();
             return 2;
           }
       }
            return 1;

    }

   public static byte[] crc16bitbybit(byte[] data)
        {
            int len = data.Length;
            if (len > 0)
            {
                ushort crc = 0xFFFF;

                for (int i = 0; i < len; i++)
                {
                    crc = (ushort)(crc ^ (data[i]));
                    for (int j = 0; j < 8; j++)
                    {
                        crc = (crc & 1) != 0 ? (ushort)((crc >> 1) ^ 0xA001) : (ushort)(crc >> 1);
                    }
                }
                byte hi = (byte)((crc & 0xFF00) >> 8);  //高位置
                byte lo = (byte)(crc & 0x00FF);         //低位置

       
            byte[] bt = new byte[data.Length + 2];
            for (int i = 0; i < data.Length; i++)
            {
                bt[i] = data[i];
            }
            bt[data.Length+1] = hi;
            bt[data.Length ] = lo;
            return (bt);
            }
            return new byte[] { 0, 0 };
        }
    public  int send(string Port, byte[] data)
    {
       
        if (sp.IsOpen)
        {

            try
            {
               sp.Write(data, 0, data.Length);      //写数据

            }
            catch
            {
              //  MessageBox.Show("串口数据发送出错，请检查.", "错误");//错误处理
               
                return -1;
            }
            return 1;
        }
      
        return -1;
    }
    public int Send(string Port,byte[]data)
    {
        int i = send(Port, data);
        int cnt = 0;
        while(i==-1 && cnt<10)
        {
            i = send(Port, data);
            Thread.Sleep(50);
            cnt++;
        }
        if (cnt == 10)
        {
            return -1;
        }
        else return 0;
    }
    public  byte[] Recv(string Port)
    {
        //  SerialPort SP = new SerialPort();
        //  SP.PortName = Port;
    
        Byte[] receivedData = new Byte[sp.BytesToRead]; //创建接收字节数组 
        if (sp.IsOpen)
        {
            byte[] byteRead = new byte[sp.BytesToRead]; //BytesToRead:sp1接收的字符个数 
            try
            {

               sp.Read(receivedData, 0, receivedData.Length); //读取数据 
               sp.DiscardInBuffer(); //清空SerialPort控件的Buffer 
                string strRcv = null;
                for (int i = 0; i < receivedData.Length; i++) //窗体显示 
                {

                    strRcv += receivedData[i].ToString("X2"); //16进制显示 
                }

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "出错提示");

            }
        }
        
        return receivedData;


    }
	
	
}
    }

