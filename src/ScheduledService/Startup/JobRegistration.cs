using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Installation;
using Shared;

namespace ScheduledService.Startup
{
    public class JobRegistration : IWantToRunWhenEndpointStartsAndStops
    {
        public async Task Start(IMessageSession session)
        {
            var registration1 = new Shared.JobRegistration
            {
                JobName = "RunOnCronSchedule",
                Parameters = new Dictionary<string, string> { { "someParameter", "aParameter" } },
                CronExpression = "0/10 * * * * ? *"
            };

            var registration2 = new Shared.JobRegistration
            {
                JobName = "RunEveryMinute",
                Parameters = new Dictionary<string, string> {{"someParameter", "aParameter"}},
                Interval = TimeSpan.FromMinutes(1)
            };

            var command = new RegisterJobExecutionScheduleCommand
            {
                EndpointName = "ScheduledService",
                JobRegistrations = new[] {registration1, registration2}
            };

            await session.Send("Scheduler", command);
        }

        public Task Stop(IMessageSession session)
        {
            return Task.CompletedTask;
        }
    }
}
