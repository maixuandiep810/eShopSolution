using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using eShopSolution.ViewModels.System.Users;
using eShopSolution.Application.System.Users;
using eShopSolution.Utilities.Exceptions;
using Microsoft.AspNetCore.Authorization;

namespace eShopSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }


        //
        //
        //      AUTH & REGIS
        //
        //

        // ROUTE: POST:/api/users/authenticate
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromForm] LoginRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var resultToken = await _userService.Authenticate(request);
            if (string.IsNullOrEmpty(resultToken) == true)
            {
                return BadRequest("Username or Password is incorrect");
            }

            return Ok(new { token = resultToken });
        }

        // ROUTE POST:api/users/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.Register(request);
            if (result == false)
            {
                return BadRequest("Register is unsuccessful");
            }

            return Created(nameof(Register), request.UserName);
        }
    }
}