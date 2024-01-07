using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public WalksController(IMapper mapper,IWalkRepository walkRepository)
        {
            _mapper = mapper;
            _walkRepository= walkRepository;
        }
        [HttpPost]
        public async Task<IActionResult> CreateWalk([FromBody] AddWalkRequestDTO addWalkRequestDTO ) 
        {
            //Map DTO to DomainModel
            var walkDomainModel = _mapper.Map<Walk>(addWalkRequestDTO);
            await _walkRepository.CreateAsync(walkDomainModel);

            //Map Domain Model to DTO
            return Ok(_mapper.Map<WalkDTO>(walkDomainModel));

            
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll()
        { 
            var walksDomainModel = await _walkRepository.GetAllAsync();
            //Map DomainModel to DTO
            return Ok(_mapper.Map<List<WalkDTO>>(walksDomainModel));
        }

    }
}
