using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvvmDemo
{
    public class SampleModel : IModel
    {
        private CheckResult.StatusEnum[] statuses_;

        public SampleModel()
        {
            SleepTime = TimeSpan.FromSeconds(1);
            statuses_ = (CheckResult.StatusEnum[])Enum.GetValues(typeof(CheckResult.StatusEnum));
        }

        public TimeSpan SleepTime { get; set; }

        public IEnumerable<CheckResult> Check(IEnumerable<string> names)
        {
            Random rnd = new Random();

            foreach (var name in names)
            {
                var status = statuses_[rnd.Next(0, statuses_.Length)];
                var chkRes = new CheckResult()
                {
                    Message = status.ToString(),
                    Name = name,
                    Status = status
                };
                yield return chkRes;

                Thread.Sleep(SleepTime);
            }
        }

        public Task CheckAsync(IEnumerable<string> names, System.Threading.CancellationToken ct, Action<CheckResult> resultCallback)
        {
            List<string> nameList = names.ToList();

            var t = Task.Factory.StartNew(() =>
            {
                foreach (var chkRes in Check(nameList))
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }

                    resultCallback(chkRes);
                }
            });

            return t;
        }
    }
}
