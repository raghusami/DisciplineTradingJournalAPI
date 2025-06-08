using DisciplineTradingJournalAPI.DataEntity;
using DisciplineTradingJournalAPI.DesignPattern;
using DisciplineTradingJournalAPI.ViewEntity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.DBModel
{
    public class SeriesService
    {

        private readonly IUnitOfWork _unitOfWork;

        public SeriesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateTrackerWithTrades(SeriesTrackerViewEntity seriesTrackerView, int userId)
        {
            try
            {
                var tracker = new SeriesTracker
                {
                    TotalTrades = seriesTrackerView.TotalTrades,
                    StartDate = seriesTrackerView.StartDate,
                    EndDate = seriesTrackerView.EndDate,
                    IsCompleted = true,
                    CreatedAt = DateTime.Now,
                    IsActive = true,
                    UserId = userId
                };

                await _unitOfWork.SeriesTrackers.AddAsync(tracker);
                await _unitOfWork.CompleteAsync(); // TrackerId is now available

                if (tracker.TrackerId > 0)
                {
                    var tradesDetails = new List<SeriesTradeDetails>();
                    string baseName = string.IsNullOrWhiteSpace(seriesTrackerView.SeriesName)
                        ? "Series"
                        : seriesTrackerView.SeriesName;

                    for (int i = 0; i < tracker.TotalTrades; i++)
                    {
                        var trade = new SeriesTradeDetails
                        {
                            TrackerId = tracker.TrackerId,
                            TradeNumber = i + 1,
                            TradeSeriesName = $"{baseName}-{(i + 1).ToString("D2")}"
                        };
                        tradesDetails.Add(trade);
                    }

                    await _unitOfWork.SeriesTradeDetails.AddRangeAsync(tradesDetails);
                    await _unitOfWork.CompleteAsync();
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional: use a logger if available)
                Console.WriteLine($"Error in CreateTrackerWithTrades: {ex.Message}");
                Console.WriteLine(ex.StackTrace);

                // Optionally, rethrow or handle as needed
                throw new ApplicationException("An error occurred while creating tracker and trades.", ex);
            }
        }

    }
}
