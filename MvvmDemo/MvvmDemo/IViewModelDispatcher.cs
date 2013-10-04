using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmDemo
{
    public interface IViewModelDispatcher
    {
        bool IsSynchronized { get; }

        void Invoke(Action act);

        void BeginInvoke(Action act);
    }
}
