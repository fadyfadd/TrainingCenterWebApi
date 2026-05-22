using DataAccessLayer.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingCenterWebApi.Services;

namespace TrainingCenterWebApi.Controllers
{
    public class CourseController : ApiBaseController
    {
        CourseService CourseService;

        public CourseController(CourseService courseService)
        {
            this.CourseService = courseService;
        }

        [HttpGet("/courses")]
        public async Task<ActionResult<List<CourseDto>>> GetAllCourses()
        { 
            var res = await this.CourseService.GetAllCourses();
            return Ok(res);
        }

        [HttpGet("/courses/enrollments")]
        public async Task<ActionResult<List<CourseDto>>> GetCoursesEnrollment()
        {
            var res = await this.CourseService.GetCoursesEnrollment();
            return Ok(res);
        }

        [HttpPost("/courseCategory/add")]
        public async Task<ActionResult<CourseCategoryDto>> AddCourseCategory([FromBody] CourseCategoryDto courseCategoryDto)
        {
            var res = await this.CourseService.AddCategory(courseCategoryDto);
            return Ok(res);
        }

        [HttpPut("/courseCategory/update")]
        public async Task<ActionResult<CourseCategoryDto>> UpdateCourseCategory([FromBody] CourseCategoryDto courseCategoryDto)
        {
            var res = await this.CourseService.UpdateCategory(courseCategoryDto);
            return Ok(res);
        }

        [HttpDelete("/courseCategory/delete/{id}")]
        public async Task<ActionResult<CourseCategoryDto>> DeleteCourseCategory(int id)
        {
            var res = await this.CourseService.DeleteCategory(id);
            return Ok(res);
        }

        [HttpPost("/course/add")]
        public async Task<ActionResult<CourseDto>> AddCourse([FromBody] CourseDto courseDto)
        {
            var res = await this.CourseService.AddCourse(courseDto);
            return Ok(res);
        }

        [HttpPut("/course/update")]
        public async Task<ActionResult<CourseDto>> UpdateCourse([FromBody] CourseDto courseDto)
        {
            var res = await this.CourseService.UpdateCourse(courseDto);
            return res;
        }

        [HttpDelete("/course/delete/{id}")]
        public async Task<ActionResult<CourseDto>> DeleteCourse(int id)
        {
            var res = await this.CourseService.DeleteCourse(id);
            return Ok(res);
        }

    }
}
