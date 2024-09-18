using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using orderit_api.Dto;
using orderit_api.Interfaces;
using orderit_api.Models;
using orderit_api.Repository;

namespace orderit_api.Controller
{
    [Route("api/stores")]
    [ApiController]
    [Authorize]
    public class StoreController : ControllerBase
    {
        private readonly IStoreRepository _storeRepository;
        private readonly IMapper _mapper;
        private object _companyRepository;

        public StoreController(IStoreRepository storeRepository, IMapper mapper)
        {
            _storeRepository = storeRepository;
            _mapper = mapper;   
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Store>))]
        public IActionResult GetStores()
        {
            var stores = _mapper.Map<List<StoreDto>>(_storeRepository.GetStores());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(stores);
        }

        [HttpGet("{storeId}")]
        [ProducesResponseType(200, Type = typeof(Store))]
        public IActionResult GetStore(int storeId)
        {
            if (!_storeRepository.StoreExist(storeId))
                return NotFound();

            var stores = _mapper.Map<StoreDto>(_storeRepository.GetById(storeId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(stores);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateStore([FromBody] StoreDto storeCreate)
        {
            if (storeCreate == null)
                return BadRequest(ModelState);

            var stores = _storeRepository.GetStores()
                 .Where(s => s.Name == storeCreate.Name)
                 .FirstOrDefault();

            if (stores != null)
            {
                ModelState.AddModelError("", "Store already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var storeMap = _mapper.Map<Store>(storeCreate);

            if (!_storeRepository.CreateStore(storeMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Store successfully created");

        }

        [HttpPut("{storeId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateStore(int storeId, [FromBody] StoreDto storeDto)
        {
            if (storeDto == null) return BadRequest(ModelState);

            if (storeId != storeDto.StoreId) return BadRequest(ModelState);

            if (!_storeRepository.StoreExist(storeId))
                return NotFound();

            if (!ModelState.IsValid) return BadRequest(ModelState);

            var storeMap = _mapper.Map<Store>(storeDto);

            if (!_storeRepository.UpdateStore(storeMap))
            {
                ModelState.AddModelError("", "Something went wrong updating store");
                return StatusCode(500, ModelState);

            }

            return NoContent();
        }

        [HttpDelete("{storeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteSalesperson(int storeId)
        {
            if (!_storeRepository.StoreExist(storeId))
            {
                return NotFound();
            }

            var storeToDelete = _storeRepository.GetById(storeId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_storeRepository.DeleteStore(storeToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting store");
            }

            return NoContent();
        }
    }
}
