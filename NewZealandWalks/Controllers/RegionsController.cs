using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalks.Data;

namespace NewZealandWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public RegionsController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult GetAll() 
        {
            var regions = _db.Regions.ToList();
            return Ok(regions);
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetRegionById([FromRoute]Guid id) 
        {
            //var region = _db.Regions.Find(id);
            var region = _db.Regions.FirstOrDefault(r => r.Id == id);

            if (region == null) 
            {
                return NotFound();
            }
            return Ok(region);
        }

    }
}
