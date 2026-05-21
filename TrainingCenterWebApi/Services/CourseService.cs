using AutoMapper;
using DataAccessLayer;
using DataAccessLayer.Dtos;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq; 

namespace TrainingCenterWebApi.Services
{
    public class CourseService
    {
        MainDataBaseContext dataContext;
        IMapper mapper;

        public CourseService(MainDataBaseContext dataContext , IMapper mapper) { 
            this.dataContext = dataContext;
            this.mapper = mapper; 
        }

        public async Task<List<CourseDto>> GetAllCourses() {
            var courses = await dataContext.Courses.Include(a=>a.CourseCategory) .ToListAsync();
            var res = mapper.Map<List<CourseDto>>(courses);
            return res; 
        }

        public async Task<List<CourseDto>> GetCoursesEnrollment()
        {
            var courses = await dataContext.Courses.Include(a => a.Students).Include(a => a.CourseCategory).ToListAsync();
            List<CourseDto> output = new List<CourseDto>();

             foreach (var course in courses) {
                var courseDto = mapper.Map<CourseDto>(course);
                var studentDtos = mapper.Map<List<StudentDto>>(course.Students);
                courseDto.Students.AddRange(studentDtos);
                output.Add(courseDto);
            }

            return output; 
           
        }
    }
}
