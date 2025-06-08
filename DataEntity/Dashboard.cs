using System;
using System.Collections.Generic;

namespace DisciplineTradingJournalAPI.DataEntity
{
    public class Dashboard
    {
        public UserPerformanceMetric performanceMetric { get; set; }

        public List<DashboardChartData> dashboardChartData { get; set; }

    }
    public class DashboardChartData
    {
        public DateTime? ExitDate { get; set; }

        public decimal? ProfitAndLoss { get; set; }

        public decimal? CumulativeProfitAndLoss { get; set; }


    }
}
