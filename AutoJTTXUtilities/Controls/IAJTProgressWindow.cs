using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoJTTXUtilities.Controls
{
    public class IAJTProgressWindow<T>
    {
        private T a;
        public T A { get => a; private set => a = value; }

        public IAJTProgressWindow(T a)
        {
            this.A = a;
        }
    }
}
