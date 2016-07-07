using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Shared
{
    public class RegisterJobExecutionScheduleCommand : ICommand
    {
        public string EndpointName { get; set; }
        public JobRegistration[] JobRegistrations { get; set; }
    }

    public class JobRegistration : ICommand
    {
        public string JobName { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public TimeSpan? Interval { get; set; }
        public string CronExpression { get; set; }
    }
}
