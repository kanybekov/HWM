namespace Hwm
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Hosting;

    public class HelloWorldHostedService : IHostedService
    {
        private Timer _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(HelloWorld, null, 0, 10000);
            return Task.CompletedTask;
        }

        void HelloWorld(object state)
        {
            Console.WriteLine("Hello World!");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //New Timer does not have a stop. 
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

    }
}