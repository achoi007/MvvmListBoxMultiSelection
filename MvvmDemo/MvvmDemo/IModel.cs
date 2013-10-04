using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmDemo
{
    public interface IModel
    {
        IEnumerable<CheckResult> Check(IEnumerable<string> names);

        Task CheckAsync(IEnumerable<string> names, CancellationToken ct, Action<CheckResult> resultCallback);
    }
}
