using Microsoft.Win32;
using QOllamaInstallerHelp.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace QOllamaInstallerHelp.Views
{
    /// <summary>
    /// InstallerSelectPage.xaml 的交互逻辑
    /// </summary>
    public partial class InstallerSelectPage : UserControl
    {
        public class OllamaRegistryItemViewModel : INotifyPropertyChanged
        {
            public OllamaRegistryTooltipItem RegistryItem { get; set; }
            public String Name => RegistryItem.Name;
            public object Value { get; set; }
            public object DefaultValue { get; set; }
            public string Description { get; set; }

            public event PropertyChangedEventHandler? PropertyChanged;
        }
        private class InstallerSelectPageViewModel : INotifyPropertyChanged
        {
            public ICommand COMMAND_OpenOllamaUrlCommand { get; set; }
            public ICommand COMMAND_TryInstall { get; set; }
            public String OllamaSetupExePath { get; set; } = ".\\OllamaSetup.exe";
            public String OllamaInstallPath { get; set; } = "C:\\Ollama";
            public string OllamaImageUrl { get; set; } = "https://mintcdn.com/ollama-9269c548/XefrxzvUktkk84RL/images/logo-dark.png?fit=max&auto=format&n=XefrxzvUktkk84RL&q=85&s=c214b467f5623414c31d4e05c66110fb";
            public bool _IsWriteRegistry = true;
            public bool IsWriteRegistry
            {
                get => _IsWriteRegistry;
                set
                {
                    _IsWriteRegistry = value;
                    PropertyChanged?.Invoke(value, new(nameof(IsWriteRegistry)));
                }
            }
            private bool _IsInstalling = false;
            public bool IsInstalling
            {
                get { return _IsInstalling; }
                set { _IsInstalling = value; PropertyChanged?.Invoke(value, new(nameof(IsInstalling))); }
            }


            public ObservableCollection<OllamaRegistryItemViewModel> RegistryItems { get; set; } = new();
            public string OLLAMA_HOST { get; set; }

            public InstallerSelectPageViewModel(Dispatcher dispatcher)
            {
                COMMAND_OpenOllamaUrlCommand = new ActionCommand((obj) =>
                {
                    Process.Start("explorer.exe", obj.ToString());
                }, (o) => !IsInstalling);
                COMMAND_TryInstall = new ActionCommand((a) =>
                {
                    try
                    {
                        TryInstall();
                        _ = InstallAsync().ContinueWith(w =>
                        {
                            //MessageBox.Show("安装完成");
                            dispatcher.Invoke(() =>
                            {
                                IsInstalling = false;
                            });
                        });

                    }
                    catch (Exception error)
                    {
                        MessageBox.Show("安装前检查失败。\n" + error.Message, "检查失败", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }, (a) => !IsInstalling);
            }

            public event PropertyChangedEventHandler? PropertyChanged;

            public void TryInstall()
            {
                //if (IsWriteRegistry && !PCUtils.IsAdministrator()) throw new("没有管理员权限，无法安装。因为修改注册表需要管理员权限。");
                if (!File.Exists(OllamaSetupExePath)) throw new("OllamaSetup.exe 文件地址不对，这是个空地址\n" + OllamaSetupExePath);
                if (!Directory.Exists(OllamaInstallPath)) throw new("安装地址不存在，需要重新输入。\n" + OllamaInstallPath);
            }
            public async Task InstallAsync()
            {
                IsInstalling = true;
                string exePath = this.OllamaSetupExePath;
                string installPath = this.OllamaInstallPath;
                string commandArg = $"/DIR=\"{installPath}\"";

                bool IsInstalled = File.Exists(installPath + "\\ollama.exe");
                var code = 0;
                if (!IsInstalled)
                {
                    code = await CreateExeInstallProcess(exePath, commandArg);
                }
                if (code == 0)
                {
                    if (IsWriteRegistry)
                    {
                        //var registry = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment", true);
                        RegistryHelp.UseSystemEnvironmentRegistry(registry =>
                        {
                            var ip = IPEndPoint.Parse(OLLAMA_HOST);
                            RegistryHelp registry_help = new(registry);
                            registry_help.SetOllamaPath(installPath);
                            if (OLLAMA_HOST != null) registry_help.SetOllamaListenIP($"{ip.Address}:{ip.Port}");
                        });
                    }
                    MessageBox.Show("安装成功");
                }
                else
                {
                    MessageBox.Show("安装失败，安装程序返回错误代码：" + code, "安装失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                IsInstalling = false;
            }
            private async Task<int> CreateExeInstallProcess(string Path, params string[] Args)
            {
                string str = string.Join(" ", Args);
                Process process = new();
                process.StartInfo.Arguments = str;
                process.StartInfo.FileName = Path;
                process.Start();
                await process.WaitForExitAsync();
                return process.ExitCode;
            }
        }
        public InstallerSelectPage()
        {
            InitializeComponent();
            this.DataContext = new InstallerSelectPageViewModel(this.Dispatcher);
        }
    }
}
