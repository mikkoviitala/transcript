using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Gui.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string callerName = null)
        {
            if (!string.IsNullOrWhiteSpace(callerName))
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
        }
    }
}
