using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalks.Data;
using NewZealandWalks.Models.Domain;
using NewZealandWalks.Models.DTOs;

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
            var regionsDomain = _db.Regions.ToList();
            var regionsDTO = new List<RegionDTO>();
            foreach (var region in regionsDomain) 
            {
                regionsDTO.Add(new RegionDTO()
                {
                    Id = region.Id,
                    Name= region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl,
                });
            }
            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetRegionById([FromRoute]Guid id) 
        {
            //var region = _db.Regions.Find(id);
            var regionsDomain = _db.Regions.FirstOrDefault(r => r.Id == id);

            if (regionsDomain == null) 
            {
                return NotFound();
            }
            var regionsDTO = new RegionDTO
            { 
                Id = regionsDomain.Id,
                Name = regionsDomain.Name,
                Code = regionsDomain.Code,
                RegionImageUrl = regionsDomain.RegionImageUrl,
            };

            return Ok(regionsDTO);
        }

        [HttpPost]
        public IActionResult CreateRegion([FromBody] AddRegionRequestDTO addRegionRequestDTO) 
        {
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
            };
            _db.Regions.Add(regionDomainModel);
            _db.SaveChanges();

            var regionDTO = new RegionDTO 
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            return CreatedAtAction(nameof(GetRegionById),new {  id = regionDTO.Id },regionDTO);

             
        }

    }
}
