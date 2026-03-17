using Hangfire;
using System.Linq.Expressions;
using Vezeeta_Clone.Service.BackgroundJobServices.Abstract;

namespace Vezeeta_Clone.Service.BackgroundJobServices.Implementation
{
    public class BackgroundJobService : IBackgroundJobService
    {

        private readonly IBackgroundJobClient _backgroundJobClient;
        private readonly IRecurringJobManager _recurringJobManager;

        public BackgroundJobService(IBackgroundJobClient backgroundJobClient,
                                     IRecurringJobManager recurringJobManager)
        {
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
        }

        public async Task<string> EnqueueAsync<T>(Expression<Func<T, Task>> methodCall)
        {
            var jobId = BackgroundJob.Enqueue(methodCall);
            return await Task.FromResult(jobId);
        }

        public void Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
        {
            _backgroundJobClient.Schedule(methodCall, delay);
        }

        public void AddOrUpdateRecurring<T>(string jobId, Expression<Func<T, Task>> methodCall, string cronExpression)
        {
            _recurringJobManager.AddOrUpdate(jobId, methodCall, cronExpression);
        }
    }
}
