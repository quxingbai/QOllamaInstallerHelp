using QOllamaInstallerHelp.Utils;
using QOllamaInstallerHelp.ViewModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    /// RegistryEditPage.xaml 的交互逻辑
    /// </summary>
    public partial class RegistryEditPage : UserControl
    {

        private class RegistryEditPageViewModel : INotifyPropertyChanged
        {
            public class RegistryItemViewModel : INotifyPropertyChanged
            {
                private RegistryEditPageViewModel _Base { get; set; }
                public String Name { get; set; }
                public String DefaultValue { get; set; }
                public String ToolTip { get; set; }
                public bool IsInLocalRegistry => LocalRegistryValue != null;
                private String? _LocalRegistryValue { get; set; }
                public String? LocalRegistryValue
                {
                    get => _LocalRegistryValue; set
                    {
                        _LocalRegistryValue = value;
                        PropertyChanged?.Invoke(this, new(nameof(LocalRegistryValue)));
                    }
                }

                public ICommand ShowRegistryEditCommand { get; set; }
                public ICommand HideRegistryEditCommand { get; set; }

                private Visibility _IsVisibile { get; set; } = Visibility.Visible;
                public Visibility IsVisibile
                {
                    get => _IsVisibile;
                    set
                    {
                        _IsVisibile = value;
                        PropertyChanged?.Invoke(this, new(nameof(IsVisibile)));
                    }
                }

                public event PropertyChangedEventHandler? PropertyChanged;

                public RegistryItemViewModel(RegistryEditPageViewModel Base)
                {
                    _Base = Base;
                    ShowRegistryEditCommand = new ActionCommand(arg =>
                    {
                        Base.ShowEditPage(this);
                    });
                    HideRegistryEditCommand = new ActionCommand(arg =>
                    {
                        Base.HideEditPage();
                    });
                }

            }
            public ObservableCollection<RegistryItemViewModel> RegistryItems { get; set; } = new();
            public RegistryItemEditPageViewModel RegistryItemEditPageViewModel { get; set; }
            private Visibility _RegistryItemEditPageVisibility = Visibility.Collapsed;
            public Visibility RegistryItemEditPageVisibility
            {
                get => _RegistryItemEditPageVisibility; set
                {
                    _RegistryItemEditPageVisibility = value;
                    PropertyChanged?.Invoke(this, new(nameof(RegistryItemEditPageVisibility)));
                }
            }
            private string _QueryBoxText = "";
            public string QueryBoxText
            {
                get => _QueryBoxText; set
                {
                    _QueryBoxText = value;
                    Query(value);
                    PropertyChanged?.Invoke(this, new(nameof(QueryBoxText)));
                }
            }
            public RegistryEditPageViewModel(Dispatcher dispatcher)
            {
                RegistryItemEditPageViewModel = new RegistryItemEditPageViewModel(dispatcher, HideEditPage);
                RegistryItemEditPageViewModel.RegistryChanged += RegistryItemEditPageViewModel_RegistryChanged;

                RegistryHelp.UseSystemEnvironmentRegistry(k =>
                {
                    List<RegistryItemViewModel> items = new();
                    OllamaRegistryITooltiptemsManager.GetAllItems().ForEach(i =>
                    {
                        var rv = k.GetValue(i.Name);
                        var item = new RegistryItemViewModel(this)
                        {
                            Name = i.Name,
                            DefaultValue = i.DefaultValue,
                            ToolTip = i.Tooltip,
                            LocalRegistryValue = rv == null ? null : rv.ToString()
                        };
                        items.Add(item);
                    });
                    foreach (var item in items.OrderByDescending(d=>d.IsInLocalRegistry))
                    {
                        this.RegistryItems.Add(item);
                    }
                });
            }

            private void RegistryItemEditPageViewModel_RegistryChanged(string arg1, string arg2)
            {
                var item=RegistryItems.Where(w => w.Name == arg1).First();
                item.LocalRegistryValue = arg2;
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            public void Query(String Text)
            {
                string trimText = Text.Trim().ToUpper();
                if (trimText == "" || trimText == "*")
                {
                    foreach (var i in this.RegistryItems)
                    {
                        i.IsVisibile = Visibility.Visible;
                    }
                }
                else
                {
                    foreach (var i in this.RegistryItems)
                    {
                        i.IsVisibile = (i.Name.ToUpper().IndexOf(trimText) != -1 || i.DefaultValue.ToUpper().IndexOf(trimText) != -1 || i.ToolTip.ToUpper().IndexOf(trimText) != -1) ? Visibility.Visible : Visibility.Collapsed;
                    }
                }
            }
            public void ShowEditPage(RegistryItemViewModel item)
            {
                this.RegistryItemEditPageViewModel.Name = item.Name;
                this.RegistryItemEditPageViewModel.Value = item.LocalRegistryValue ?? "";
                this.RegistryItemEditPageViewModel.DefaultValue = item.DefaultValue;
                this.RegistryItemEditPageViewModel.ToolTip = item.ToolTip;
                this.RegistryItemEditPageVisibility = Visibility.Visible;
            }
            public void HideEditPage()
            {
                this.RegistryItemEditPageVisibility = Visibility.Collapsed;
            }
        }
        public RegistryEditPage()
        {
            InitializeComponent();
            this.DataContext = new RegistryEditPageViewModel(Dispatcher);
        }
    }
}
