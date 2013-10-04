using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MvvmDemo
{
    public class WpfViewModelDispatcher : IViewModelDispatcher
    {
        private Dispatcher dispatcher_;

        public WpfViewModelDispatcher(Dispatcher dispatcher)
        {
            dispatcher_ = dispatcher;
        }

        public bool IsSynchronized
        {
            get { return dispatcher_.Thread == Thread.CurrentThread; }
        }

        public void Invoke(Action act)
        {
            if (IsSynchronized)
            {
                act();
            }
            else
            {
                dispatcher_.Invoke(act);
            }
        }

        public void BeginInvoke(Action act)
        {
            dispatcher_.BeginInvoke(act);
        }
    }
}
