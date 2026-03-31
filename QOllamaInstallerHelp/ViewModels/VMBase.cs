using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace QOllamaInstallerHelp.ViewModels
{
    public class VMBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public virtual void OnPropertyChanged([CallerMemberName] string PropertyName = "")
        {
            PropertyChanged?.Invoke(this, new(PropertyName));
        }
    }
}
