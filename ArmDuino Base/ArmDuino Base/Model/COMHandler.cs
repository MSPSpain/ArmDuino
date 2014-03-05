﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows;

namespace ArmDuino_Base.Model
{
    class COMHandler
    {
        public static SerialPort Port;
        public bool isConnected = false;


        public COMHandler()
        {

        }

        public void Initialize()
        {
            string[] portnames = SerialPort.GetPortNames();

            while (portnames.Length == 0)
            {
                MessageBoxResult error = MessageBox.Show("No COM Port found", "Le Fail", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                if (error == MessageBoxResult.OK)
                {
                    portnames = SerialPort.GetPortNames();
                }
                else
                {
                    Application.Current.Shutdown();
                    throw new Exception("PUM");
                }
            }

            Port = new SerialPort("COM6", 115200);
            while (!Port.IsOpen)
            {
                try
                {
                    Port.Open();
                }
                catch (Exception)
                {
                    MessageBoxResult error = MessageBox.Show("No COM Port found", "Le Fail", MessageBoxButton.OKCancel, MessageBoxImage.Error);
                    if (error == MessageBoxResult.OK)
                    {
                        continue;
                    }
                    else
                    {
                        Application.Current.Shutdown();
                        throw new Exception("PUM");
                    }
                }
            }
        }



        public String dataToString(Arm currentArm)
        {
            String result = "";
            for (int i = 0; i < currentArm.CurrentAngles.Length; i++)
            {
                if (currentArm.CurrentAngles[i] < 10)
                {
                    result += "00" + currentArm.CurrentAngles[i];
                }
                else if (currentArm.CurrentAngles[i] < 100)
                {
                    result += "0" + currentArm.CurrentAngles[i];
                }
                else result += currentArm.CurrentAngles[i];
            }
            return result;
        }

        public byte[] dataToBytes(Arm currentArm)
        {
            char[] result = dataToString(currentArm).ToCharArray();
            byte[] buffer = new byte[21];
            for (int i = 0; i < result.Length; i++)
            {
                buffer[i] = (byte)Int32.Parse(""+result[i]);
            }
            return buffer;
        }

        public void writeDataBytes(Arm currentArm)
        {
            try
            {
                byte[] buffer = dataToBytes(currentArm);
                Port.Write(buffer, 0, buffer.Length);
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.StackTrace);
            }
        }
    }
}
