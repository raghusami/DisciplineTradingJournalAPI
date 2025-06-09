namespace DisciplineTradingJournalAPI
{
    using DisciplineTradingJournalAPI.Contract;
    using DisciplineTradingJournalAPI.DBModel;
    using DisciplineTradingJournalAPI.DesignPattern;
    using DisciplineTradingJournalAPI.Helper;
    using Microsoft.Extensions.DependencyInjection;
    using JWTAuthenticationManager;

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
            services.AddScoped<ISeriesTrackerRepository, SeriesTrackerRepository>();
            services.AddScoped<ISeriesTradeDetailsRepository, SeriesTradeDetailsRepository>();
            services.AddScoped<IPreTradeChecklistRepository, PreTradeChecklistRepository>();
            services.AddScoped<ITradeEmotionRepository, TradeEmotionRepository>();
            services.AddScoped<SeriesService>();
            return services;

        }
    }
}
