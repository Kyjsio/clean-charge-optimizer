using System.Net;
using System.Text.Json;
using CleanCharge_Optimizer.Models;
using CleanCharge_Optimizer.Service;
using Moq;
using Moq.Protected;


namespace CleanCharge_Optimizer.Tests
{
    public class EnergyMixServiceTests
    {
        [Fact]
        public async Task GetEnergyMix_CalculatesAverages()
        {

            var testDate = DateTime.UtcNow;


            var apiResponseModel = new CarbonResponse
            {
                Data = new List<GenerationData>
                {
                    new GenerationData
                    {
                        From = testDate.AddHours(12),
                        GenerationMix = new List<GenerationMix>
                        {
                            new GenerationMix { Fuel = "solar", Perc = 50 },
                            new GenerationMix { Fuel = "coal", Perc = 50 }  
                        }
                    },
                    new GenerationData
                    {
                        From = testDate.AddHours(13),
                        GenerationMix = new List<GenerationMix>
                        {
                            new GenerationMix { Fuel = "solar", Perc = 30 }, 
                            new GenerationMix { Fuel = "coal", Perc = 70 }  
                        }
                    }
                }
            };

         
            var jsonResponse = JsonSerializer.Serialize(apiResponseModel);

          
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var service = new EnergyMixService(httpClient);

           
            var result = await service.GetEnergyMix();


            Assert.Single(result);
            var dayResult = result.First();


      
            Assert.Equal(40m, dayResult.AverageCleanEnergyPercent);

            var solar = dayResult.AverageFuelMix.FirstOrDefault(x => x.Fuel == "solar");
            var coal = dayResult.AverageFuelMix.FirstOrDefault(x => x.Fuel == "coal");

            Assert.NotNull(solar);
            Assert.Equal(40, solar.Perc);

            Assert.NotNull(coal);
            Assert.Equal(60, coal.Perc);
        }

        [Fact]
        public async Task GetEnergyMix_IgnoresZeroPercentFuels()
        {
            var apiResponseModel = new CarbonResponse
            {
                Data = new List<GenerationData>
                {
                    new GenerationData
                    {
                        From = DateTime.Now,
                        GenerationMix = new List<GenerationMix>
                        {
                            new GenerationMix { Fuel = "wind", Perc = 100 },
                            new GenerationMix { Fuel = "nuclear", Perc = 0 } 
                        }
                    }
                }
            };

            var jsonResponse = JsonSerializer.Serialize(apiResponseModel);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var service = new EnergyMixService(new HttpClient(handlerMock.Object));


            var result = await service.GetEnergyMix();

            var dayResult = result.First();

            Assert.DoesNotContain(dayResult.AverageFuelMix, f => f.Fuel == "nuclear");
            Assert.Contains(dayResult.AverageFuelMix, f => f.Fuel == "wind");
        }
    }
}