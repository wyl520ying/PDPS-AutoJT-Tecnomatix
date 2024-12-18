using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoJTTXUtilities.Controls
{
    public class AJTPropertyChanged : INotifyPropertyChanged
	{
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string name = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void SetPropNotify<T>(ref T property, T value, [CallerMemberName] string name = null)
        {
            property = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    //用法
    /*
     
    set
    {
        对应的字段 = value;
        NotifyPropertyChanged();
    }
 
    // 或者
    set => SetPropNotify(ref 对应的字段, value);
      
    */
}