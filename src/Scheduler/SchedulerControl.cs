using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;
using Quartz;

namespace Scheduler
{
    public class SchedulerControl : IWantToRunWhenEndpointStartsAndStops
    {
        private readonly IScheduler _scheduler;
        private readonly IWindsorContainer _container;

        public SchedulerControl(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public Task Start(IMessageSession session)
        {
            _scheduler.Start();
            return Task.CompletedTask;
        }

        public Task Stop(IMessageSession session)
        {
            _scheduler.Shutdown(true);
            return Task.CompletedTask;
        }
    }
}
