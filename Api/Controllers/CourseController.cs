using Api.Services;
using Data;
using Data.Dtos; 
using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc;
 
namespace Api.Controllers
{
    public class CourseController : ApiBaseController
    {
        CourseService CourseService;

        public CourseController(CourseService courseService)
        {
            this.CourseService = courseService;
        }

        [Authorize(Roles = nameof(UserRole.Administrator))]
        [HttpGet("all")]
        public async Task<ActionResult<List<CourseDto>>> GetAllCourses()
        {
            var res = await this.CourseService.GetAllCourses();
            return Ok(res);
        }

        [HttpGet("courses/enrollments")]
        public async Task<ActionResult<List<CourseDto>>> GetCoursesEnrollment()
        {
            var res = await this.CourseService.GetCoursesEnrollment();
            return Ok(res);
        }

        [HttpPost("courseCategory/add")]
        public async Task<ActionResult<CourseCategoryDto>> AddCourseCategory([FromBody] CourseCategoryDto courseCategoryDto)
        {
            var res = await this.CourseService.AddCategory(courseCategoryDto);
            return Ok(res);
        }

        [HttpPut("courseCategory/update")]
        public async Task<ActionResult<CourseCategoryDto>> UpdateCourseCategory([FromBody] CourseCategoryDto courseCategoryDto)
        {
            var res = await this.CourseService.UpdateCategory(courseCategoryDto);
            return Ok(res);
        }

        [HttpDelete("courseCategory/delete/{id}")]
        public async Task<ActionResult<CourseCategoryDto>> DeleteCourseCategory(int id)
        {
            var res = await this.CourseService.DeleteCategory(id);
            return Ok(res);
        }

        [HttpPost("add")]
        public async Task<ActionResult<CourseDto>> AddCourse([FromBody] CourseDto courseDto)
        {
            var res = await this.CourseService.AddCourse(courseDto);
            return Ok(res);
        }

        [HttpPut("update")]
        public async Task<ActionResult<CourseDto>> UpdateCourse([FromBody] CourseDto courseDto)
        {
            var res = await this.CourseService.UpdateCourse(courseDto);
            return res;
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult<CourseDto>> DeleteCourse(int id)
        {
            var res = await this.CourseService.DeleteCourse(id);
            return Ok(res);
        }

        
        [HttpGet("categories")]
        public async Task<ActionResult<List<CourseCategoryDto>>> GetAllCategories(int id)
        {
            var res = await CourseService.GetAllCategories(); 
            return Ok(res);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CourseDto>> GetCourse(int id)
        {
            var res = await CourseService.GetCourse(id);
            return Ok(res);
        }

    }
}
