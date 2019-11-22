using Incite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Incite.Discord.Services
{
    public class EventScheduledTaskOptions
    {
        public int PeriodInMinutes { get; set; }
        public int UpcomingEventThresholdInMinutes { get; set; }
    }

    public class EventScheduledTask : IHostedService, IDisposable
    {
        readonly InciteDbContext m_dbContext;
        readonly Timer m_timer;
        readonly EventScheduledTaskOptions m_options;

        public EventScheduledTask(InciteDbContext dbContext, IOptions<EventScheduledTaskOptions> options)
        {
            m_dbContext = dbContext;
            m_options = options.Value;
            m_timer = new Timer(Update, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        }

        public void Dispose()
        {
            m_timer.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            m_timer.Change(TimeSpan.Zero, TimeSpan.FromMinutes(m_options.PeriodInMinutes));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            m_timer.Change(Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
            return Task.CompletedTask;
        }

        void Update(object state)
        {
            var upcomingEvents = m_dbContext.Events
                .Where(x => EF.Functions.DateDiffMinute(DateTimeOffset.UtcNow, x.DateTime) >= 0
                    && EF.Functions.DateDiffMinute(DateTimeOffset.UtcNow, x.DateTime) <= m_options.UpcomingEventThresholdInMinutes);

            foreach (var upcomingEvent in upcomingEvents)
            {

            }
        }
    }
}
