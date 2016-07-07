using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Shared;

namespace ScheduledService
{
    public class JobExecutionHandler : IHandleMessages<ExecuteJobCommand>
    {
        public Task Handle(ExecuteJobCommand message, IMessageHandlerContext context)
        {
            var now = DateTimeOffset.UtcNow;
            if (message.DueTime < now)
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}] Rejecting job '{message.JobName}' which was due on {message.DueTime:s}");
            }
            else
            {
                Console.WriteLine(
                    $"[{DateTime.UtcNow:s}] Executing job '{message.JobName}' with parameter '{message.Parameters["someParameter"]}'");
            }
            return Task.CompletedTask;
        }
    }
}
