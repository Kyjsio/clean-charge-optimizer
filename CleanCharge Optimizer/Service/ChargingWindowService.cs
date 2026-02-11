using CleanCharge_Optimizer.DTOs;
using CleanCharge_Optimizer.Exceptions;
using static System.Reflection.Metadata.BlobBuilder;

namespace CleanCharge_Optimizer.Service
{
    public class ChargingWindowService
    {
        private readonly DownloadDataService _downloadService;

private readonly HashSet<string> _cleanFuelNames = CleanEnergyHelper.GetCleanEnergySources();

        public ChargingWindowService(HttpClient httpClient)
        {
            _downloadService = new DownloadDataService(httpClient);

        }
        public async Task<List<ChargingWindowDto>> GetChargingWindow(int chargingHours)
        {
            if (chargingHours < 1 || chargingHours > 6 )
            {
                throw new InvalidChargingParametersException("Charging time must be between 1 and 6 hours");
            }

            var data = await _downloadService.DownloadDataAsync(3);

            if (data == null || !data.Any())
            {
                throw new InsufficientDataException("External API returned empty data.");
            }

            var futureData = data.Where(x => x.From >= DateTime.UtcNow).ToList();

            Console.WriteLine($"Future data points available: {futureData.Count}");
        
            int slots = chargingHours * 2;

            if (futureData.Count < slots)
            {
                throw new InsufficientDataException(
                    $"Not enough future data points. Needed: {slots}, Available: {futureData.Count}");
            }

            ChargingWindowDto bestWindow = null;
            decimal maxCleanEnergyAvg = 0;

            for (int i = 0; i <= futureData.Count - slots; i++)
            {
               
                decimal currentWindowSum = 0;

                for (int j=0; j<slots;j++)
                {
                    var section = futureData[i + j];
                    var cleanInSection = section.GenerationMix
                        .Where(m => _cleanFuelNames.Contains(m.Fuel))
                        .Sum(m => m.Perc);

                    currentWindowSum += cleanInSection;
                }
               

                decimal currentAvg = currentWindowSum / slots;

               //Console.WriteLine($"Window {futureData[i].From} to {futureData[i + slots - 1].To} - Avg Clean Energy: {currentAvg}%");
               //Console.WriteLine(maxCleanEnergyAvg);

                if (currentAvg > maxCleanEnergyAvg)
                {
                    maxCleanEnergyAvg = currentAvg;

                    var firstSlot = futureData[i];
                    var lastSlot = futureData[i + slots - 1];
         
                    bestWindow = new ChargingWindowDto
                    {
                        StartTime = firstSlot.From,
                        EndTime = lastSlot.To,
                        AverageCleanEnergyPercent = Math.Round(currentAvg, 2)
                    };
                }
            }

            return new List<ChargingWindowDto> { bestWindow };
        }
    }
}
