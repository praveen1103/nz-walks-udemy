using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    // https://localhost/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        // GET ALL REGIONS
        // http://localhost/api/regions
        [HttpGet]
        public IActionResult GetAll() 
        {
            var regionsDomain = dbContext.Regions.ToList();

            var regionsDto = new List<RegionDto>();

            foreach(var regionDomain in  regionsDomain) 
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl,
                });
            }

            return Ok(regionsDto);
        }

        // GET SINGLE ID
        // http://localhost/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id) 
        {
            //var region = dbContext.Regions.Find(id);

            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if(regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };


            return Ok(regionDto);
        }

        // CREATE NEW REGION
        [HttpPost]
        public IActionResult Create([FromBody] CreateRegionRequestDto createRegionRequestDto)
        {
            var regionDomain = new Region { Code = createRegionRequestDto.Code, Name = createRegionRequestDto.Name, RegionImageUrl = createRegionRequestDto.RegionImageUrl };

            dbContext.Regions.Add(regionDomain);

            dbContext.SaveChanges();

            var regionDto = new RegionDto
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            };

            return CreatedAtAction(nameof(GetById), new { id = regionDomain.Id }, regionDto);
        }
    }
}
