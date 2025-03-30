using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace winver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label11.Text = "Version: " + ReadRegistryValue(@"SYSTEM\LumiNT", "version");
            label12.Text = "Release Date: " + ReadRegistryValue(@"SYSTEM\LumiNT", "release");
            label2.Text = "CPU: " + GetHardwareInfo("Win32_Processor", "Name");
            label3.Text = "RAM: " + GetRamInfo();
            label4.Text = "Storage: " + GetStorageInfo();
            label5.Text = "GPU: " + GetHardwareInfo("Win32_VideoController", "Name");
        }
        static string ReadRegistryValue(string subKey, string valueName)
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(subKey))
            {
                return key?.GetValue(valueName)?.ToString() ?? "";
            }
        }
        static string GetHardwareInfo(string hwclass, string syntax)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM {hwclass}");
            foreach (ManagementObject obj in searcher.Get())
            {
                return $"{obj[syntax]}";
            }
            return "Unknown";
        }
        static string GetRamInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
            foreach (ManagementObject obj in searcher.Get())
            {
                return $"{Convert.ToInt64(obj["TotalPhysicalMemory"]) / (1024 * 1024 * 1024)} GB";
            }
            return "Unknown";
        }
        static string GetStorageInfo()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
            foreach (ManagementObject obj in searcher.Get())
            {
                return $"{obj["Model"]}, {Convert.ToInt64(obj["Size"]) / (1024 * 1024 * 1024)} GB";
            }
            return "Unknown";
        }
    }
}
