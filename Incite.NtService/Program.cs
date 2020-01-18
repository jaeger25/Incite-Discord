using System;
using Topshelf;

namespace Incite.NtService
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.UseAssemblyInfoForServiceInfo();

                x.Service(settings => new InciteService(), s =>
                {
                });

                x.SetStartTimeout(TimeSpan.FromSeconds(10));
                x.SetStopTimeout(TimeSpan.FromSeconds(10));

                x.EnableServiceRecovery(r =>
                {
                    r.RestartService(1);
                    r.OnCrashOnly();
                    r.SetResetPeriod(1);
                });
            });
        }
    }
}
