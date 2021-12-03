
using Microsoft.AspNetCore.Mvc;
using GeoAPI.Services;



namespace GeoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LABoundaryController : ControllerBase
    {
        private readonly IDataService _dataService;
        public LABoundaryController(IDataService dataService)
        {
            _dataService = dataService;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var records = _dataService.Get();
            if (records != null)
                    return Ok(records);
            return NotFound();
        }
        
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
