using System;
using System.Threading;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using NServiceBus;
using NServiceBus.Features;
using NServiceBus.Installation;
using NServiceBus.Logging;
using NServiceBus.Settings;
using NameValueCollection = Common.Logging.Configuration.NameValueCollection;

namespace Scheduler.Startup
{
    [EndpointName("Scheduler")]
    public class EndpointConfig : IConfigureThisEndpoint
    {
        public void Customize(EndpointConfiguration endpointConfiguration)
        {
            var container = Bootstrapper.Run();

            LogManager.Use<Log4NetFactory>();
            endpointConfiguration.UseContainer<WindsorBuilder>(c => c.ExistingContainer(container));
            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.UseTransport<SqlServerTransport>().ConnectionStringName("Scheduler");
            endpointConfiguration.SendFailedMessagesTo("error");
            endpointConfiguration.AuditProcessedMessagesTo("audit");
        }
    }

    public class SessionRegistration : IWantToRunWhenEndpointStartsAndStops
    {
        private readonly IWindsorContainer _container;

        public SessionRegistration(IWindsorContainer container)
        {
            _container = container;
        }

        public Task Start(IMessageSession session)
        {
            _container.Register(Component.For<IMessageSession>().Instance(session));
            return Task.CompletedTask;
        }

        public Task Stop(IMessageSession session)
        {
            return Task.CompletedTask;
        }
    }
}
