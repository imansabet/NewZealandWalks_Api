using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalks.CustomActionFilters;
using NewZealandWalks.Models.Domain;
using NewZealandWalks.Models.DTOs;
using NewZealandWalks.Repositories;

namespace NewZealandWalks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IWalkRepository _walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository = walkRepository;
        }
        
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDTO addWalkRequestDTO)
        {
            
             //Map DTO to DomainModel
             var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDTO);
             await _walkRepository.CreateAsync(walkDomainModel);

             //Map Domain Model to DTO
             return Ok(_mapper.Map<WalkDTO>(walkDomainModel));

        }

        //GET"/api/walks/?filterOn=Name&filterQuery=Track&sortBy=Name&isAscending=true&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? filterOn, [FromQuery] string? filterQuery
            , [FromQuery] string? sortBy, [FromQuery] bool? isAscending 
            , [FromQuery] int  pageNumber = 1, [FromQuery] int pageSize = 1000 )
        {
            var walksDomainModel = await _walkRepository.GetAllAsync(filterOn,filterQuery,sortBy,isAscending ?? true, pageNumber, pageSize);

            //create an exception
            throw new Exception("This is a new Exception");

            //Map DomainModel to DTO
            return Ok(_mapper.Map<List<WalkDTO>>(walksDomainModel));
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetWalkById([FromRoute] Guid id) 
        {
            var walkDomainModel = await _walkRepository.GetByIdAsync(id);
            if (walkDomainModel == null)
            {
                return NotFound();
            }
            //map domain model to DTO
            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));
        }

        [HttpPut] 
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk([FromRoute] Guid id, UpdateWalkRequestDTO updateWalkRequestDTO ) 
        {
            
                //map DTO to domainmodel
                var walkdomainModel = _mapper.Map<Walk>(updateWalkRequestDTO);
                walkdomainModel = await _walkRepository.UpdateAsync(id, walkdomainModel);
                if (walkdomainModel == null)
                {
                    return NotFound();
                }
                return Ok(_mapper.Map<WalkDTO>(walkdomainModel));
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteWalk([FromRoute] Guid id) 
        {
            var deletedWalkDomain = await _walkRepository.DeleteAsync(id);
            if (deletedWalkDomain == null) 
            {
                return NotFound();
            }
            //map domain to dto
            return Ok(_mapper.Map<WalkDTO>(deletedWalkDomain));
        }

    }
}
