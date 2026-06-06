using System.Text;
using Api;
using Api.Infrastructure;
using Api.Services;
using Data;
using Data.Entities;
using Data.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
 

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var configuration = builder.Configuration;
var generalSettings = configuration.GetSection(GeneralSettings.sectionName).Get<GeneralSettings>();

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(context.Configuration);
});



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

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => { options.User.RequireUniqueEmail = true; })
    .AddEntityFrameworkStores<MainDataBaseContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
.AddJwtBearer(options =>
{
    Console.WriteLine("JwtBearer configuration executed");

    string secretKey = generalSettings.JwtSettings.Key;
    var keyBytes = Encoding.UTF8.GetBytes(secretKey);


    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(keyBytes),

        ValidateIssuer = true,
        ValidIssuer = generalSettings.JwtSettings.Issuer,

        ValidateAudience = true,
        ValidAudience = generalSettings.JwtSettings.Audience,

        ValidateLifetime = true,
        RequireExpirationTime = true,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization((options) =>
{
    Console.WriteLine("Authorization configuration executed");
});
builder.Services.AddControllers(options =>
{
    options.Filters.Add<RequestProfilingFilter>();
})

.AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new StringTrimmerConverter());
    }).ConfigureApiBehaviorOptions(options =>
    {

    });

builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<JwtTokenServices>();

builder.Services.AddAutoMapper(cfg => { }, typeof(DefaultProfile).Assembly);

builder.Services.AddDbContext<MainDataBaseContext>(options =>
    options.UseNpgsql(generalSettings.ConnectionString));




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
