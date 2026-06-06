using AutoMapper;
using Data;
using Data.Dtos;
using Data.Entities;
using Microsoft.EntityFrameworkCore;


namespace Api.Services
{
    public class CourseService
    {
        MainDataBaseContext dataContext;
        IMapper mapper;

        public CourseService(MainDataBaseContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<List<CourseDto>> GetAllCourses()
        {
            var courses = await dataContext.Courses.Include(a => a.CourseCategory).OrderBy(c=>c.Id).ToListAsync();
            var res = mapper.Map<List<CourseDto>>(courses);
            return res;
        }

        public async Task<CourseDto> GetCourse(int id)
        {
            var course = await dataContext.Courses.Include((c)=>c.CourseCategory).FirstOrDefaultAsync(c=>c.Id == id);
            var res = mapper.Map<CourseDto>(course);
            return res;
        }

        public async Task<List<CourseCategoryDto>> GetAllCategories()
        {
            var categories = await dataContext.CourseCategories.ToListAsync();
            var dtos = mapper.Map<List<CourseCategoryDto>>(categories);
            return dtos;
        }
        
        public async Task<CourseCategoryDto> AddCategory(CourseCategoryDto courseCategoryDto)
        {
            var courseCategory = new CourseCategory
            {
                Name = courseCategoryDto.Name
            };

            dataContext.CourseCategories.Add(courseCategory);
            await dataContext.SaveChangesAsync();
            var res = mapper.Map<CourseCategoryDto>(courseCategory);
            return res;
        }

        public async Task<CourseCategoryDto> UpdateCategory(CourseCategoryDto courseCategoryDto)
        {
            var entity = await dataContext.CourseCategories.FindAsync(courseCategoryDto.Id);
            entity.Name = courseCategoryDto.Name;
            var res = mapper.Map<CourseCategoryDto>(entity);
            dataContext.SaveChanges();
            return res;
        }

        public async Task<CourseCategoryDto> DeleteCategory(int id)
        {
            var courseCategory = await dataContext.CourseCategories.FindAsync(id);

            dataContext.CourseCategories.Remove(courseCategory);
            await dataContext.SaveChangesAsync();
            var res = mapper.Map<CourseCategoryDto>(courseCategory);
            dataContext.SaveChanges();
            return res;
        }

        public async Task<CourseDto> AddCourse(CourseDto courseDto)
        {
            var course = mapper.Map<Course>(courseDto);
            course.Id = 0;
            var category = await dataContext.CourseCategories.FindAsync(courseDto.CourseCategoryId);
            course.CourseCategory = category;
            dataContext.Courses.Add(course);
            await dataContext.SaveChangesAsync();
            var res = mapper.Map<CourseDto>(course);
            return res;
        }

        public async Task<CourseDto> UpdateCourse(CourseDto courseDto)
        {
            var course = await dataContext.Courses.FindAsync(courseDto.Id);

            if (courseDto.CourseCategoryId != null)
            {
                //var category = await dataContext.CourseCategories.FindAsync(courseDto.CourseCategoryId);
                course.CourseCategoryId = courseDto.CourseCategoryId.Value;

            }

            course.Title = courseDto.Title;
            await dataContext.SaveChangesAsync();
            var res = mapper.Map<CourseDto>(course);
            return res;
        }

        public async Task<CourseDto> DeleteCourse(int id)
        {
            var course = await dataContext.Courses.FindAsync(id);

            dataContext.Courses.Remove(course);
            await dataContext.SaveChangesAsync();
            var res = mapper.Map<CourseDto>(course);
            return res;


        }

        public async Task<List<CourseWithStudentsDto>> GetCoursesEnrollment()
        {
            var courses = await dataContext.Courses.Include(a => a.Students).Include(a => a.CourseCategory).ToListAsync();
            List<CourseDto> output = new List<CourseDto>();
            var res = mapper.Map<List<CourseWithStudentsDto>>(courses);
            return res;

        }
    }
}
