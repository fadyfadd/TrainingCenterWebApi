using System;
using AutoMapper;
using DataAccessLayer;
using DataAccessLayer.Dtos;
using DataAccessLayer.Entities;
using DataAccessLayer.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

    public async Task<ApplicationUserDto> UpdateStudentEmail(ApplicationUserDto applicationUserDto)
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
