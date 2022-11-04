using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WepSerApp.Model
{
    public abstract class Bindable : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void PropertyIsChanged([CallerMemberName] string memberName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
