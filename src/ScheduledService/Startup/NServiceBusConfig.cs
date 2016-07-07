using Castle.MicroKernel.Registration;
using NServiceBus;
using NServiceBus.Config.ConfigurationSource;
using NServiceBus.Logging;
using Scheduler;

namespace ScheduledService.Startup
{
    [EndpointName("ScheduledService")]
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            var container = Bootstrapper.Run();

            LogManager.Use<Log4NetFactory>();
            endpointConfiguration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseTransport<SqlServerTransport>()
                .ConnectionStringName("Scheduler");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
            endpointConfiguration.LimitMessageProcessingConcurrencyTo(1);
        }
    }
}
