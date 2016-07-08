using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Routing;
using Quartz;
using Shared;

namespace Scheduler
{
    public class SendCommandJob : IJob
    {
        private readonly IMessageSession _messageSession;

        public SendCommandJob(IMessageSession messageSession)
        {
            _messageSession = messageSession;
        }

        public void Execute(IJobExecutionContext context)
        {
            var command = new ExecuteJobCommand
            {
                JobName = context.JobDetail.Key.Name,
                Parameters = context.JobDetail.JobDataMap.ToDictionary(x => x.Key, x => x.Value.ToString()),
                ScheduledTime = context.ScheduledFireTimeUtc.Value,
                DueTime = context.NextFireTimeUtc
            };
            
            _messageSession.Send(context.JobDetail.Key.Group, command).Wait();
        }
    }
}
