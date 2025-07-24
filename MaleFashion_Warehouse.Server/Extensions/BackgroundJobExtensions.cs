using Hangfire;
using MaleFashion_Warehouse.Server.Services.Interfaces;

namespace MaleFashion_Warehouse.Server.Extensions
{
    public static class BackgroundJobExtensions
    {
        public static IApplicationBuilder UseBackgroundJobs(this WebApplication app)
        {
            IRecurringJobManager recurringJobs = app.Services.GetRequiredService<IRecurringJobManager>();

            recurringJobs.AddOrUpdate<ICartsService>(
                "cleanup-abandoned-carts-job",
                job => job.CleanupAbandonedCartsAsync(),
                app.Configuration["BackgroundJobs:Carts:CleanupAbandoned:Schedule"]
            );

            return app;
        }
    }
}
