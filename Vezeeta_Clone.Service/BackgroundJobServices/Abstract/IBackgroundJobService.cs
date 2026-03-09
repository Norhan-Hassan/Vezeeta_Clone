namespace Vezeeta_Clone.Service.BackgroundJobServices.Abstract
{
    public interface IBackgroundJobService
    {
        // Fire-and-forget
        void Enqueue<T>(System.Linq.Expressions.Expression<Func<T, Task>> methodCall);

        // Delayed
        void Schedule<T>(System.Linq.Expressions.Expression<Func<T, Task>> methodCall, TimeSpan delay);

        // Recurring
        void AddOrUpdateRecurring<T>(string jobId, System.Linq.Expressions.Expression<Func<T, Task>> methodCall, string cronExpression);
    }
}
