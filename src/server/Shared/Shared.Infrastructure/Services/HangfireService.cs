using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using Shared.Core.Interfaces.Services;

namespace Shared.Infrastructure.Services
{
    public class HangfireService : IJobService
    {
        public string Enqueue(Expression<Func<Task>> methodCall)
        {
            return BackgroundJob.Enqueue(methodCall);
        }
    }
}