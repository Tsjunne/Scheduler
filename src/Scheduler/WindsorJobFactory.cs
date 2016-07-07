using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

namespace Scheduler
{
    public class WindsorJobFactory : SimpleJobFactory
    {
        private readonly IWindsorContainer _container;

        public WindsorJobFactory(IWindsorContainer container)
        {
            _container = container;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                using (_container.BeginScope())
                {
                    return (IJob) this._container.Resolve(bundle.JobDetail.JobType);
                }
            }
            catch (Exception e)
            {
                throw new SchedulerException(
                    $"Problem while instantiating job '{bundle.JobDetail.Key}' from the WindsorJobFactory.", e);
            }
        }
    }
}
