using System;
using System.IO.Ports;
using System.Text;

namespace gdled
{
    class Arduino
    {
        SerialPort Port;

        public Arduino(string portName)
        {
            Port = new SerialPort(portName, 9600);
            Port.Open();
            Port.DataReceived += this.Port_DataReceived;
        }

        public void SendProgres(int progres)
        {
            byte[] type = new byte[] { 1 };
            byte[] pByte = BitConverter.GetBytes(progres);

            byte[] result = new byte[5];
            type.CopyTo(result, 0);
            pByte.CopyTo(result, 1);
            Port.Write(result, 0, result.Length);
        }

        void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buffer = new byte[Port.BytesToRead];
            Port.Read(buffer, 0, Port.BytesToRead);
            Console.WriteLine($"[Arduino] {Encoding.ASCII.GetString(buffer)}");
        }

        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }
    }
}
