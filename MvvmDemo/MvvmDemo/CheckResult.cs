using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmDemo
{
    public class CheckResult
    {
        public enum StatusEnum
        {
            Normal,
            Down,
            Slow,
            Unreliable,
        }

        public string Name { get; set; }

        public StatusEnum Status { get; set; }

        public string Message { get; set; }

        public bool IsOK
        {
            get { return Status == StatusEnum.Normal; }
        }
    }
}
