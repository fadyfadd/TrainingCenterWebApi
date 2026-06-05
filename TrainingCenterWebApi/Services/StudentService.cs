 using AutoMapper;
using DataAccessLayer;

namespace Api.Services;

public class StudentService
{
    private readonly MainDataBaseContext dataContext;
    private readonly IMapper mapper;
    public StudentService(MainDataBaseContext dataContext , IMapper mapper)
    {
        this.dataContext = dataContext;
        this.mapper = mapper;
    }
}
