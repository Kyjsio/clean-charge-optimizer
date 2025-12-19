namespace CleanCharge_Optimizer.DTOs
{
    public class ChargingWindowDto
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal AverageCleanEnergyPercent { get; set; }
    }
}
