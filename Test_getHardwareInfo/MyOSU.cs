using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
//using System.Windows.Forms;

namespace Eclipse_MyOSU
{


    class MyOSU
    {

        public MyOSU()
        {


            //// Pre run the Data

            // CPU ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            try
            {
                ManagementObjectSearcher myVideoObject1 = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (ManagementObject obj in myVideoObject1.Get()) { CPU_Name = obj["Name"].ToString(); }
            }
            catch (Exception ex)
            {
                CPU_Name = "CPU : ";
                //MessageBox.Show("osu:CPU:Name" + ex.Message);
            }

            try
            {
                ManagementObjectSearcher myVideoObject1 = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (ManagementObject obj in myVideoObject1.Get()) { CPU_ProcessorID = obj["ProcessorID"].ToString(); }
            }
            catch (Exception ex)
            {
                CPU_ProcessorID = "PID : ";
                //MessageBox.Show("osu:CPU:ProcessorID" + ex.Message);
            }

            try
            {
                ManagementObjectSearcher myVideoObject1 = new ManagementObjectSearcher("select * from Win32_Processor");
                foreach (ManagementObject obj in myVideoObject1.Get()) { CPU_VirtualizationFirmwareEnabled = obj["VirtualizationFirmwareEnabled"].ToString(); }
            }
            catch (Exception ex)
            {
                CPU_VirtualizationFirmwareEnabled = "VT : ";
                //MessageBox.Show("osu:CPU:VirtualizationFirmwareEnabled" + ex.Message);
            }



            // GPU ----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            try
            {
                ManagementObjectSearcher myVideoObject2 = new ManagementObjectSearcher("select * from Win32_VideoController");
                foreach (ManagementObject obj in myVideoObject2.Get())
                {
                    if (obj["Name"].ToString().Contains("Microsoft Basic") == false
                        &&
                        obj["Name"].ToString().Contains("HD Graphics") == false
                         &&
                        obj["Name"].ToString().Contains("mv video hook driver2") == false
                        )
                    {
                        GPU_Name = obj["Name"].ToString();
                    }

                }
            }
            catch (Exception ex)
            {
                GPU_Name = "GPU : ";
                //MessageBox.Show("osu:GPU:Name" + ex.Message);
            }

            try
            {
                ManagementObjectSearcher myVideoObject2 = new ManagementObjectSearcher("select * from Win32_VideoController");
                foreach (ManagementObject obj in myVideoObject2.Get()) { GPU_AdapterRAM = (Int64.Parse(obj["AdapterRAM"].ToString()) / 1024 / 1024).ToString(); }
            }
            catch (Exception ex)
            {
                GPU_AdapterRAM = "ARAM : ";
                //MessageBox.Show("osu:GPU:AdapterRAM" + ex.Message);
            }


            // MOBO - MotherBoard 
            try
            {
                ManagementObjectSearcher myVideoObject3 = new ManagementObjectSearcher("select * from Win32_BaseBoard");
                foreach (ManagementObject obj in myVideoObject3.Get())
                {
                    MOBO_Manufacturer = obj["Manufacturer"].ToString();
                    MOBO_Product = obj["Product"].ToString();
                    MOBO_SerialNumber = obj["SerialNumber"].ToString();
                    MOBO_Version = obj["Version"].ToString();
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("osu:MOBO" + ex.Message);
            }


            // RAM
            try
            {
                ManagementObjectSearcher myVideoObject4 = new ManagementObjectSearcher("select * from Win32_PhysicalMemory");
                foreach (ManagementObject obj in myVideoObject4.Get())
                {
                    RAM_Manufacturer += obj["Manufacturer"].ToString().Trim() + ",";
                    RAM_PartNumber += obj["PartNumber"].ToString().Trim() + ",";
                    RAM_Capacity += (Int64.Parse(obj["Capacity"].ToString()) / 1024 / 1024).ToString().Trim() + ",";
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("osu:RAM" + ex.Message);
            }

            // NIC
            try
            {
                ManagementObjectSearcher myVideoObject5 = new ManagementObjectSearcher("root/WMI", "select * from MSNdis_PhysicalMediumType");
                foreach (ManagementObject obj in myVideoObject5.Get())
                {
                    NIC_InstanceName = obj["InstanceName"].ToString();
                }
                NIC_IP = Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
                NIC_MAC = get_MAC();
                NIC_Speed = get_NetworkAdapterSpeed();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("osu:NIC" + ex.Message);
            }


            // MON 

            // Monitor Manufacturer
            try
            {
                //    foreach (var target in WindowsDisplayAPI.DisplayConfig.PathDisplayTarget.GetDisplayTargets())
                //    {
                //        if (target.FriendlyName.Trim() != "")
                //        {
                //            MON_Manufacturer = target.FriendlyName;
                //           //MessageBox.Show(MON_Manufacturer);
                //        }
                //    }
            }
            catch (Exception)
            {

                throw;
            }


            // Monitor Refresh Rate 
            try
            {
                ManagementObjectSearcher myVideoObject6 = new ManagementObjectSearcher("root/CIMV2", "select * from Win32_VideoController ");
                foreach (ManagementObject obj in myVideoObject6.Get())
                {

                    try
                    {
                        MON_FPS = obj["CurrentRefreshRate"].ToString();
                        //MessageBox.Show(MON_FPS);
                        break;
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show("Monitor Refresh Rate :" + ex.Message);
                    }


                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("osu:MON" + ex.Message);
            }


            // PC 
            try
            {
                //MessageBox.Show("a" + Environment.MachineName);
                PC_Name = Environment.MachineName;

                ManagementObjectSearcher myVideoObject7 = new ManagementObjectSearcher("root/CIMV2", "select * from Win32_OperatingSystem");
                foreach (ManagementObject obj in myVideoObject7.Get())
                {
                    PC_OS = obj["Caption"].ToString();
                    PC_OS_Ver = obj["Version"].ToString();
                }

                PC_StartUpTime = UpTime.ToString();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("osu:PC" + ex.Message);
            }

        }

        public TimeSpan UpTime
        {
            get
            {
                using (var uptime = new PerformanceCounter("System", "System Up Time"))
                {
                    uptime.NextValue();       //Call this an extra time before reading its value
                    return TimeSpan.FromSeconds(uptime.NextValue());
                }
            }
        }


        //// ----------------------------------------------------------------------------------------------------- Computer Info - ADVANCE
        public string CPU_Name { set; get; }
        public string CPU_ProcessorID { set; get; }

        public string CPU_VirtualizationFirmwareEnabled { set; get; }

        public string GPU_Name { set; get; }
        public string GPU_AdapterRAM { set; get; }

        public string MOBO_Manufacturer { set; get; }
        public string MOBO_Product { set; get; }
        public string MOBO_SerialNumber { set; get; }
        public string MOBO_Version { set; get; }

        public string RAM_Manufacturer { set; get; }
        public string RAM_PartNumber { set; get; }
        public string RAM_Capacity { set; get; }

        public string NIC_InstanceName { set; get; }

        public string NIC_IP { set; get; }
        public string NIC_MAC { set; get; }
        public string NIC_Speed { set; get; }

        public string MON_FPS { set; get; }

        public string MON_Manufacturer { set; get; }

        public string PC_Name { set; get; }

        public string PC_OS { set; get; }
        public string PC_OS_Ver { set; get; }

        public string PC_StartUpTime { set; get; }


        //// ----------------------------------------------------------------------------------------------------- Computer Info (To be Replace)


        // Get PC name
        public string get_PCName()
        {
            return Environment.MachineName;
        }

        // Get IP
        public string get_IP()
        {
            return Dns.GetHostByName(Dns.GetHostName()).AddressList[0].ToString();
        }

        // Get MAC
        public string get_MAC()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            return sMacAddress;
        }

        // Get TimeNow
        public string get_DateTime_Now()
        {
            return DateTime.Now.ToString("yyyy:MM:dd hh:mm:ss");
        }

        // Get GPU
        public string get_GPU()
        {
            string result = "";
            // get GPU info
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                result = queryObj["Description"].ToString();
            }
            return result;
        }

        public string get_NetworkAdapterSpeed()
        {

            string result = "";
            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                result += netInterface.Name.Substring(0, 3) + ":"; // Name
                result += netInterface.Speed / 1000 / 1000 + "M" + ", "; // Speed
                //result += netInterface.GetPhysicalAddress().ToString() + "\n\n";// MAC
            }
            return result;



            //string result = "";
            //NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            //foreach (NetworkInterface adapter in adapters)
            //{
            //    IPInterfaceProperties properties = adapter.GetIPProperties();
            //    IPv4InterfaceStatistics stats = adapter.GetIPv4Statistics();
            //    ////MessageBox.Show("adapter.Description = " + adapter.Description);
            //    result = (adapter.Speed / 1024 / 1024).ToString();
            //    ////MessageBox.Show("stats.OutputQueueLength = " + stats.OutputQueueLength);
            //}
            //return result;
        }


















