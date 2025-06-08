namespace DisciplineTradingJournalAPI
{
    using DisciplineTradingJournalAPI.Contract;
    using DisciplineTradingJournalAPI.DBModel;
    using DisciplineTradingJournalAPI.DesignPattern;
    using DisciplineTradingJournalAPI.Helper;
    using JWTAuthenticationManager;
    using Microsoft.Extensions.DependencyInjection;

    public static class DIExtensions
    {
        public static IServiceCollection RegisterModelDependencies(this IServiceCollection services)
        {
            services.AddScoped<IAsyncLogger, Logger>();
            services.AddScoped<ITradingUsersRepository, TradingUsersRepository>();
            services.AddScoped<IUserTradesRepository, UserTradesRepository>();
            services.AddScoped<IUserClaimManager, UserClaimManager>();
            services.AddScoped<ITradingChargesRepository, TradingChargesRepository>();
            services.AddScoped<IPerformanceMetricRepository, PerformanceMetricRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IOptionsAnalysis, OptionsAnalysisModel>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ISeriesTracker, SeriesTrackerRepository>();
            services.AddScoped<ISeriesTradeDetailsRepository, SeriesTradeDetailsRepository>();
            services.AddScoped<SeriesService>();

            return services;
        }
    }
}
