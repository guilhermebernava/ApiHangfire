using Hangfire;
using Hangfire.SqlServer;

namespace Api.Injections;

public static class HangfireInjection
{
    public static void AddConfiguredHangfire(this IServiceCollection services, string connection)
    {
        // Add Hangfire services.
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            //add the db that hangfire uses to control jobs
            .UseSqlServerStorage(connection, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        // Add the processing server as IHostedService
        services.AddHangfireServer();
    }
}
