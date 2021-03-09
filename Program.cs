using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.ServiceProcess;
using System.Diagnostics;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
//using System.Linq;
using System.Reflection;

namespace IrondomeICTFSWService
{
    class Program : ServiceBase
    {
        private String version = "0.1, By El Pepe...";
        
        private String ictLogsSourcePath = String.Empty;
        
        private String serviceName = String.Empty;
        private String serviceDescription = String.Empty;
        private String serviceExecutionPath = Application.StartupPath;        

        private FileSystemWatcher fswNewFiles = new FileSystemWatcher();

        private List<String> files;
                
        private int createdFiles = 0;

        static void Main(string[] args)
        {
            ServiceBase.Run(new Program());
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Utilities.WriteLogs("");
                Utilities.WriteLogs("Starting...");
                base.OnStart(args);
                Utilities.WriteLogs("");
                Utilities.WriteLogs("Setting up " + ServiceName + " " + version);

                ictLogsSourcePath = Utilities.ReadIniFile("ICTLogsSourcePath");
                Utilities.WriteLogs(ServiceName + " ICT Logs Source Path : " + ictLogsSourcePath);

                fswNewFiles.Filter = Utilities.ReadIniFile("Filter");
                Utilities.WriteLogs(ServiceName + " FSW Filter: " + fswNewFiles.Filter);

                fswNewFiles.Path = ictLogsSourcePath;
                Utilities.WriteLogs(ServiceName + " FSW Path: " + fswNewFiles.Path);

                fswNewFiles.IncludeSubdirectories = true;
                Utilities.WriteLogs(ServiceName + " FSW IncludeSubdirectories : " + fswNewFiles.IncludeSubdirectories);

                fswNewFiles.EnableRaisingEvents = true;
                Utilities.WriteLogs(ServiceName + " Service EnableRaisingEvents : " + fswNewFiles.EnableRaisingEvents);

                serviceName = Utilities.ReadIniFile("ServiceName");
                serviceDescription = Utilities.ReadIniFile("ServiceDescription");
                serviceExecutionPath = Application.StartupPath;

                Utilities.WriteLogs(ServiceName + " Service Name : " + serviceName);
                Utilities.WriteLogs(ServiceName + " Service Description : " + serviceDescription);
                Utilities.WriteLogs(ServiceName + " Service Execution Path : " + serviceExecutionPath);                

                if (!Directory.Exists("ServiceLogs"))
                {
                    Directory.CreateDirectory("ServiceLogs");
                }

                Utilities.WriteLogs(ServiceName + " Service Logs Path = " + serviceExecutionPath + "\\ServiceLogs\\");

                Utilities.WriteLogs(ServiceName + "");
                Utilities.WriteLogs("");
            }
            catch (Exception ex)
            {
                Utilities.WriteExceptions(MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.ToString());
            }
        }

        protected override void OnStop()
        {
            Utilities.WriteLogs("");
            Utilities.WriteLogs("Stoping " + ServiceName);
            base.OnStop();
            Utilities.WriteLogs("Stoped " + ServiceName);
            Utilities.WriteLogs("");
        }

        public Program()
        {            
            ServiceName = Utilities.ReadIniFile("ServiceName");

            fswNewFiles.Created += new System.IO.FileSystemEventHandler(fswNewFiles_Created);
            fswNewFiles.Created += new System.IO.FileSystemEventHandler(fswNewFiles_Changed);
        }

        private void fswNewFiles_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                createdFiles++;
                Utilities.WriteLogs("File Created: " + e.FullPath + ", created. Total created files: " + createdFiles);

                if (e.FullPath.EndsWith("basicRunImage") || e.FullPath.EndsWith("log") || e.FullPath.EndsWith("report"))
                {
                }
                else
                {
                    Process copyFile = new Process();
                    copyFile.StartInfo.FileName = Application.StartupPath + "\\CopyFile.exe";
                    copyFile.StartInfo.WorkingDirectory = Application.StartupPath;
                    copyFile.StartInfo.Arguments = e.FullPath;
                    Utilities.WriteLogs("Calling copy.exe, arguments: \"" + copyFile.StartInfo.Arguments + "\"");
                    copyFile.Start();
                }
            }
            catch (Exception ex)
            {
                Utilities.WriteExceptions(MethodBase.GetCurrentMethod().Name + Environment.NewLine + ex.ToString());                
            }
        }

        private void fswNewFiles_Changed(object sender, FileSystemEventArgs e)
        {
        }
    }   
}
