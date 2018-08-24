using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace cdrom
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            int i = 0;   
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int time = 0;
            for (int j = 0; j < 300; j++)
            {
                if (j % 2 == 0 && time == 0)
                {
                    string[] logDrives = System.IO.Directory.GetLogicalDrives();
                    string s = "";
                    StringBuilder volumeName = new StringBuilder(256);
                    int srNum = new int();
                    int comLen = new int();
                    string sysName = "";
                    int sysFlags = new int();
                    int result;
                    

                    for (int i = 0; i < logDrives.Length; i++)
                    {

                        if (api.GetDriveType(logDrives[i]) == 5)
                        {
                            s += "Your CD ROM is on drive : " +
                                      logDrives[i].ToString() + "\n";
                            result = api.GetVolumeInformation(logDrives[i].ToString(),
                                    volumeName, 256, srNum, comLen, sysFlags, sysName, 256);
                            if (result == 0)
                                s += "there is NO CD in ur CD ROM";
                            else
                            {
                                s += "There is a CD inside ur CD ROM and its name is " + volumeName;
                            }
                            api.mciSendString("set CDAudio door open", null, 127, 0);
                            if (j / 10 == 0)
                                time = 1;
                        }

                        else if(time == 1)
                        {
                            api.mciSendString("set CDAudio door closed", null, 127, 0);
                            if ((j+1) / 10 == 0)
                                time = 0;
                        }
                    }
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            api.mciSendString("set CDAudio door closed", null, 127, 0);
        }

    }
    }

    public class api
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA")]
        public static extern int mciSendString (string lpstrCommand, 
           string lpstrReturnString, int uReturnLength, int hwndCallback);

        [DllImport("kernel32.dll", EntryPoint="GetVolumeInformationA")]
        public static extern int GetVolumeInformation (string lpRootPathName, 
           StringBuilder lpVolumeNameBuffer, int nVolumeNameSize, 
           int lpVolumeSerialNumber, int lpMaximumComponentLength, 
           int lpFileSystemFlags, string lpFileSystemNameBuffer, 
           int nFileSystemNameSize);

        [DllImport("kernel32.dll", EntryPoint="GetDriveTypeA")]
        public static extern int GetDriveType (string nDrive);
    }
