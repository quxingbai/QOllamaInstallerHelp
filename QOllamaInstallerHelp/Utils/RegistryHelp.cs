using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QOllamaInstallerHelp.Utils
{

    public record class OllamaRegistryTooltipItem(String Name, string DefaultValue, string Tooltip);

    /// <summary>
    /// Ollama注册表示例
    /// </summary>
    public static class OllamaRegistryITooltiptemsManager
    {
        private static Dictionary<string, OllamaRegistryTooltipItem> Items = new Dictionary<string, OllamaRegistryTooltipItem>();
        static OllamaRegistryITooltiptemsManager()
        {
            string localJsonFilePath = "./OLLAMA_RegistryTooltips.json";
            if (File.Exists(localJsonFilePath))
            {
                string text = File.ReadAllText(localJsonFilePath);
                var items = JsonConvert.DeserializeObject<List<OllamaRegistryTooltipItem>>(text);
                foreach (var i in items)
                {
                    Items.Add(i.Name, i);
                }
            }
        }

        public static OllamaRegistryTooltipItem GetItem(string Name)
        {
            if (Items.ContainsKey(Name))
            {
                return Items[Name];
            }
            else
            {
                return new OllamaRegistryTooltipItem(Name, "", "No description");
            }
        }
        public static List<OllamaRegistryTooltipItem> GetAllItems()
        {
            return Items.Values.ToList();
        }

    }

    public class RegistryHelp
    {
        private RegistryKey _Target = null;
        /// <summary>
        /// 注册表控制器
        /// </summary>
        /// <param name="Target">目标 比如Registry.CurrentUser</param>
        public RegistryHelp(RegistryKey Target)
        {
            this._Target = Target;
        }
        public void SetKey(string Key, string Value)
        {
            _Target.SetValue(Key, Value);
        }
        public void SetOllamaPath(string OllamaPath)
        {
            var str = _Target.GetValue("Path").ToString();
            var sp = str.Split(';');
            if (!sp.Any(a => a.ToLower() == OllamaPath.ToLower()))
            {
                string newRegi = str + ";" + OllamaPath;
                _Target.SetValue("PATH", newRegi, RegistryValueKind.ExpandString);
            }
            var OLN = "OLLAMA_MODELS";
            _Target.SetValue(OLN, OllamaPath + "\\models");
        }
        public void SetOllamaListenIP(string IP)
        {
            string OLN = "OLLAMA_HOST";
            _Target.SetValue(OLN, IP);
        }
        /// <summary>
        /// 使用全局变量的注册表，会自动关闭
        /// </summary>
        public static void UseSystemEnvironmentRegistry(Action<RegistryKey> Action)
        {
            var registry = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment", true);
            Action(registry);
            registry.Close();
        }
    }
}
