using QOllamaInstallerHelp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace QOllamaInstallerHelp.ViewModels
{
    public class RegistryItemEditPageViewModel : VMBase
    {
        private String _name;
        private String _value;
        private String _default_value;
        private String _tooltip;
        public String Name
        {
            get => _name; set
            {
                _name = value; OnPropertyChanged();
            }
        }
        public String Value
        {
            get => _value; set
            {
                _value = value; OnPropertyChanged();
            }
        }
        public String DefaultValue
        {
            get => _default_value; set
            {
                _default_value = value; OnPropertyChanged();
            }
        }
        public String ToolTip
        {
            get => _tooltip; set
            {
                _tooltip = value; OnPropertyChanged();
            }
        }

        public event Action<string, string> RegistryChanged;
        public ICommand COMMAND_UpdateRegistry { get; set; }
        public ICommand COMMAND_Hidden { get; set; }
        public RegistryItemEditPageViewModel(Dispatcher dispatcher, Action hiddenCallback)
        {
            COMMAND_UpdateRegistry = new ActionCommand(arg =>
            {
                UpdateRegistryValue();
            });
            COMMAND_Hidden = new ActionCommand(arg =>
            {
                hiddenCallback();
            });
        }

        private void UpdateRegistryValue()
        {
            RegistryHelp.UseSystemEnvironmentRegistry(registry =>
            {
                try
                {
                    registry.SetValue(Name, Value);
                    COMMAND_Hidden.Execute(null);
                    RegistryChanged?.Invoke(Name, Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"注册表更新失败: {ex.Message}", "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }
    }
}
