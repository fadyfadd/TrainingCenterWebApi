using Microsoft.AspNetCore.Mvc;

namespace TrainingCenterWebApi.Controllers
{
    public class CourseController : ApiBaseController
    {

        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Course controller is working!");
        }
    }
}
