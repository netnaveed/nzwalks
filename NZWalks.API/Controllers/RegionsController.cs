using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Controllers
{
    // https://localhost:<port>/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            // Get data from the database - Domain models
            var regionsDomain = dbContext.Regions.ToList();

            // Map Domain Models to DTOs
            var regionsDto = new List<RegionDto>();
            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl
                });
            }


            // Return DTO
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            // Get region domain model
            //var region = dbContext.Regions.Find(id);
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map region domain model to domain DTO
            var regionDto = new RegionDto(){
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            }; 

            // Return DTO
            return Ok(regionDto);
        }
        
        // POST to Create New Region
        [HttpPost]
        public IActionResult Create([FromBody] RegionCreateDto regionCreateDto)
        {
            // Map Region DTO to Domain Model
            var regionDomain = new Region()
            {
                Code = regionCreateDto.Code,
                Name = regionCreateDto.Name,
                RegionImageUrl= regionCreateDto.RegionImageUrl
            };

            // Use Domain Model to create Region
            dbContext.Regions.Add(regionDomain);
            dbContext.SaveChanges();

            // Map Domain Model back to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return CreatedAtAction( nameof(GetById), new {id = regionDto.Id}, regionDto );
        }

        // PUT to Update Existing Region
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] RegionUpdateDto regionUpdateDto)
        {
            // Check if region exists
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if( regionDomain == null )
            {
                return NotFound();
            }

            // Map DTO to Domain model to update
            regionDomain.Code = regionUpdateDto.Code;
            regionDomain.Name = regionUpdateDto.Name;
            regionDomain.RegionImageUrl = regionUpdateDto.RegionImageUrl;
            dbContext.SaveChanges();

            // Domain model to DTO
            var regionDto = new RegionDto()
            {
               Id = regionDomain.Id,
               Code = regionDomain.Code,
               Name = regionUpdateDto.Name,
               RegionImageUrl = regionUpdateDto.RegionImageUrl
            };

            // return
            return Ok(regionDto);
        }

        // DELETE to Delete Existing Region
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);

            if( regionDomain == null ) { 
                return NotFound(); 
            }

            // Delete Region
            dbContext.Regions.Remove(regionDomain);
            dbContext.SaveChanges();

            // Map Domain Model to DTO
            var regionDto = new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            };

            return Ok(regionDto);
        }
    }
}
