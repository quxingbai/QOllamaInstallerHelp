using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace QOllamaInstallerHelp.Utils
{
    public class ActionCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private Action<object?> _Action = null;
        private Func<object?, bool> _CanExe = null;
        public ActionCommand(Action<object?> action, Func<object?, bool> CanExecute = null)
        {
            this._Action = action;
            this._CanExe = CanExecute;
        }
        public bool CanExecute(object? parameter)
        {
            return _CanExe == null ? true : _CanExe(parameter);
        }

        public void Execute(object? parameter)
        {
            _Action(parameter);
        }
    }
}
