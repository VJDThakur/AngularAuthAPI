using AngularAuthAPI.IUsers;
using AngularAuthAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AngularAuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Iuser _users;
        public UserController(Iuser users)
        {
            _users = users; 
        }
        [HttpPost("login")] 
        public async Task<IActionResult> Login ([FromBody] User user)
        {
            var isLogin =await _users.Autheticate(user);
            return Ok(isLogin);
        }
        [HttpPost("SignUp")] 
        public async Task <IActionResult> RagisterUser ([FromBody]User user)   
        {
            var isRagister = await _users.RagisterUser (user);
            return Ok(isRagister); 
        }
    }
}
