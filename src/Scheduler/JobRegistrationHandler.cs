using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Simpl;
using Shared;

namespace Scheduler
{
    public class JobRegistrationHandler : IHandleMessages<RegisterJobExecutionScheduleCommand>
    {
        private readonly IScheduler _scheduler;

        public JobRegistrationHandler(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public Task Handle(RegisterJobExecutionScheduleCommand message, IMessageHandlerContext context)
        {
            ClearOldRegistrations(message);

            foreach (var registration in message.JobRegistrations)
            {
                RegisterJob(message, registration);
            }

            return Task.CompletedTask;
        }

        public void ClearOldRegistrations(RegisterJobExecutionScheduleCommand message)
        {
            var query = GroupMatcher<JobKey>.GroupEquals(message.EndpointName);
            var jobKeys = _scheduler.GetJobKeys(query).ToArray();
            
            _scheduler.DeleteJobs(jobKeys);
        }

        public void RegisterJob(RegisterJobExecutionScheduleCommand message, JobRegistration registration)
        {
            var jobKey = new JobKey(registration.JobName, message.EndpointName);

            var job = _scheduler.GetJobDetail(jobKey);

            if (job != null)
            {
                _scheduler.DeleteJob(jobKey);
            }

            var dataMap = new JobDataMap();
            foreach (var parameter in registration.Parameters)
            {
                dataMap.Add(parameter.Key, parameter.Value);
            }

            job = JobBuilder.Create<SendCommandJob>()
                .SetJobData(dataMap)
                .WithIdentity(jobKey)
                .StoreDurably(true)
                .Build();
            
            var triggerBuilder = TriggerBuilder.Create()
                .WithIdentity(registration.JobName, message.EndpointName)
                .StartNow();

            if (registration.Interval.HasValue)
            {
                triggerBuilder
                    .WithSimpleSchedule(x => x
                        .WithInterval(registration.Interval.Value)
                        .RepeatForever());
            }
            else
            {
                triggerBuilder
                    .WithCronSchedule(registration.CronExpression);
            }

            _scheduler.ScheduleJob(job, triggerBuilder.Build());
        }
    }
}
