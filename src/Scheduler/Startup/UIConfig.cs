using System;
using System.Threading.Tasks;
using CrystalQuartz.Owin;
using Microsoft.Owin.Hosting;
using NServiceBus;
using Owin;
using Quartz;

namespace Scheduler.Startup
{
    public class UIConfig : IWantToRunWhenEndpointStartsAndStops
    {
        private readonly IScheduler _scheduler;
        private IDisposable _server;

        public UIConfig(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public Task Start(IMessageSession session)
        {
            _server = WebApp.Start("http://localhost:12345", app =>
            {
                app.UseCrystalQuartz(_scheduler);
            });

            return Task.CompletedTask;
        }

        public Task Stop(IMessageSession session)
        {
            _server.Dispose();
            return Task.CompletedTask;
        }
    }
}
