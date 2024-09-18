using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace orderit_api.Controller
{
    [Route("api/test")]
    [ApiController]
    [Authorize]
    public class TestController: ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Te has autenticado!";
        }
    }
}
