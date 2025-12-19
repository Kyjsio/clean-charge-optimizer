using CleanCharge_Optimizer.Service;
using Microsoft.AspNetCore.Mvc;

namespace CleanCharge_Optimizer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CleanEnergyController : Controller
    {
        private readonly EnergyMixService _EnergyMixService;
        private readonly ChargingWindowService _chargingwindowservice;
        public CleanEnergyController(EnergyMixService EnergyMixService, ChargingWindowService ChargingWindowService)
        {
            _EnergyMixService = EnergyMixService;
            _chargingwindowservice = ChargingWindowService;
        }
        [HttpGet("chargingwindow/{ChargingTime}")]
        public async Task<IActionResult> GetChargingWindow(int ChargingTime)
        {
            var result = await _chargingwindowservice.GetChargingWindow(ChargingTime);

            return Ok(result);
        }


        [HttpGet("mixFuel")]
        public async Task<IActionResult> GetEnergyMix()
        {
            var result = await _EnergyMixService.GetEnergyMix();

            return Ok(result);
        }
    }
}
