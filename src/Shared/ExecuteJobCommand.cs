using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;

namespace Shared
{
    public class ExecuteJobCommand : ICommand
    {
        public string JobName { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public DateTimeOffset ScheduledTime { get; set; }
        public DateTimeOffset? DueTime { get; set; }
    }
}
