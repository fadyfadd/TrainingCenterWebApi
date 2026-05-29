using System.Text;
using DataAccessLayer;
using DataAccessLayer.Entities;
using DataAccessLayer.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    string secretKey = generalSettings.JwtSettings.Key;
    var keyBytes = Encoding.UTF8.GetBytes(secretKey); 

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

        //ValidateIssuer = true,
        ValidIssuer = generalSettings.JwtSettings.Issuer,

        //ValidateAudience = true,
        ValidAudience = generalSettings.JwtSettings.Audience,

        //ValidateLifetime = true,
        //RequireExpirationTime = true,
        //ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllers(options =>
{
    options.Filters.Add<RequestProfilingFilter>();
})

.AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new ReadyStringTrimmerConverter());
    }).ConfigureApiBehaviorOptions(options =>
    {

    });

builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JwtTokenServices>();

builder.Services.AddAutoMapper(cfg => { }, typeof(DefaultProfile).Assembly);

builder.Services.AddDbContext<MainDataBaseContext>(options =>
    options.UseNpgsql(generalSettings.ConnectionString));


builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => { options.User.RequireUniqueEmail = true; })
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

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();


app.Run();
