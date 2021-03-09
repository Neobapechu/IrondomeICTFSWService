using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace IrondomeICTFSWService
{
    public static class Utilities
    {
        public static void WriteLogs(String message)
        {
            DateTime now = DateTime.Now;

            string directory = Application.StartupPath + "\\FSWServiceLogs\\" + now.Year + "\\" + now.Month + "\\" + now.Day;
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            String logFile =  directory + "\\Service_" + Utilities.ReadIniFile("ServiceName") + "_"  +  now.ToString("MMddyyyy-HH") + ".ks";
            File.AppendAllText(logFile, now.ToString("MM-dd-yyyy HH:mm:ss.ff : ") + message + Environment.NewLine);
        }

        public static void WriteLogs(String message, bool debug)
        {
            if (debug)
            {
                DateTime now = DateTime.Now;

                string directory = Application.StartupPath + "\\FSWServiceLogs\\" + now.Year + "\\" + now.Month + "\\" + now.Day;
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                String logFile = directory + "\\" + Utilities.ReadIniFile("ServiceName") + "_" + now.ToString("MMddyyyy-HH") + ".ks";
                File.AppendAllText(logFile, now.ToString("MM-dd-yyyy HH:mm:ss.ff : ") + message + Environment.NewLine);
            }
        }

        public static void WriteExceptions(string exception)
        {
            DateTime now = DateTime.Now;

            if (!Directory.Exists("servicelogs"))
            {
                Directory.CreateDirectory("servicelogs");
            }

            String logFile = Application.StartupPath + "\\FSWServiceLogs\\Exceptions.ks";
            File.AppendAllText(logFile, now.ToString("yyyy-MM-dd HH:mm:ss.ff : ") + exception.ToString() + Environment.NewLine + Environment.NewLine);                        
        }        
        
        public static String ReadIniFile(String param)
        {
            String name = "";
            String[] fileContent = File.ReadAllLines(Application.StartupPath + "\\configFSW.ini" );
            
            for (int i = 0; i < fileContent.Length; i++)
            {            
                if (fileContent[i].StartsWith(param))
                {
                    name = fileContent[i].Split('=')[1].Trim();
                    break;
                }
            }
            return name;
        }      
    }
}