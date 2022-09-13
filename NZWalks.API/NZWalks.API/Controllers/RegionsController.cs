using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace NZWalks.API.Controllers
{
    [ApiController]
    //Lez. 27 circa al minuto 2:
    //La Route è praticamente l'endpoint che devo chiamare per raggiungere questo controller
    //posso chiamarlo come voglio, tipo così: [Route("Regions")]
    //Invece usando il decorator controller, gli assegna automaticamente lo stesso nome del controller
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
        public async  Task<IActionResult> GetAllRegions()
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
    }
}
