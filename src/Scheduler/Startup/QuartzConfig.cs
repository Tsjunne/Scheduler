using System.Collections.Specialized;
using System.Configuration;
using Castle.Windsor;
using Quartz;
using Quartz.Impl;

namespace Scheduler.Startup
{
    public static class QuartzConfig
    {
        public static IScheduler Configure(IWindsorContainer container)
        {
            Common.Logging.LogManager.Adapter = new Common.Logging.Log4Net.Log4NetLoggerFactoryAdapter(new Common.Logging.Configuration.NameValueCollection());

            var properties = new NameValueCollection
            {
                {"quartz.jobStore.type", "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz"},
                {"quartz.jobStore.driverDelegateType", "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz"},
                {"quartz.jobStore.tablePrefix", "QRTZ_"},
                {"quartz.jobStore.dataSource", "myDS"},
                {"quartz.dataSource.myDS.connectionString", ConfigurationManager.ConnectionStrings["Scheduler"].ConnectionString},
                {"quartz.dataSource.myDS.provider", "SqlServer-20"},
                {"quartz.jobStore.clustered", "true"},
                {"quartz.scheduler.instanceId", "AUTO"}
            };
            
             ISchedulerFactory schedFact = new StdSchedulerFactory(properties);
            
            // get a scheduler
            var scheduler = schedFact.GetScheduler();
            scheduler.JobFactory = new WindsorJobFactory(container);

            return scheduler;
        }
    }
}
