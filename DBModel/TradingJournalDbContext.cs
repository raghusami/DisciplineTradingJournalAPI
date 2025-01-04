using DisciplineTradingJournalAPI.DataEntity;
using Microsoft.EntityFrameworkCore;

namespace DisciplineTradingJournalAPI.DBModel
{
    public class TradingJournalDbContext : DbContext
    {
        public TradingJournalDbContext(DbContextOptions<TradingJournalDbContext> options) : base(options)
        {

        }
        public DbSet<TradingUsers> TradingUsers { get; set; }
        public DbSet<TradingUserProfile> UserProfiles { get; set; }
        public DbSet<UserTrades> UserTrades { get; set; }
        public DbSet<PerformanceMetric> PerformanceMetrics { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<TradeNote> TradeNotes { get; set; }
        public DbSet<TradeSettings> Settings { get; set; }
        public DbSet<TradingAccountValues> AccountValues { get; set; }
        public DbSet<TradeStrategy> TradeStrategies { get; set; }
        public DbSet<UserAlerts> Alerts { get; set; }
        public DbSet<OptionsAnalysisCE> OptionsAnalysisCE { get; set; }
        public DbSet<OptionsAnalysisPE> OptionsAnalysisPE { get; set; }
        public DbSet<OptionsAnalysisCrudeOil> OptionsAnalysisCrudeOil { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Configure the ClaimIdentifier property
            modelBuilder.Entity<TradingUsers>().Property(u => u.ClaimId).HasDefaultValueSql("NEWID()");
        }
    }
}
