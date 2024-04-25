using Domain.DTO_s.StudentDTO;
using Domain.Filters;
using Domain.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.StudentService
{
    public interface IStudentService
    {
        Task<PageResponse<List<GetStudentDto>>> GetStudentAsync(StudentFilter filter);
        Task<Response<GetStudentDto>> GetStudentByIdAsync(int id); 
        Task<Response<string>>AddStudentAsync(AddStudentDto studentDto); 
        Task<Response<string>>UpdateStudentAsync(UpdateStudentDto studentDto);  
        Task<Response<bool>>DeleteStudentAsync(int studentId);
    }
}
