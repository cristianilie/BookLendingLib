using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookLendingLib.ViewModels
{
    /// <summary>
    /// Base class that implements the INotifyPropertyChanged, so that it can be inherited
    /// </summary>
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
