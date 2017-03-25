using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BookLendingLib.ViewModels.Comands
{
    public class TblQryCommand : ICommand
    {
        private Action _exec;

        public TblQryCommand(Action exc)
        {
            if (exc == null)
                throw new NullReferenceException("Exec is null!");
            _exec = exc;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _exec.Invoke();
        }
    }
}
