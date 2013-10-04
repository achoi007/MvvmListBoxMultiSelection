using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace MvvmDemo
{
    public class ViewModel : INotifyPropertyChanged
    {
        private IViewModelDispatcher dispatcher_;

        public ViewModel(IModel model, IViewModelDispatcher dispatcher)
        {
            Model = model;
            dispatcher_ = dispatcher;
            checkLevel_ = 1;

            // Set names to check list
            Names = new BindingList<Selectable>(new string[] {
                "People", "Countries", "Instruments", "Topics", "Colors"
            }
            .Select(s => new Selectable(s)).ToList());

            // Set check result list
            Results = new BindingList<CheckResult>();

            // Set commands
            CheckCommand = new DelegateCommand<string>(OnCheck, OnCanCheck);
            CxlCheckCommand = new DelegateCommand<object>(OnCxl, OnCanCxl);
        }

        private bool OnCanCxl(object obj)
        {
            return cts_ != null;
        }

        private void OnCxl(object obj)
        {
            cts_.Cancel();
        }

        private bool OnCanCheck(string obj)
        {
            return cts_ == null;
        }

        private void OnCheck(string obj)
        {
            cts_ = new CancellationTokenSource();
            UpdateCommands();
            Results.Clear();

            IEnumerable<string> names = null;

            // Figure out the list of names to check based on which check command parameter and 
            // which items are selected.
            if (!string.IsNullOrEmpty(obj))
            {
                if (obj == "selected")
                {
                    names = Names.Where(s => s.IsSelected).Select(s => s.Name);
                }
                else if (obj == "unselected")
                {
                    names = Names.Where(s => !s.IsSelected).Select(s => s.Name);
                }
            }
            else
            {
                names = Names.Select(s => s.Name);
            }

            // No names to check, just skip
            if (names == null)
            {
                names = Enumerable.Empty<string>();
            }

            // Check the list of names
            Model.CheckAsync(names, cts_.Token, chkRes =>
            {
                dispatcher_.Invoke(() => Results.Add(chkRes));
            })
            .ContinueWith(t =>
            {
                cts_ = null;
                UpdateCommands();
            });
        }

        public IModel Model { get; private set; }

        public int CheckLevel
        {
            get { return checkLevel_; }
            set
            {
                checkLevel_ = value;
                NotifyUpdate();
            }
        }

        public BindingList<Selectable> Names { get; private set; }

        public BindingList<CheckResult> Results { get; private set; }

        public ICommand CheckCommand { get; private set; }

        public ICommand CxlCheckCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private int checkLevel_;
        private CancellationTokenSource cts_;

        private void NotifyUpdate([CallerMemberName] string name = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void UpdateCommands()
        {
            dispatcher_.Invoke(() =>
            {
                ((DelegateCommand<string>)CheckCommand).RaiseChangeExecute();
                ((DelegateCommand<object>)CxlCheckCommand).RaiseChangeExecute();
            });
        }
    }
}
