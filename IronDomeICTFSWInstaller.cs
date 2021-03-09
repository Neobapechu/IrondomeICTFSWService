using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration.Install;
using System.ComponentModel;
using System.ServiceProcess;
using System.IO;
using System.Windows.Forms;

namespace IrondomeICTFSWService
{
    [RunInstaller(true)]
    public class IronDomeICTFSWInstaller: Installer
    {        
        public IronDomeICTFSWInstaller()
        {
            if (!Directory.Exists("FSWServicelogs"))
            {
                Directory.CreateDirectory("FSWServiceLogs");
            }

            Utilities.WriteLogs("Installing service...");

            String serviceName = Utilities.ReadIniFile("ServiceName");
            String serviceDescription = Utilities.ReadIniFile("ServiceDescription");
              
            
            var processInstaller = new ServiceProcessInstaller();            
            var serviceInstaller = new ServiceInstaller();
            processInstaller.Account = ServiceAccount.LocalSystem;

            //CHANGE
            serviceInstaller.DisplayName = serviceName;
            Utilities.WriteLogs("DisplayName " + serviceName);

            serviceInstaller.ServiceName = serviceName;
            Utilities.WriteLogs("Service Name: " + serviceName);

            serviceInstaller.Description = serviceDescription;
            Utilities.WriteLogs("Service Description: " + serviceDescription);

            serviceInstaller.StartType = ServiceStartMode.Automatic;
            Utilities.WriteLogs("StartType: " + serviceInstaller.StartType.ToString());

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);                        
        }
    }    
}