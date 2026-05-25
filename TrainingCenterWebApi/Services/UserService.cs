using System;
using AutoMapper;
using DataAccessLayer;
using DataAccessLayer.Dtos;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace TrainingCenterWebApi.Services;

public class UserService
{

    private readonly UserManager<ApplicationUser> userManager;
    private readonly IMapper mapper;
    private readonly MainDataBaseContext dbContext;

    public UserService(MainDataBaseContext dataContext, UserManager<ApplicationUser> userManager, IMapper mapper)
    {
        this.userManager = userManager;
        this.mapper = mapper;
        this.dbContext = dataContext;
    }

    public async Task<ApplicationUserDto> RegisterUser(ApplicationUserDto applicationUserDto)
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
                g => g.Select(e => e.Description).ToArray()
);
            throw new BusinessException("4455ebd1", "User registration failed.", null, errors);
        }

        student.UserId = applicationUser.Id;
        dbContext.Students.Add(student);
        dbContext.SaveChanges();

        var res = mapper.Map<ApplicationUserDto>(applicationUser);
        var studentDto = mapper.Map<StudentDto>(student);
        res.Student = studentDto;

        await transaction.CommitAsync();
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
        }
        return new JwtTokenDto
        {
            Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c",
            Expiration = DateTime.UtcNow.AddHours(1),
            FirstName = userDto.Student?.FirstName,
            LastName = userDto.Student.LastName
        };
    }


}
