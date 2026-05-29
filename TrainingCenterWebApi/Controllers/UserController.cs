using DataAccessLayer.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingCenterWebApi.Services;

namespace TrainingCenterWebApi.Controllers
{

    public class UserController : ApiBaseController
    {

        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService; ;
        }

        [HttpPost("register/student")]
        public async Task<ActionResult<ApplicationUserDto>> RegisterStudent([FromBody] ApplicationUserDto applicationUserDto)
        {
            await userService.RegisterStudent(applicationUserDto);
            return Ok(applicationUserDto);
        }

        [HttpPost("login")]
        public async Task<ActionResult<JwtTokenDto>> Login([FromBody] LoginDto loginDto)
        {
            var jwtToken = await userService.Login(loginDto);
            return Ok(jwtToken);
        }

        [HttpPut("student/updateEmail")]
        public async Task<ActionResult<ApplicationUserDto>> UpdateStudentEmail([FromBody] ApplicationUserDto applicationUserDto)
        {
            var res = await userService.UpdateStudentEmail(applicationUserDto);
            return Ok(res);

        }

        [HttpPut("student/update")]
        public async Task<ActionResult<ApplicationUserDto>> UpdateStudent([FromBody] StudentDto studentDto)
        {
            var res = await userService.UpdateStudent(studentDto);
            return Ok(res);
        }

        [HttpDelete("student/delete/{userId}")]
        public async Task<ActionResult<ApplicationUserDto>> DeleteStudent(int userId)
        {
            var res = await userService.DeleteStudent(userId);
            return Ok(res);
        }

    }
}