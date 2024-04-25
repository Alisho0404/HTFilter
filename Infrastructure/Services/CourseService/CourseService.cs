using AutoMapper;
using Domain.DTO_s.CourseDTO;
using Domain.DTO_s.GroupDTO;
using Domain.Enteties;
using Domain.Filters;
using Domain.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure.Services.CourseService
{
    public class CourseService : ICourseService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public CourseService(DataContext context,IMapper mapper)
        {
            _context = context; 
            _mapper = mapper;
        }

        public async Task<Response<string>> AddCourseAsync(AddCourseDto course)
        {
            try
            {
                var courses = await _context.Courses.FirstOrDefaultAsync(x => x.CourseName==course.CourseName);
                if (courses != null)
                {
                    return new Response<string>(HttpStatusCode.BadRequest, "Error, already exist");
                }
                var mapped = _mapper.Map<Course>(course);

                await _context.Courses.AddAsync(mapped);
                await _context.SaveChangesAsync();

                return new Response<string>("Succesfully added");

            }
            catch (Exception e)
            {
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);

            }
        }

        public async Task<Response<bool>> DeleteCourseAsync(int Id)
        {
            try
            {
                var course = await _context.Courses.Where(x => x.Id == Id).ExecuteDeleteAsync();
                if (course == 0)
                    return new Response<bool>(HttpStatusCode.BadRequest, " Not found");

                return new Response<bool>(true);
            }
            catch (Exception e)
            {
                return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<Response<GetCourseDto>> GetCourseByIdAsync(int id)
        {
            try
            {
                var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == id);
                if (course == null)
                    return new Response<GetCourseDto>(HttpStatusCode.BadRequest, "Not found");
                var mapped = _mapper.Map<GetCourseDto>(course);
                return new Response<GetCourseDto>(mapped);
            }
            catch (Exception e)
            {
                return new Response<GetCourseDto>(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        public async Task<PageResponse<List<GetCourseDto>>> GetCoursesAsync(CourseFilter filter)
        {
            try
            {
                var course = _context.Courses.AsQueryable();
                if (!string.IsNullOrEmpty(filter.CourseName))
                {
                    course=course.Where(x=>x.CourseName.ToLower().Contains(filter.CourseName.ToLower()));
                } 
                var response=await course
                    .Skip((filter.PageNumber-1)*filter.PageSize)
                    .Take(filter.PageSize).ToListAsync();
                var totalRecord = course.Count();
                var mapped=_mapper.Map<List<GetCourseDto>>(response);
                return new PageResponse<List<GetCourseDto>>(mapped, filter.PageNumber, filter.PageSize, totalRecord);
            }

            catch (Exception ex)
            {
                return new PageResponse<List<GetCourseDto>>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        public async Task<Response<string>> UpdateCourseAsync(UpdateCourseDto course)
        {
            try
            {
                var mapped = _mapper.Map<Course>(course);
                _context.Courses.Update(mapped);
                var update = await _context.SaveChangesAsync();
                if (update == 0) return new Response<string>(HttpStatusCode.BadRequest, "Not found");
                return new Response<string>("Updated successfully");
            }
            catch (Exception e)
            {
                return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
