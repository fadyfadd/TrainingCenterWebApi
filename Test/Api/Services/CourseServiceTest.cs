using Api.Services;
using Data;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Transactions;

namespace TestProject.Api.Services
{
    public class CourseServiceTest : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly WebApplicationFactory<Program> _factory;

        public CourseServiceTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }   

        [Fact]
        public async Task GetAllCourses_ReturnsAllCourses()
        {
            using var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            using var contextScope = _factory.Services.CreateScope();
            var dbContext = contextScope.ServiceProvider.GetRequiredService<MainDataBaseContext>();
            string sqlFilePath = Path.Combine(AppContext.BaseDirectory, "seed_data.sql");
            var sql = await File.ReadAllTextAsync(sqlFilePath);
            await dbContext.Database.ExecuteSqlRawAsync(sql);
            var myService = contextScope.ServiceProvider.GetRequiredService<CourseService>();
            var result = await myService.GetAllCourses();
            Assert.True(result.Count == 11);

               

        }
    }
}