        //// ----------------------------------------------------------------------------------------------------- CMD

        // run CMD - By Squence - Hidden
        public void run_Cmd_bySequence(object command)
        {

            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                //procStartInfo.RedirectStandardOutput = true;
                //procStartInfo.UseShellExecute = false;
                //// Do not create the black window.
                //procStartInfo.CreateNoWindow = false;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output. 
            }
            catch (Exception)
            {
                // Log the exception
            }
        }

        // run CMD - By Squence - Hidden
        public void run_Cmd_bySequence_Hidden(object command)
        {

            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = false;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output.

            }
            catch (Exception)
            {
                // Log the exception
            }
        }

        // run CMD - By Pararal - Hidden
        public void run_Cmd_byParallel_Hidden(object command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                //procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = true;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output. 
            }
            catch (Exception)
            {
                // Log the exception
            }
        }

        // run CMD - By Pararal
        public void run_Cmd_byParallel(object command)
        {
            try
            {
                // create the ProcessStartInfo using "cmd" as the program to be run,
                // and "/c " as the parameters.
                // Incidentally, /c tells cmd that we want it to execute the command that follows,
                // and then exit.
                System.Diagnostics.ProcessStartInfo procStartInfo =
                    new System.Diagnostics.ProcessStartInfo("cmd", "/c " + command);

                // The following commands are needed to redirect the standard output.
                // This means that it will be redirected to the Process.StandardOutput StreamReader.
                //procStartInfo.RedirectStandardOutput = true;
                procStartInfo.UseShellExecute = false;
                // Do not create the black window.
                procStartInfo.CreateNoWindow = false;
                // Now we create a process, assign its ProcessStartInfo and start it
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo = procStartInfo;
                proc.Start();
                // Get the output into a string
                string result = proc.StandardOutput.ReadToEnd();
                // Display the command output. 
            }
            catch (Exception)
            {
                // Log the exception
            }
        }

        //// ----------------------------------------------------------------------------------------------------- Check Prog

        // Get Running Program Lit
        public ArrayList get_RunningProgramList()
        {
            ArrayList result1 = new ArrayList();
            ArrayList result2 = new ArrayList();

            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Process ");

                //rtb_Disp.Text += "# Win32_Process " + "\n";
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    //result1.Add(queryObj["Caption"] + "|" + queryObj["CommandLine"] + "|" + queryObj["ExecutablePath"]);
                    result1.Add(queryObj["Caption"] + "|" + queryObj["ExecutablePath"]);
                    //result1.Add(queryObj["ExecutablePath"]);
                }

                // Sort Accending Order
                result1.Sort();

                // Distinct Duplicate Value
                string temStr = "";


                foreach (string item in result1)
                {
                    if (temStr != item)
                    {
                        result2.Add(temStr);
                        temStr = item;
                    }
                    else
                    {
                        temStr = item;
                    }
                }
            }
            catch (ManagementException)
            {
                ////MessageBox.Show("An error occurred while querying for WMI data: " + ex.Message);
            }
            return result2;
        }

        // check Running Program Status
        public bool Check_ProgramRunningStatus(string tmpStr1)
        {
            bool tmpBool = false;
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Process ");

                //rtb_Disp.Text += "# Win32_Process " + "\n";
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["Caption"].ToString() == tmpStr1)
                    {
                        tmpBool = true;
                        ////MessageBox.Show("1." + queryObj["Caption"].ToString() + " | " + tmpStr1 + " | " + tmpBool + "\n" );
                    }

                }
            }
            catch (ManagementException)
            {
                ////MessageBox.Show("An error occurred while querying for WMI data: " + ex.Message);
                tmpBool = false;
            }
            return tmpBool;
        }

        // check Running Program Status And Location Exist
        public bool Check_ProgramRunningStatusAndLocationExist(string tmpStr1, string tmpStr2)
        {
            bool tmpBool = false;
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                    "SELECT * FROM Win32_Process ");

                //rtb_Disp.Text += "# Win32_Process " + "\n";
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    if (queryObj["Caption"].ToString() == tmpStr1)
                    {

                        string p1 = queryObj["ExecutablePath"].ToString();

                        ArrayList al = new ArrayList();
                        al.AddRange(p1.Split('\\'));
                        al.RemoveAt(al.Count - 1);

                        string p2 = "";
                        foreach (var item in al)
                        {
                            p2 += item + "\\";
                        }

                        // Check Dir Exist
                        if (Directory.Exists(p1 + tmpStr2) == true)
                        {
                            tmpBool = true;
                        }
                        else if (Directory.Exists(p1 + tmpStr2) == false)
                        {
                            tmpBool = false;
                        }

                    }
                    if (queryObj["Caption"].ToString() != tmpStr1)
                    {

                        tmpBool = false;
                    }
                }
            }
            catch (ManagementException)
            {
                ////MessageBox.Show("An error occurred while querying for WMI data: " + ex.Message);
                tmpBool = false;
            }
            return tmpBool;
        }



        // check Running Program Title and Return Exe Name
        public string get_ProgramRunningTitle_returnExeName(string windowsTitleName)
        {
            Process[] processlist = Process.GetProcesses();

            string result = "";
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    if (process.MainWindowTitle.ToLower().Trim().Contains(windowsTitleName.ToLower().Trim())// t
                            &&

                            !(
                            
                            process.MainWindowTitle.ToLower().Trim().Contains("chrome") // ori : chrome , snoppy  need disable this

                            ||

                            process.MainWindowTitle.ToLower().Trim().Contains("firefox")// ori : firefox, snoppy  need disable this

                            ||

                            process.MainWindowTitle.ToLower().Trim().Contains("edge") // ori : edge, snoppy  need disable this 

                            )
                        )
                    {
                        result = process.ProcessName + ".exe";
                        break;
                    }
                    else
                    {
                        result = "NA";
                    }
                }
            }

            return result;
        }



        //// ----------------------------------------------------------------------------------------------------- Read & Write Text File         

        //
        ArrayList result = new ArrayList();
        public ArrayList Read_TextFile(string fileLoc)
        {
            result.Clear();

            using (StreamReader reader = new StreamReader(fileLoc))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                    {
                        break;
                    }
                    result.Add(line);
                }
            }
            return result;

        }

        public bool Write_TextFile(string fileLoc, string content, bool mode)
        {
            bool temp = false;

            using (StreamWriter writer = new StreamWriter(fileLoc, mode))
            {
                writer.WriteLine(content);
            }

            return temp;
        }












        //// ----------------------------------------------------------------------------------------------------- Block Keyboard

        // Disable TaskManager
        public void DisableTaskManager()
        {
            RegistryKey regkey = default(RegistryKey);
            string keyValueInt = "1";
            string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
            try
            {
                regkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(subKey);
                regkey.SetValue("DisableTaskMgr", keyValueInt);
                regkey.Close();
            }
            catch (Exception)
            {
                ////MessageBox.Show("#01a\n" + ex.Message);
            }

        }

        // Enable TaskManager
        public void EnableTaskManager()
        {
            RegistryKey regkey = default(RegistryKey);
            //string keyValueInt = "0";
            //0x00000000 (0)
            string subKey = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";
            try
            {
                regkey = Registry.CurrentUser.CreateSubKey(subKey);
                regkey.DeleteValue("DisableTaskMgr"); // in win10 : if this NOT exist TM will Enabled
                regkey.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("#01b\n" + ex.Message);
            }

        }

        // Block Global - Alt + Tab, Alt + Esc , Alt + F4, Left Win, Right Win, Ctrl + Esc 
        public const int WH_KEYBOARD_LL = 13;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int VK_TAB = 0x9;
        public const int VK_MENU = 0x12; /* Alt key */
        public const int VK_ESCAPE = 0x1B;
        public const int VK_F4 = 0x73;
        public const int VK_LWIN = 0x5B;
        public const int VK_RWIN = 0x5C;
        public const int VK_CONTROL = 0x11;
        public const int VK_LCONTROL = 0xA2;
        public const int VK_RCONTROL = 0xA3;

        [StructLayout(LayoutKind.Sequential)]
        public class KeyBoardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall, SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        static int hKeyboardHook = 0;
        HookProc KeyboardHookProcedure;

        public void HookStart()
        {
            if (hKeyboardHook == 0)
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                using (Process curProcess = Process.GetCurrentProcess())
                using (ProcessModule curModule = curProcess.MainModule)
                {
                    IntPtr hModule = GetModuleHandle(curModule.ModuleName);
                    hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, hModule, 0);
                }
                if (hKeyboardHook == 0)
                {
                    int error = Marshal.GetLastWin32Error();
                    HookStop();
                    throw new Exception("SetWindowsHookEx() function failed. " + "Error code: " + error.ToString());
                }
            }
        }

        public void HookStop()
        {
            bool retKeyboard = true;
            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }
            if (!(retKeyboard))
            {
                throw new Exception("UnhookWindowsHookEx failed.");
            }
        }

        private int KeyboardHookProc(int nCode, int wParam, IntPtr lParam)
        {
            KeyBoardHookStruct kbh = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
            bool bMaskKeysFlag = false;
            switch (wParam)
            {
                case WM_KEYDOWN:
                case WM_KEYUP:
                case WM_SYSKEYDOWN:
                case WM_SYSKEYUP:
                    bMaskKeysFlag = ((kbh.vkCode == VK_TAB) && (kbh.flags == 32))      /* Tab + Alt */
                                    | ((kbh.vkCode == VK_ESCAPE) && (kbh.flags == 32))   /* Esc + Alt */
                                    | ((kbh.vkCode == VK_F4) && (kbh.flags == 32))       /* F4 + Alt */
                                    | ((kbh.vkCode == VK_LWIN) && (kbh.flags == 1))    /* Left Win */
                                    | ((kbh.vkCode == VK_RWIN) && (kbh.flags == 1))    /* Right Win */
                                    | ((kbh.vkCode == VK_ESCAPE) && (kbh.flags == 0)); /* Ctrl + Esc */
                    break;
                default:
                    break;
            }

            if (bMaskKeysFlag == true)
            {
                return 1;
            }
            else
            {
                return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
            }
        }

        //// ----------------------------------------------------------------------------------------------------- Block Mouse

        // Block Global - Mouse Input
        [DllImport("user32.dll")]
        private static extern bool BlockInput(bool block);

        public void Mouse_Freeze()
        {
            BlockInput(true);
        }

        public void Mouse_Unfreeze()
        {
            BlockInput(false);
        }


        //// ----------------------------------------------------------------------------------------------------- Change Mouse Cursors

        string AppStarting = "";
        string Arrow = "";
        string Hand = "";
        string Help = "";
        string IBeam = "";
        string No = "";

        string NWPen = "";
        string Person = "";
        string Pin = "";
        string SizeAll = "";
        string SizeNESW = "";

        string SizeNS = "";
        string SizeNWSE = "";
        string SizeWE = "";
        string UpArrow = "";
        string Wait = "";

        private void Get_MouseCursor_OriValue()
        {
            RegistryKey rkey = Registry.CurrentUser.OpenSubKey(@"Control Panel\Cursors\");
            if (rkey != null)
            {
                AppStarting = (string)rkey.GetValue("AppStarting");
                Arrow = (string)rkey.GetValue("Arrow");
                Hand = (string)rkey.GetValue("Hand");
                Help = (string)rkey.GetValue("Help");
                IBeam = (string)rkey.GetValue("IBeam");
                No = (string)rkey.GetValue("No");

                NWPen = (string)rkey.GetValue("NWPen");
                Person = (string)rkey.GetValue("Person");
                Pin = (string)rkey.GetValue("Pin");
                SizeAll = (string)rkey.GetValue("SizeAll");
                SizeNESW = (string)rkey.GetValue("SizeNESW");

                SizeNS = (string)rkey.GetValue("SizeNS");
                SizeNWSE = (string)rkey.GetValue("SizeNWSE");
                SizeWE = (string)rkey.GetValue("SizeWE");
                UpArrow = (string)rkey.GetValue("UpArrow");
                Wait = (string)rkey.GetValue("Wait");
            }
            else
            {
                //MessageBox.Show("#4");
            }

            //MessageBox.Show(
            //"AppStarting = " + AppStarting + "\n" +
            //"Arrow = " + Arrow + "\n" +
            //"Hand = " + Hand + "\n" +
            //"Help = " + Help + "\n" +
            //"No = " + No + "\n" +

            //"NWPen = " + NWPen + "\n" +
            //"Person = " + Person + "\n" +
            //"Pin = " + Pin + "\n" +
            //"SizeAll = " + SizeAll + "\n" +
            //"SizeNESW = " + SizeNESW + "\n" +

            //"SizeNS = " + SizeNS + "\n" +
            //"SizeNWSE = " + SizeNWSE + "\n" +
            //"SizeWE = " + SizeWE + "\n" +
            //"UpArrow = " + UpArrow + "\n" +
            //"Wait = " + Wait + "\n"
            //);
        }

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, uint pvParam, uint fWinIni);
        const int SPI_SETCURSORS = 0x0057;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDCHANGE = 0x02;

        public void Change_MouseCursor_toInvisible()
        {
            try
            {
                Get_MouseCursor_OriValue();

                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "AppStarting", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Arrow", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                //Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Hand", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Help", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "IBeam", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "No", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");

                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "NWPen", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                //Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Person", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                //Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Pin", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "SizeAll", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "SizeNESW", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");

                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "SizeNS", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "SizeNWSE", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "SizeWE", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "UpArrow", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Wait", System.IO.Directory.GetCurrentDirectory() + @"\invisible.cur");

                SystemParametersInfo(SPI_SETCURSORS, 0, 0, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("#5\n" + ex.Message);
            }
        }

        public void Change_MouseCursor_toOriginal()
        {
            try
            {

                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "AppStarting", AppStarting);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Arrow", Arrow);
                //Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Hand", Hand);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Help", Help);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "IBeam", IBeam);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "No", No);

                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "NWPen", NWPen);
                //Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Person", Person);
                //Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Pin", Pin);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "SizeAll", SizeAll);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "SizeNESW", SizeNESW);

                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "SizeNS", SizeNS);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "SizeNWSE", SizeNWSE);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "SizeWE", SizeWE);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "UpArrow", UpArrow);
                Registry.SetValue(@"HKEY_CURRENT_USER\Control Panel\Cursors\", "Wait", Wait);


                SystemParametersInfo(SPI_SETCURSORS, 0, 0, SPIF_UPDATEINIFILE | SPIF_SENDCHANGE);
            }
            catch (Exception ex)
            {

            }
        }

        //// ----------------------------------------------------------------------------------------------------- Get Current Mouse Cursor Position XY
        ///ie. MyOSU.GetCursorPosition()  <---- must direct use ClassName call

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);
        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }


    }

}
