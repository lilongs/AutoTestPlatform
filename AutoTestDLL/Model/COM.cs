using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoTestDLL.Model
{
    public class COM
    {
        public string portName { get; set; }
        public int baudrate { get; set; }
        public Parity parity { get; set; }
        public int dataBits { get; set; }
        public StopBits stopBits { get; set; }
        public Handshake handshake { get; set; }

        private SerialPort SP_ReadData = null;

        public byte[] read_data = new byte[] { };

        public COM(){}

        public COM(string PortName, int BaudRate, int DataBits, StopBits StopBits, Parity Parity)
        {
            SerialPort serial = new SerialPort();
            SP_ReadData.PortName = PortName;
            SP_ReadData.BaudRate = BaudRate;
            SP_ReadData.DataBits = DataBits;
            SP_ReadData.StopBits = StopBits;
            SP_ReadData.Parity = Parity;

            this.SP_ReadData = serial;
            this.portName = PortName;
            this.baudrate = BaudRate;
            this.dataBits = DataBits;
            this.stopBits = StopBits;
            this.parity = Parity;
        }

        public void Open()
        {
            if (!SP_ReadData.IsOpen)
            {
                SP_ReadData.DataReceived += SP_ReadData_DataReceived;
                SP_ReadData.Open();
            }            
        }

        private void SP_ReadData_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // 接收数据
            byte[] buffer = null;
            byte[] data = new byte[2048];
            int receiveCount = 0;
            while (true)
            {
                System.Threading.Thread.Sleep(20);
                if (SP_ReadData.BytesToRead < 1)
                {
                    buffer = new byte[receiveCount];
                    Array.Copy(data, 0, buffer, 0, receiveCount);
                    break;
                }

                receiveCount += SP_ReadData.Read(data, receiveCount, SP_ReadData.BytesToRead);
            }

            if (receiveCount == 0) return;
            data = buffer;
        }

        public void Send(byte[] bt)
        {
            SP_ReadData?.Write(bt, 0, bt.Length);
        }

        public void Close()
        {
            SP_ReadData.Close();
        }
    }
}
