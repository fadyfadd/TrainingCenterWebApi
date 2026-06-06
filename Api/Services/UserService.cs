 using System.Security.Claims;
using Api.Infrastructure;
using AutoMapper;
using Data;
using Data.Dtos;
using Data.Entities;
using Data.Exceptions;
 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
 

namespace Api.Services;

public class UserService
{

    private readonly UserManager<ApplicationUser> userManager;
    private readonly IMapper mapper;
    private readonly MainDataBaseContext dbContext;
    private readonly JwtTokenServices jwtTokenServices;
    private readonly GeneralSettings generalSettings;

    public UserService(MainDataBaseContext dataContext,
    UserManager<ApplicationUser> userManager, IMapper mapper, JwtTokenServices tokenServices,
    IOptions<GeneralSettings> generalSettings)
    {
        this.userManager = userManager;
        this.mapper = mapper;
        this.dbContext = dataContext;
        this.jwtTokenServices = tokenServices;
        this.generalSettings = generalSettings.Value;
    }

    public async Task<ApplicationUserDto> DeleteStudent(int userId)
    {

        var user = await userManager.FindByIdAsync(userId.ToString());
        var student = await dbContext.Students.Where(s => s.UserId == userId).FirstOrDefaultAsync();


        if (student == null)
            throw new BusinessException("76b2bea7", "Student Not Found.", null, null);

        await userManager.DeleteAsync(user);

        var res = mapper.Map<ApplicationUserDto>(user);
        var studentDto = mapper.Map<StudentDto>(student);
        res.PlainPassword = "";
        res.Student = studentDto;

        return res;

    }

    public async Task<ApplicationUserDto> RegisterStudent(ApplicationUserDto applicationUserDto)
    {

        using var transaction = await dbContext.Database.BeginTransactionAsync();

        var applicationUser = mapper.Map<ApplicationUser>(applicationUserDto);
        applicationUser.EmailConfirmed = true;
        var student = mapper.Map<Student>(applicationUserDto.Student);

        var result = await userManager.CreateAsync(applicationUser, password: applicationUserDto.PlainPassword);

        if (!result.Succeeded)
        {
            await transaction.RollbackAsync();
            var errors = result.Errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Description).ToArray());
            throw new BusinessException("4455ebd1", "User Registration Failed.", null, errors);
        }

        student.UserId = applicationUser.Id;
        dbContext.Students.Add(student);
        dbContext.SaveChanges();

        var res = mapper.Map<ApplicationUserDto>(applicationUser);
        var studentDto = mapper.Map<StudentDto>(student);
        res.PlainPassword = "";
        res.Student = studentDto;

        await transaction.CommitAsync();
        return res;

    }

    public async Task<ApplicationUserDto> UpdateUserEmail(ApplicationUserDto applicationUserDto)
    {

        var user = await userManager.FindByNameAsync(applicationUserDto.UserName);
        user.Email = applicationUserDto.Email;
        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var errors = result.Errors
            .GroupBy(e => e.Code)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.Description).ToArray());
            throw new BusinessException("4455ebd1", "Email Modification Failed.", null, errors);
        }


        var res = mapper.Map<ApplicationUserDto>(user);

        return res;

    }

    public async Task<StudentDto> UpdateStudent(StudentDto studentDto)
    {

        var student = await dbContext.Students.FindAsync(studentDto.Id);

        student.FirstName = studentDto.FirstName;
        student.LastName = studentDto.LastName;
        student.EnrolledAt = studentDto.EnrolledAt.Value;
        var res = mapper.Map<StudentDto>(student);

        dbContext.SaveChanges();

        return res;

    }

    public async Task<JwtTokenDto> Login(LoginDto loginDto)
    {
        var user = await userManager.FindByNameAsync(loginDto.UserName);

        if (user == null)
        {
            throw new BusinessException("4455ebd2", "Invalid Credentials", null, null);
        }
        var passwordValid = await this.userManager.CheckPasswordAsync(user, loginDto.Password);

        if (!passwordValid)
        {
            throw new BusinessException("4455ebd2", "Invalid Credentials", null, null);
        }

        var userDto = mapper.Map<ApplicationUserDto>(user);
        var student = dbContext.Students.FirstOrDefault(s => s.UserId == user.Id);
        if (student != null)
        {
            userDto.Student = mapper.Map<StudentDto>(student);

            List<Claim> claims = new() {
            new Claim(ClaimTypes.Name, loginDto.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, nameof(UserRole.Student)),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var tokenDto = jwtTokenServices.CreateToken(claims, generalSettings.JwtSettings.ExpiryInMinutes);
            tokenDto.FirstName = student.FirstName;
            tokenDto.LastName = student.LastName;
            tokenDto.Role = UserRole.Student;

            return tokenDto;

        }
        else
        {
            var administrator = dbContext.Administrators.FirstOrDefault(a => a.UserId == user.Id);

            if (administrator != null)
            {
                List<Claim> claims = new() {
                new Claim(ClaimTypes.Name, loginDto.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, nameof(UserRole.Administrator)),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                var tokenDto = jwtTokenServices.CreateToken(claims, generalSettings.JwtSettings.ExpiryInMinutes);
                tokenDto.FirstName = administrator.FirstName;
                tokenDto.LastName = administrator.LastName;
                tokenDto.Role = UserRole.Administrator;

                return tokenDto;

            }
        }

        throw new BusinessException("4455ebd2", "Invalid Credentials", null, null);


    }


}
