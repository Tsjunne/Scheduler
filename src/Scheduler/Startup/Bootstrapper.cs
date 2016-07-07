using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Quartz;
using Scheduler.Startup;

namespace Scheduler
{
    public static class Bootstrapper
    {
        public static IWindsorContainer Run()
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IWindsorContainer>().Instance(container));

            var scheduler = QuartzConfig.Configure(container);
            container.Register(Component.For<IScheduler>().Instance(scheduler));

            container.Register(Classes.FromAssemblyContaining<SendCommandJob>()
                .BasedOn<IJob>()
                .LifestyleScoped());

            return container;
        }

    }
}
