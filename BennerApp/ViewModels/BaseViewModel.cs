using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BennerApp.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Set<T>(ref T field, T value, [CallerMemberName] string prop = null)
        {
            if (!object.Equals(field, value))
            {
                field = value;
                var handler = PropertyChanged;
                if (handler != null) handler(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
