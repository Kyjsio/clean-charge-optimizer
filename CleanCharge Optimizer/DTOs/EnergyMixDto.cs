namespace CleanCharge_Optimizer.DTOs
{
    public class DailyEnergyMix
    {
        public string Date { get; set; }
        public decimal AverageCleanEnergyPercent { get; set; }
        public List<FuelSourceList> AverageFuelMix { get; set; }
    }
    public class FuelSourceList
    {
        public string Fuel { get; set; }
        public decimal Perc { get; set; }
    }
}

