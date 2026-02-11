using CleanCharge_Optimizer.Enum;
using CleanCharge_Optimizer.DTOs;

namespace CleanCharge_Optimizer.Service
{
    public class ChargingWindowService
    {
        private readonly DownloadDataService _downloadService;

        private readonly HashSet<string> _cleanFuelNames;


        public ChargingWindowService(HttpClient httpClient)
        {
            _downloadService = new DownloadDataService(httpClient);
            _cleanFuelNames = System.Enum.GetNames(typeof(CleanEnergySource))
                             .Select(name => name.ToLower())
                             .ToHashSet();
        }
        public async Task<List<ChargingWindowDto>> GetChargingWindow(int chargingHours)
        {
            if (chargingHours < 1 || chargingHours > 6 )
            {
                throw new ArgumentOutOfRangeException(nameof(chargingHours), "Charging time must be between 1 and 6 hours");
            }

            var data = await _downloadService.DownloadDataAsync(3);

            var futureData = data.Where(x => x.From >= DateTime.UtcNow).ToList();

            Console.WriteLine($"Future data points available: {futureData.Count}");

            int slots = chargingHours * 2;
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
