
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace dotnet2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class StudentController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents(){
            string[] studentNames = new string[]{"john","lol","Mark"};
            return Ok(studentNames);
        }
    }
}