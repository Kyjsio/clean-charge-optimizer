using CleanCharge_Optimizer.DTOs;

namespace CleanCharge_Optimizer.Service
{
    public class EnergyMixService
    {
        private readonly DownloadDataService _downloadService;
        private readonly string[] cleanFuels = { "biomass", "nuclear", "hydro", "wind", "solar" };

        public EnergyMixService(HttpClient httpClient)
        {
            _downloadService = new DownloadDataService(httpClient);
        }

        public async Task<List<DailyEnergyMix>> GetEnergyMix()
        {
            var data = await _downloadService.DownloadDataAsync(3);

            var cutoffDate = DateTime.UtcNow.Date;

            var result = data
                .Where(d => d.From.Date >= cutoffDate)
                .GroupBy(d => d.From.Date)
                .Select(dayGroup => new DailyEnergyMix
                {
                    Date = dayGroup.Key.ToString("yyyy-MM-dd"),

                   AverageCleanEnergyPercent = Math.Round(dayGroup.Average(section =>
                        section.GenerationMix
                            .Where(m => cleanFuels.Contains(m.Fuel)) 
                            .Sum(m => m.Perc) 
                    ), 2),

                    AverageFuelMix = dayGroup
                        .SelectMany(x => x.GenerationMix)
                        .GroupBy(mix => mix.Fuel)
                        .Select(f => new FuelSourceList
                        {
                            Fuel = f.Key,
                            Perc = Math.Round(f.Average(mix => mix.Perc), 2)
                        })
                        .Where(f => f.Perc != 0)
                        .ToList()
                }) 
                .OrderBy(x => x.Date)
                .ToList();

            return result;
        }
    }
}
