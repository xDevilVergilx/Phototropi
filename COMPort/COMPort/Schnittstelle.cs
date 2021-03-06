﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;

namespace RobotSerialPort
{
    class COMport
    {
        public static List<string> Log = new List<string>();
        public static SerialPort sport;
        public delegate void Received(string data);
        public static event Received DataReceived;

        private static double[] ServoAngle = new double[2];
        private static double[] Light = new double[5];
        private static string info = "";

        public string[] getLog()
        {
            return Log.ToArray();
        }

        private static void setLog(string info)
        {
            Log.Add(DateTime.Now.ToShortTimeString() + ": " + info + Environment.NewLine);
            if (Log.Count() > 10000)
            {
                Log.RemoveAt(0);
            }
        }

        public static string[] getPortNames()
        {
            setLog("getPorts:");
            List<string> ports = new List<string>();
            foreach (String portName in System.IO.Ports.SerialPort.GetPortNames())
            {
                ports.Add(portName);
                setLog(portName);
            }
            return ports.ToArray();
        }

        public static void Connect(String port)
        {
            try
            {
                setLog("Connect to: " + port);
                serialport_connect(port, 9600, Parity.Odd, 8, StopBits.One);
            }
            catch (Exception ex) { setLog(ex.Message); throw; }
        }

        public static bool Disconnect()
        {
            setLog("Disconnect");
            if (sport.IsOpen)
            {
                sport.Close();
                setLog("Port is Closed");
                return true;
            }
            setLog("Port was Closed");
            return false;
        }

        public static void Send(string data)
        {
            setLog("Send: " + data);
            sport.Write(data);
        }

        private static void serialport_connect(String port, int baudrate, Parity parity, int databits, StopBits stopbits)
        {
            sport = new SerialPort(port, baudrate, parity, databits, stopbits);
            try
            {
                sport.Open();
                sport.DataReceived += new SerialDataReceivedEventHandler(sport_DataReceived);
            }
            catch (Exception ex)
            {
                setLog(ex.Message);
                throw;
            }
        }

        public static void sport_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string input = sport.ReadExisting();
            setLog("Received: " + input);

            DataReceived(input);

            string[] data = input.Split(':');
            switch (data[0])
            {
                case "L0": Light[0] = double.Parse(data[1]); break;
                case "L1": Light[0] = double.Parse(data[1]); break;
                case "L2": Light[0] = double.Parse(data[1]); break;
                case "L3": Light[0] = double.Parse(data[1]); break;
                case "L4": Light[0] = double.Parse(data[1]); break;

                case "S0": Light[0] = double.Parse(data[1]); break;
                case "S1": Light[0] = double.Parse(data[1]); break;
                default: info = input; break;
            }
        }

        public static double getServoAngle(int ID)
        {
            return ServoAngle[ID];
        }

        public static double getLight(int ID)
        {
            return Light[ID];
        }

        public static string getInfo(int ID)
        {
            return info;
        }

    }
}
