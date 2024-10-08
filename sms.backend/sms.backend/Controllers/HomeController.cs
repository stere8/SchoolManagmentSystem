﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;
using sms.backend.Views;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("student")]
    public class StudentController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly ILogger<StudentController> _logger;

        public StudentController(SchoolContext context, ILogger<StudentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("timetable")]
        public async Task<ActionResult<IEnumerable<TimetableView>>> GetTimetable()
        {
            try
            {
                _logger.LogInformation("Getting student timetable");
                var timetables = await _context.Timetables
                    .Join(_context.Lessons,
                        timetable => timetable.LessonId,
                        lesson => lesson.LessonId,
                        (timetable, lesson) => new TimetableView
                        {
                            DayOfWeek = timetable.DayOfWeek,
                            LessonName = lesson.Name,
                            StartTime = timetable.StartTime,
                            EndTime = timetable.EndTime
                        })
                    .ToListAsync();

                if (timetables == null || !timetables.Any())
                {
                    _logger.LogWarning("No timetables found");
                    return NotFound();
                }
                return Ok(timetables);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the student timetable");
                return StatusCode(500, $"An error occurred while processing your home request.{ex.Message}");
            }
        }

        [HttpGet("grades")]
        public async Task<ActionResult<IEnumerable<Mark>>> GetGrades()
        {
            try
            {
                _logger.LogInformation("Getting student grades");
                var grades = await _context.Marks.ToListAsync();
                if (grades == null || !grades.Any())
                {
                    _logger.LogWarning("No grades found");
                    return NotFound();
                }
                return Ok(grades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the student grades");
                return StatusCode(500, $"An error occurred while processing your home request.{ex.Message}");
            }
        }

        [HttpGet("attendance")]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendance()
        {
            try
            {
                _logger.LogInformation("Getting student attendance");
                var attendance = await _context.Attendances.ToListAsync();
                if (attendance == null || !attendance.Any())
                {
                    _logger.LogWarning("No attendance records found");
                    return NotFound();
                }
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the student attendance");
                return StatusCode(500, $"An error occurred while processing your home request.{ex.Message}");
            }
        }
    }
}