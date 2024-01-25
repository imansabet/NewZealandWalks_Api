using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewZealandWalks.CustomActionFilters;
using NewZealandWalks.Data;
using NewZealandWalks.Models.Domain;
using NewZealandWalks.Models.DTOs;
using NewZealandWalks.Repositories;
using System.Text.Json;

namespace NewZealandWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IRegionRepository _regionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegionsController> _logger;

        public RegionsController(ApplicationDbContext db
            ,IRegionRepository regionRepository
            ,IMapper mapper
            ,ILogger<RegionsController> logger)
        {
            _db = db;
            _regionRepository = regionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        //[Authorize(Roles = "Reader,Writer" )]
        public async Task<IActionResult> GetAll() 
        {
            _logger.LogInformation("GetAll Action Method Was Invoked");
            var regionsDomain = await  _regionRepository.GetAllAsync();
            //var regionsDTO = new List<RegionDTO>();
            //foreach (var region in regionsDomain) 
            //{
            //    regionsDTO.Add(new RegionDTO()
            //    {
            //        Id = region.Id,
            //        Name= region.Name,
            //        Code = region.Code,
            //        RegionImageUrl = region.RegionImageUrl,
            //    });
            //}

            //map domain model to dto 
            var regionsDTO = _mapper.Map<List<RegionDTO>>(regionsDomain);
            _logger.LogInformation($"Finished GetAll Regions Request with data : {JsonSerializer.Serialize(regionsDomain)}");
            return Ok(regionsDTO);
        }
        
        [HttpGet]
        [Route("{id:Guid}")]
        //[Authorize(Roles = "Reader")]
         public async Task<IActionResult> GetRegionById([FromRoute]Guid id) 
        {
            //var region = _db.Regions.Find(id);
            var regionsDomain = await _regionRepository.GetByIdAsync(id);

            if (regionsDomain == null) 
            {
                return NotFound();
            }
            //var regionsDTO = new RegionDTO
            //{ 
            //    Id = regionsDomain.Id,
            //    Name = regionsDomain.Name,
            //    Code = regionsDomain.Code,
            //    RegionImageUrl = regionsDomain.RegionImageUrl,
            //};


            return Ok(_mapper.Map<RegionDTO>(regionsDomain));
        }
        
        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateRegion([FromBody] AddRegionRequestDTO addRegionRequestDTO) 
        {
            
                //map DTO to DomainModel
                //var regionDomainModel = new Region
                //{
                //    Code = addRegionRequestDTO.Code,
                //    Name = addRegionRequestDTO.Name,
                //    RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
                //};
                var regionDomainModel = _mapper.Map<Region>(addRegionRequestDTO);


                //Use DomainModel To create Region
                regionDomainModel = await _regionRepository.CreateAsync(regionDomainModel);

                //map DTO to DomainModel
                //var regionDTO = new RegionDTO 
                //{
                //    Id = regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl,
                //};
                var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);

                //201
                return CreatedAtAction(nameof(GetRegionById), new { id = regionDTO.Id }, regionDTO);
            
        }
       
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> UpdateRegion([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO) 
        {
                //map dto to domain model
                //var regionDomainModel = new Region
                //{
                //   Code = updateRegionRequestDTO.Code,
                //   Name = updateRegionRequestDTO.Name,
                //   RegionImageUrl= updateRegionRequestDTO.RegionImageUrl,
                //};
                var regionDomainModel = _mapper.Map<Region>(updateRegionRequestDTO);

                regionDomainModel = await _regionRepository.UpdateAsync(id, regionDomainModel);
                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                await _db.SaveChangesAsync();
                //Convert DomainModel to DTO
                //var regionDTO = new RegionDTO 
                //{
                //    Id= regionDomainModel.Id,
                //    Name = regionDomainModel.Name,
                //    Code = regionDomainModel.Code,
                //    RegionImageUrl = regionDomainModel.RegionImageUrl,
                //};
                var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);

                //We Always Pass DTO to Client
                return Ok(regionDTO);
        }
        
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteRegion([FromRoute] Guid id) 
        {
            var regionDomainModel = await _regionRepository.DeleteAsync(id);
            if (regionDomainModel == null) 
            {
                return NotFound();
            }

            //return deleted region back
            //map domain model to dto
            //var regionDTO = new RegionDTO 
            //{
            //    Id = regionDomainModel.Id,
            //    Name = regionDomainModel.Name,
            //    Code = regionDomainModel.Code,
            //    RegionImageUrl = regionDomainModel.RegionImageUrl,
            //};
            var regionDTO = _mapper.Map<RegionDTO>(regionDomainModel);


            return Ok(regionDTO);
        }

    }
}
