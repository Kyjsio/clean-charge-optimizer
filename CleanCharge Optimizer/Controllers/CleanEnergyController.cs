using CleanCharge_Optimizer.Service;
using Microsoft.AspNetCore.Mvc;
using CleanCharge_Optimizer.Exceptions;
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
            try
            {
                var result = await _chargingwindowservice.GetChargingWindow(ChargingTime);
                return Ok(result);
            }
            catch (InvalidChargingParametersException ex)
            {
                return BadRequest(new { message = ex.Message }); 
            }
            catch (InsufficientDataException ex)
            {
                return UnprocessableEntity(new { message = ex.Message }); 
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An internal server error occurred." });
            }
        }
        


        [HttpGet("mixFuel")]
        public async Task<IActionResult> GetEnergyMix()
        {
            var result = await _EnergyMixService.GetEnergyMix();

            return Ok(result);
        }
    }
}
