using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QOllamaInstallerHelp.Utils
{
  public static  class PCUtils
    {
        public static bool IsAdministrator()
        {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
            {
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }


        public static void RestartAsAdministrator()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;  // 必须设置为 true
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = Process.GetCurrentProcess().MainModule.FileName;
            startInfo.Verb = "runas";  // 关键：请求管理员权限

            try
            {
                Process.Start(startInfo);
                Environment.Exit(0);  // 关闭当前进程
            }
            catch (Exception ex)
            {
                Console.WriteLine($"无法提升权限：{ex.Message}");
                Console.WriteLine("用户拒绝了管理员权限请求");
            }
        }
    }
}
