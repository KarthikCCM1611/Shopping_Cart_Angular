using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebAPI.Models;
using WebAPI.Services;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MasterController : ControllerBase
    {
        private readonly IMaster _masterSrc;
        public MasterController(IMaster masterSrc) { 
            _masterSrc = masterSrc;
        }

        [HttpPost("Login")]
        public IActionResult Login(Login loginObj)
        {
            return Content(_masterSrc.Login(loginObj));
        }

        [HttpPost("Register")]
        public IActionResult Register(Register registerObj)
        {
            return Content(_masterSrc.Register(registerObj));
        }
    }
}
