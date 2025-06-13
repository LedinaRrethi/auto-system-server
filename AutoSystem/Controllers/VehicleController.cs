//using Domain.Contracts;
//using DTO.VehicleDTO;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;

//namespace AutoSystem.Controllers
//{
//    [ApiController]
//    [Route("api/user/vehicles")]
//    [Authorize(Roles = "Individ")]
//    public class VehicleController : ControllerBase
//    {
//        private readonly IVehicleDomain _vehicleDomain;

//        public VehicleController(IVehicleDomain vehicleDomain)
//        {
//            _vehicleDomain = vehicleDomain;
//        }

//        [HttpPost("register")]
//        public async Task<IActionResult> Register([FromBody] VehicleRegisterDTO dto)
//        {
//            await _vehicleDomain.RegisterVehicleAsync(dto);
//            return Ok(new { message = "Vehicle registration submitted." });
//        }

//        [HttpGet("my")]
//        public async Task<ActionResult<List<VehicleDTO>>> GetMyVehicles()
//        {
//            var result = await _vehicleDomain.GetMyVehiclesAsync();
//            return Ok(result);
//        }
//    }
//}
