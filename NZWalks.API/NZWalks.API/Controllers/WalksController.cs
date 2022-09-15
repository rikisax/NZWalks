using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]   //controller fra parentesi quadre signidica "il contenuto di controller". Altrimenti mi darebbe proprio cotroller come stringa invece di Walks
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await walkRepository.GetAllAsync();
            //automapper passa walks (che è una lista di Domain.Walk) come argomento e restituisce una lista di DTO.Walk.
            //Lo fa' mappano automaticamente i nomi dei campi uguali o simili evitando di farlo a mano
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);
            return Ok(walksDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walk = await walkRepository.GetAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDTO);
        }
        [HttpPost]
        public async Task<IActionResult> AddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            var walk = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };
            walk = await walkRepository.AddAsync(walk);
            if (walk == null)
            {
                return BadRequest();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walk = await walkRepository.DeleteAsync(id);
            if (walk == null)
            {
                return NotFound();
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
            return Ok(walkDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //var walk=mapper.Map<Models.Domain.Walk>(updateWalkRequest);
            var walk = new Models.Domain.Walk
            {
                Id = id,
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };
            walk = await walkRepository.UpdateAsync(id, walk);
            if (walk == null)
            {
                return NotFound();
            }
            else
            {
                var walkDTO = mapper.Map<Models.DTO.Walk>(walk);
                return Ok(walkDTO);
            }
        }
    }
}
