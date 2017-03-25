using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BookLendingLib.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var hndlr = PropertyChanged;
            if (hndlr != null)
            {
                hndlr(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
