using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using orderit_api.Dto;
using orderit_api.Interfaces;
using orderit_api.Models;
using orderit_api.Repository;
using orderit_api.Services;
using System.Reflection.Metadata.Ecma335;

namespace orderit_api.Controller
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ISalespersonRepository _salespersonRepository;
        public AuthController(IAuthService authService, ISalespersonRepository salespersonRepository)
        {
            _authService = authService;
            _salespersonRepository = salespersonRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(LoginUser user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!user.SalespersonId.HasValue)
            {
                throw new ArgumentNullException(nameof(user.SalespersonId), "SalespersonId cannot be null.");
            }

            if (!_salespersonRepository.SalespersonExist(user.SalespersonId.Value))
            {
                return BadRequest("Salesperson not found");
            }

            var identityUser = await _authService.RegisterUser(user);

            if (identityUser == null)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            //Agrega la relacion del nuevo usuario al Salesperson correspondiente   
            var salesperson = _salespersonRepository.GetById(user.SalespersonId.Value);
            if(!_salespersonRepository.UpdateUserId(salesperson, identityUser.Id))
            {
                return BadRequest($"Error en asignar userId al salesperson: {salesperson}");
            }

            //Con esto tengo el id del usuario
            var newUser = await _authService.GetUserByUsername(user.UserName);

            //Uso el id del usuario para buscarlo en Salesperson
            if (newUser == null)
            {
                return BadRequest("User not found");
            }

            var authResponse = _authService.GenerateTokenStringAndClaims(newUser, salesperson);
            return Ok(identityUser.Id);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginUser user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _authService.Login(user)) 
            {
                return BadRequest("Something went wrong");
            }

            //Con esto tengo el id del usuario
            var newUser = await _authService.GetUserByUsername(user.UserName);

            //Uso el id del usuario para buscarlo en Salesperson
            if (newUser==null)
            {
                return BadRequest("User not found");
            }

            var salesperson = _salespersonRepository.GetSalespersonByUserId(newUser.Id);
            if (salesperson == null)
            {
                return BadRequest("Salesperson not found");
            }

            var authResponse = await _authService.GenerateTokenStringAndClaims(newUser, salesperson);
            return Ok(authResponse);
        }

        [HttpGet("users")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<LoginUser>))]
        public async Task<IActionResult> GetAllUsers()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var users = await _authService.GetAllUsers();
            return Ok(users);
        }
    }


}
