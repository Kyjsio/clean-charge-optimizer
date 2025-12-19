using CleanCharge_Optimizer.Models;
using CleanCharge_Optimizer.Service;
using Moq;
using Moq.Protected;
using System.Net;
using System.Text.Json;

namespace CleanCharge_Optimizer.Tests
{
    public class ChargingWindowServiceTests
    {
        [Theory]
        [InlineData(0)] 
        [InlineData(7)]  
        public async Task GetChargingWindow_ThrowsException(int hours)
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(mockHandler.Object);
            var service = new ChargingWindowService(httpClient);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => service.GetChargingWindow(hours));
        }

        [Fact]
        public async Task GetChargingWindow_FindsBestWindow()
        {

            var testDate = DateTime.UtcNow;


            var apiResponse = new CarbonResponse
            {
                Data = new List<GenerationData>
                {
                    new GenerationData
                    {
                        From = testDate.AddHours(12),
                        To = testDate.AddHours(12.5),
                        GenerationMix = new List<GenerationMix>
                        {
                            new GenerationMix { Fuel = "solar", Perc = 50 },
                            new GenerationMix { Fuel = "coal", Perc = 50 }
                        }
                    },
                    new GenerationData
                    {
                        From = testDate.AddHours(12.5),
                        To = testDate.AddHours(13),
                        GenerationMix = new List<GenerationMix>
                        {
                            new GenerationMix { Fuel = "solar", Perc = 90 },
                            new GenerationMix { Fuel = "coal", Perc = 10 }
                        }
                    }
                }
            };

            var jsonResponse = JsonSerializer.Serialize(apiResponse);

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

            var service = new ChargingWindowService(new HttpClient(handlerMock.Object));

            var resultList = await service.GetChargingWindow(1);

            Assert.NotNull(resultList);
            Assert.Single(resultList);

            var bestWindow = resultList.First();
            Assert.NotNull(bestWindow);

            Assert.Equal(70, bestWindow.AverageCleanEnergyPercent);


            Assert.Equal(testDate.AddHours(12), bestWindow.StartTime);
          
            Assert.Equal(testDate.AddHours(13), bestWindow.EndTime);
        }

        [Fact]
        public async Task GetChargingWindow_CalculatesAverage()
        {
            var testDate = DateTime.UtcNow;

            var apiResponse = new CarbonResponse
            {
                Data = new List<GenerationData>
                {
                    new GenerationData
                    {
                        From = testDate.AddHours(0.5),
                        To = testDate.AddHours(1),
                        GenerationMix = new List<GenerationMix>
                        {
                            new GenerationMix { Fuel = "solar", Perc = 10 },
                            new GenerationMix { Fuel = "coal", Perc = 90 }
                        }
                    },
                    new GenerationData
                    {
                        From = testDate.AddHours(1),
                        To = testDate.AddHours(1.5),
                        GenerationMix = new List<GenerationMix>
                        {
                            new GenerationMix { Fuel = "solar", Perc = 80 },
                            new GenerationMix { Fuel = "coal", Perc = 20 }
                        }
                    },
                    new GenerationData
                    {
                        From = testDate.AddHours(1.5),
                        To = testDate.AddHours(2),
                        GenerationMix = new List<GenerationMix>
                        {
                            new GenerationMix { Fuel = "solar", Perc = 90 },
                            new GenerationMix { Fuel = "coal", Perc = 10 }
                        }
                    },
                    new GenerationData
                    {
                        From = testDate.AddHours(2),
                        To = testDate.AddHours(2.5),
                        GenerationMix = new List<GenerationMix>
                        {
                            new GenerationMix { Fuel = "solar", Perc = 10 },
                            new GenerationMix { Fuel = "coal", Perc = 90 }
                        }
                    },
                    new GenerationData
                    {
                        From = testDate.AddHours(2.5),
                        To = testDate.AddHours(3),
                        GenerationMix = new List<GenerationMix>
                        {
                            new GenerationMix { Fuel = "solar", Perc = 100 },
                            new GenerationMix { Fuel = "coal", Perc = 0 }
                        }
                    }
                }

            };

            var jsonResponse = JsonSerializer.Serialize(apiResponse);

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            var service = new ChargingWindowService(new HttpClient(handlerMock.Object));

            var resultList = await service.GetChargingWindow(1);

            var bestWindow = resultList.First();


            Assert.Equal(85, bestWindow.AverageCleanEnergyPercent);


            Assert.Equal(testDate.AddHours(1), bestWindow.StartTime);

            Assert.Equal(testDate.AddHours(2), bestWindow.EndTime);
        }

      
    }
}