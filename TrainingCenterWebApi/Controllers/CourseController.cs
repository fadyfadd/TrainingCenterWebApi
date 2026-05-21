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

        [HttpGet("/courses/enrollment")]
        public async Task<ActionResult<List<CourseDto>>> GetCoursesEnrollment()
        {
            var res = await this.CourseService.GetCoursesEnrollment();
            return Ok(res);

        }
    }
}
