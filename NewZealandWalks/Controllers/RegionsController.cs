using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewZealandWalks.Data;
using NewZealandWalks.Models.Domain;
using NewZealandWalks.Models.DTOs;
using NewZealandWalks.Repositories;

namespace NewZealandWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IRegionRepository _regionRepository;
        public RegionsController(ApplicationDbContext db,IRegionRepository regionRepository)
        {
            _db = db;
            _regionRepository = regionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() 
        {
            var regionsDomain = await  _regionRepository.GetAllAsync();
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
        public async Task<IActionResult> GetRegionById([FromRoute]Guid id) 
        {
            //var region = _db.Regions.Find(id);
            var regionsDomain = await _regionRepository.GetByIdAsync(id);

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
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDTO addRegionRequestDTO) 
        {
            //map DTO to DomainModel
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
            };
            await _db.Regions.AddAsync(regionDomainModel);
            await _db.SaveChangesAsync();

            //Use DomainModel To create Region
            var regionDTO = new RegionDTO 
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            //201
            return CreatedAtAction(nameof(GetRegionById),new {  id = regionDTO.Id },regionDTO);
        }
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO) 
        {
            var regionDomainModel = await _db.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomainModel == null) 
            {
                return NotFound();
            }
            //map DTO to DomainModel
            regionDomainModel.Code = updateRegionRequestDTO.Code;
            regionDomainModel.Name = updateRegionRequestDTO.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDTO.RegionImageUrl;

            await _db.SaveChangesAsync();
            //Convert DomainModel to DTO
            var regionDTO = new RegionDTO 
            {
                Id= regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            //We Always Pass DTO to Client
            return Ok(regionDTO);



        }
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id) 
        {
            var regionDomainModel =  await _db.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (regionDomainModel == null) 
            {
                return NotFound();
            }
            _db.Regions.Remove(regionDomainModel);
            await _db.SaveChangesAsync();
            //return deleted region back
            var regionDTO = new RegionDTO 
            {
                Id = regionDomainModel.Id,
                Name = regionDomainModel.Name,
                Code = regionDomainModel.Code,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };


            return Ok(regionDTO);
        }

    }
}
