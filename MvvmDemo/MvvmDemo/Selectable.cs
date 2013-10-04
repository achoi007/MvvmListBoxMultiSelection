using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MvvmDemo
{
    /// <summary>
    /// Encapsulate the idea of something is selectable, has a name and a value.  Perfect for use in list bounded
    /// to a WPF ListBox for example.
    /// </summary>
    public class Selectable : INotifyPropertyChanged
    {
        private string name_;
        private object value_;
        private bool isSelected_;

        public Selectable()
        {
            name_ = string.Empty;
            isSelected_ = false;
        }

        public Selectable(string name, bool isSelected = false, object val = null)
        {
            name_ = name;
            value_ = val;
            isSelected_ = isSelected;
        }

        public string Name 
        {
            get { return name_; }
            set
            {
                name_ = value;
                NotifyPropChange();
            }
        }


        public object Value
        {
            get { return value_;  }
            set
            {
                value_ = value;
                NotifyPropChange();
            }
        }

        public bool IsSelected
        {
            get { return isSelected_; }
            set
            {
                isSelected_ = value;
                NotifyPropChange();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void NotifyPropChange([CallerMemberName] string name = "")
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
