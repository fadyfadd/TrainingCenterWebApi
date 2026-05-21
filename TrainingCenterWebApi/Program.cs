using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainingCenterWebApi;
using TrainingCenterWebApi.Infrastructure;
using TrainingCenterWebApi.Services;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var configuration = builder.Configuration;
var generalSettings = configuration.GetSection(GeneralSettings.sectionName).Get<GeneralSettings>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins(generalSettings.CorsAllowedOrigins)
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});


builder.Services.AddControllers(options =>
{
    options.Filters.Add<RequestProfilingFilter>();
});
builder.Services.AddScoped<CourseService>();
builder.Services.AddAutoMapper(cfg => { }, typeof(DefaultProfile).Assembly);

builder.Services.AddDbContext<MainDataBaseContext>(options =>
    options.UseNpgsql(generalSettings.ConnectionString));


builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<MainDataBaseContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSwaggerGen();


builder.Services.Configure<GeneralSettings>(configuration.GetSection(GeneralSettings.sectionName));
var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseMiddleware<MyCustomMiddleware>();
app.UseAuthorization();
app.MapControllers();


app.Run();
