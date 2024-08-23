using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using orderit_api.Dto;
using orderit_api.Interfaces;
using orderit_api.Models;
using orderit_api.Repository;

namespace orderit_api.Controller
{
    [Route("api/salespersons")]
    [ApiController]
    public class SalespersonController : ControllerBase
    {
        private readonly ISalespersonRepository _salespersonRepository;
        private readonly IMapper _mapper;

        public SalespersonController(ISalespersonRepository salespersonRepository, IMapper  mapper)
        {
            _salespersonRepository = salespersonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Salesperson>))]
        public IActionResult GetSalespersons()
        {
            var salespersons = _mapper.Map<List<SalespersonDto>>(_salespersonRepository.GetSalespersons());

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(salespersons);        
        }

        [HttpGet("{salespersonId}")]
        [ProducesResponseType(200, Type = typeof(Salesperson))]
        public IActionResult GetSalesperson(int salespersonId)
        {
            if (!_salespersonRepository.SalespersonExist(salespersonId))
                return NotFound();

            var salespersons = _mapper.Map<SalespersonDto>(_salespersonRepository.GetById(salespersonId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(salespersons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public IActionResult CreateSalespeson([FromBody] SalespersonDto salespersonCreate) 
        {
            if (salespersonCreate == null)
                return BadRequest(ModelState);

            var salesperson = _salespersonRepository.GetSalespersons()
                             .Where(s =>
                                 s.FirstName.Trim().ToUpper() == salespersonCreate.FirstName.TrimEnd().ToUpper() &&
                                 s.LastName.Trim().ToUpper() == salespersonCreate.LastName.TrimEnd().ToUpper()
                             )
                             .FirstOrDefault();

            if (salesperson != null) 
            {
                ModelState.AddModelError("", "Salesperson already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var salespersonMap = _mapper.Map<Salesperson>(salespersonCreate);

            if (!_salespersonRepository.CreateSalesperson(salespersonMap)) 
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully created");
        }

        [HttpPut("{salespersonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int salespersonId, [FromBody] SalespersonDto salespersonDto)
        {
            if (salespersonDto == null) return BadRequest(ModelState);

            if (salespersonId != salespersonDto.SalespersonId) return BadRequest(ModelState);

            if (!_salespersonRepository.SalespersonExist(salespersonId))
                return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var salespersonMap = _mapper.Map<Salesperson>(salespersonDto);

            if (!_salespersonRepository.UpdateSalesperson(salespersonMap))
            {
                ModelState.AddModelError("", "Something went wrong updating salesperson");
                return StatusCode(500, ModelState);

            }

            return NoContent();
        }

        [HttpDelete("{salespersonId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSalesperson(int salespersonId)
        {
            if (!_salespersonRepository.SalespersonExist(salespersonId))
            {
                return NotFound();
            }

            var salespersonToDelete = _salespersonRepository.GetById(salespersonId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_salespersonRepository.DeleteSalesperson(salespersonToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting salesperson");
            }

            return NoContent();
        }

    }
}
