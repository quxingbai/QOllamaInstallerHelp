using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QOllamaInstallerHelp.Utils
{
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
            _Target.SetValue(OLN, OllamaPath+"\\models");
            //var llamaKey = _Target.OpenSubKey("LLAMA_MODELS");
            //if(llamaKey==null)llamaKey = _Target.CreateSubKey("LLAMA_MODELS");
            //llamaKey.SetValue("", OllamaPath);

        }
    }
}
