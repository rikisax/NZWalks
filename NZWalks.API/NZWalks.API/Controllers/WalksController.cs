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
        private readonly IRegionRepository regionRepository;
        private readonly IWalkDifficultyRepository walkDifficultyRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, 
            IRegionRepository regionRepository, IWalkDifficultyRepository walkDifficultyRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
            this.walkDifficultyRepository = walkDifficultyRepository;
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

            if (! await ValidateAddWalkAsync(addWalkRequest) )
            {
                return BadRequest(ModelState);
            }

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

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            if (addWalkRequest == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest), "La Walk non può essere null");
                return false;  //ritorno false qui così non prosegue i controlli, sarebbe inutile.
            }
            if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            {
                ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} deve essere specificato.");
            }
            if (addWalkRequest.Length <= 0)
            {
                ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} deve essere maggiore di zero.");
            }


            //if (addWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest),
            //        $"{nameof(addWalkRequest)} cannot be empty.");
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Name),
            //        $"{nameof(addWalkRequest.Name)} is required.");
            //}

            //if (addWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Length),
            //        $"{nameof(addWalkRequest.Length)} should be greater than zero.");
            //}

            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
                if (region == null)
                {
                    ModelState.AddModelError(nameof(addWalkRequest.RegionId),
                        $"{nameof(addWalkRequest.RegionId)} is invalid.");
                }

                var walkDifficulty = await walkDifficultyRepository.GetAsync(addWalkRequest.WalkDifficultyId);
                if (walkDifficulty == null)
                {
                    ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId),
                           $"{nameof(addWalkRequest.WalkDifficultyId)} is invalid.");

                }

            return (ModelState.ErrorCount == 0);

        }
    }
}
