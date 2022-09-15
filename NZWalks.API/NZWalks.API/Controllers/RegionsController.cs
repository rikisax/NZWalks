using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace NZWalks.API.Controllers
{
    [ApiController]
    //Lez. 27 circa al minuto 2:
    //La Route è praticamente l'endpoint che devo chiamare per raggiungere questo controller
    //posso chiamarlo come voglio, tipo così: [Route("Regions")]
    //Invece usando l' attribute "controller", gli assegna automaticamente lo stesso nome del controller
    //(cioè RegionsController in questo caso, a cui toglie "Controller" quindi diventa solo "Regions"
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository,IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }


        [HttpGet]
        public async  Task<IActionResult> GetAllRegionsAsync()
        {
            //con questa variabile regions a cui aggiungo due regioni, posso testare con Swagger se l'api funziona, prima di connetterla col database.
            //var regions = new List<Region>()
            //{
            //    new Region
            //    {
            //        Id= Guid.NewGuid(),
            //        Name = "Wellington",
            //        Code = "WLG",
            //        Area = 227755,
            //        Lat= -1.8822,
            //        Long = 299.88,
            //        Population= 500000
            //    },
            //    new Region
            //    {
            //        Id= Guid.NewGuid(),
            //        Name = "Auckland",
            //        Code = "AUCK",
            //        Area = 227755,
            //        Lat= -1.8822,
            //        Long = 299.88,
            //        Population= 500000
            //    }
            //};
            var regions = await regionRepository.GetAllAsync();
            //return DTO regions: soluzione senza Automapper
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population
            //    };
            //    regionsDTO.Add(regionDTO);
            //});
            
            //return DTO regions: soluzione CON Automapper
            var regionsDTO=  mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);
        }
        [HttpGet]
        [Route("{id:guid}")]
        //il parametro id arriva dalla route (i parametri nell'URL del chiamante)
        //possiamo quindi definire il parametro id fra parentesi: questo sarà di seguito alla Route del controller (vedi sopra:  [Route("[controller]")]) 
        //e possiamo anche specificare il tipo :guid
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }
            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);

        }
        [HttpPost]
        public async Task<IActionResult>  AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Request to Domain model
            //metodo convenzionale senza il mapper.
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Name = addRegionRequest.Name,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Population = addRegionRequest.Population
            };

            //Pass details to Repository
            region = await regionRepository.AddAsync(region);
            if (region == null)
            {
                return BadRequest();
            }
            //Convert back to DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,   
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            };
            //Ritorna al client che ha creato l'oggetto e quindi fa' eseguire l'action denominata sopra 
            //nel metodo GetRegionAsync nel suo attribute:   [ActionName("GetRegionAsync")]
            return CreatedAtAction(nameof(GetRegionAsync), new {id=regionDTO.Id},regionDTO);

        }
        [HttpDelete]
        [Route("{id:guid}")]
        public async  Task<IActionResult>  DeleteRegionAsync(Guid id)
        {
            //Get region from database by Delete
            var region = await regionRepository.DeleteAsync(id);
            //if null NotFound
            if (region == null) 
            { 
                return NotFound(); 
            }
            //      convert response back to DTO
            var regionDTO = new Models.DTO.Region  //ci vanno le parentesi qui ???..sembra ininfluente
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population

            };
            return Ok(regionDTO);
        }
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //convert DTO to Domain model
            var region = new Models.Domain.Region
            {
                Code = updateRegionRequest.Code,
                Name = updateRegionRequest.Name,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Population = updateRegionRequest.Population

            };
            //Update region using repository
            region = await regionRepository.UpdateAsync(id, region);
            //if Null then NotFound
            if (region == null)
            {
                return NotFound();
            }

            //Convert Domain back to DTO
            //Return OK response
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population
            }; 
           return Ok(regionDTO);

        }
    }
}
