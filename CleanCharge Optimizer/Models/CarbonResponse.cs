using System.Text.Json.Serialization;

namespace CleanCharge_Optimizer.Models
{
    public class CarbonResponse
    {
        [JsonPropertyName("data")]
        public List<GenerationData> Data { get; set;} =new();
    }
    public class GenerationData
    {
        [JsonPropertyName("from")]
        public DateTime From { get; set; }

        [JsonPropertyName("to")]
        public DateTime To { get; set; }

        [JsonPropertyName("generationmix")]
        public List<GenerationMix> GenerationMix { get; set;} =new();

    }
    public class GenerationMix
    {
        [JsonPropertyName("fuel")]
        public string Fuel { get; set; }

        [JsonPropertyName("perc")]
        public decimal Perc { get; set; }

    }
}
